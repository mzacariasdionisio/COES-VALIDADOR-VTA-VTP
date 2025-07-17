using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Siosein.Helper;
using COES.MVC.Intranet.Areas.Siosein.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.SIOSEIN;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Siosein.Controllers
{
    public class InformesMensualController : BaseController
    {
        private readonly SIOSEINAppServicio servicio = new SIOSEINAppServicio();
        private readonly PR5ReportesAppServicio _servicioPR5 = new PR5ReportesAppServicio();

        #region Declaracion de variables de Sesión

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Excepciones ocurridas en el controlador
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NameController, ex);
                throw;
            }
        }

        #endregion

        #region Menu Informe Mensual

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuInformeMensual(string mes)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            SioseinModel model = new SioseinModel();

            DateTime fechaConsulta = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);
            if (!string.IsNullOrEmpty(mes)) fechaConsulta = DateTime.ParseExact(mes, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

            model.MesActual = fechaConsulta.ToString(ConstantesAppServicio.FormatoMes);

            return View(model);
        }

        /// <summary>
        /// lista de menu para informe mensual
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarMenu()
        {
            SioseinModel model = new SioseinModel();

            List<SiMenureporteDTO> listaItems = _servicioPR5.GetListaAdmReporte(ConstantesSioSein.ReptipcodiMensual);
            model.Menu = UtilAnexoAPR5.ListaMenuHtml(listaItems);

            return Json(model);
        }

        #endregion

        #region Util

        /// <summary>
        /// Determinar si la sesion es válida, si se selecciono fecha para reporte de Anexo A
        /// </summary>
        /// <returns></returns>
        public bool EsOpcionValida()
        {
            return base.IsValidSesionView();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult RedireccionarOpcionValida()
        {
            if (!base.IsValidSesionView())
            {
                return base.RedirectToLogin();
            }
            else
            {
                return RedirectToAction("MenuInformeMensual", "Siosein/InformesMensual", new { area = string.Empty });
            }
        }

        /// <summary>
        /// Setear valores para cada index
        /// </summary>
        /// <param name="model"></param>
        /// <param name="repcodi"></param>
        private SioseinModel GetModelGenericoIndex(int repcodi, int verscodi)
        {
            SioseinModel model = new SioseinModel();

            //version seleccionada
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(verscodi);
            List<SiVersionDTO> listaVersion = servicio.ListaVersionByFecha(objVersion.Versfechaperiodo, ConstantesPR5ReportesServicio.ReptipcodiInformeMensual);
            model.ListaVersion = listaVersion;
            model.Verscodi = verscodi;

            model.MesActual = objVersion.Versfechaperiodo.ToString(ConstantesAppServicio.FormatoMes);

            model.Repcodi = repcodi;
            model.Idnumeral = repcodi;
            model.Tiporeporte = ConstantesSiosein.TipoInformeMensual;

            SiMenureporteDTO objItem = _servicioPR5.GetByIdMenuReporte(repcodi);
            model.TituloWeb = objItem.Mreptituloweb;

            model.Url = Url.Content("~/");

            return model;
        }

        private FechasPR5 GetObjetFecha(int codigoVersion)
        {
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);

            FechasPR5 objFecha = UtilInfMensual.ObtenerFechasInformesMensual(objVersion.Versfechaperiodo);

            return objFecha;
        }

        [HttpPost]
        public JsonResult CargarDetalleTablaResumen(string sfechaInicial, string sfechaFinal, string filtroRER)
        {
            DateTime fechaInicial = DateTime.ParseExact(sfechaInicial, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(sfechaFinal, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            filtroRER = !string.IsNullOrEmpty(filtroRER) ? filtroRER : "-1";

            SioseinModel model = new SioseinModel
            {
                ListaDetalleProduccion = servicio.ListarDetalleResumenProduccionGeneracion(fechaInicial, fechaFinal, filtroRER)
            };

            return Json(model);
        }

        [HttpPost]
        public JsonResult CargarDetalleTablaResumenMD(string sfechaInicial, string sfechaFinal, string sFechaMaximaDemanda)
        {
            DateTime fechaInicial = DateTime.ParseExact(sfechaInicial, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(sfechaFinal, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaMD = DateTime.ParseExact(sFechaMaximaDemanda, ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);

            SioseinModel model = new SioseinModel
            {
                ListaDetalleProduccion = servicio.ListarDetalleResumenProduccionGeneracionMD(fechaInicial, fechaFinal, fechaMD)
            };

            return Json(model);
        }

        [HttpPost]
        public JsonResult CargarDetalleTablaInterconexion(string sfechaInicial, string sfechaFinal)
        {
            DateTime fechaInicial = DateTime.ParseExact(sfechaInicial, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(sfechaFinal, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            SioseinModel model = new SioseinModel
            {
                ListaDetalleInterconexion = servicio.ListarDetalleResumenInterconexion(fechaInicial, fechaFinal)
            };

            return Json(model);
        }

        [HttpPost]
        public JsonResult CargarDetalleTablaInterconexionMD(string sFechaMaximaDemanda)
        {
            DateTime fechaMD = DateTime.ParseExact(sFechaMaximaDemanda, ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);

            SioseinModel model = new SioseinModel
            {
                ListaDetalleInterconexion = servicio.ListarDetalleResumenInterconexionMD(fechaMD)
            };

            return Json(model);
        }

        [HttpPost]
        public JsonResult GenerarArchivoExcelDetalleTablaResumen(string sfechaInicial, string sfechaFinal, string filtroRER)
        {
            SioseinModel model = new SioseinModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaInicial = DateTime.ParseExact(sfechaInicial, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFinal = DateTime.ParseExact(sfechaFinal, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                filtroRER = !string.IsNullOrEmpty(filtroRER) ? filtroRER : "-1";

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesPR5ReportesServicio.RutaReportes;

                servicio.GenerarRptExcelResumenProduccionGeneracion(ruta, fechaInicial, fechaFinal, filtroRER, out string nameFile);

                model.Resultado = nameFile;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult GenerarArchivoExcelDetalleTablaResumenMD(string sfechaInicial, string sfechaFinal, string sFechaMaximaDemanda)
        {
            SioseinModel model = new SioseinModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaInicial = DateTime.ParseExact(sfechaInicial, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFinal = DateTime.ParseExact(sfechaFinal, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaMD = DateTime.ParseExact(sFechaMaximaDemanda, ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesPR5ReportesServicio.RutaReportes;

                servicio.GenerarRptExcelResumenProduccionGeneracionMD(ruta, fechaInicial, fechaFinal, fechaMD, out string nameFile);

                model.Resultado = nameFile;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult GenerarArchivoExcelDetalleInterconexion(string sfechaInicial, string sfechaFinal)
        {
            SioseinModel model = new SioseinModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaInicial = DateTime.ParseExact(sfechaInicial, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFinal = DateTime.ParseExact(sfechaFinal, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesPR5ReportesServicio.RutaReportes;

                servicio.GenerarRptExcelResumenDetalleInterconexion(ruta, fechaInicial, fechaFinal, out string nameFile);

                model.Resultado = nameFile;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult GenerarArchivoExcelDetalleInterconexionMD(string sfechaInicial, string sfechaFinal, string sFechaMaximaDemanda)
        {
            SioseinModel model = new SioseinModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaInicial = DateTime.ParseExact(sfechaInicial, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFinal = DateTime.ParseExact(sfechaFinal, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaMD = DateTime.ParseExact(sFechaMaximaDemanda, ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesPR5ReportesServicio.RutaReportes;

                servicio.GenerarRptExcelResumenDetalleInterconexionMD(ruta, fechaInicial, fechaFinal, fechaMD, out string nameFile);

                model.Resultado = nameFile;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        #endregion

        #region Versiones

        [HttpPost]
        public JsonResult ListadoVersion(string fechaPeriodo)
        {
            SioseinModel model = new SioseinModel();

            try
            {
                this.ValidarSesionJsonResult();

                DateTime dFechaPeriodo = DateTime.ParseExact(fechaPeriodo, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                //Validar existencia de pdf
                model.ListaVersion = servicio.ListaVersionByFechaInformeMensual(dFechaPeriodo, ConstantesInformeMensual.ReptipcodiMensual);
                //model.ListaVersion = ListaVersiones;
                foreach (var item in model.ListaVersion)
                {
                    string fileName = string.Format(ConstantesPR5ReportesServicio.ArchivoInformeMensualPDF, EPDate.f_NombreMes(dFechaPeriodo.Month), dFechaPeriodo.Year, item.Verscodi);
                    if (FileServer.VerificarExistenciaFile(ConstantesPR5ReportesServicio.PathArchivosInformeMensualPDF, fileName,
                        string.Empty))
                        item.TienePdf = true;
                }

                //Obtener las fechas que se usan para los cálculos
                FechasPR5 objFecha = UtilInfMensual.ObtenerFechasInformesMensual(dFechaPeriodo);
                model.ObjFecha = objFecha;

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Guardar versión
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="motivo"></param>
        /// <returns></returns>
        public JsonResult GuardarNuevaVersion(string fechaPeriodo, string motivo)
        {
            SioseinModel model = new SioseinModel();

            try
            {
                this.ValidarSesionJsonResult();

                DateTime dFechaPeriodo = DateTime.ParseExact(fechaPeriodo, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                motivo = (motivo ?? "").Trim();
                if (motivo == null || motivo.Trim() == string.Empty)
                {
                    throw new Exception("Debe ingresar motivo.");
                }

                if (true)
                {
                    //Guardar registro
                    SiVersionDTO objVersion = new SiVersionDTO()
                    {
                        Versfechaperiodo = dFechaPeriodo,
                        Mprojcodi = ConstantesPR5ReportesServicio.MprojcodiSIOSEIN2,
                        Tmrepcodi = ConstantesPR5ReportesServicio.ReptipcodiInformeMensual,
                        Versmotivo = motivo,
                        Versfechaversion = DateTime.Now,
                        Versusucreacion = base.UserName,
                        Versfeccreacion = DateTime.Now
                    };

                    int verscodi = servicio.SaveSiVersion(objVersion);

                    //Cargar datos de energia y potencia instalada
                    servicio.GenerarInsumoInfMensual(dFechaPeriodo, base.UserName);

                    //Guardar versión
                    servicio.GuardarVersionInfMensual(verscodi, dFechaPeriodo);

                    //Genera Excel
                    servicio.GenerarArchivoExcelTodoInformeMensual(verscodi, out string fileName);
                    servicio.MoverArchivoInformeMensualFileServer(fileName);

                    model.Resultado = verscodi.ToString();
                }
                else
                {
                    throw new ArgumentException("No se puede generar una nueva versión: No existe diferencias con la versión anterior.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        #endregion

        #region Descarga archivo Excel individual

        /// <summary>
        /// Permite generar el archivo
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="versi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarReporteMensualIndividual(int reporcodi, int versi)
        {
            var model = new SioseinModel();
            try
            {
                string file = this.servicio.GenerarArchivoExcelTodoInformeMensualIndividual(versi, reporcodi, out string filename);

                model.Resultado = filename;
                model.Total = 1;
            }
            catch (Exception ex)
            {
                model.Total = -1;
                model.Resultado2 = ex.Message + ConstantesAppServicio.CaracterEnter + ex.StackTrace;
                model.Mensaje = ex.Message;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// Permite descargar archivo excel del Reporte Ejecutivo Mensual
        /// </summary>
        /// <param name="nameFile"></param>
        /// <returns></returns>
        public virtual ActionResult ExportarReporteXlsIndividual(string nameFile)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesPR5ReportesServicio.Directorio;
            string fullPath = ruta + nameFile;
            return File(fullPath, ConstantesAppServicio.ExtensionExcel, nameFile);
        }

        /// <summary>
        /// Permite descargar archivo excel del Reporte Ejecutivo Mensual
        /// </summary>
        /// <param name="nameFile"></param>
        /// <returns></returns>
        public virtual ActionResult ExportarReporteXls(string nameFile)
        {
            string subcarpetaDestino = ConstantesPR5ReportesServicio.RutaReportes;
            string directorioDestino = AppDomain.CurrentDomain.BaseDirectory + subcarpetaDestino;
            string fullPath = directorioDestino + nameFile;

            //eliminar archivo temporal
            byte[] buffer = null;
            if (System.IO.File.Exists(fullPath))
            {
                buffer = System.IO.File.ReadAllBytes(fullPath);
                System.IO.File.Delete(fullPath);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nameFile);
        }

        #endregion

        #region RESÚMEN

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexResumenRelevante(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            SioseinModel model = GetModelGenericoIndex(ConstantesInformeMensual.IndexMensualResumenRelevante, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Maxima Demanda Tipo de Generacion Semanal
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarResumenRelevante(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeMensual.IndexMensualResumenRelevante,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionResumenRelevanteMensual(objFiltro);

            List<SioseinModel> listaModel = new List<SioseinModel>();
            SioseinModel model;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //RESUMEN RELEVANTE
            model = new SioseinModel
            {
                Resultado = UtilInfMensual.ReporteResumenMensualHtml(objReporte.ObjTexto)
            };
            listaModel.Add(model);

            //Grafico participacion de Recursos energéticos
            model = new SioseinModel
            {
                Grafico = objReporte.GraficoPieSemAct_AnioAct
            };
            listaModel.Add(model);

            model = new SioseinModel
            {
                Grafico = objReporte.GraficoPieSemAct_Anio1Ant
            };
            listaModel.Add(model);

            return Json(listaModel);
        }

        #endregion

        #region 1. Oferta de generacion electrica

        #region 1.1. Ingreso en Operación Comercial al SEIN

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexMenIngresoOpComercSEIN(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            SioseinModel model = GetModelGenericoIndex(ConstantesInformeMensual.IndexMensualIngresoOpComercSEIN, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Ingreso Operacion Comercial SEIN
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaIngresoOpComercSEIN(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            int tipoOperacion = 1; // Es Ingreso de operacion
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeMensual.IndexMensualIngresoOpComercSEIN,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionIngresoOpComercSEINMensual(objFiltro, tipoOperacion);

            SioseinModel model = new SioseinModel
            {
                Resultado = UtilSemanalPR5.GenerarListadoIngresoRetiroOpComercialSeinHtml(objReporte.Tabla, tipoOperacion),
                Grafico = objReporte.Grafico
            };

            return Json(model);
        }

        #endregion

        #region 1.2. Retiro de Operación Comercial del SEIN

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexMenRetiroOpComercSEIN(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            SioseinModel model = GetModelGenericoIndex(ConstantesInformeMensual.IndexMensualRetiroOpComercSEIN, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Retiro de Operacion Comercial SEIN
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaRetiroOpComercSEIN(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            int tipoOperacion = 2; // Es Retiro de operacion
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeMensual.IndexMensualRetiroOpComercSEIN,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionIngresoOpComercSEINMensual(objFiltro, tipoOperacion);

            SioseinModel model = new SioseinModel
            {
                Resultado = UtilSemanalPR5.GenerarListadoIngresoRetiroOpComercialSeinHtml(objReporte.Tabla, tipoOperacion),
                Grafico = objReporte.Grafico
            };

            return Json(model);
        }

        #endregion

        #region 1.3. Potencia Instalada en el SEIN
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexMenPotenciaInstSEIN(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            SioseinModel model = GetModelGenericoIndex(ConstantesInformeMensual.IndexMensualPotenciaInstaladaSEIN, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de de potencias instaladas en el SEIN
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaPotenciaInstaladaSEIN(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeMensual.IndexMensualPotenciaInstaladaSEIN,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionPotenciaInstSEIN(objFiltro);

            SioseinModel model = new SioseinModel
            {
                Resultado = UtilInfMensual.GenerarListadoPotenciaInstaladaSeinHtml(objReporte.Tabla),
                Resultados = objReporte.ListaMensaje,
                Grafico = objReporte.Grafico
            };

            return Json(model);
        }

        #endregion

        #endregion

        #region 2. Produccion de energia electrica

        #region 2.1. Producción por tipo de Generación

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexMenProdTipoGen(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            SioseinModel model = GetModelGenericoIndex(ConstantesInformeMensual.IndexMensualProdTipoGen, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Produccionpor Tipo de Generacion
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaProduccionTipoGen(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeMensual.IndexMensualProdTipoGen,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionReporteProduccionXTgeneracionInfMensual(objFiltro);

            List<SioseinModel> listaModel = new List<SioseinModel>();
            SioseinModel model = new SioseinModel
            {
                Resultado = UtilSemanalPR5.ListaReporteProduccionTipoGenHTML(objReporte.Tabla)
            };
            listaModel.Add(model);

            model = new SioseinModel
            {
                Grafico = objReporte.Grafico
            };
            listaModel.Add(model);

            return Json(listaModel);
        }

        #endregion

        #region 2.2. Producción por tipo de Recurso Energético

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexMenProdTipoRecurso(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            SioseinModel model = GetModelGenericoIndex(ConstantesInformeMensual.IndexMensualProdTipoRecurso, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Produccion por Tipo de Recurso
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaProduccionTipoRecurso(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeMensual.IndexMensualProdTipoRecurso,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = servicio.ListarDataProduccionXTipoRecursoEnergetico(objFiltro);

            List<SioseinModel> listaModel = new List<SioseinModel>();
            SioseinModel model = new SioseinModel
            {
                Resultado = UtilSemanalPR5.ReporteProdXTipoRecursoEnergeticoHtml(objReporte.Tabla)
            };
            listaModel.Add(model);

            model = new SioseinModel
            {
                Grafico = objReporte.Grafico
            };
            listaModel.Add(model);

            return Json(listaModel);
        }

        #endregion

        #region 2.3. Por Producción RER

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexMenProdRER(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            SioseinModel model = GetModelGenericoIndex(ConstantesInformeMensual.IndexMensualProdRER, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Produccion RER
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaProduccionRER(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeMensual.IndexMensualProdRER,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionProduccionRERInfMensual(objFiltro);

            List<SioseinModel> listaModel = new List<SioseinModel>();
            SioseinModel model = new SioseinModel
            {
                Resultado = UtilSemanalPR5.ReporteProduccionRERHTML(objReporte.Tabla)
            };
            listaModel.Add(model);

            model = new SioseinModel
            {
                Grafico = objReporte.ListaGrafico[0]
            };
            listaModel.Add(model);

            model = new SioseinModel
            {
                Grafico = objReporte.ListaGrafico[1]
            };
            listaModel.Add(model);

            return Json(listaModel);
        }

        #endregion

        #region 2.4. Factor de planta de las centrales RER

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexMenFactorPlantaRER(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            SioseinModel model = GetModelGenericoIndex(ConstantesInformeMensual.IndexMensualFactorPlantaRER, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista Factor de Planta RER
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaFactorPlantaRER(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeMensual.IndexMensualFactorPlantaRER,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionFactorPlantaCentralesRER(objFiltro);

            List<SioseinModel> listaModel = new List<SioseinModel>();

            // Producción de energía eléctrica (GWh) y factor de planta de las centrales con recursos energético renovables en el SEIN.
            SioseinModel model = new SioseinModel
            {
                Resultado = UtilSemanalPR5.ReporteFactorPlantaCentralesRERHtml(objReporte.Tabla)
            };
            listaModel.Add(model);

            //Factor de planta de las centrales RER  Acumulado al FECHA_FIN
            model = new SioseinModel
            {
                Grafico = objReporte.Grafico
            };
            listaModel.Add(model);

            //Producción de energía eléctrica (GWh) y factor de planta de las centrales con recursos energético renovables por tipo de generación en el SEIN - semana operativa N
            foreach (var reg in objReporte.ListaGrafico)
            {
                model = new SioseinModel
                {
                    Grafico = reg
                };
                listaModel.Add(model);
            }

            return Json(listaModel);
        }

        #endregion

        #region 2.5. Participación de la producción por empresas Integrantes

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexMenParticipacionEmpresas(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            SioseinModel model = GetModelGenericoIndex(ConstantesInformeMensual.IndexMensualParticipacionEmpresas, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Produccion Empresas
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaProduccionEmpresas(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeMensual.IndexMensualParticipacionEmpresas,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionProduccionEmpresasIntegrantesInfMensua(objFiltro);

            SioseinModel model = new SioseinModel
            {
                Resultado = UtilSemanalPR5.ReporteProduccionEmpresasIntegrantesHtml(objReporte.Tabla),
                Grafico = objReporte.Grafico
            };

            return Json(model);
        }

        #endregion

        #endregion

        #region 3. Maxima  Demanda Coincidente de potencia

        #region 3.1. MÁXIMA DEMANDA COINCIDENTE DE POTENCIA POR TIPO DE GENERACIÓN (MW)

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexMenMaximaDemandaTipoGeneracion(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            SioseinModel model = GetModelGenericoIndex(ConstantesInformeMensual.IndexMensualMaximaDemandaTipoGeneracion, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Maxima Demanda Tipo de Generacion Semanal
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarMaximaDemandaTipoGeneracionMensual(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeMensual.IndexMensualMaximaDemandaTipoGeneracion,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionCargarMaximaDemandaTipoGeneracionMen(objFiltro);

            List<SioseinModel> listaModel = new List<SioseinModel>();
            SioseinModel model;

            //Máxima demanda coincidente de potencia (MW) por tipo de generación en el SEIN.
            model = new SioseinModel
            {
                Resultado = UtilSemanalPR5.ListarMaximaDemandaTipoGeneracionSemanalHtml(objReporte.Tabla)
            };
            listaModel.Add(model);

            //Comparación de la máxima demanda coincidente de potencia(MW) por tipo de generación en el SEIN, en la semana operativa 36 - (37) de los años 2015, 2016 y 2017
            model = new SioseinModel
            {
                Grafico = objReporte.GraficoCompMD
            };
            listaModel.Add(model);

            return Json(listaModel);
        }

        #endregion

        #region 3.2. PARTICIPACIÓN DE LAS EMPRESAS INTEGRANTES EN LA MÁXIMA DEMANDA COINCIDENTE (MW)

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexMenMaximaDemandaXEmpresa(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            SioseinModel model = GetModelGenericoIndex(ConstantesInformeMensual.IndexMensualMaximaDemandaXEmpresa, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Maxima Demanda X Empresa Semanal
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarMaximaDemandaXEmpresaSemanal(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeMensual.IndexMensualMaximaDemandaXEmpresa,
                Verscodi = codigoVersion
            };
            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionCargarMaximaDemandaXEmpresaInfMen(objFiltro);

            SioseinModel model = new SioseinModel
            {
                //Cuadro N° 9: Participación de las empresas generadoras del COES en la máxima demanda coincidente (MW) durante la semana operativa
                Resultado = UtilSemanalPR5.ReporteMaximaDemandaXEmpresaSemanalHtml(objReporte.Tabla),
                //Gráfico N° 17: Comparación de la máxima demanda coincidente  (MW) de las empresas generadoras del COES durante la semana operativa N de los años  Y1 - Y2
                Grafico = objReporte.Grafico
            };
            return Json(model);
        }

        #endregion

        #endregion

        #region 4. Hidrologia

        #region 4.1. Volumen útil de los embalses y lagunas (Mm3)

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexMenVolUtilEmbLag(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            SioseinModel model = GetModelGenericoIndex(ConstantesInformeMensual.IndexMensualVolUtilEmbLag, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Volumenes de Embales y Lagunas
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>        
        public JsonResult CargarListaVolEmbalesLagunas(int codigoVersion)
        {
            SioseinModel model = new SioseinModel();

            //filtros
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            //reporte                        
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeMensual.IndexMensualVolUtilEmbLag,
                Verscodi = codigoVersion
            };
            InfSGIReporteVersionado objReporte = _servicioPR5.ListarDataVersionVolumenUtilEmbalsesLagunas(objFiltro);

            /// Output          
            TablaReporte dataTabla = UtilSemanalPR5.ObtenerDataTablaVolumenUtilEmbalsesLagunas(objFecha, objReporte.ListaPtoEmbalsesLagunas, objReporte.ListaDataXPto);
            model.Resultado = UtilSemanalPR5.GenerarRHtmlVolumenUtilEmbalsesLagunas(objFecha, dataTabla);

            return Json(model);
        }

        #endregion

        #region 4.2. Evolucion de volumenes de embalses y lagunas (Mm3)

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexMenEvolucionVolEmbLag(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            SioseinModel model = GetModelGenericoIndex(ConstantesInformeMensual.IndexMensualEvolucionVolEmbLag, codigoVersion);

            return View(model);
        }

        public JsonResult CargarListaEvolEmbalesLagunas(int codigoVersion)
        {
            SioseinModel model = new SioseinModel();

            //filtros
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            //reporte                        
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeMensual.IndexMensualEvolucionVolEmbLag,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioPR5.ListarDataVersionEvolucionVolumenUtilSemanal(objFiltro);

            /// Output          
            model.Graficos = new List<GraficoWeb>();
            model.Graficos.AddRange(objReporte.ListaGrafico);

            return Json(model);
        }

        #endregion

        #region 4.3. Promedio mensual de los caudales (m3/s)

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexMenPromCaudales(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            SioseinModel model = GetModelGenericoIndex(ConstantesInformeMensual.IndexMensualPromMensualCaudales, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Promedio Mensual de Caudales para Ejecutivo Semanal
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaPromMensualCaudales(int codigoVersion)
        {
            SioseinModel model = new SioseinModel();

            //filtros
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeMensual.IndexMensualPromMensualCaudales,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioPR5.ListarDataVersionCuadroCaudalSemanal(objFiltro);

            List<string> listaHtml = new List<string>();
            foreach (var objTabla in objReporte.ListaTabla)
            {
                listaHtml.Add(UtilSemanalPR5.GenerarRHtmlPromedioMensualCaudalesSemanal(objFecha, objTabla));
            }

            if (listaHtml.Count > 0)
            {
                model.Resultado = listaHtml[0];
                model.Resultado2 = listaHtml[1];
            }
            return Json(model);
        }

        #endregion

        #region 4.4. Evolucion mensual de los caudales (m3/s)

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexMenEvolucionCaudales(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            SioseinModel model = GetModelGenericoIndex(ConstantesInformeMensual.IndexMensualEvolucionCaudales, codigoVersion);

            return View(model);
        }

        public JsonResult CargarListaPromMensualEvolCaudales(int codigoVersion)
        {
            SioseinModel model = new SioseinModel();

            //filtros
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);
            DateTime fechaInicial = objVersion.Versfechaperiodo;
            DateTime fechaFinal = fechaInicial.AddDays(6);

            //reporte
            FechasPR5 objFecha = UtilSemanalPR5.ObtenerFechasInformeSemanal(fechaInicial, fechaFinal);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeMensual.IndexMensualEvolucionCaudales,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioPR5.ListarDataVersionEvolucionCaudales(objFiltro);
            model.Graficos = objReporte.ListaGrafico;

            return Json(model);
        }

        #endregion

        #endregion

        #region 5. Costos marginales promedios 

        #region 5.1.  COSTOS MARGINALES EN LAS PRINCIPALES BARRAS DEL SEIN (US$/MWh)

        public ActionResult IndexMenCostosMarginalesProm(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            SioseinModel model = GetModelGenericoIndex(ConstantesInformeMensual.IndexMensualCostosMarginalesProm, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fechaInicioM"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ConsultaCostosMarginalesBarrasSein(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            var model = new SioseinModel();

            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                Mrepcodi = ConstantesInformeMensual.IndexMensualCostosMarginalesProm,
                Verscodi = codigoVersion,
                ObjFecha = objFecha
            };

            InfSGIReporteVersionado objReporte = this.servicio.ListarDataCostosMarginalesPromedioBarra(objFiltro);

            model.Resultados = new List<string>();
            foreach (var reg in objReporte.ListaTabla)
            {
                model.Resultados.Add(this.servicio.GenerarRptHtmlMensualCostosMarginalesBarrasSein(reg));
            }

            model.Graficos = objReporte.ListaGrafico;

            return Json(model);
        }

        #endregion

        #endregion

        #region 6. HORAS DE CONGESTIÓN EN LAS PRINCIPALES EQUIPOS DE TRANSMISIÓN DEL SEIN (Horas)

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexMenHorasCongestionAreaOpe(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            SioseinModel model = GetModelGenericoIndex(ConstantesInformeMensual.IndexMensualHorasCongestionAreaOpe, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Horas de Congestion por Area Operativa Mensual
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarHorasCongestionAreaOpe(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeMensual.IndexMensualHorasCongestionAreaOpe,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioPR5.ListarDataVersionHorasCongestionPorArea(objFiltro);
            SioseinModel model = new SioseinModel
            {
                Resultado = UtilSemanalPR5.ListaHorasCongestionDeEquiposTransmisioHTML(objFecha, objReporte.Tabla),
                Grafico = objReporte.Grafico
            };

            return Json(model);
        }

        #endregion

        #region 7. EVENTOS Y FALLAS QUE OCASIONARON INTERRUPCIÓN Y DISMINUCIÓN DE SUMINISTRO ELÉCTRICO

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexMenEventoFallaSuministroEnerg(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            SioseinModel model = GetModelGenericoIndex(ConstantesInformeMensual.IndexMensualEventoFallaSuministroEnerg, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Evento, Falla de Suministros de Energ
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarEventoFallaSuministroEnerg(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeMensual.IndexMensualEventoFallaSuministroEnerg,
                Verscodi = codigoVersion
            };
            InfSGIReporteVersionado objReporte = _servicioPR5.ListarDataVersionEventoFallaSuministroEnerg(objFiltro);

            SioseinModel model = new SioseinModel
            {
                Resultado = UtilSemanalPR5.GenerarRHtmlFallasXFamiliaYCausa(objReporte.Tabla),
                Graficos = new List<GraficoWeb>()
            {
                objReporte.GraficoEveFallaXTipo, objReporte.GraficoEveFallaXFam, objReporte.GraficoEveEnergXFam
            }
            };

            return Json(model);
        }

        #endregion

        #region 8. ANEXOS

        #region 8.1 PRODUCCION DE ELECTRICIDAD MENSUAL POR EMPRESA

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexMenProduccionElectricidad(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            SioseinModel model = GetModelGenericoIndex(ConstantesInformeMensual.IndexMenProduccionElectricidad, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Evento, Falla de Suministros de Energ
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarProduccionElectricidad(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeMensual.IndexMenProduccionElectricidad,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionProduccionElectricidad(objFiltro);
            SioseinModel model = new SioseinModel
            {
                Resultado = UtilInfMensual.ListarResumenProduccionMensualHtml(objReporte.Tabla)
            };

            return Json(model);
        }

        #endregion

        #region 8.2 MAXIMA POTENCIA COINCIDENTE MENSUAL

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexMenMaximaPotenciaCoincidente(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            SioseinModel model = GetModelGenericoIndex(ConstantesInformeMensual.IndexMenMaximaPotenciaCoincidente, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Evento, Falla de Suministros de Energ
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarMaximaPotenciaCoincidente(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeMensual.IndexMenMaximaPotenciaCoincidente,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionMaximaPotenciaCoincidente(objFiltro);
            SioseinModel model = new SioseinModel
            {
                Resultado = UtilInfMensual.ListarResumenMaximaDemandaMensualHtml(objReporte.Tabla)
            };

            return Json(model);
        }

        #endregion

        #region 8.3 LISTADO DE EVENTOS Y FALLAS

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexMenListadoEventos(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            SioseinModel model = GetModelGenericoIndex(ConstantesInformeMensual.IndexMenListadoEventos, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Evento, Falla de Suministros de Energ
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListadoEventos(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            SioseinModel model = new SioseinModel();

            //filtros
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeMensual.IndexMenListadoEventos,
                Verscodi = codigoVersion
            };
            InfSGIReporteVersionado objReporte = _servicioPR5.ListarDataVersionDetalleEventos(objFiltro);
            model.Resultado = objReporte.Resultado;

            return Json(model);
        }

        #endregion

        #endregion

        #region Carga de Archivo PDF

        /// <summary>
        /// Permite cargar el archivo de interrupciones
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public ActionResult UploadArchivoPDF(int idVersion)
        {
            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    var objVersion = servicio.GetByIdSiVersion(idVersion);
                    DateTime dFechaPeriodo = objVersion.Versfechaperiodo;
                    string fileName = string.Format(ConstantesPR5ReportesServicio.ArchivoInformeMensualPDF, EPDate.f_NombreMes(dFechaPeriodo.Month), dFechaPeriodo.Year, idVersion);

                    //string fileName = string.Format(ConstantesPR5ReportesServicio.ArchivoInformeSemanalPDF, idVersion);

                    if (!FileServer.VerificarExistenciaDirectorio(ConstantesPR5ReportesServicio.PathArchivosInformeMensualPDF, string.Empty))
                        FileServer.CreateFolder(ConstantesPR5ReportesServicio.PathArchivosInformeMensualPDF, string.Empty, string.Empty);

                    if (FileServer.VerificarExistenciaFile(ConstantesPR5ReportesServicio.PathArchivosInformeMensualPDF, fileName,
                        string.Empty))
                        FileServer.DeleteBlob(ConstantesPR5ReportesServicio.PathArchivosInformeMensualPDF + fileName,
                            string.Empty);

                    FileServer.UploadFromStream(file.InputStream, ConstantesPR5ReportesServicio.PathArchivosInformeMensualPDF, fileName,
                        string.Empty);
                }
                return Json(new { success = true, indicador = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Permite descargar el archivo de informe semanal
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarArchivoPDF(int idVersion)
        {
            var objVersion = servicio.GetByIdSiVersion(idVersion);
            DateTime dFechaPeriodo = objVersion.Versfechaperiodo;
            string fileName = string.Format(ConstantesPR5ReportesServicio.ArchivoInformeMensualPDF, EPDate.f_NombreMes(dFechaPeriodo.Month), dFechaPeriodo.Year, idVersion);
            Stream stream = FileServer.DownloadToStream(ConstantesPR5ReportesServicio.PathArchivosInformeMensualPDF + fileName,
                string.Empty);

            return File(stream, Constantes.AppPdf, fileName);
        }


        /// <summary>
        /// Permite descargar el archivo excel de informe semanal
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarArchivoExcel(int idVersion)
        {

            string folder = _servicioPR5.GetCarpetaInformeMensual();
            var objVersion = _servicioPR5.GetByIdSiVersion(idVersion);
            string nombreArchivo = UtilInfMensual.GetNombreArchivoInformeMensual(objVersion.Versfechaperiodo, idVersion);
            Stream stream = FileServer.DownloadToStream(folder + nombreArchivo,
                string.Empty);

            return File(stream, Constantes.AppExcel, nombreArchivo);
        }

        /// <summary>
        /// Permite descargar pdf para vista previa
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual FileContentResult DownloadArchivoPDF(int idVersion)
        {
            var objVersion = servicio.GetByIdSiVersion(idVersion);
            DateTime dFechaPeriodo = objVersion.Versfechaperiodo;
            string fileName = string.Format(ConstantesPR5ReportesServicio.ArchivoInformeMensualPDF, EPDate.f_NombreMes(dFechaPeriodo.Month), dFechaPeriodo.Year, idVersion);
            byte[] stream = FileServer.DownloadToArrayByte(ConstantesPR5ReportesServicio.PathArchivosInformeMensualPDF + fileName,
                string.Empty);
            string mimeType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "inline; filename=" + fileName);
            return File(stream, mimeType);
        }

        /// <summary>
        /// Permite descargar excel para vista previa
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        //[HttpGet]
        //public virtual FileContentResult DownloadArchivoExcel(int idVersion)
        //{
        //    string folder = this.servicio.GetCarpetaInformeSemanal();
        //    var objVersion = this.servicio.GetByIdSiVersion(idVersion);
        //    DateTime fechaInicio = objVersion.Versfechaperiodo;
        //    DateTime fechaFin = fechaInicio.AddDays(6);
        //    string nombreArchivo = this.servicio.GetNombreArchivoInformeSemanal("", fechaInicio, fechaFin, idVersion);

        //    byte[] stream = FileServer.DownloadToArrayByte(folder + nombreArchivo,
        //        string.Empty);
        //    string mimeType = Constantes.AppExcel;
        //    Response.AppendHeader("Content-Disposition", "inline; filename=" + nombreArchivo);
        //    return File(stream, mimeType);
        //}

        #endregion

        #region UTIL

        public JsonResult ListarGraficosReporte(int reporcodi)
        {
            SioseinModel model = new SioseinModel();

            try
            {
                base.ValidarSesionJsonResult();

                List<MeReporteDTO> listaGraficosReporte = _servicioPR5.ListMeReportes().Where(x => x.Reporesgrafico == Constantes.SI).ToList();
                var ListaGraficosVisibles = _servicioPR5.ListSiMenureporteGraficos().Where(x => x.Mrepcodi == reporcodi).ToList();
                foreach (var obj in ListaGraficosVisibles)
                {
                    if (obj.Mrgrestado == ConstantesInformeSemanalPR5.PerteneceNumeral) // Pertenece al numeral
                    {
                        var objFind = listaGraficosReporte.Find(x => x.Reporcodi == obj.Reporcodi);
                        if (objFind != null)
                        {
                            objFind.IsCheck = ConstantesInformeSemanalPR5.PerteneceNumeral;
                        }
                    }
                }


                model.ListaGraficosReporte = listaGraficosReporte;
                model.Resultado = "1";


            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        public JsonResult GuardarVisibleGraficos(string reporcodisVisible, int ireporcodi)
        {
            SioseinModel model = new SioseinModel();

            try
            {
                base.ValidarSesionJsonResult();

                List<int> listaReporcodisVisible = new List<int>();
                if (!string.IsNullOrEmpty(reporcodisVisible))
                    listaReporcodisVisible = reporcodisVisible.Split(',').Select(x => int.Parse(x)).ToList();
                _servicioPR5.GuardarListaGraficosVisibles(ireporcodi, listaReporcodisVisible);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        #endregion

        [HttpPost]
        public JsonResult VisualizarArchivo(int idVersion)
        {
            SioseinModel model = new SioseinModel();

            try
            {
                string folder = _servicioPR5.GetCarpetaInformeMensual();
                var objVersion = this.servicio.GetByIdSiVersion(idVersion);
                DateTime fechaInicio = objVersion.Versfechaperiodo;
                DateTime fechaFin = fechaInicio.AddDays(6);
                string nombreArchivo = UtilInfMensual.GetNombreArchivoInformeMensual(fechaInicio, idVersion);

                string subcarpetaDestino = ConstantesPR5ReportesServicio.RutaReportes;
                string directorioDestino = AppDomain.CurrentDomain.BaseDirectory + subcarpetaDestino;
                string fileName = nombreArchivo;

                Stream stream = FileServer.DownloadToStream(folder + nombreArchivo, string.Empty);
                FileServer.UploadFromStream(stream, string.Empty, fileName, directorioDestino);
                string url = subcarpetaDestino + fileName;
                model.Resultado = url;
                model.Detalle = fileName;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }
    }
}
