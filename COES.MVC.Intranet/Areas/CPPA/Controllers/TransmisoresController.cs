using COES.Dominio.DTO.Transferencias;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.CPPA.Models;
using COES.MVC.Intranet.Areas.TransfPotencia.Helper;
using COES.MVC.Intranet.Areas.YupanaContinuo.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.CPPA;
using COES.Servicios.Aplicacion.CPPA.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia;
using log4net;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Web.Mvc;
using static COES.Servicios.Aplicacion.Migraciones.Helper.ConstantesMigraciones;

namespace COES.MVC.Intranet.Areas.CPPA.Controllers
{
    public class TransmisoresController : BaseController
    {
        // GET: CPPA/TransmisoresController/

        #region Declaración de variables
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        private readonly CPPAAppServicio ServicioCppaApp = new CPPAAppServicio();

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }

        public TransmisoresController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        CentralGeneracionAppServicio servicioCentralGeneracion = new CentralGeneracionAppServicio();
        ConstantesTransfPotencia libFuncion = new ConstantesTransfPotencia();
        #endregion

        /// <summary>
        /// Pantalla inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaAnio = ServicioCppaApp.ObtenerAnios(out List<CpaRevisionDTO> ListRevision);
                ViewBag.ListRevision = Newtonsoft.Json.JsonConvert.SerializeObject(ListRevision);
                model.sResultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }

            return View(model);
        }

        #region Grilla Excel
        /// <summary>
        /// Muestra la grilla excel
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcel(int cpatdanio = 0, string cpatdajuste = "", int cparcodi = 0)
        {
            base.ValidarSesionUsuario();

            GridExcelModel model = new GridExcelModel();
            model.Visible = -1;
            model.sMensaje = string.Empty;

            CpaTotalTransmisoresModel modelTransmisores = new CpaTotalTransmisoresModel();
            modelTransmisores.Cpatdanio = cpatdanio;
            modelTransmisores.Cpatdajuste = cpatdajuste;
            modelTransmisores.Cparcodi = cparcodi;

            #region Armando de contenido
            // headers y tamaños de las columnas
            List<string> header0 = new List<string>() { "EMPRESA", "Ene-" + (cpatdanio - 1).ToString().Substring(2, 2), "Feb-" + (cpatdanio - 1).ToString().Substring(2, 2), "Mar-" + (cpatdanio - 1).ToString().Substring(2, 2), "Abr-" + (cpatdanio - 1).ToString().Substring(2, 2), "May-" + (cpatdanio - 1).ToString().Substring(2, 2), "Jun-" + (cpatdanio - 1).ToString().Substring(2, 2), "Jul-" + (cpatdanio - 1).ToString().Substring(2, 2), "Ago-" + (cpatdanio - 1).ToString().Substring(2, 2), "Sep-" + (cpatdanio - 1).ToString().Substring(2, 2), "Oct-" + (cpatdanio - 1).ToString().Substring(2, 2), "Nov-" + (cpatdanio - 1).ToString().Substring(2, 2), "Dic-" + (cpatdanio - 1).ToString().Substring(2, 2) };
            List<int> width = new List<int>() { 400, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150 };

            string[] headers = header0.ToArray(); // Headers final a enviar
            int[] widths = width.ToArray(); // widths final a enviar
            int total = header0.Count(); // Obtener cantidad de columnas
            object[] columnas = new object[13];

            // Obtener el estado de la revision seleccionada
            model.EstadoRevision = this.ServicioCppaApp.ObtenerEstadoRevisionTransmisores(cparcodi);
            if (model.EstadoRevision == ConstantesCPPA.Cerrado || model.EstadoRevision == ConstantesCPPA.Anulado)
            {
                model.sMensaje += "La Revisión del Ajuste presupuestal está cerrada o anulada por lo que no es posible registrar información";
                model.Visible = 0;
            }

            // Verificar si el “Cálculo de porcentaje de Presupuesto” ya ha sido ejecutado
            if (this.ServicioCppaApp.ObtenerNroRegistrosCPPEJTransmisores(cparcodi) > 0)
            {
                model.sMensaje += "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;El Cálculo de porcentaje de Presupuesto ya ha sido ejecutado, debe eliminarse para actualizar nueva información.";
                model.Visible = 0;
            }

            // Selecciono filtros e Indica el boton Consultar y primero verifica si existen envios en la tabla
            if (this.ServicioCppaApp.ObtenerNroRegistrosEnviosTransmisores() > 0)
            {
                if (this.ServicioCppaApp.ObtenerNroRegistroEnviosFiltrosTransmisores(modelTransmisores.Cparcodi) > 0)
                {
                    // Trae el ultimo envio para mostrarlo por defecto
                    modelTransmisores.ListaTotalTransmisoresDetalle = this.ServicioCppaApp.ObtenerUltimoEnvioTransmisores(modelTransmisores.Cparcodi);

                    if (modelTransmisores.ListaTotalTransmisoresDetalle.Count > 0)
                    {
                        model.Codigo = modelTransmisores.ListaTotalTransmisoresDetalle[0].Cpattcodi;
                        model.FechaRegistro = modelTransmisores.ListaTotalTransmisoresDetalle[0].Cpattdfeccreacion.ToString("dd/MM/yyyy");
                        model.sMensaje += "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Código de envío " + model.Codigo + " Fecha de Envío " + model.FechaRegistro;
                        model.Visible = 1;
                    }
                    else
                    {
                        // Trae envio vacio
                        modelTransmisores.ListaTotalTransmisoresDetalle = this.ServicioCppaApp.EnvioVacioTransmisores(modelTransmisores.Cparcodi);

                        if (modelTransmisores.ListaTotalTransmisoresDetalle.Count > 0)
                        {
                            model.sMensaje += "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Aun no existen registros de envio, correspondientes a los filtros seleccionados. Por favor registre los datos para este envio";
                            model.Visible = 3;
                        }
                        else
                        {
                            model.sMensaje += "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Aun no ha registrado parametros de ajuste del año presupuestal, correspondientes a los filtros seleccionados.";
                            model.Visible = 4;
                        }
                    }
                }
                else
                {
                    // Trae envio vacio
                    modelTransmisores.ListaTotalTransmisoresDetalle = this.ServicioCppaApp.EnvioVacioTransmisores(modelTransmisores.Cparcodi);

                    if (modelTransmisores.ListaTotalTransmisoresDetalle.Count > 0)
                    {
                        model.Codigo = null;
                        model.FechaRegistro = null;
                        model.Visible = 5;
                        model.sMensaje += "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Aun no existen registros de envio, correspondientes a los filtros seleccionados. Por favor registre los datos para este envio";
                    }
                    else
                    {
                        model.Codigo = null;
                        model.FechaRegistro = null;
                        model.Visible = 0;
                        model.sMensaje += "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Aun no ha registrado parametros de ajuste del año presupuestal, correspondientes a los filtros seleccionados.";
                    }
                }
            }
            else  // No existen envios
            {
                // Obtener el envio vacio
                modelTransmisores.ListaTotalTransmisoresDetalle = this.ServicioCppaApp.EnvioVacioTransmisores(modelTransmisores.Cparcodi);
                model.sMensaje += "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;No existen registros de envios en la Base de datos..";
                model.Visible = 2;
            }


            // Se arma la matriz de datos
            string[][] data;
            if (modelTransmisores.ListaTotalTransmisoresDetalle != null)
            {
                data = new string[modelTransmisores.ListaTotalTransmisoresDetalle.Count() + 1][];

                // Asigna la cabeceras a la matriz
                data[0] = header0.ToArray();

                // Asigna la data de la lista a la matriz
                int index = 1;
                foreach (CpaTotalTransmisoresDetDTO item in modelTransmisores.ListaTotalTransmisoresDetalle)
                {
                    string[] itemDato = { item.Emprnomb,
                                          item.Cpattdtotmes01.ToString(),
                                          item.Cpattdtotmes02.ToString(),
                                          item.Cpattdtotmes03.ToString(),
                                          item.Cpattdtotmes04.ToString(),
                                          item.Cpattdtotmes05.ToString(),
                                          item.Cpattdtotmes06.ToString(),
                                          item.Cpattdtotmes07.ToString(),
                                          item.Cpattdtotmes08.ToString(),
                                          item.Cpattdtotmes09.ToString(),
                                          item.Cpattdtotmes10.ToString(),
                                          item.Cpattdtotmes11.ToString(),
                                          item.Cpattdtotmes12.ToString(),
                                          item.Cpattcodi.ToString(),
                                          item.Cpattdfeccreacion.ToString()
                                       };
                    data[index] = itemDato;
                    index++;
                }
            }
            else
            {
                int index = 0;
                data = new string[1][];
                string[] itemDato = { "", "", "", "", "", "", "", "", "", "", "", "", "" };
                data[index] = itemDato;
            }

            // ARMANDO COLUMNAS
            columnas[0] = new
            {   
                // Emprnomb
                type = GridExcelModel.TipoTexto,
                source = (new List<string>()).ToArray(),
                strict = false,
                dateFormat = string.Empty,
                correctFormat = false,
                defaultDate = string.Empty,
                format = string.Empty,
                readOnly = false
            };
            columnas[1] = new
            {   
                // Cpattdtotmes01
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[2] = new
            {   
                // Cpattdtotmes02
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[3] = new
            {   
                // Cpattdtotmes03
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[4] = new
            {   
                // Cpattdtotmes04
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[5] = new
            {   
                // Cpattdtotmes05
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[6] = new
            {   
                // Cpattdtotmes06
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[7] = new
            {   
                // Cpattdtotmes07
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[8] = new
            {   
                // Cpattdtotmes08
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[9] = new
            {   
                // Cpattdtotmes09
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[10] = new
            {   
                // Cpattdtotmes10
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[11] = new
            {   
                // Cpattdtotmes11
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[12] = new
            {   
                // Cpattdtotmes12
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            #endregion
            
            model.Data = data;
            model.Headers = headers;
            model.Widths = widths;
            model.Columnas = columnas;
            model.Anio = (cpatdanio - 1);
            model.FixedRowsTop = 1;
            model.FixedColumnsLeft = 1;

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
        public JsonResult GrabarGrillaExcel(int Anio, string IdAjuste, int IdRevision, string[][] datos)
        {
            base.ValidarSesionUsuario();

            string sResultado = "1";
            if (Anio == 0 || IdAjuste == "" || IdRevision == 0)
            {
                sResultado = "Lo sentimos, debe seleccionar la revisión";
                return Json(sResultado);
            }

            try
            {
                // Grabamos la cabecera
                CpaTotalTransmisoresModel model = new CpaTotalTransmisoresModel();
                model.Anio = Anio;
                model.Ajuste = IdAjuste;
                model.IdRevision = IdRevision;
                model.UsuCreacion = User.Identity.Name;
                model.FecCreacion = DateTime.Now;

                int idCabecera = this.ServicioCppaApp.SaveCpaTotalTransmisores(model.Anio, 
                                                                               model.Ajuste, 
                                                                               model.IdRevision,
                                                                               model.UsuCreacion,
                                                                               model.FecCreacion);
                if (idCabecera > 0)
                {
                    // Grabamos el detalle em base a los datos de la grilla Excel
                    // Obtener cantidad de filas y columnas de la matriz
                    int col = datos[0].Length;
                    int row = datos.Length;

                    // Loop para recorrer matriz y grabar datos
                    for (int i = 1; i < row; i++)
                    {
                        // Buscar Id de Empresa
                        string Emprnomb = "";
                        Emprnomb = Convert.ToString(datos[i][0]);
                        var empr = this.servicioEmpresa.GetByNombre(Emprnomb);

                        // Crear registro
                        model.EntidadTotalTransmisoresDetalle = new CpaTotalTransmisoresDetDTO();
                        model.EntidadTotalTransmisoresDetalle.Cpattcodi = idCabecera;

                        model.EntidadTotalTransmisoresDetalle.Emprcodi = empr.EmprCodi;

                        model.EntidadTotalTransmisoresDetalle.Cpattdtotmes01 = UtilCPPA.ValidarNumeroDecimalNull(datos[i][1].ToString());
                        model.EntidadTotalTransmisoresDetalle.Cpattdtotmes02 = UtilCPPA.ValidarNumeroDecimalNull(datos[i][2].ToString());
                        model.EntidadTotalTransmisoresDetalle.Cpattdtotmes03 = UtilCPPA.ValidarNumeroDecimalNull(datos[i][3].ToString());
                        model.EntidadTotalTransmisoresDetalle.Cpattdtotmes04 = UtilCPPA.ValidarNumeroDecimalNull(datos[i][4].ToString());
                        model.EntidadTotalTransmisoresDetalle.Cpattdtotmes05 = UtilCPPA.ValidarNumeroDecimalNull(datos[i][5].ToString());
                        model.EntidadTotalTransmisoresDetalle.Cpattdtotmes06 = UtilCPPA.ValidarNumeroDecimalNull(datos[i][6].ToString());
                        model.EntidadTotalTransmisoresDetalle.Cpattdtotmes07 = UtilCPPA.ValidarNumeroDecimalNull(datos[i][7].ToString());
                        model.EntidadTotalTransmisoresDetalle.Cpattdtotmes08 = UtilCPPA.ValidarNumeroDecimalNull(datos[i][8].ToString());
                        model.EntidadTotalTransmisoresDetalle.Cpattdtotmes09 = UtilCPPA.ValidarNumeroDecimalNull(datos[i][9].ToString());
                        model.EntidadTotalTransmisoresDetalle.Cpattdtotmes10 = UtilCPPA.ValidarNumeroDecimalNull(datos[i][10].ToString());
                        model.EntidadTotalTransmisoresDetalle.Cpattdtotmes11 = UtilCPPA.ValidarNumeroDecimalNull(datos[i][11].ToString());
                        model.EntidadTotalTransmisoresDetalle.Cpattdtotmes12 = UtilCPPA.ValidarNumeroDecimalNull(datos[i][12].ToString());
                        
                        model.EntidadTotalTransmisoresDetalle.Cpattdusucreacion = User.Identity.Name;
                        model.EntidadTotalTransmisoresDetalle.Cpattdfeccreacion = DateTime.Now;

                        this.ServicioCppaApp.SaveCpaTotalTransmisoresDet(model.EntidadTotalTransmisoresDetalle);
                    }
                }

                return Json(sResultado);
            }
            catch (Exception e)
            {
                sResultado = e.Message; // "-1"
                return Json(sResultado);
            }
        }
        #endregion

        /// <summary>
        /// Permite exportar a un archivo todos los registros en pantalla
        /// </summary>
        /// <param name="Anio">Código del Ingreso de Potencia Efectiva, Firme y Firme Remuneravle</param>
        /// <param name="IdAjuste">Código del Mes de valorización</param>
        /// <param name="IdRevision">Código de la Versión de Recálculo de Potencia</param>
        /// <param name="NombRevision">Nombre de la Versión de Recálculo de Potencia</param>
        /// <param name="formato">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarData(int Anio, string IdAjuste, int IdRevision, string NombRevision, int formato = 1)
        {
            base.ValidarSesionUsuario();

            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; // RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteTransmisores].ToString();
                string file = this.ServicioCppaApp.GenerarFormatoTotalTransmisoresDetalle(Anio, 
                                                                                          IdAjuste, 
                                                                                          IdRevision,
                                                                                          NombRevision,
                                                                                          formato, 
                                                                                          pathFile, 
                                                                                          pathLogo);

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
            string mes = DateTime.Now.ToString("MM");
            string path = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteTransmisores].ToString() + file;
            string app = (formato == 1) ? ConstantesCPPA.AppExcel : (formato == 2) ? ConstantesCPPA.AppPdf : ConstantesCPPA.AppWord;

            return File(path, app, file);
        }

        /// <summary>
        /// Permite cargar un archivo Excel
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadExcel()
        {
            base.ValidarSesionUsuario();

            string sNombreArchivo = "";
            string path = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteTransmisores].ToString();
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
        public JsonResult ProcesarArchivo(string sarchivo, int cpatdanio = 0, string cpatdajuste = "", int cparcodi = 0)
        {
            base.ValidarSesionUsuario();

            GridExcelModel model = new GridExcelModel();
            model.Visible = -1;
            model.sMensaje = string.Empty;

            CpaTotalTransmisoresModel modelTransmisores = new CpaTotalTransmisoresModel();
            modelTransmisores.Cpatdanio = cpatdanio;
            modelTransmisores.Cpatdajuste = cpatdajuste;
            modelTransmisores.Cparcodi = cparcodi;

            string path = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteTransmisores].ToString();
            int iRegError = 0;
            string sMensajeError = "";

            #region Armando de contenido
            List<string> header0 = new List<string>() { "EMPRESA", "Ene-" + (cpatdanio - 1).ToString().Substring(2, 2), "Feb-" + (cpatdanio - 1).ToString().Substring(2, 2), "Mar-" + (cpatdanio - 1).ToString().Substring(2, 2), "Abr-" + (cpatdanio - 1).ToString().Substring(2, 2), "May-" + (cpatdanio - 1).ToString().Substring(2, 2), "Jun-" + (cpatdanio - 1).ToString().Substring(2, 2), "Jul-" + (cpatdanio - 1).ToString().Substring(2, 2), "Ago-" + (cpatdanio - 1).ToString().Substring(2, 2), "Sep-" + (cpatdanio - 1).ToString().Substring(2, 2), "Oct-" + (cpatdanio - 1).ToString().Substring(2, 2), "Nov-" + (cpatdanio - 1).ToString().Substring(2, 2), "Dic-" + (cpatdanio - 1).ToString().Substring(2, 2) };
            List<int> width = new List<int>() { 400, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150 };

            string[] headers = header0.ToArray(); // Headers final a enviar
            int[] widths = width.ToArray(); // widths final a enviar
            int total = header0.Count(); // Obtener cantidad de columnas
            object[] columnas = new object[13];

            // Obtener la matriz original de la grilla para apoyo en la validaciones
            // Trae el ultimo envio para mostrarlo por defecto
            modelTransmisores.ListaTotalTransmisoresDetalle = this.ServicioCppaApp.ObtenerUltimoEnvioTransmisores(modelTransmisores.Cparcodi);

            // Obtener el estado de la revision seleccionada
            model.EstadoRevision = this.ServicioCppaApp.ObtenerEstadoRevisionTransmisores(cparcodi);
            if (model.EstadoRevision == ConstantesCPPA.Cerrado || model.EstadoRevision == ConstantesCPPA.Anulado)
            {
                sMensajeError = "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;La Revisión del Ajuste presupuestal está cerrada o anulada por lo que no es posible registrar información. Por favor volver a ejecutar la opción consultar.";
                model.Visible = 0;
                model.RegError = 1;
                model.sMensaje = sMensajeError;

                return Json(model);
            }

            // Verificar si el “Cálculo de porcentaje de Presupuesto” ya ha sido ejecutado
            if (this.ServicioCppaApp.ObtenerNroRegistrosCPPEJTransmisores(cparcodi) > 0)
            {
                sMensajeError = "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;El Cálculo de porcentaje de Presupuesto ya ha sido ejecutado, debe eliminarse para actualizar nueva información. Por favor volver a ejecutar la opción consultar.";
                model.Visible = 0;
                model.RegError = 1;
                model.sMensaje = sMensajeError;

                return Json(model);
            }

            // Traemos la primera hoja del archivo
            DataSet ds = new DataSet();
            ds = this.servicioTransfPotencia.GeneraDataset(path + sarchivo, 1);

            string[][] data = new string[ds.Tables[0].Rows.Count - 3][]; //-3 por las primeras filas del encabezado

            // Asigna la cabeceras a la matriz
            data[0] = header0.ToArray();

            int index = 1;
            int iFila = 0;
            int contEmpr = 0;
            foreach (DataRow dtRow in ds.Tables[0].Rows)
            {
                iFila++;
                if (iFila < 5)
                {
                    continue;
                }

                int iNumFila = iFila + 1;

                // Valida si la empresa del Excel importado existe
                string sEmprnomb = dtRow[1].ToString();
                EmpresaDTO dtoEmpresa = this.servicioEmpresa.GetByNombre(sEmprnomb);
                if (dtoEmpresa == null)
                {
                    sMensajeError = "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Fila: " + iNumFila + " - No existe la empresa: " + sEmprnomb + ". Por favor volver a ejecutar la opción consultar.";
                    iRegError++;
                    model.RegError = iRegError;
                    model.sMensaje = sMensajeError;

                    return Json(model);
                }
                contEmpr++;

                // Valida si la empresa del Excel importado es del tipo Transmisor
                string sTipoEmpr = this.ServicioCppaApp.ObtenerTipoEmpresaCPATransmisoresNombre(modelTransmisores.Cparcodi, 
                                                                                                sEmprnomb);
                if (sTipoEmpr != "T")
                {
                    sMensajeError = "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Fila: " + iNumFila + " La empresa: " + sEmprnomb + " - No es del Tipo Transmisor. Por favor volver a ejecutar la opción consultar.";
                    iRegError++;
                    continue;
                }


                string[] itemDato = { sEmprnomb,
                                      (dtRow[2].ToString() != "null" ? dtRow[2].ToString() : ""),
                                      (dtRow[3].ToString() != "null" ? dtRow[3].ToString() : ""),
                                      (dtRow[4].ToString() != "null" ? dtRow[4].ToString() : ""),
                                      (dtRow[5].ToString() != "null" ? dtRow[5].ToString() : ""),
                                      (dtRow[6].ToString() != "null" ? dtRow[6].ToString() : ""),
                                      (dtRow[7].ToString() != "null" ? dtRow[7].ToString() : ""),
                                      (dtRow[8].ToString() != "null" ? dtRow[8].ToString() : ""),
                                      (dtRow[9].ToString() != "null" ? dtRow[9].ToString() : ""),
                                      (dtRow[10].ToString() != "null" ? dtRow[10].ToString() : ""),
                                      (dtRow[11].ToString() != "null" ? dtRow[11].ToString() : ""),
                                      (dtRow[12].ToString() != "null" ? dtRow[12].ToString() : ""),
                                      (dtRow[13].ToString() != "null" ? dtRow[13].ToString() : "")
                                    };
                data[index] = itemDato;
                index++;
            }

            // Valida si existen elementos adicionales en el contenido entre la matriz de la grilla y el Excel importado
            if (modelTransmisores.ListaTotalTransmisoresDetalle.Count > contEmpr)
            {
                sMensajeError = "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;el formato que se intenta cargar no corresponde con el formato descargado porque se existe un elemento mas con respeccto al listado original. Por favor volver a ejecutar la opción consultar.";
                iRegError++;
                model.RegError = iRegError;
                model.sMensaje = sMensajeError;

                return Json(model);
            }

            // Valida si se elimino elemento en el contenido entre la matriz de la grilla y el Excel importado
            if (modelTransmisores.ListaTotalTransmisoresDetalle.Count < contEmpr)
            {
                sMensajeError = "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;el formato que se intenta cargar no corresponde con el formato descargado porque se existe un elemento menos con respeccto al listado original. Por favor volver a ejecutar la opción consultar.";
                iRegError++;
                model.RegError = iRegError;
                model.sMensaje = sMensajeError;

                return Json(model);
            }

            columnas[0] = new
            {
                // Emprnomb
                type = GridExcelModel.TipoTexto,
                source = (new List<string>()).ToArray(),
                strict = false,
                dateFormat = string.Empty,
                correctFormat = false,
                defaultDate = string.Empty,
                format = string.Empty,
                readOnly = false
            };
            columnas[1] = new
            {   
                // Cpattdtotmes01
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[2] = new
            {   
                // Cpattdtotmes02
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[3] = new
            {   
                // Cpattdtotmes03
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[4] = new
            {   
                // Cpattdtotmes04
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[5] = new
            {   
                // Cpattdtotmes05
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[6] = new
            {   
                // Cpattdtotmes06
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[7] = new
            {   
                // Cpattdtotmes07
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[8] = new
            {   
                // Cpattdtotmes08
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[9] = new
            {   
                // Cpattdtotmes09
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[10] = new
            {   
                // Cpattdtotmes10
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[11] = new
            {   
                // Cpattdtotmes11
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[12] = new
            {   
                // Cpattdtotmes12
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            #endregion

            model.Data = data;
            model.Headers = headers;
            model.Widths = widths;
            model.Columnas = columnas;
            model.RegError = iRegError;
            model.sMensaje = sMensajeError;
            model.Anio = (cpatdanio - 1);
            model.FixedRowsTop = 1;
            model.FixedColumnsLeft = 1;

            return Json(model);
        }

        /// <summary>
        /// Trae los envios registrados
        /// </summary>
        /// <param name="cparcodi">Revisión</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarEnvios(int cparcodi = 0)
        {
            CpaTotalDemandaModel model = new CpaTotalDemandaModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.HtmlList = this.ServicioCppaApp.ReporteHtmlEnviosTransmisores(cparcodi);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";

                model.Mensaje = ex.Message;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// Muestra la grilla excel
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarGrillaExcel(int cpattcodi = 0, int cpatdanio = 0, string cpatdajuste = "", int cparcodi = 0)
        {
            base.ValidarSesionUsuario();

            GridExcelModel model = new GridExcelModel();
            model.Visible = -1;
            model.sMensaje = string.Empty;

            CpaTotalTransmisoresModel modelTransmisores = new CpaTotalTransmisoresModel();

            modelTransmisores.Cpattcodi = cpattcodi;

            #region Armando de contenido
            // headers y tamaños de las columnas
            List<string> header0 = new List<string>() { "EMPRESA", "Ene-" + (cpatdanio - 1).ToString().Substring(2, 2), "Feb-" + (cpatdanio - 1).ToString().Substring(2, 2), "Mar-" + (cpatdanio - 1).ToString().Substring(2, 2), "Abr-" + (cpatdanio - 1).ToString().Substring(2, 2), "May-" + (cpatdanio - 1).ToString().Substring(2, 2), "Jun-" + (cpatdanio - 1).ToString().Substring(2, 2), "Jul-" + (cpatdanio - 1).ToString().Substring(2, 2), "Ago-" + (cpatdanio - 1).ToString().Substring(2, 2), "Sep-" + (cpatdanio - 1).ToString().Substring(2, 2), "Oct-" + (cpatdanio - 1).ToString().Substring(2, 2), "Nov-" + (cpatdanio - 1).ToString().Substring(2, 2), "Dic-" + (cpatdanio - 1).ToString().Substring(2, 2) };
            List<int> width = new List<int>() { 400, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150 };

            string[] headers = header0.ToArray(); // Headers final a enviar
            int[] widths = width.ToArray(); // widths final a enviar
            int total = header0.Count(); // Obtener cantidad de columnas
            object[] columnas = new object[13];

            // Obtener el estado de la revision seleccionada
            model.EstadoRevision = this.ServicioCppaApp.ObtenerEstadoRevisionTransmisores(cparcodi);
            if (model.EstadoRevision == ConstantesCPPA.Cerrado || model.EstadoRevision == ConstantesCPPA.Anulado)
            {
                model.sMensaje += "La Revisión del Ajuste presupuestal está cerrada o anulada por lo que no es posible registrar información";
                model.Visible = 0;
            }

            // Verificar si el “Cálculo de porcentaje de Presupuesto” ya ha sido ejecutado
            if (this.ServicioCppaApp.ObtenerNroRegistrosCPPEJTransmisores(cparcodi) > 0)
            {
                model.sMensaje += "<br>El Cálculo de porcentaje de Presupuesto ya ha sido ejecutado, debe eliminarse para actualizar nueva información.";
                model.Visible = 0;
            }

            // Obtener lista de datos de Energia y Potencia Efectiva
            modelTransmisores.ListaTotalTransmisoresDetalle = this.ServicioCppaApp.GetByIdTransmisores(modelTransmisores.Cpattcodi);

            // Se arma la matriz de datos
            string[][] data;
            if (modelTransmisores.ListaTotalTransmisoresDetalle.Count() > 0)
            {
                data = new string[modelTransmisores.ListaTotalTransmisoresDetalle.Count() + 1][];

                // Asigna la cabeceras a la matriz
                data[0] = header0.ToArray();

                // Asigna la data de la lista a la matriz
                int index = 1;
                foreach (CpaTotalTransmisoresDetDTO item in modelTransmisores.ListaTotalTransmisoresDetalle)
                {
                    string[] itemDato = { item.Emprnomb,
                                          item.Cpattdtotmes01.ToString(),
                                          item.Cpattdtotmes02.ToString(),
                                          item.Cpattdtotmes03.ToString(),
                                          item.Cpattdtotmes04.ToString(),
                                          item.Cpattdtotmes05.ToString(),
                                          item.Cpattdtotmes06.ToString(),
                                          item.Cpattdtotmes07.ToString(),
                                          item.Cpattdtotmes08.ToString(),
                                          item.Cpattdtotmes09.ToString(),
                                          item.Cpattdtotmes10.ToString(),
                                          item.Cpattdtotmes11.ToString(),
                                          item.Cpattdtotmes12.ToString() };
                    data[index] = itemDato;
                    index++;

                    model.Visible = 1;
                }
            }
            else
            {
                int index = 0;
                data = new string[1][];
                string[] itemDato = { "", "", "", "", "", "", "", "", "", "", "", "", "" };
                data[index] = itemDato;

                model.Visible = 0;
            }

            // ARMANDO COLUMNAS
            columnas[0] = new
            {
                // Emprnomb
                type = GridExcelModel.TipoTexto,
                source = (new List<string>()).ToArray(),
                strict = false,
                dateFormat = string.Empty,
                correctFormat = false,
                defaultDate = string.Empty,
                format = string.Empty,
                readOnly = false
            };
            columnas[1] = new
            {   
                // Cpattdtotmes01
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[2] = new
            {   
                // Cpattdtotmes02
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[3] = new
            {   
                // Cpattdtotmes03
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[4] = new
            {   
                // Cpattdtotmes04
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[5] = new
            {   
                // Cpattdtotmes05
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[6] = new
            {   
                // Cpattdtotmes06
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[7] = new
            {   
                // Cpattdtotmes07
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[8] = new
            {   
                // Cpattdtotmes08
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[9] = new
            {   
                // Cpattdtotmes09
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[10] = new
            {   
                // Cpattdtotmes10
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[11] = new
            {   
                // Cpattdtotmes11
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            columnas[12] = new
            {   
                // Cpattdtotmes12
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.000",
                readOnly = false
            };
            #endregion

            model.Data = data;
            model.Headers = headers;
            model.Widths = widths;
            model.Columnas = columnas;
            model.Anio = (cpatdanio - 1);
            model.FixedRowsTop = 1;
            model.FixedColumnsLeft = 1;

            return Json(model);
        }

    }
}