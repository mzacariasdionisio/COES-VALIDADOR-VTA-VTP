//using COES.Dominio.DTO.Sic; //temporal
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.TransfPotencia.Helper;
using COES.MVC.Intranet.Areas.TransfPotencia.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
//using COES.Servicios.Aplicacion.General; //temporal
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Controllers
{
    public class PeajeIngresoController : BaseController
    {
        // GET: /TransfPotencia/PeajeIngreso/
        //[CustomAuthorize]

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        ConstantesTransfPotencia libFuncion = new ConstantesTransfPotencia();

        /// <summary>
        /// Muestra la pantalla inicial
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        public ActionResult Index(int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();
            Session["pericodi"] = pericodi;
            Session["recpotcodi"] = recpotcodi;
            PeajeIngresoModel model = new PeajeIngresoModel();
            model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name);
            return View(model);
        }

        /// <summary>
        /// Muestra la lista de datos de PeajeIngreso
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Lista()
        {
            int pericodi = Convert.ToInt32(Session["pericodi"].ToString());
            int recpotcodi = Convert.ToInt32(Session["recpotcodi"].ToString());
            PeajeIngresoModel model = new PeajeIngresoModel();
            model.ListaPeajeIngreso = this.servicioTransfPotencia.ListVtpPeajeIngresoView(pericodi, recpotcodi);
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, User.Identity.Name);
            model.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, User.Identity.Name);
            return PartialView(model);
        }

        /// <summary>
        /// Prepara una vista para ingresar un nuevo registro
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        public ActionResult New(int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();
            PeajeIngresoModel model = new PeajeIngresoModel();
            model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            model.Entidad = new VtpPeajeIngresoDTO();
            if (model.Entidad == null)
            {
                return HttpNotFound();
            }
            model.Entidad.Pingcodi = 0;
            model.Entidad.Pingtipo = "SPT";
            model.Entidad.Pingpago = "SI";
            model.Entidad.Pingtransmision = "SI";
            model.Entidad.Pericodi = model.EntidadRecalculoPotencia.Pericodi;
            model.Entidad.Recpotcodi = model.EntidadRecalculoPotencia.Recpotcodi;
            //Lista de Empresas
            model.ListaEmpresas = this.servicioEmpresa.ListEmpresasSTR();
            //Lista de Reparto de Recaudación de Peajes
            model.ListaRepaRecaPeaje = this.servicioTransfPotencia.GetByCriteriaVtpRepaRecaPeajes(pericodi, recpotcodi);
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            return PartialView(model);
        }

        /// <summary>
        /// Prepara una vista para editar un nuevo registro
        /// </summary>
        /// <param name="pingcodi">Código del registro</param>
        /// <returns></returns>
        public ActionResult Edit(int pericodi, int recpotcodi, int pingcodi)
        {
            base.ValidarSesionUsuario();
            PeajeIngresoModel model = new PeajeIngresoModel();
            model.Entidad = this.servicioTransfPotencia.GetByIdVtpPeajeIngreso(pericodi, recpotcodi, pingcodi);
            if (model.Entidad == null)
            {
                return HttpNotFound();
            }
            //Lista de Empresas
            model.ListaEmpresas = this.servicioEmpresa.ListEmpresasSTR();
            //Lista de Reparto de Recaudación de Peajes
            model.ListaRepaRecaPeaje = this.servicioTransfPotencia.GetByCriteriaVtpRepaRecaPeajes(model.Entidad.Pericodi, model.Entidad.Recpotcodi);
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar los datos del formulario
        /// </summary>
        /// <param name="model">Contiene los datos del regitsro a grabar</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(PeajeIngresoModel model)
        {
            base.ValidarSesionUsuario();
            if (ModelState.IsValid)
            {
                model.Entidad.Pingusumodificacion = User.Identity.Name;

                try
                {
                    model.Entidad.Pingusucreacion = User.Identity.Name;
                    this.servicioTransfPotencia.SaveVtpPeajeIngreso(model.Entidad);
                    TempData["sMensajeExito"] = ConstantesTransfPotencia.MensajeOkInsertarReistro;
                }
                catch(Exception ex)
                {
                    this.servicioTransfPotencia.UpdateVtpPeajeIngreso(model.Entidad);
                    TempData["sMensajeExito"] = ConstantesTransfPotencia.MensajeOkEditarReistro;
                }

                return RedirectToAction("Index", new { pericodi = model.Entidad.Pericodi, recpotcodi = model.Entidad.Recpotcodi });
            }
            //Error
            model.sError = ConstantesTransfPotencia.MensajeErrorGrabarReistro;
            //Lista de Empresas
            model.ListaEmpresas = this.servicioEmpresa.ListEmpresasSTR();
            //Lista de Reparto de Recaudación de Peajes
            model.ListaRepaRecaPeaje = this.servicioTransfPotencia.ListVtpRepaRecaPeajes(); //agregar como parametros el pericodi y el recpotcodi
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar un registro de forma definitiva en la base de datos
        /// </summary>
        /// <param name="pingcodi">Código del registro</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int pericodi, int recpotcodi, int pingcodi = 0)
        {
            base.ValidarSesionUsuario();
            PeajeIngresoModel model = new PeajeIngresoModel();
            this.servicioTransfPotencia.DeleteVtpPeajeIngreso(pericodi, recpotcodi, pingcodi);
            return "true";
        }

        /// <summary>
        /// Muestra un registro 
        /// </summary>
        /// <param name="pingcodi">Código del registro</param>
        /// <returns></returns>
        public ActionResult View(int pericodi, int recpotcodi, int pingcodi = 0)
        {
            PeajeIngresoModel model = new PeajeIngresoModel();
            model.Entidad = this.servicioTransfPotencia.GetByIdVtpPeajeIngresoView(pericodi, recpotcodi, pingcodi);
            return PartialView(model);
        }

        /// <summary>
        /// Permite exportar a un archivo todos los registros en pantalla
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarData(int pericodi = 0, int recpotcodi = 0, int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString();
                string file = this.servicioTransfPotencia.GenerarFormatoPeajeIngreso(pericodi, recpotcodi, formato, pathFile, pathLogo);
                return Json(file);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Descarga el archivo excel
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString() + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;

            return File(path, app, sFecha + "_" + file);
        }

        #region Grilla Excel
        /// <summary>
        /// Carga la hoja de calculo
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        public ActionResult Desarrollo(int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();
            PeajeIngresoModel model = new PeajeIngresoModel();
            model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            return View(model);
        }

        /// <summary>
        /// Muestra la grilla excel para el Desarrollo de peajes e ingresos tarifarios
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcel(int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();

            #region Armando de contenido

            string[] headers = { "", "Tipo", "Titular", "Nombre", "Peaje mensual S/", "Ingreso tarifario mensual S/", "Regulado S/ / kW-mes", "Libre S/ / kW-mes", "Gran usuario S/ / kW-mes" };
            int[] widths = { 1, 49, 250, 250, 150, 150, 150, 150, 150 };
            object[] columnas = new object[9];

            //List<string> tiposEquipos = this.servicio.ListEqFamilias().Select(x => x.Famnomb).ToList();
            List<VtpPeajeIngresoDTO> ListaPeajeIngreso = this.servicioTransfPotencia.ListVtpPeajeIngresoView(pericodi, recpotcodi);

            string[][] data = new string[ListaPeajeIngreso.Count][];
            int index = 0;
            foreach (VtpPeajeIngresoDTO item in ListaPeajeIngreso)
            {
                string[] itemDato = { item.Pingcodi.ToString(), item.Pingtipo, item.Emprnomb, item.Pingnombre, item.Pingpeajemensual.ToString(), item.Pingtarimensual.ToString(), item.Pingregulado.ToString(), item.Pinglibre.ToString(), item.Pinggranusuario.ToString() };
                data[index] = itemDato;
                index++;
            }

            columnas[0] = new
            {   //Pingcodi
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                readOnly = true,
            };
            columnas[1] = new
            {   //Pingtipo
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                //strict = false,               //Si el valor verdadero escrito ingresado en la celda debe coincidir con la fuente de autocompletado . De lo contrario celular no será válida.
                //dateFormat = string.Empty,    //Date validation format. Default 'DD/MM/YYYY'
                //correctFormat = true,         //Si las fechas verdaderas entonces se formatean automáticamente para que coincida con el formato deseado.
                //defaultDate = string.Empty,   //Definición del valor por defecto que llenará las celdas vacías .
                //format = string.Empty,        //Opción deseada para la columna qué tipo 'Numérico ' 
                readOnly = true
            };
            columnas[2] = new
            {   //Emprnomb
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                readOnly = true
            };
            columnas[3] = new
            {   //Pingnombre
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                readOnly = true
            };
            columnas[4] = new
            {   //Pingpeajemensual
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.0000",
                readOnly = false
            };
            columnas[5] = new
            {   //Pingtarimensual
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.0000",
                readOnly = false
            };
            columnas[6] = new
            {   //Pingregulado
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000000000000",
                readOnly = false
            };
            columnas[7] = new
            {   //Pinglibre
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000000000000",
                readOnly = false
            };
            columnas[8] = new
            {   //Pinggranusuario
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000000000000",
                readOnly = false
            };

            #endregion

            model.Data = data;
            model.Headers = headers;
            model.Widths = widths;
            model.Columnas = columnas;
            model.FixedRowsTop = 0;
            model.FixedColumnsLeft = 4;

            return Json(model);
        }

        /// <summary>
        /// Permite grabar los datos del excel
        /// </summary>
        /// <param name="datos">Matriz que contiene los datos de la hoja de calculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarGrillaExcel(int pericodi, int recpotcodi, string[][] datos)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";
            if (pericodi == 0 || recpotcodi == 0)
            {
                sResultado = "Lo sentimos, debe seleccionar un mes de valorización y una versión de recalculo";
                return Json(sResultado);
            }
            try
            {
                //Eliminamos la información procesada en el periodo / versión
                string sBorrar = this.servicioTransfPotencia.EliminarProceso(pericodi, recpotcodi);
                int bar = this.servicioTransfPotencia.ListVtpPeajeIngresoView(pericodi, recpotcodi).Count();
                if (!sBorrar.Equals("1"))
                {
                    return Json(sBorrar);
                }

                //Recorremos la matriz para obtener los totales:
                decimal dTotalRegulado = 0;
                decimal dTotalLibre = 0;
                decimal dTotalGranUsuario = 0;
                for (int f = 0; f < datos.GetLength(0); f++)
                {   //Por Fila
                    int pingcodi = Convert.ToInt32(datos[f][0]);
                    VtpPeajeIngresoDTO dtoPeajeIngreso;
                    if (bar > 0) { dtoPeajeIngreso = this.servicioTransfPotencia.GetByIdVtpPeajeIngreso(pericodi, recpotcodi, pingcodi); }
                    else { dtoPeajeIngreso = this.servicioTransfPotencia.GetByIdVtpPeajeIngreso(pericodi - 1, 1, pingcodi); }
                    if (dtoPeajeIngreso != null)
                    {

                        dTotalRegulado += UtilTransfPotencia.ValidarNumero(datos[f][6].ToString());
                        dTotalLibre += UtilTransfPotencia.ValidarNumero(datos[f][7].ToString());
                        dTotalGranUsuario += UtilTransfPotencia.ValidarNumero(datos[f][8].ToString());
                    }
                }
                if (dTotalRegulado == 0)
                {
                    dTotalRegulado = 1;
                }
                if (dTotalLibre == 0)
                {
                    dTotalLibre = 1;
                }
                if (dTotalGranUsuario == 0)
                {
                    dTotalGranUsuario = 1;
                }
                //Recorremos la matriz nuevamente para actualizar los registros
                for (int f = 0; f < datos.GetLength(0); f++)
                {   //Por Fila
                    int pingcodi = Convert.ToInt32(UtilTransfPotencia.ValidarNumero(datos[f][0].ToString()));
                    VtpPeajeIngresoDTO dtoPeajeIngreso;
                    if (bar > 0) { dtoPeajeIngreso = this.servicioTransfPotencia.GetByIdVtpPeajeIngreso(pericodi, recpotcodi, pingcodi); }
                    else { dtoPeajeIngreso = this.servicioTransfPotencia.GetByIdVtpPeajeIngreso(pericodi - 1, 1, pingcodi); }
                    if (dtoPeajeIngreso != null)
                    {
                        dtoPeajeIngreso.Pericodi = pericodi;
                        dtoPeajeIngreso.Recpotcodi = recpotcodi;
                        dtoPeajeIngreso.Pingpeajemensual = UtilTransfPotencia.ValidarNumero(datos[f][4].ToString());
                        dtoPeajeIngreso.Pingtarimensual = UtilTransfPotencia.ValidarNumero(datos[f][5].ToString());
                        dtoPeajeIngreso.Pingregulado = UtilTransfPotencia.ValidarNumero(datos[f][6].ToString());
                        dtoPeajeIngreso.Pingporctregulado = UtilTransfPotencia.ValidarNumero(datos[f][6].ToString()) / dTotalRegulado;
                        dtoPeajeIngreso.Pinglibre = UtilTransfPotencia.ValidarNumero(datos[f][7].ToString());
                        dtoPeajeIngreso.Pingporctlibre = UtilTransfPotencia.ValidarNumero(datos[f][7].ToString()) / dTotalLibre;
                        dtoPeajeIngreso.Pinggranusuario = UtilTransfPotencia.ValidarNumero(datos[f][8].ToString());
                        dtoPeajeIngreso.Pingporctgranusuario = UtilTransfPotencia.ValidarNumero(datos[f][8].ToString()) / dTotalGranUsuario;

                        dtoPeajeIngreso.Pingusumodificacion = User.Identity.Name;
                        //Editar registro
                        if(bar > 0)
                        {
                            this.servicioTransfPotencia.UpdateVtpPeajeIngresoDesarrollo(dtoPeajeIngreso);
                        }else this.servicioTransfPotencia.SaveVtpPeajeIngreso(dtoPeajeIngreso);

                    }
                }

                VtpRecalculoPotenciaDTO dtoRecalculoPot;
                dtoRecalculoPot = this.servicioTransfPotencia.GetByIdVtpRecalculoPotencia(pericodi, recpotcodi);
                if(dtoRecalculoPot != null)
                {
                    dtoRecalculoPot.Recpotpreciopeajeppm = dTotalRegulado;
                    this.servicioTransfPotencia.UpdateVtpRecalculoPotencia(dtoRecalculoPot);
                }
                return Json(sResultado);
            }
            catch (Exception e)
            {
                sResultado = e.Message; //"-1"
                return Json(sResultado);
            }
        }

        #endregion

        /// <summary>
        /// Permite cargar un archivo Excel]
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadExcel()
        {
            base.ValidarSesionUsuario();
            string sNombreArchivo = "";
            string path = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString();
            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    sNombreArchivo = file.FileName;
                    if (System.IO.File.Exists(path + sNombreArchivo))
                    {
                        System.IO.File.Delete(path + sNombreArchivo);
                    }
                    file.SaveAs(path + sNombreArchivo);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Lee datos desde el archivo excel pasan a la hoja de calculo en pantalla [NO GRABA LOS DATOS
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivo(string sarchivo, int pericodi = 0, int recpotcodi = 0)
        {
            try
            {
                int bar = this.servicioTransfPotencia.ListVtpPeajeIngresoView(pericodi, recpotcodi).Count();
                base.ValidarSesionUsuario();
                GridExcelModel model = new GridExcelModel();
                string path = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString();
                int iRegError = 0;
                string sMensajeError = "";

                #region Armando de contenido

                string[] headers = { "", "Tipo", "Titular", "Nombre", "Peaje mensual S/", "Ingreso tarifario mensual S/", "Regulado S/ / kW-mes", "Libre S/ / kW-mes", "Gran usuario S/ / kW-mes" };
                int[] widths = { 1, 49, 250, 250, 150, 150, 150, 150, 150 };
                object[] columnas = new object[9];

                //Traemos la primera hoja del archivo
                DataSet ds = new DataSet();
                ds = this.servicioTransfPotencia.GeneraDataset(path + sarchivo, 1);

                string[][] data = new string[ds.Tables[0].Rows.Count - 4][]; //-4 por las primeras filas del encabezado

                int index = 0;
                int iFila = 0;
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 5)
                    {
                        continue;
                    }
                    string sPingnombre = dtRow[3].ToString();
                    if (sPingnombre.Equals(""))
                    {
                        break;
                    }

                    if(bar > 0)
                    {
                        //Buscar el pingcodi con el nombre.
                        VtpPeajeIngresoDTO dtoPeajeIngreso = new VtpPeajeIngresoDTO();
                        dtoPeajeIngreso = this.servicioTransfPotencia.GetByNomIngTarVtpPeajeIngreso(pericodi, recpotcodi, sPingnombre);
                        if (dtoPeajeIngreso == null)
                        {
                            iRegError = 1;
                            sMensajeError = "No existe el nombre de tarifa: " + sPingnombre;

                            break;
                        }
                        string[] itemDato = { dtoPeajeIngreso.Pingcodi.ToString(), dtoPeajeIngreso.Pingtipo, dtoPeajeIngreso.Emprnomb, dtoPeajeIngreso.Pingnombre, dtRow[8].ToString(), dtRow[9].ToString(), dtRow[10].ToString(), dtRow[11].ToString(), dtRow[12].ToString() };
                        data[index] = itemDato;
                    }
                    else
                    {
                        //Buscar el pingcodi con el nombre.
                        VtpPeajeIngresoDTO dtoPeajeIngreso = new VtpPeajeIngresoDTO();
                        dtoPeajeIngreso = this.servicioTransfPotencia.GetByNomIngTarVtpPeajeIngreso(pericodi - 1, 1, sPingnombre);
                        if (dtoPeajeIngreso == null)
                        {
                            iRegError = 1;
                            sMensajeError = "No existe el nombre de tarifa: " + sPingnombre;

                            break;
                        }
                        string[] itemDato = { dtoPeajeIngreso.Pingcodi.ToString(), dtoPeajeIngreso.Pingtipo, dtoPeajeIngreso.Emprnomb, dtoPeajeIngreso.Pingnombre, dtRow[8].ToString(), dtRow[9].ToString(), dtRow[10].ToString(), dtRow[11].ToString(), dtRow[12].ToString() };
                        data[index] = itemDato;
                    }
                    
                    index++;
                }


                columnas[0] = new
                {   //Pingcodi
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    readOnly = true,
                };
                columnas[1] = new
                {   //Pingtipo
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    //strict = false,               //Si el valor verdadero escrito ingresado en la celda debe coincidir con la fuente de autocompletado . De lo contrario celular no será válida.
                    //dateFormat = string.Empty,    //Date validation format. Default 'DD/MM/YYYY'
                    //correctFormat = true,         //Si las fechas verdaderas entonces se formatean automáticamente para que coincida con el formato deseado.
                    //defaultDate = string.Empty,   //Definición del valor por defecto que llenará las celdas vacías .
                    //format = string.Empty,        //Opción deseada para la columna qué tipo 'Numérico ' 
                    readOnly = true
                };
                columnas[2] = new
                {   //Emprnomb
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    readOnly = true
                };
                columnas[3] = new
                {   //Pingnombre
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    readOnly = true
                };
                columnas[4] = new
                {   //Pingpeajemensual
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.0000",
                    readOnly = false
                };
                columnas[5] = new
                {   //Pingtarimensual
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.0000",
                    readOnly = false
                };
                columnas[6] = new
                {   //Pingregulado
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.000000000000",
                    readOnly = false
                };
                columnas[7] = new
                {   //Pinglibre
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.000000000000",
                    readOnly = false
                };
                columnas[8] = new
                {   //Pinggranusuario
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.000000000000",
                    readOnly = false
                };

                #endregion

                model.Data = data;
                model.Headers = headers;
                model.Widths = widths;
                model.Columnas = columnas;
                model.FixedRowsTop = 1;
                model.FixedColumnsLeft = 3;
                model.RegError = iRegError;
                model.MensajeError = sMensajeError;
                return Json(model);
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }
    }
}
