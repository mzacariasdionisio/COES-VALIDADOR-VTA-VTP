using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.MVC.Intranet.Areas.TransfPotencia.Helper;
using COES.MVC.Intranet.Areas.TransfPotencia.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
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
    public class RetiroPotenciaSCController : BaseController
    {
        // GET: /TransfPotencia/RetiroPotenciaSC/
        //[CustomAuthorize]

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        BarraAppServicio servicioBarra = new BarraAppServicio();
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
            RetiroPotenciaSCModel model = new RetiroPotenciaSCModel();
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            if (model.ListaPeriodos.Count > 0 && pericodi == 0)
            { pericodi = model.ListaPeriodos[0].PeriCodi; }
            if (pericodi > 0)
            {
                model.ListaRecalculoPotencia = this.servicioTransfPotencia.ListByPericodiVtpRecalculoPotencia(pericodi); //Ordenado en descendente
                if (model.ListaRecalculoPotencia.Count > 0 && recpotcodi == 0)
                { recpotcodi = (int)model.ListaRecalculoPotencia[0].Recpotcodi; }
            }

            if (pericodi > 0 && recpotcodi > 0)
            {
                model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            }
            else
            {
                model.EntidadRecalculoPotencia = new VtpRecalculoPotenciaDTO();
            }
            model.Pericodi = pericodi;
            model.Recpotcodi = recpotcodi;
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name); 
            return View(model);
        }
        
        #region Grilla Excel

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
            //Definimos la cabecera como una matriz
            string[] Cabecera1 = { "Cliente", "Barra", "Tipo de Usuario", "Precio PPB S/ /kW-mes", "Precio Potencia S/ /kW-mes", "Potencia Egreso kW", "Peaje Unitario S/ /kW-mes", "PARA FLUJO DE CARGA OPTIMO", "", "", "Calidad" };
            string[] Cabecera2 = { "", "", "", "", "", "", "", "Barra", "Potencia Activa kW", "Potencia Reactiva kW", "" };
            
            //Anvho de cada columna
            int[] widths = { 200, 160, 70, 100, 100, 100, 100, 150, 120, 140, 110 };  
            object[] columnas = new object[11];

            //Obtener las empresas para el dropdown
            var ListaEmpresas = this.servicioEmpresa.ListEmpresasSTR().Select(x => x.EmprNombre).ToList();
            //Obtener las barras para el dropdown
            var ListaBarras = this.servicioBarra.ListBarras().Select(x => x.BarrNombre).ToList();
            //Lista de retiros de Potencia Sin Contrato
            List<VtpRetiroPotescDTO> ListaRetiroPotenciaSC = this.servicioTransfPotencia.ListVtpRetiroPotenciaSCView(pericodi, recpotcodi);

            string[][] data = new string[ListaRetiroPotenciaSC.Count + 2][]; // +2: por la cabecera
            data[0] = Cabecera1;
            data[1] = Cabecera2;
            int index = 2;
            foreach (VtpRetiroPotescDTO item in ListaRetiroPotenciaSC)
            {
                string[] itemDato = { item.Emprnomb.ToString(), item.Barrnombre.ToString(), item.Rpsctipousuario, item.Rpscprecioppb.ToString(), item.Rpscpreciopote.ToString(), item.Rpscpoteegreso.ToString(), item.Rpscpeajeunitario.ToString(), item.Barrnombrefco.ToString(), item.Rpscpoteactiva.ToString(), item.Rpscpotereactiva.ToString(), item.Rpsccalidad };
                data[index] = itemDato;
                index++;
            }
            string[] aTipoUsuario = { "Regulado", "Libre", "Gran Usuario" };
            string[] aCalidad = { "Mejor información", "Final", "Preliminar" };
            //En caso la hoja de calculo se va a mostrar vacia
            //string[][] data2 = new string[1][]; 
            //if (index == 0)
            //{   string[] itemDato = { null, null, null, null, null, null, null, null, null, null, null };
            //    data2[0] = itemDato;
            //}

            columnas[0] = new
            {   //Emprcodi - Emprnomb
                type = GridExcelModel.TipoLista,
                source = ListaEmpresas.ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = false,
            };
            columnas[1] = new
            {   //Barrcodi - Barrnombre
                type = GridExcelModel.TipoLista,
                source = ListaBarras.ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = false,
            };
            columnas[2] = new
            {   //Rpsctipousuario
                type = GridExcelModel.TipoLista,
                source = aTipoUsuario,
                strict = false,
                correctFormat = true,
                readOnly = false
            };
            columnas[3] = new
            {   //Rpscprecioppb
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00",
                readOnly = false
            };
            columnas[4] = new
            {   //Rpscpreciopote
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00",
                readOnly = false
            };
            columnas[5] = new
            {   //Rpscpoteegreso
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00",
                readOnly = false
            };
            columnas[6] = new
            {   //Rpscpeajeunitario
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[7] = new
            {   //Barrcodifco - Barrnombrefco
                type = GridExcelModel.TipoLista,
                source = ListaBarras.ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = false,
            };
            columnas[8] = new
            {   //Rpscpoteactiva
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00",
                readOnly = false
            };
            columnas[9] = new
            {   //Rpscpotereactiva
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00",
                readOnly = false
            };
            columnas[10] = new
            {   //Rpsccalidad
                type = GridExcelModel.TipoLista,
                source = aCalidad,
                strict = false,
                correctFormat = true,
                readOnly = false
            };
            
            #endregion
            model.ListaEmpresas = ListaEmpresas.ToArray();
            model.ListaBarras = ListaBarras.ToArray();
            model.ListaTipoUsuario = aTipoUsuario.ToArray();
            model.ListaCalidad = aCalidad.ToArray();

            model.Data = data;
            model.Widths = widths;
            model.Columnas = columnas;
            model.FixedRowsTop = 2;
            model.FixedColumnsLeft = 2;

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
                if (!sBorrar.Equals("1"))
                {
                    return Json(sBorrar);
                }

                //Elimimanos los registros actuales en el periodo año
                this.servicioTransfPotencia.DeleteVtpRetiroPotesc(pericodi, recpotcodi);

                //Recorremos la matriz que se inicia en la fila 2
                for (int f = 2; f < datos.GetLength(0); f++)
                {   //Por Fila
                    if (datos[f][1]==null)
                        break; //FIN

                    //INSERTAR EL REGISTRO
                    VtpRetiroPotescDTO dtoRetiroPoteSC = new VtpRetiroPotescDTO();
                    if (!datos[f][0].Equals(""))
                    {
                        dtoRetiroPoteSC.Emprnomb = Convert.ToString(datos[f][0]);
                        EmpresaDTO dtoEmpresa = this.servicioEmpresa.GetByNombre(dtoRetiroPoteSC.Emprnomb);
                        if (dtoEmpresa != null)
                        { dtoRetiroPoteSC.Emprcodi = dtoEmpresa.EmprCodi; }
                        else
                        { continue;  }
                    }
                    if (!datos[f][1].Equals(""))
                    {
                        dtoRetiroPoteSC.Barrnombre = Convert.ToString(datos[f][1]);
                        BarraDTO dtoBarra = this.servicioBarra.GetByBarra(dtoRetiroPoteSC.Barrnombre);
                        if (dtoBarra != null)
                        { dtoRetiroPoteSC.Barrcodi = dtoBarra.BarrCodi; }
                        else
                        { continue; }
                    }
                    if (!datos[f][2].Equals(""))
                    {
                        string sTipoUsuario = Convert.ToString(datos[f][2]).ToString().ToUpper();
                        if (sTipoUsuario == "REGULADO")
                        { dtoRetiroPoteSC.Rpsctipousuario = "Regulado"; }
                        else if (sTipoUsuario == "LIBRE")
                        { dtoRetiroPoteSC.Rpsctipousuario = "Libre"; }
                        else if (sTipoUsuario == "GRAN USUARIO")
                        { dtoRetiroPoteSC.Rpsctipousuario = "Gran Usuario"; }
                        else
                        {
                            dtoRetiroPoteSC.Rpsctipousuario = "";
                        }
                    }

                    dtoRetiroPoteSC.Rpscprecioppb = UtilTransfPotencia.ValidarNumero(datos[f][3].ToString());
                    dtoRetiroPoteSC.Rpscpreciopote = UtilTransfPotencia.ValidarNumero(datos[f][4].ToString());
                    dtoRetiroPoteSC.Rpscpoteegreso = UtilTransfPotencia.ValidarNumero(datos[f][5].ToString());
                    dtoRetiroPoteSC.Rpscpeajeunitario = UtilTransfPotencia.ValidarNumero(datos[f][6].ToString());
                    if (!datos[f][7].Equals(""))
                    {
                        dtoRetiroPoteSC.Barrnombrefco = Convert.ToString(datos[f][7]);
                        BarraDTO dtoBarrafco = this.servicioBarra.GetByBarra(dtoRetiroPoteSC.Barrnombrefco);
                        if (dtoBarrafco != null)
                        { dtoRetiroPoteSC.Barrcodifco = dtoBarrafco.BarrCodi; }
                        else
                        { continue; }
                    }
                    dtoRetiroPoteSC.Rpscpoteactiva = UtilTransfPotencia.ValidarNumero(datos[f][8].ToString());
                    dtoRetiroPoteSC.Rpscpotereactiva = UtilTransfPotencia.ValidarNumero(datos[f][9].ToString());
                    if (!datos[f][10].Equals(""))
                    { 
                        string sCalidad = Convert.ToString(datos[f][10]).ToString().ToUpper();
                        if (sCalidad == "MEJOR INFORMACIÓN")
                        { dtoRetiroPoteSC.Rpsccalidad = "Mejor información"; }
                        else if (sCalidad == "FINAL")
                        { dtoRetiroPoteSC.Rpsccalidad = "Final"; }
                        else if (sCalidad == "PRELIMINAR")
                        { dtoRetiroPoteSC.Rpsccalidad = "Preliminar"; }
                    }

                    dtoRetiroPoteSC.Pericodi = pericodi;
                    dtoRetiroPoteSC.Recpotcodi = recpotcodi;
                    dtoRetiroPoteSC.Rpscusucreacion = User.Identity.Name;
                    dtoRetiroPoteSC.Rpscusumodificacion = User.Identity.Name;
                    //Insertar registro
                    this.servicioTransfPotencia.SaveVtpRetiroPotesc(dtoRetiroPoteSC);
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
        /// Permite eliminar todos los registros del periodo y versión de recalculo
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarDatos(int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";
            try
            {
                this.servicioTransfPotencia.DeleteVtpRetiroPotesc(pericodi, recpotcodi);
            }
            catch (Exception e)
            {
                sResultado = e.Message; //"-1";
            }

            return Json(sResultado);

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
                string file = this.servicioTransfPotencia.GenerarFormatoRetiroPotenciaSC(pericodi, recpotcodi, formato, pathFile, pathLogo);
                return Json(file);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Descarga el archivo 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString() + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;

            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, app, sFecha + "_" + file);
        }

        /// <summary>
        /// Permite cargar un archivo Excel
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
        /// Lee datos desde el archivo excel
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivo(string sarchivo, int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            string path = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString();
            string sResultado = "1";
            int iRegError = 0;
            string sMensajeError = "";

            try
            {
                #region Armando de contenido
                //Definimos la cabecera como una matriz
                string[] Cabecera1 = { "Cliente", "Barra", "Tipo de Usuario", "Precio PPB S/ /kW-mes", "Precio Potencia S/ /kW-mes", "Potencia Egreso kW", "Peaje Unitario S/ /kW-mes", "PARA FLUJO DE CARGA OPTIMO", "", "", "Calidad" };
                string[] Cabecera2 = { "", "", "", "", "", "", "", "Barra", "Potencia Activa kW", "Potencia Reactiva kW", "" };

                //Anvho de cada columna
                int[] widths = { 200, 160, 70, 100, 100, 100, 100, 150, 120, 140, 110 };
                object[] columnas = new object[11];

                //Obtener las empresas para el dropdown
                var ListaEmpresas = this.servicioEmpresa.ListEmpresasSTR().Select(x => x.EmprNombre).ToList();
                //Obtener las barras para el dropdown
                var ListaBarras = this.servicioBarra.ListBarras().Select(x => x.BarrNombre).ToList();
               
                //Traemos la primera hoja del archivo
                DataSet ds = new DataSet();
                ds = this.servicioTransfPotencia.GeneraDataset(path + sarchivo, 1);

                string[][] data = new string[ds.Tables[0].Rows.Count - 3][]; // -5 por las primeras filas del encabezado
                data[0] = Cabecera1;
                data[1] = Cabecera2;
                int index = 2;
                int iFila = 0;
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 6)
                    {
                        continue;
                    }
                    int iNumFila = iFila + 1;
                    //VtpRetiroPotescDTO dtoRetiroPotenciaSC = new VtpRetiroPotescDTO();
                    string sCliente = dtRow[1].ToString();
                    EmpresaDTO dtoCliente = this.servicioEmpresa.GetByNombre(sCliente);
                    if (dtoCliente == null)
                    {
                        sMensajeError += "<br>Fila:" + iNumFila + " - No existe: " + sCliente;
                        iRegError++;
                        continue;
                    }
                    string sBarra = dtRow[2].ToString();
                    BarraDTO dtoBarra = this.servicioBarra.GetByBarra(sBarra);
                    if (dtoBarra == null)
                    {
                        sMensajeError += "<br>Fila:" + iNumFila + " - No existe: " + sBarra;
                        iRegError++;
                        continue;
                    }
                    string sBarraFCO = dtRow[8].ToString();
                    BarraDTO dtoBarraFCO = this.servicioBarra.GetByBarra(sBarraFCO);
                    if (dtoBarraFCO == null)
                    {
                        sMensajeError += "<br>Fila:" + iNumFila + " - No existe: " + sBarraFCO;
                        iRegError++;
                        continue;
                    }
                    string[] itemDato = { sCliente, sBarra, dtRow[3].ToString(), dtRow[4].ToString(), dtRow[5].ToString(), dtRow[6].ToString(), dtRow[7].ToString(), sBarraFCO, dtRow[9].ToString(), dtRow[10].ToString(), "Mejor información" };
                    data[index] = itemDato;
                    index++;
                    
                }
                string[] aTipoUsuario = { "Regulado", "Libre", "Gran Usuario" };
                string[] aCalidad = { "Mejor información", "Final", "Preliminar" };
                columnas[0] = new
                {   //Emprcodi - Emprnomb
                    type = GridExcelModel.TipoLista,
                    source = ListaEmpresas.ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = false,
                };
                columnas[1] = new
                {   //Barrcodi - Barrnombre
                    type = GridExcelModel.TipoLista,
                    source = ListaBarras.ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = false,
                };
                columnas[2] = new
                {   //Rpsctipousuario
                    type = GridExcelModel.TipoLista,
                    source = aTipoUsuario,
                    strict = false,
                    correctFormat = true,
                    readOnly = false
                };
                columnas[3] = new
                {   //Rpscprecioppb
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = false
                };
                columnas[4] = new
                {   //Rpscpreciopote
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = false
                };
                columnas[5] = new
                {   //Rpscpoteegreso
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = false
                };
                columnas[6] = new
                {   //Rpscpeajeunitario
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.000",
                    readOnly = false
                };
                columnas[7] = new
                {   //Barrcodifco - Barrnombrefco
                    type = GridExcelModel.TipoLista,
                    source = ListaBarras.ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = false,
                };
                columnas[8] = new
                {   //Rpscpoteactiva
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = false
                };
                columnas[9] = new
                {   //Rpscpotereactiva
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = false
                };
                columnas[10] = new
                {   //Rpsccalidad
                    type = GridExcelModel.TipoLista,
                    source = aCalidad,
                    strict = false,
                    correctFormat = true,
                    readOnly = false
                };

                #endregion
                model.ListaEmpresas = ListaEmpresas.ToArray();
                model.ListaBarras = ListaBarras.ToArray();
                model.ListaTipoUsuario = aTipoUsuario.ToArray();
                model.ListaCalidad = aCalidad.ToArray();

                model.Data = data;
                model.Widths = widths;
                model.Columnas = columnas;
                model.FixedRowsTop = 2;
                model.FixedColumnsLeft = 2;
                model.RegError = iRegError;
                model.MensajeError = sMensajeError;

                return Json(model);
            }
            catch (Exception e)
            {
                sResultado = e.Message;
                return Json(sResultado);
            }

        }
        
    }
}
