// using COES.Dominio.DTO.Sic;
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
using DevExpress.CodeParser;
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
    public class DemandaController : BaseController
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

        public DemandaController()
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
        /// <param name="cpatdtipo"></param>
        /// <param name="cpatdmes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcel(int cpatdanio = 0, string cpatdajuste = "", int cparcodi = 0, string cpatdtipo = "", int cpatdmes = 0, string cpatdnomtipo = "", string cpatdnommes = "")
        {
            base.ValidarSesionUsuario();

            GridExcelModel model = new GridExcelModel();
            model.Visible = -1;
            model.sMensaje = string.Empty;

            CpaTotalDemandaModel modelDemanda = new CpaTotalDemandaModel();
            modelDemanda.Cpatdanio = cpatdanio;
            modelDemanda.Cpatdajuste = cpatdajuste;
            modelDemanda.Cparcodi = cparcodi;
            modelDemanda.Cpatdtipo = cpatdtipo;
            modelDemanda.Cpatdmes = cpatdmes;

            #region Armando de contenido
            // headers y tamaños de las columnas
            List<string> header0 = new List<string>() { cpatdanio + "-" + cpatdajuste + "-" + cpatdnomtipo, "Energia", "", "Potencia", "" };
            List<string> header1 = new List<string>() { cpatdnommes, "MWh", "Miles S./", "MW", "Miles S./" };
            List<int> width = new List<int>() { 400, 150, 150, 150, 150 };

            string[] headers = header1.ToArray(); // Headers final a enviar
            int[] widths = width.ToArray(); // widths final a enviar
            int total = header1.Count(); // Obtener cantidad de columnas
            object[] columnas = new object[5];

            // Obtener el estado de la revision seleccionada
            model.EstadoRevision = this.ServicioCppaApp.ObtenerEstadoRevisionDemanda(cparcodi);
            if (model.EstadoRevision == ConstantesCPPA.Cerrado || model.EstadoRevision == ConstantesCPPA.Anulado)
            {
                model.sMensaje += "La Revisión del Ajuste presupuestal está cerrada o anulada por lo que no es posible registrar información.";
                model.Visible = 0;
            }

            // Verificar si el “Cálculo de porcentaje de Presupuesto” ya ha sido ejecutado
            if (this.ServicioCppaApp.ObtenerNroRegistrosCPPEJDemanda(cparcodi) > 0)
            {
                model.sMensaje += "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;El Cálculo de porcentaje de Presupuesto ya ha sido ejecutado, debe eliminarse para actualizar nueva información.";
                model.Visible = 0;
            }

            // Selecciono filtros e Indica el boton Consultar y primero verifica si existen envios en la tabla
            if (this.ServicioCppaApp.ObtenerNroRegistrosEnvios() > 0) // Existen envios
            {
                if (this.ServicioCppaApp.ObtenerNroRegistroEnviosFiltros(modelDemanda.Cparcodi, modelDemanda.Cpatdtipo, modelDemanda.Cpatdmes) > 0)
                {
                    // Trae el ultimo envio para mostrarlo por defecto
                    modelDemanda.ListaTotalDemandaDetalle = this.ServicioCppaApp.ObtenerUltimoEnvio(modelDemanda.Cparcodi, 
                                                                                                    modelDemanda.Cpatdtipo, 
                                                                                                    modelDemanda.Cpatdmes);

                    if (modelDemanda.ListaTotalDemandaDetalle.Count > 0)
                    {
                        model.Codigo = modelDemanda.ListaTotalDemandaDetalle[0].Cpatdcodi;
                        model.FechaRegistro = modelDemanda.ListaTotalDemandaDetalle[0].Cpatddfeccreacion.ToString("dd/MM/yyyy");
                        model.sMensaje += "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Código de envío " + model.Codigo + " Fecha de Envío " + model.FechaRegistro;
                        model.Visible = 1;
                    }
                    else
                    {
                        // Trae envio vacio
                        modelDemanda.ListaTotalDemandaDetalle = this.ServicioCppaApp.EnvioVacio(modelDemanda.Cparcodi,
                                                                                                modelDemanda.Cpatdtipo,
                                                                                                modelDemanda.Cpatdmes);
                        if (modelDemanda.ListaTotalDemandaDetalle.Count > 0)
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
                    modelDemanda.ListaTotalDemandaDetalle = this.ServicioCppaApp.EnvioVacio(modelDemanda.Cparcodi,
                                                                                            modelDemanda.Cpatdtipo,
                                                                                            modelDemanda.Cpatdmes);

                    if (modelDemanda.ListaTotalDemandaDetalle.Count > 0)
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
            else // No existen envios
            {
                // Trae envio vacio
                modelDemanda.ListaTotalDemandaDetalle = this.ServicioCppaApp.EnvioVacio(modelDemanda.Cparcodi,
                                                                                        modelDemanda.Cpatdtipo,
                                                                                        modelDemanda.Cpatdmes);

                model.sMensaje += "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;No existen registros de envios en la Base de datos.";
                model.Visible = 2;
            }

            // Se arma la matriz de datos
            string[][] data;
            if (modelDemanda.ListaTotalDemandaDetalle != null)
            {
                data = new string[modelDemanda.ListaTotalDemandaDetalle.Count() + 2][];

                // Asigna la cabeceras a la matriz
                data[0] = header0.ToArray();
                data[1] = header1.ToArray();

                // Asigna la data de la lista a la matriz
                int index = 2;
                foreach (CpaTotalDemandaDetDTO item in modelDemanda.ListaTotalDemandaDetalle)
                {
                    string[] itemDato = {
                                            item.Emprnomb.ToString(),
                                            item.Cpatddtotenemwh.ToString(),
                                            item.Cpatddtotenesoles.ToString(),
                                            item.Cpatddtotpotmw.ToString(),
                                            item.Cpatddtotpotsoles.ToString(),
                                            item.Cpatdcodi.ToString(),
                                            item.Cpatddfeccreacion.ToString()
                                        };

                    data[index] = itemDato;
                    index++;
                }
            }
            else
            {
                int index = 0;
                data = new string[1][];
                string[] itemDato = { "", "", "", "", "" };
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
                // Cpatotaldemenemwh
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00000000",
                readOnly = false
            };
            columnas[2] = new
            {   
                // Cpatotaldemeneso
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
                // Cpatotaldempotmw
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00000000",
                readOnly = false
            };
            columnas[4] = new
            {   
                // Cpatotaldempotso
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
        public JsonResult GrabarGrillaExcel(int Anio, string IdAjuste, int IdRevision, string IdTipoParticipacion, int Mes, string[][] datos)
        {
            base.ValidarSesionUsuario();

            string sResultado = "1";
            if (IdRevision == 0 || IdTipoParticipacion == "" || Mes == 0)
            {
                sResultado = "Lo sentimos, debe seleccionar un mes de valorización, el tipo de participación y el mes";
                return Json(sResultado);
            }

            try
            {
                // Grabamos la cabecera
                CpaTotalDemandaModel model = new CpaTotalDemandaModel();
                model.Anio = Anio;
                model.Ajuste = IdAjuste;
                model.IdRevision = IdRevision;
                model.IdTipoParticipacion = IdTipoParticipacion;
                model.Mes = Mes;
                model.UsuCreacion = User.Identity.Name;
                model.FecCreacion = DateTime.Now;

                int idCabecera = this.ServicioCppaApp.SaveCpaTotalDemanda(model.Anio,
                                                                          model.Ajuste,
                                                                          model.IdRevision,
                                                                          model.IdTipoParticipacion,
                                                                          model.Mes,
                                                                          model.UsuCreacion,
                                                                          model.FecCreacion);
                if (idCabecera > 0)
                {
                    // Grabamos el detalle em base a los datos de la grilla Excel
                    // Obtener cantidad de filas y columnas de la matriz
                    int col = datos[0].Length;
                    int row = datos.Length;

                    // Loop para recorrer matriz y grabar datos
                    for (int i = 2; i < row; i++)
                    {
                        // Buscar Id de Empresa
                        string Emprnomb = "";
                        Emprnomb = Convert.ToString(datos[i][0]);
                        var empr = this.servicioEmpresa.GetByNombre(Emprnomb);

                        // Crear registro
                        model.EntidadTotalDemandaDetalle = new CpaTotalDemandaDetDTO();
                        model.EntidadTotalDemandaDetalle.Cpatdcodi = idCabecera;

                        model.EntidadTotalDemandaDetalle.Emprcodi = empr.EmprCodi;

                        model.EntidadTotalDemandaDetalle.Cpatddtotenemwh = UtilCPPA.ValidarNumeroDecimalNull(datos[i][1].ToString());
                        model.EntidadTotalDemandaDetalle.Cpatddtotenesoles = UtilCPPA.ValidarNumeroDecimalNull(datos[i][2].ToString());
                        model.EntidadTotalDemandaDetalle.Cpatddtotpotmw = UtilCPPA.ValidarNumeroDecimalNull(datos[i][3].ToString());
                        model.EntidadTotalDemandaDetalle.Cpatddtotpotsoles = UtilCPPA.ValidarNumeroDecimalNull(datos[i][4].ToString());
                        
                        model.EntidadTotalDemandaDetalle.Cpatddusucreacion = User.Identity.Name;
                        model.EntidadTotalDemandaDetalle.Cpatddfeccreacion = DateTime.Now;

                        this.ServicioCppaApp.SaveCpaTotalDemandaDet(model.EntidadTotalDemandaDetalle);
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
        /// <param name="Anio">Año del calculo total de transmisores detalle</param>
        /// <param name="IdAjuste">Código de ajuste del total de transmisores detalle</param>
        /// <param name="IdRevision">Código de la revision del total de transmisores detalle</param>
        /// <param name="Tipo">Tipo</param>
        /// <param name="Mes">Mes</param>
        /// <param name="NombRevision">Nombre de la Versión de Recálculo de Potencia</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarData(int Anio, string IdAjuste, int IdRevision, int Mes, string IdTipoParticipacion, string NombTipo = "", string NombMes = "", string NombRevision = "", int Formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; // RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteDemanda].ToString();
                string file = this.ServicioCppaApp.GenerarFormatoTotalDemandaDetalle(Anio, 
                                                                                     IdAjuste, 
                                                                                     IdRevision, 
                                                                                     IdTipoParticipacion, 
                                                                                     Mes,
                                                                                     NombTipo,
                                                                                     NombMes,
                                                                                     NombRevision,
                                                                                     Formato,
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
            string path = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteDemanda].ToString() + file;
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
            string path = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteDemanda].ToString();
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
        public JsonResult ProcesarArchivo(string sarchivo, int cpatdanio = 0, string cpatdajuste = "", int cparcodi = 0, string cpatdtipo = "", int cpatdmes = 0, string cpatdnomtipo = "", string cpatdnommes = "")
        {
            base.ValidarSesionUsuario();

            GridExcelModel model = new GridExcelModel();
            model.Visible = -1;
            model.sMensaje = string.Empty;

            CpaTotalDemandaModel modelDemanda = new CpaTotalDemandaModel();
            modelDemanda.Cpatdanio = cpatdanio;
            modelDemanda.Cpatdajuste = cpatdajuste;
            modelDemanda.Cparcodi = cparcodi;
            modelDemanda.Cpatdtipo = cpatdtipo;
            modelDemanda.Cpatdmes = cpatdmes;

            string path = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteDemanda].ToString();
            int iRegError = 0;
            string sMensajeError = "";

            #region Armando de contenido
            List<string> header0 = new List<string>() { cpatdanio + "-" + cpatdajuste + "-" + cpatdnomtipo, "Energia", "", "Potencia", "" };
            List<string> header1 = new List<string>() { cpatdnommes, "MWh", "Miles S./", "MW", "Miles S./" };
            List<int> width = new List<int>() { 400, 150, 150, 150, 150 };

            string[] headers = header1.ToArray(); // Headers final a enviar
            int[] widths = width.ToArray(); // widths final a enviar
            int total = header1.Count(); // Obtener cantidad de columnas
            object[] columnas = new object[5];

            // Obtener la matriz original de la grilla para apoyo en la validaciones
            // Trae el ultimo envio para mostrarlo por defecto
            modelDemanda.ListaTotalDemandaDetalle = this.ServicioCppaApp.ObtenerUltimoEnvio(modelDemanda.Cparcodi,
                                                                                            modelDemanda.Cpatdtipo,
                                                                                            modelDemanda.Cpatdmes);

            // Obtener el estado de la revision seleccionada
            model.EstadoRevision = this.ServicioCppaApp.ObtenerEstadoRevisionDemanda(cparcodi);
            if (model.EstadoRevision == ConstantesCPPA.Cerrado || model.EstadoRevision == ConstantesCPPA.Anulado)
            {
                sMensajeError = "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;La Revisión del Ajuste presupuestal está cerrada o anulada por lo que no es posible registrar información. Por favor volver a ejecutar la opción consultar.";
                model.Visible = 0;
                model.RegError = 1;
                model.sMensaje = sMensajeError;

                return Json(model);
            }

            // Verificar si el “Cálculo de porcentaje de Presupuesto” ya ha sido ejecutado
            if (this.ServicioCppaApp.ObtenerNroRegistrosCPPEJDemanda(cparcodi) > 0)
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

            string[][] data = new string[ds.Tables[0].Rows.Count - 3][]; // -3 por las primeras filas del encabezado

            // Asigna la cabeceras a la matriz
            data[0] = header0.ToArray();
            data[1] = header1.ToArray();

            int index = 2;
            int iFila = 0;
            int contEmpr = 0;
            foreach (DataRow dtRow in ds.Tables[0].Rows)
            {
                iFila++;
                if (iFila < 6)
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

                // Valida si la empresa del Excel importado es del tipo seleccionado (D=Distribuidor o U=Usuario Libre)
                string sTipoEmpr = this.ServicioCppaApp.ObtenerTipoEmpresaCPADemandaPorNombre(modelDemanda.Cparcodi,
                                                                                              modelDemanda.Cpatdtipo, 
                                                                                              sEmprnomb);
                if (sTipoEmpr != cpatdtipo)
                {
                    sMensajeError = "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Fila: " + iNumFila + " La empresa: " + sEmprnomb + " - No es del Tipo: " + (cpatdtipo == "D" ? "Distribuidor": "Usuario Libre") + ". Por favor volver a ejecutar la opción consultar.";
                    iRegError++;
                    model.RegError = iRegError;
                    model.sMensaje = sMensajeError;

                    return Json(model);
                }


                string[] itemDato = { 
                                        sEmprnomb, 
                                        (dtRow[2].ToString() != "null" ? dtRow[2].ToString() : ""), 
                                        (dtRow[3].ToString() != "null" ? dtRow[3].ToString() : ""), 
                                        (dtRow[4].ToString() != "null" ? dtRow[4].ToString() : ""), 
                                        (dtRow[5].ToString() != "null" ? dtRow[5].ToString() : "")
                                    };
                data[index] = itemDato;
                index++;
            }

            // Valida si existen elementos adicionales en el contenido entre la matriz de la grilla y el Excel importado
            if (modelDemanda.ListaTotalDemandaDetalle.Count > contEmpr)
            {
                sMensajeError = "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;el formato que se intenta cargar no corresponde con el formato descargado. Por favor volver a ejecutar la opción consultar.";
                iRegError++;
                model.RegError = iRegError;
                model.sMensaje = sMensajeError;

                return Json(model);
            }

            // Valida si se elimino elemento en el contenido entre la matriz de la grilla y el Excel importado
            if (modelDemanda.ListaTotalDemandaDetalle.Count < contEmpr)
            {
                sMensajeError = "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;el formato que se intenta cargar no corresponde con el formato descargado. Por favor volver a ejecutar la opción consultar.";
                iRegError++;
                model.RegError = iRegError;
                model.sMensaje = sMensajeError;

                return Json(model);
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
                // Cpatotaldemenemwh
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00000000",
                readOnly = false
            };
            columnas[2] = new
            {   
                // Cpatotaldemeneso
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
                // Cpatotaldempotmw
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00000000",
                readOnly = false
            };
            columnas[4] = new
            {   
                // Cpatotaldempotso
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

            model.FixedRowsTop = 1;
            model.FixedColumnsLeft = 1;

            return Json(model);
        }

        /// <summary>
        /// Trae los envios registrados
        /// </summary>
        /// <param name="cparcodi">Revisión</param>
        /// <param name="cpatdtipo">Tipo</param>
        /// <param name="cpatdmes">Mes</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarEnvios(int cparcodi = 0, string cpatdtipo = "", int cpatdmes = 0)
        {
            CpaTotalDemandaModel model = new CpaTotalDemandaModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.HtmlList = this.ServicioCppaApp.ReporteHtmlEnviosDemandas(cparcodi, cpatdtipo, cpatdmes);
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
        public JsonResult CargarGrillaExcel(int cpatdcodi = 0, int cpatdanio = 0, string cpatdajuste = "", int cparcodi = 0, string cpatdtipo = "", int cpatdmes = 0, string cpatdnomtipo = "", string cpatdnommes = "")
        {
            base.ValidarSesionUsuario();

            GridExcelModel model = new GridExcelModel();
            model.Visible = -1;
            model.sMensaje = string.Empty;

            CpaTotalDemandaModel modelDemanda = new CpaTotalDemandaModel();

            modelDemanda.IdCabecera = cpatdcodi;

            #region Armando de contenido
            // headers y tamaños de las columnas
            List<string> header0 = new List<string>() { cpatdanio + "-" + cpatdajuste + "-" + cpatdnomtipo, "Energia", "", "Potencia", "" };
            List<string> header1 = new List<string>() { cpatdnommes, "MWh", "Miles S./", "MW", "Miles S./" };
            List<int> width = new List<int>() { 400, 150, 150, 150, 150 };

            string[] headers = header1.ToArray(); // Headers final a enviar
            int[] widths = width.ToArray(); // widths final a enviar
            int total = header1.Count(); // Obtener cantidad de columnas
            object[] columnas = new object[5];

            // Obtener el estado de la revision seleccionada
            model.EstadoRevision = this.ServicioCppaApp.ObtenerEstadoRevisionDemanda(cparcodi);
            if (model.EstadoRevision == ConstantesCPPA.Cerrado || model.EstadoRevision == ConstantesCPPA.Anulado)
            {
                model.sMensaje += "<br>La Revisión del Ajuste presupuestal está cerrada o anulada por lo que no es posible registrar información";
                model.Visible = 0;
            }

            // Verificar si el “Cálculo de porcentaje de Presupuesto” ya ha sido ejecutado
            if (this.ServicioCppaApp.ObtenerNroRegistrosCPPEJDemanda(cparcodi) > 0)
            {
                model.sMensaje += "<br>El Cálculo de porcentaje de Presupuesto ya ha sido ejecutado, debe eliminarse para actualizar nueva información.";
                model.Visible = 0;
            }

            // Obtener lista de datos de Energia y Potencia Efectiva
            modelDemanda.ListaTotalDemandaDetalle = this.ServicioCppaApp.GetByIdDemanda(modelDemanda.IdCabecera);

            // Se arma la matriz de datos
            string[][] data;
            if (modelDemanda.ListaTotalDemandaDetalle.Count() > 0)
            {
                data = new string[modelDemanda.ListaTotalDemandaDetalle.Count() + 2][];

                // Asigna la cabeceras a la matriz
                data[0] = header0.ToArray();
                data[1] = header1.ToArray();

                // Asigna la data de la lista a la matriz
                int index = 2;
                foreach (CpaTotalDemandaDetDTO item in modelDemanda.ListaTotalDemandaDetalle)
                {
                    string[] itemDato = {
                                            item.Emprnomb.ToString(),
                                            item.Cpatddtotenemwh.ToString(),
                                            item.Cpatddtotenesoles.ToString(),
                                            item.Cpatddtotpotmw.ToString(),
                                            item.Cpatddtotpotsoles.ToString() 
                                        };
                    data[index] = itemDato;
                    index++;

                    model.Visible = 1;
                }
            }
            else
            {
                int index = 0;
                data = new string[1][];
                string[] itemDato = { "", "", "", "", "" };
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
                // Cpatotaldemenemwh
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00000000",
                readOnly = false
            };
            columnas[2] = new
            {   
                // Cpatotaldemeneso
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
                // Cpatotaldempotmw
                type = GridExcelModel.TipoNumerico,
                source = (new List<Double>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00000000",
                readOnly = false
            };
            columnas[4] = new
            {   
                // Cpatotaldempotso
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
            model.FixedRowsTop = 1;
            model.FixedColumnsLeft = 1;

            return Json(model);
        }

    }
}
