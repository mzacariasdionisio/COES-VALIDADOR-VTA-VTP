using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia;
using COES.MVC.Intranet.Areas.TransfPotencia.Models;
using COES.MVC.Intranet.Areas.TransfPotencia.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Configuration;
using System.Globalization;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Controllers;
using System.Data;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Controllers
{
    public class IngresoPotefrDetalleController : BaseController
    {
        // GET: /Transfpotencia/IngresoPotefrDetalleController/
        //[CustomAuthorize]

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        CentralGeneracionAppServicio servicioCentralGeneracion = new CentralGeneracionAppServicio();
        ConstantesTransfPotencia libFuncion = new ConstantesTransfPotencia();

        /// <summary>
        /// Muestra la pantalla inicial
        /// </summary>
        /// <param name="ipefrcodi">Código del Ingreso de Potencia Efectiva, Firme y Firme Remuneravle</param>
        /// <param name="pericodi">Código de la Versión de Recálculo de Potencia</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        public ActionResult Index(int ipefrcodi = 0, int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();
            IngresoPotefrDetalleModel model = new IngresoPotefrDetalleModel();
            model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            model.Ipefrcodi = ipefrcodi;
            model.Pericodi = pericodi;
            model.Recpotcodi = recpotcodi;
            var IngresoPoteFR = this.servicioTransfPotencia.GetByIdVtpIngresoPotefr(ipefrcodi);
            model.Intervalo = (int)IngresoPoteFR.Ipefrintervalo;
            model.ListaIngresoPotefrDetalle = this.servicioTransfPotencia.GetByCriteriaVtpIngresoPotefrDetalles(ipefrcodi, pericodi, recpotcodi);
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name);
            return View(model);
        }

        #region Grilla Excel

        /// <summary>
        /// Muestra la grilla excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcel(int ipefrcodi = 0, int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            IngresoPotefrDetalleModel modelpfd = new IngresoPotefrDetalleModel();

            modelpfd.Ipefrcodi = ipefrcodi;
            modelpfd.Pericodi = pericodi;
            modelpfd.Recpotcodi = recpotcodi;

            #region Armando de contenido

            //headers y tamaños de las columnas
            List<string> header = new List<string>() { "EMPRESA", "CENTRAL/UNIDAD GENERACIÓN", "POT. EFECTIVA kW", "POT. FIRME kW", "POT. FIRME REMUNERABLE kW" };
            List<int> width = new List<int>() { 400, 400, 150, 150, 200 };

            string[] headers = header.ToArray(); //Headers final a enviar
            int[] widths = width.ToArray(); //widths final a enviar
            int total = header.Count(); //Obtener cantidad de columnas
            object[] columnas = new object[5];

            //Obtener empresas para el dropdown
            List<string> ListaEmpresas = this.servicioEmpresa.ListEmpresas().Select(x => x.EmprNombre.Trim()).ToList();
            List<string> ListaCentralGeneracion = this.servicioCentralGeneracion.ListCentralGeneracion().Select(x => x.CentGeneNombre).ToList();

            //Obtener lista de datos de Potencia Efectiva, Firme y Firme Remunerable
            modelpfd.ListaIngresoPotefrDetalle = this.servicioTransfPotencia.GetByCriteriaVtpIngresoPotefrDetalles(modelpfd.Ipefrcodi, modelpfd.Pericodi, modelpfd.Recpotcodi);

            //Se arma la matriz de datos
            string[][] data;
            int index = 0;
            if (modelpfd.ListaIngresoPotefrDetalle.Count() > 0)
            {
                data = new string[modelpfd.ListaIngresoPotefrDetalle.Count()][];
                foreach (VtpIngresoPotefrDetalleDTO item in modelpfd.ListaIngresoPotefrDetalle)
                {
                    string[] itemDato = { item.Emprnomb, item.Cenequinomb, item.Ipefrdpoteefectiva.ToString(), item.Ipefrdpotefirme.ToString(), item.Ipefrdpotefirmeremunerable.ToString() };
                    data[index] = itemDato;
                    index++;
                }
            }
            else
            {
                data = new string[1][];
                string[] itemDato = { "", "", "0", "0", "0" };
                data[index] = itemDato;
            }
            /////////////////ARMANDO COLUMNASSSSSSSSSSSSSSSSSSSSSS!!
            columnas[0] = new
            {   //Emprnomb
                type = GridExcelModel.TipoLista,
                source = ListaEmpresas.ToArray(),
                strict = false,
                dateFormat = string.Empty,
                correctFormat = false,
                defaultDate = string.Empty,
                format = string.Empty,
                readOnly = false,

            };
            columnas[1] = new
            {   //Cenequinomb
                type = GridExcelModel.TipoLista,
                source = ListaCentralGeneracion.ToArray(),
                strict = false,
                dateFormat = string.Empty,
                correctFormat = true,
                defaultDate = string.Empty,
                format = string.Empty,
                readOnly = false,

            };
            columnas[2] = new
            {   //Ipefrdpoteefectiva
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.0000000000",
                readOnly = false
            };
            columnas[3] = new
            {   //Ipefrdpotefirme
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.0000000000",
                readOnly = false
            };
            columnas[4] = new
            {   //Ipefrdpotefirmeremunerable
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.0000000000",
                readOnly = false
            };

            #endregion
            model.ListaCentralGeneracion = ListaCentralGeneracion.ToArray();
            model.ListaEmpresas = ListaEmpresas.ToArray();
            model.Data = data;
            model.Headers = headers;
            model.Widths = widths;
            model.Columnas = columnas;

            return Json(model);
        }

        /// <summary>
        /// Permite grabar los datos del excel
        /// </summary>
        /// <param name="baseDirectory"></param>
        /// <param name="url"></param>
        /// <param name="datos"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarGrillaExcel(int ipefrcodi, int pericodi, int recpotcodi, string[][] datos)
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

                IngresoPotefrDetalleModel model = new IngresoPotefrDetalleModel();
                model.Ipefrcodi = ipefrcodi;
                model.Pericodi = pericodi;
                model.Recpotcodi = recpotcodi;

                //Elimnina datos antes de grabar
                this.servicioTransfPotencia.DeleteByCriteriaVtpIngresoPotefrDetalle(model.Ipefrcodi, model.Pericodi, model.Recpotcodi);

                //Obtener cantidad de filas y columnas de la matriz
                int col = datos[0].Length;
                int row = datos.Length - 1;

                //Loop para recorrer matriz y grabar datos
                for (int i = 0; i < row; i++)
                {
                    string Emprnomb = "";
                    string Centralnomb = "";
                    string UnidadNomb = "";
                    model.Entidad = new VtpIngresoPotefrDetalleDTO();
                    //Empresa
                    Emprnomb = Convert.ToString(datos[i][0]);
                    var empr = this.servicioEmpresa.GetByNombre(Emprnomb);
                    if (empr != null)
                    {
                        model.Entidad.Emprcodi = empr.EmprCodi;
                    }
                    else
                    { continue; }

                    //Obtiene Codigo de Empresa de la columna par de izquierda a derecha
                    Centralnomb = Convert.ToString(datos[i][1]);
                    var central = this.servicioCentralGeneracion.GetByCentGeneNomb(Centralnomb);
                    if (central != null)
                    {
                        model.Entidad.Cenequicodi = central.CentGeneCodi;
                    }
                    else
                    { continue; }

                    model.Entidad.Ipefrdpoteefectiva = UtilTransfPotencia.ValidarNumero(datos[i][2].ToString());
                    model.Entidad.Ipefrdpotefirme = UtilTransfPotencia.ValidarNumero(datos[i][3].ToString());
                    model.Entidad.Ipefrdpotefirmeremunerable = UtilTransfPotencia.ValidarNumero(datos[i][4].ToString());

                    //Crear registro                                          
                    model.Entidad.Ipefrcodi = model.Ipefrcodi;
                    model.Entidad.Pericodi = model.Pericodi;
                    model.Entidad.Recpotcodi = model.Recpotcodi;
                    model.Entidad.Ipefrdusucreacion = User.Identity.Name;
                    model.Entidad.Ipefrdusumodificacion = User.Identity.Name;

                    this.servicioTransfPotencia.SaveVtpIngresoPotefrDetalle(model.Entidad);
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

        public JsonResult EliminarDatos(int ipefrcodi = 0, int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";
            try
            {
                this.servicioTransfPotencia.DeleteByCriteriaVtpIngresoPotefrDetalle(ipefrcodi, pericodi, recpotcodi);
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
        /// <param name="ipefrcodi">Código del Ingreso de Potencia Efectiva, Firme y Firme Remuneravle</param>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarData(int ipefrcodi = 0, int pericodi = 0, int recpotcodi = 0, int intervalo = 0, int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString();
                string file = this.servicioTransfPotencia.GenerarFormatoIngresoPotefrDetalle(ipefrcodi, pericodi, recpotcodi, intervalo, formato, pathFile, pathLogo, false);
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

            return File(path, app, sFecha + "_" + file);
        }

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
        public JsonResult ProcesarArchivo(string sarchivo, int ipefrcodi = 0, int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            string path = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString();
            int iRegError = 0;
            string sMensajeError = "";

            #region Armando de contenido

            List<string> header = new List<string>() { "EMPRESA", "CENTRAL/UNIDAD GENERACIÓN", "POT. EFECTIVA kW", "POT. FIRME kW", "POT. FIRME REMUNERABLE kW" };
            List<int> width = new List<int>() { 400, 400, 150, 150, 200 };
            string[] headers = header.ToArray(); //Headers final a enviar
            int[] widths = width.ToArray(); //widths final a enviar
            int total = header.Count(); //Obtener cantidad de columnas
            object[] columnas = new object[5];

            //Obtener empresas para el dropdown
            List<string> ListaEmpresas = this.servicioEmpresa.ListEmpresas().Select(x => x.EmprNombre.Trim()).ToList();
            List<string> ListaCentralGeneracion = this.servicioCentralGeneracion.ListCentralGeneracion().Select(x => x.CentGeneNombre).ToList();

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
                int iNumFila = iFila + 1;
                string sEmprnomb = dtRow[1].ToString();
                EmpresaDTO dtoEmpresa = this.servicioEmpresa.GetByNombre(sEmprnomb);
                if (dtoEmpresa == null)
                {
                    sMensajeError += "<br>Fila:" + iNumFila + " - No existe: " + sEmprnomb;
                    iRegError++;
                    continue;
                }
                string sCenequinomb = dtRow[2].ToString();
                CentralGeneracionDTO dtoCentralGeneracion = this.servicioCentralGeneracion.GetByCentGeneNomb(sCenequinomb);
                if (dtoCentralGeneracion == null)
                {
                    sMensajeError += "<br>Fila:" + iNumFila + " - No existe: " + sCenequinomb;
                    iRegError++;
                    continue;
                }
                string[] itemDato = { sEmprnomb, sCenequinomb, dtRow[3].ToString(), dtRow[4].ToString(), dtRow[5].ToString() };
                data[index] = itemDato;
                index++;
            }

            columnas[0] = new
            {   //Emprnomb
                type = GridExcelModel.TipoLista,
                source = ListaEmpresas.ToArray(),
                strict = false,
                dateFormat = string.Empty,
                correctFormat = false,
                defaultDate = string.Empty,
                format = string.Empty,
                readOnly = false,

            };
            columnas[1] = new
            {   //Cenequinomb
                type = GridExcelModel.TipoLista,
                source = ListaCentralGeneracion.ToArray(),
                strict = false,
                dateFormat = string.Empty,
                correctFormat = true,
                defaultDate = string.Empty,
                format = string.Empty,
                readOnly = false,

            };
            columnas[2] = new
            {   //Ipefrdpoteefectiva
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00",
                readOnly = false
            };
            columnas[3] = new
            {   //Ipefrdpotefirme
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00",
                readOnly = false
            };
            columnas[4] = new
            {   //Ipefrdpotefirmeremunerable
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00",
                readOnly = false
            };

            #endregion
            model.ListaCentralGeneracion = ListaCentralGeneracion.ToArray();
            model.ListaEmpresas = ListaEmpresas.ToArray();
            model.Data = data;
            model.Headers = headers;
            model.Widths = widths;
            model.Columnas = columnas;
            model.RegError = iRegError;
            model.MensajeError = sMensajeError;

            return Json(model);
        }
    }
}
