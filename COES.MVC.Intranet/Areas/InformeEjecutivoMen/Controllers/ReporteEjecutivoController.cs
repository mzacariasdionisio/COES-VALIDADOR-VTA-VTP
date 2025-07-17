using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.InformeEjecutivoMen.Models;
using COES.MVC.Intranet.Areas.Siosein.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.SIOSEIN;
using COES.Servicios.Aplicacion.Siosein2;
using COES.Servicios.Aplicacion.Siosein2.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.InformeEjecutivoMen.Controllers
{
    public class ReporteEjecutivoController : BaseController
    {
        private readonly Siosein2AppServicio _servicioSiosein2;
        private readonly SIOSEINAppServicio _servicioSiosein;
        private readonly PR5ReportesAppServicio _servicioPR5;

        #region Declaracion de variables de Sesión

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public ReporteEjecutivoController()
        {
            _servicioSiosein = new SIOSEINAppServicio();
            _servicioSiosein2 = new Siosein2AppServicio();
            _servicioPR5 = new PR5ReportesAppServicio();
        }

        /// <inheritdoc />
        /// <summary>
        /// Protected de log de errores page
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
            if (!base.IsValidSesion)
            {
                return base.RedirectToLogin();
            }
            else
            {
                return RedirectToAction("Index", "ReporteEjecutivo", new { area = string.Empty });
            }
        }

        /// <summary>
        /// Setear valores para cada index
        /// </summary>
        /// <param name="model"></param>
        /// <param name="repcodi"></param>
        private Siosein2Model GetModelGenericoIndex(int mrepcodi, int verscodi)
        {
            Siosein2Model model = new Siosein2Model();

            //version seleccionada
            SiVersionDTO objVersion = _servicioPR5.GetByIdSiVersion(verscodi);
            List<SiVersionDTO> listaVersion = _servicioPR5.ListaVersionByFecha(objVersion.Versfechaperiodo, ConstantesPR5ReportesServicio.ReptipcodiEjecutivoMensual);
            model.ListaVersion = listaVersion;
            model.Verscodi = verscodi;

            model.MesActual = objVersion.Versfechaperiodo.ToString(Constantes.FormatoMesAnio);

            model.Repcodi = mrepcodi;
            model.Idnumeral = mrepcodi;

            SiMenureporteDTO objItem = _servicioPR5.GetByIdMenuReporte(mrepcodi);
            model.Titulo = objItem.Repdescripcion;
            model.TituloWeb = objItem.Repdescripcion;

            model.Url = Url.Content("~/");

            return model;
        }

        private FechasPR5 GetObjetFecha(int codigoVersion)
        {
            SiVersionDTO objVersion = _servicioPR5.GetByIdSiVersion(codigoVersion);

            FechasPR5 objFecha = UtilEjecMensual.ObtenerFechasEjecutivoMensual(objVersion.Versfechaperiodo);

            return objFecha;
        }

        #endregion

        #region Versiones

        [HttpPost]
        public JsonResult ListadoVersion(string fechaPeriodo)
        {
            Siosein2Model model = new Siosein2Model();
            try
            {
                ValidarSesionJsonResult();
                DateTime dFechaPeriodo = DateTime.ParseExact(fechaPeriodo, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                //Validar existencia de pdf
                model.ListaVersion = _servicioSiosein.ListaVersionByFechaInformeMensual(dFechaPeriodo, ConstantesInformeEjecutivoMensual.ReptiprepcodiEjecMensual);
                foreach (var item in model.ListaVersion)
                {
                    string fileName = string.Format(ConstantesPR5ReportesServicio.ArchivoInformeEjecutivoMensualPDF, EPDate.f_NombreMes(dFechaPeriodo.Month), dFechaPeriodo.Year, item.Verscodi);
                    if (FileServer.VerificarExistenciaFile(ConstantesPR5ReportesServicio.PathArchivosInformeEjecutivoMensualPDF, fileName,
                        string.Empty))
                        item.TienePdf = true;
                }

                //Obtener las fechas que se usan para los cálculos
                FechasPR5 objFecha = UtilEjecMensual.ObtenerFechasEjecutivoMensual(dFechaPeriodo);
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
            Siosein2Model model = new Siosein2Model();
            try
            {
                ValidarSesionJsonResult();

                DateTime dFechaPeriodo = DateTime.ParseExact(fechaPeriodo, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                motivo = (motivo ?? "").Trim();
                if (motivo == null || motivo.Trim() == string.Empty)
                {
                    throw new Exception("Debe ingresar motivo.");
                }

                if (true)
                {
                    int verscodi = _servicioSiosein2.SaveGenerarVersion(dFechaPeriodo, motivo, base.UserName);

                    //Cargar datos de energia y máxima demanda
                    _servicioSiosein2.GenerarInsumoEjecMensual(dFechaPeriodo, base.UserName);

                    //Guardar versión
                    _servicioSiosein2.GuardarVersionInfEjecMensual(verscodi, dFechaPeriodo);

                    //Genera Excel
                    _servicioSiosein2.GenerarArchivoExcelTodoEjecutivoMensual(verscodi, out string fileName);
                    _servicioSiosein2.MoverArchivoInformeEjecutivoMensualFileServer(fileName);
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

        #region Menu Informe Ejecutivo Mensual

        /// <summary>
        /// Permite mostrar la pantalla inicial del Reporte Ejecutivo
        /// </summary>
        /// <returns></returns>
        /// 
        public ActionResult Index(string mes)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            DateTime fechaConsulta = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);
            if (!string.IsNullOrEmpty(mes)) fechaConsulta = DateTime.ParseExact(mes, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

            var model = new Siosein2Model
            {
                MesActual = fechaConsulta.ToString(Constantes.FormatoMesAnio)
            };

            return View(model);
        }

        /// <summary>
        /// lista de menu para ejecutivo mensual
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarMenu()
        {
            SioseinModel model = new SioseinModel();

            List<SiMenureporteDTO> listaItems = _servicioPR5.GetListaAdmReporte(ConstantesSiosein2.ReporteEjecutivo);
            model.Menu = UtilAnexoAPR5.ListaMenuHtml(listaItems);

            return Json(model);
        }

        #endregion

        #region Descarga archivo Excel

        /// <summary>
        /// Permite generar el archivo
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="versi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarReporteMensual(int reporcodi, int versi)
        {
            try
            {
                string file = _servicioSiosein2.GenerarArchivoExcelIndividualEjecutivoMensual(versi, reporcodi, out string filename);

                return Json(new { Total = 1, Resultado = file });
            }
            catch (Exception ex)
            {
                return Json(new { Total = -1, Mensaje = ex.ToString() });
            }
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
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string fullPath = ruta + nameFile;
            return File(fullPath, ConstantesAppServicio.ExtensionExcel, nameFile);
        }

        #endregion

        #region 1. PRODUCCION Y POTENCIA COINCIDENTE EN BORNES DE GENERACIÓN DEL SEIN

        #region 1.1. Produccion por empresa generadora

        public ActionResult ProduccionEmpresaGeneradora(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexProdEmpresaGeneradora, codigoVersion);

            return View(model);
        }


        [HttpPost]
        public JsonResult ConsultarProduccionEmpresaGeneradora(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);

            //reporte                        
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeEjecutivoMensual.IndexProdEmpresaGeneradora,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioSiosein2.ListarDataVersionObtenerDataProduccionEmpresaGeneradora(objFiltro);
            var listaHtml = UtilEjecMensual.GenerarRHtmlProduccionEmpresaGeneradora(objFecha, objReporte.TextoResumen, objReporte.Tabla);

            return Json(listaHtml);
        }

        #endregion

        #region 1.2. Producción total de centrales de generación eléctrica

        public ActionResult TotalCentralesGeneracion(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexTotalCentralesGeneracion, codigoVersion);

            return View(model);
        }

        [HttpPost]
        public JsonResult ConsultarTotalCentralesGeneracion(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            //reporte                        
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeEjecutivoMensual.IndexTotalCentralesGeneracion,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioSiosein2.ListarDataVersionrProduccionXCentralEjecMensual(objFiltro);
            var model = new Siosein2Model();
            var listaProduccionTotalCentrales = UtilEjecMensual.GenerarRHtmlProduccionTotalCentralesGeneracion(objReporte.Tabla);
            model.Resultado = listaProduccionTotalCentrales;
            model.Estado = (int)Siosein2Model.TipoEstado.Ok;

            return Json(model);
        }

        #endregion

        #region 1.3. Participación por empresas en la producción total de energía del mes

        public ActionResult ParticipacionEmpresasProduccionMes(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexParticEmpProduccionMes, codigoVersion);

            return View(model);
        }

        [HttpPost]
        public JsonResult ConsultarParticipacionEmpresasProduccionMes(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);

            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeEjecutivoMensual.IndexParticEmpProduccionMes,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioSiosein2.ListarDataVersionParticipacionEmpresasProdMensual(objFiltro);
            var model = new Siosein2Model
            {
                Graficos = new List<GraficoWeb>()
                {
                    objReporte.GrafPartXEmprProducTotal,
                    objReporte.GraficoEmpresaOtro

                }
            };

            return Json(model);
        }

        #endregion

        #region 1.4. Evolución del crecimiento mensual de la máxima potencia coincidente sin exportación a ecuador

        public ActionResult CrecimientoMensualMaxPotencia(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexCrecimientoMensualMaxPotencia, codigoVersion);

            return View(model);
        }

        [HttpPost]
        public JsonResult ConsultarCrecimientoMensualMaxPotencia(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeEjecutivoMensual.IndexCrecimientoMensualMaxPotencia,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioSiosein2.ListarDataVersionCrecimientoMensualMaxPotencial(objFiltro);
            var model = new Siosein2Model
            {
                Resultados = objReporte.ListaResumen,
                Grafico = objReporte.Grafico
            };

            return Json(model);
        }

        #endregion

        #region 1.5. Comparación de la cobertura de la máxima demanda por tipo de generación

        public ActionResult ComparacionCoberturaMaxDemanda(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexComparacionCoberturaMaxDemanda, codigoVersion);

            return View(model);
        }

        [HttpPost]
        public JsonResult ConsultarComparacionCoberturaMaxDemanda(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);

            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeEjecutivoMensual.IndexComparacionCoberturaMaxDemanda,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioSiosein2.ListarDataVersionComparacionCoberturaMaxDemanda(objFiltro);
            var model = new Siosein2Model
            {
                Grafico = objReporte.Grafico
            };
            return Json(model);
        }

        #endregion

        #region 1.6. Despacho en el día de máxima potencia coincidente

        public ActionResult DespachoMaxPotenciaCoincidente(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexDespachoMaxPotenciaCoincidente, codigoVersion);

            return View(model);
        }

        [HttpPost]
        public JsonResult ConsultarDespachoMaxPotenciaCoincidente(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            DateTime fechaInicio = objFecha.AnioAct.Fecha_Inicial;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeEjecutivoMensual.IndexDespachoMaxPotenciaCoincidente,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioSiosein2.ListarDataVersionDespachoMaxPotenciaCoincidente(objFiltro);
            var model = new Siosein2Model
            {
                Grafico = objReporte.Grafico,
                Estado = (int)Siosein2Model.TipoEstado.Ok
            };

            return Json(model);
        }

        #endregion

        #region 1.7. Cobertura de la máxima potencia coincidente por tipo de tecnología

        public ActionResult CoberturaMaxPotenciaCoincidenteTecnologia(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexCobMaxPotCoincidenteTecnologia, codigoVersion);

            return View(model);
        }

        [HttpPost]
        public JsonResult ConsultarCoberturaMaxPotenciaCoincidenteTecnologia(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeEjecutivoMensual.IndexCobMaxPotCoincidenteTecnologia,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioSiosein2.ListarDataVersionCoberturaMaxPotenciaCoincidenteTecnologia(objFiltro);

            var model = new Siosein2Model
            {
                Grafico = objReporte.Grafico,
                Resultados = objReporte.ListaMensaje
            };

            return Json(model);
        }

        #endregion

        #region 1.8. Utilización de los recursos energéticos

        public ActionResult UtilizacionRecursosEnergeticos(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexUtilizacionRecursosEnergeticos, codigoVersion);

            return View(model);
        }

        public JsonResult ConsultarUtilizacionRecursosEnergeticos(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeEjecutivoMensual.IndexUtilizacionRecursosEnergeticos,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioSiosein2.ListarDataVersionUtilizacionRecursosEnergeticos(objFiltro);
            var model = new Siosein2Model
            {
                Resultado = UtilEjecMensual.GenerarHtmlUtilizacionRecursosEnergeticoHtml(objReporte.Tabla),
                Resultados = objReporte.ListaMensaje,
                Graficos = new List<GraficoWeb>
            {
                objReporte.GraficoPieMD,
                objReporte.GraficoBarraMD,
                objReporte.GraficoPieGen,
                objReporte.GraficoBarraGen
            }
            };

            return Json(model);
        }

        #endregion

        #region 1.9. Participación de la utilización de los recursos energéticos en la producción de energía eléctrica

        public ActionResult UtilizacionRecursosEnergeticosProduccionElectrica(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexUtilizacionRecEnergeticosProdElec, codigoVersion);

            return View(model);
        }

        public JsonResult ConsultarUtilizacionRecursosEnergeticosProduccionElectrica(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);

            //reporte                        
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeEjecutivoMensual.IndexUtilizacionRecEnergeticosProdElec,
                Verscodi = codigoVersion
            };
            InfSGIReporteVersionado objReporte = _servicioSiosein2.ListarDataVersionParticipacionRREEEjecMensual(objFiltro);
            var model = new Siosein2Model
            {
                Resultado = UtilEjecMensual.GenerarRHtmlUtilizacionRecursosEnergeticoEnProduccion(objReporte.Tabla),
                Grafico = objReporte.Grafico
            };

            return Json(model);
        }

        #endregion

        #endregion

        #region 2. DEMANDA DE ENERGÍA EN BARRAS DE TRANSFERENCIAS DEL SEIN

        #region 2.1 DEMANDA DE ENERGÍA ZONA NORTE 

        [HttpGet]
        public ActionResult DemandaZonaNorte(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexDemandaZonaNorte, codigoVersion);

            return View(model);
        }

        [HttpPost]
        public JsonResult CargarDemandaZonaNorte(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);

            return ObtenerModelDemandaEnergiaBarrasTransf(objFecha, ConstantesSiosein2.MrepcodiDemandaEnergiaZonaNorte);
        }

        private JsonResult ObtenerModelDemandaEnergiaBarrasTransf(FechasPR5 objFecha, int mrepcodi)
        {
            _servicioSiosein2.ObtenerListasDemandaEnergiaBarrasTransf(objFecha, mrepcodi, out string tituloColumna, out List<DemandaEnergiaZona> listaDataDemandaEnergiXZona);

            var model = new Siosein2Model
            {
                Resultado = _servicioSiosein2.GenerarRHtmlDemandaEnergiaZona(listaDataDemandaEnergiXZona, tituloColumna, objFecha.AnioAct.Fecha_Inicial),
                Grafico = _servicioSiosein2.GenerarGWebVariacionAcumuladaDemZona(listaDataDemandaEnergiXZona, tituloColumna, objFecha.AnioAct.Fecha_Inicial)
            };
            return Json(model);
        }

        #endregion

        #region 2.2 DEMANDA DE ENERGÍA ZONA CENTRO

        [HttpGet]
        public ActionResult DemandaZonaCentro(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexDemandaZonaCentro, codigoVersion);

            return View(model);
        }

        [HttpPost]
        public JsonResult CargarDemandaZonaCentro(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);

            return ObtenerModelDemandaEnergiaBarrasTransf(objFecha, ConstantesSiosein2.MrepcodiDemandaEnergiaZonaCentro);
        }

        #endregion 

        #region 2.3 DEMANDA DE ENERGÍA ZONA SUR

        [HttpGet]
        public ActionResult DemandaZonaSur(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexDemandaZonaSur, codigoVersion);

            return View(model);
        }

        [HttpPost]
        public JsonResult CargarDemandaZonaSur(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);

            return ObtenerModelDemandaEnergiaBarrasTransf(objFecha, ConstantesSiosein2.MrepcodiDemandaEnergiaZonaSur);
        }

        #endregion

        #endregion

        #region 3. HIDROLOGÍA PARA LA OPERACIÓN DEL SEIN

        #region 3.1. VOLÚMEN UTIL DE LOS EMBALSES Y LAGUNAS (Millones de m3)

        [HttpGet]
        public ActionResult VolumenEmbLag(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexVolumenEmbLag, codigoVersion);

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
            Siosein2Model model = new Siosein2Model();

            //filtros
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            //reporte                        
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeEjecutivoMensual.IndexVolumenEmbLag,
                Verscodi = codigoVersion
            };
            InfSGIReporteVersionado objReporte = _servicioPR5.ListarDataVersionVolumenUtilEmbalsesLagunas(objFiltro);

            TablaReporte dataTabla = UtilSemanalPR5.ObtenerDataTablaVolumenUtilEmbalsesLagunas(objFecha, objReporte.ListaPtoEmbalsesLagunas, objReporte.ListaDataXPto);
            model.Resultado = UtilSemanalPR5.GenerarRHtmlVolumenUtilEmbalsesLagunas(objFecha, dataTabla);

            return Json(model);
        }

        #endregion

        #region 3.2 EVOLUCION DE LOS VOLUMENES (m3/s)

        [HttpGet]
        public ActionResult IndexMenEvolVolumenes(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexEvolucionVolumenes, codigoVersion);

            return View(model);
        }

        public JsonResult CargarListaEvolEmbalesLagunas(int codigoVersion)
        {
            Siosein2Model model = new Siosein2Model();

            //filtros
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeEjecutivoMensual.IndexEvolucionVolumenes,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioPR5.ListarDataVersionEvolucionVolumenUtilSemanal(objFiltro);
            model.Graficos = new List<GraficoWeb>();
            model.Graficos.AddRange(objReporte.ListaGrafico);

            return Json(model);
        }

        #endregion

        #region 3.3 PROMEDIO MENSUAL DE LOS CAUDALES (m3/s)

        [HttpGet]
        public ActionResult PromedioCaudales(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexPromedioCaudales, codigoVersion);

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
            Siosein2Model model = new Siosein2Model();

            //filtros
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeEjecutivoMensual.IndexPromedioCaudales,
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

        #region 3.4 EVOLUCION DE LOS CAUDALES

        [HttpGet]
        public ActionResult IndexMenEvolCaudades(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexEvolucionCaudales, codigoVersion);

            return View(model);
        }

        public JsonResult CargarListaPromMensualEvolCaudales(int codigoVersion)
        {
            Siosein2Model model = new Siosein2Model();

            //filtros
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeEjecutivoMensual.IndexEvolucionCaudales,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioPR5.ListarDataVersionEvolucionCaudales(objFiltro);
            model.Graficos = objReporte.ListaGrafico;

            return Json(model);
        }

        #endregion

        #endregion

        #region 4. INTERCONEXIONES

        [HttpGet]
        public ActionResult Interconexiones(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexInterconexiones, codigoVersion);

            return View(model);
        }

        [HttpPost]
        public JsonResult ConsultaInterconexiones(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            DateTime fechaInicio = objFecha.AnioAct.Fecha_Inicial;
            DateTime fechaFin = objFecha.AnioAct.Fecha_Final;

            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeEjecutivoMensual.IndexInterconexiones,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioSiosein2.ListarDataVersionInterconexiones(objFiltro);
            var model = new Siosein2Model
            {
                Resultados = new List<string>()
            {
                _servicioSiosein2.GenerarRHtmlFlujoMaximoInterconexiones(objReporte.ListaPotenciaMaximaTransNor,objReporte.ListaPuntosCenNor,fechaInicio,fechaFin,"NORTE"),
                _servicioSiosein2.GenerarRHtmlFlujoMaximoInterconexiones(objReporte.ListaPotenciaMaximaTransSur,objReporte.ListaPuntosCenSur,fechaInicio,fechaFin,"SUR")
            },

                Graficos = new List<GraficoWeb>()
            {
                _servicioSiosein2.GenerarGwebPotenciaMaxTrnasmitida(objReporte.ListaPotenciaMaximaTransNor,objReporte.ListaPuntosCenNor,fechaInicio,fechaFin,"NORTE"),
                _servicioSiosein2.GenerarGwebPotenciaMaxTrnasmitida(objReporte.ListaPotenciaMaximaTransSur,objReporte.ListaPuntosCenSur,fechaInicio,fechaFin,"SUR")
            }
            };

            return Json(model);
        }

        #endregion

        #region 5. HORAS CONGESTION EN LOS PRINCIPALES EQUIPOS DE TRANSMISIÓN

        [HttpGet]
        public ActionResult CongestionEqTransmision(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexCongestionEqTransmision, codigoVersion);

            return View(model);
        }

        [HttpPost]
        public JsonResult ConsultaCongestionEqTransmision(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);

            DateTime fechaInicio = objFecha.AnioAct.Fecha_Inicial;
            DateTime fechaFin = objFecha.AnioAct.Fecha_Final;
            fechaInicio = fechaInicio.AddYears(-1);
            objFecha.FechaInicial = fechaInicio;
            objFecha.FechaFinal = fechaFin;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeEjecutivoMensual.IndexCongestionEqTransmision,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioSiosein2.ListarDataVersionCongestionConjunto(objFiltro);

            var model = new Siosein2Model
            {
                Resultado = UtilEjecMensual.ListaHorasCongestionEquiposTransmisioHTML(objReporte.Tabla),
                Grafico = objReporte.Grafico
            };

            return Json(model);
        }

        #endregion

        #region 6. EVOLUCIÓN DE LOS COSTOS MARGINALES

        #region 6.1. EVOLUCIÓN DEL COSTO MARGINAL EN BARRA DE REFERENCIA

        public ActionResult EvolucionCMGbarra(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexEvolucionCMGbarra, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// carga lista EvolucionCMGbarra
        /// </summary>
        /// <returns></returns>
        public JsonResult CargarEvolucionCMGbarra(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            DateTime fechaFin = objFecha.AnioAct.Fecha_Final;

            _servicioSiosein2.ListarEvolucionCMgBarraReferencia(objFecha, out List<WbCmvstarifaDTO> listaDataEvolucionCMG);

            var model = new Siosein2Model
            {
                Grafico = _servicioSiosein2.GenerarGWebGraficoEvolucionCMGbarra(listaDataEvolucionCMG),
                Resultado = _servicioSiosein2.GenerarRHtmlEvolucionCMGbarra(listaDataEvolucionCMG, fechaFin)
            };

            return Json(model);
        }

        #endregion

        #region 6.2. COSTOS MARGINALES DE LOS PRINCIPALES MODOS DE OPERACIÓN

        public ActionResult CostosMarginalesModoOpe(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexCostosMarginalesModoOpe, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// carga lista CostosMarginalesModoOpe
        /// </summary>
        /// <returns></returns>
        public JsonResult CargarCostosMarginalesModoOpe(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            DateTime fechaFin = objFecha.AnioAct.Fecha_Final;

            _servicioSiosein2.ListarCMgModoOperacion(objFecha, out List<PrCvariablesDTO> listaCostoVariableUsdMwh, out List<SiCostomarginalDTO> listaCmgStaRosaUsdMwh);

            var modelo = new Siosein2Model
            {
                Graficos = new List<GraficoWeb>()
                {
                    _servicioSiosein2.GenerarGWebCostosMarginalesModoOpe(listaCostoVariableUsdMwh.Where(x=>x.Mocmtipocomb == ConstantesSiosein2.MocmtipocombLiquido), fechaFin, "EVOLUCIÓN DE LOS COSTOS VARIABLES PROMEDIOS"),
                    _servicioSiosein2.GenerarGWebCostosMarginalesModoOpeYCMGStaRosa(listaCostoVariableUsdMwh.Where(x => x.Mocmtipocomb == ConstantesSiosein2.MocmtipocombGasNatural), fechaFin, listaCmgStaRosaUsdMwh)
                }
            };

            return Json(modelo);
        }

        #endregion

        #region 6.3.  COSTOS MARGINALES EN LAS PRINCIPALES BARRAS DEL SEIN (US$/MWh)

        public ActionResult CostosMarginalesBarrasSein(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexCostosMarginalesBarrasSein, codigoVersion);

            return View(model);
        }

        [HttpPost]
        public JsonResult ConsultaCostosMarginalesBarrasSein(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            DateTime fechaFin = objFecha.AnioAct.Fecha_Final;

            //_servicioSiosein2.ListarCMgBarrasPrincipales(objFecha, out List<CostoMarginalDTO> listaDataCmgXZona);

            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                Mrepcodi = ConstantesInformeEjecutivoMensual.IndexCostosMarginalesBarrasSein,
                Verscodi = codigoVersion,
                ObjFecha = objFecha
            };

            InfSGIReporteVersionado objReporte = _servicioSiosein2.ListarDataCMgBarrasPrincipales(objFiltro);

            var lstCmgNorte = objReporte.listaDataCmgXZona.Where(x => x.Barrzarea == (int)ConstantesSiosein2.BarraZona.Norte).ToList();
            var lstCmgCentro = objReporte.listaDataCmgXZona.Where(x => x.Barrzarea == (int)ConstantesSiosein2.BarraZona.Centro).ToList();
            var lstCmgSur = objReporte.listaDataCmgXZona.Where(x => x.Barrzarea == (int)ConstantesSiosein2.BarraZona.Sur).ToList();

            var model = new Siosein2Model
            {
                Resultados = new List<string>()
                {
                    _servicioSiosein2.GenerarRHtmlCostosMarginalesBarrasSein(-1, "Norte", lstCmgNorte, fechaFin),
                    _servicioSiosein2.GenerarRHtmlCostosMarginalesBarrasSein(-1, "Centro", lstCmgCentro, fechaFin),
                    _servicioSiosein2.GenerarRHtmlCostosMarginalesBarrasSein(-1, "Sur", lstCmgSur, fechaFin),
                },

                Graficos = new List<GraficoWeb>()
                {
                    _servicioSiosein2.GenerarGWebCostosMarginalesBarrasSein(-1, "Norte", lstCmgNorte, "COSTOS MARGINALES NORTE", fechaFin),
                    _servicioSiosein2.GenerarGWebCostosMarginalesBarrasSein(-1, "Centro", lstCmgCentro, "COSTOS MARGINALES CENTRO", fechaFin),
                    _servicioSiosein2.GenerarGWebCostosMarginalesBarrasSein(-1, "Sur", lstCmgSur, "COSTOS MARGINALES SUR", fechaFin),
                }
            };

            return Json(model);
        }

        #endregion

        #endregion

        #region 7. MANTENIMIENTOS EJECUTADOS

        #region 7.1.  Mantenimientos Ejecutados

        [HttpGet]
        public ActionResult MantenimientosEjecutados(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexMantenimientosEjecutados, codigoVersion);

            return View(model);
        }

        [HttpPost]
        public JsonResult ConsultaMantenimientosEjecutados(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            DateTime fechaInicio = objFecha.AnioAct.Fecha_Inicial;
            DateTime fechaFin = objFecha.AnioAct.Fecha_Final;

            _servicioSiosein2.ListarMantenimientoEjecutadoEjecMensual(objFecha, out List<EveManttoDTO> listaEjeMantto, out List<EveManttoDTO> listaProgMantto);

            var model = new Siosein2Model
            {
                Resultado = _servicioSiosein2.ListaMantenimientoEjecutadoHtml(fechaInicio, fechaFin, listaEjeMantto, listaProgMantto),
                Grafico = _servicioSiosein2.GenerarGWebCumplimientosManttoxEmpresa(fechaInicio, fechaFin, listaEjeMantto, listaProgMantto)
            };

            return Json(model);
        }

        #endregion		

        #endregion

        #region 8. TRANSFERENCIAS DE ENERGIA Y POTENCIA

        #region 8.1. TRANSFERENCIAS DE ENERGIA ACTIVA

        public ActionResult TransferenciaEnergiaActiva(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexTransferenciaEnergiaActiva, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// carga lista transferencias de energia
        /// </summary>
        /// <returns></returns>
        public JsonResult CargarTransferenciaEnergiaActiva(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);

            _servicioSiosein2.ListarTransfEnergActiva(objFecha, out List<TransferenciaEntregaDetalleDTO> dataTransferenciaEntrega,
                    out List<TransferenciaRetiroDetalleDTO> dataTransferenciaRetiro, out List<ValorTransferenciaDTO> dataValorTransferencia);

            var model = new Siosein2Model()
            {
                NRegistros = dataTransferenciaEntrega.Count,
                Resultados = new List<string>()
                    {
                        _servicioSiosein2.GenerarRHtmlTranferenciaBalanceEnergia(dataTransferenciaEntrega, dataTransferenciaRetiro),
                        _servicioSiosein2.GenerarRHtmlTranferenciaEnergiaSoles(dataValorTransferencia),
                    }
            };

            return Json(model);
        }

        #endregion

        #region 8.2. TRANSFERENCIAS DE POTENCIA

        public ActionResult TransferenciaPotencia(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexTransferenciaPotencia, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// carga lista transferencias de energia
        /// </summary>
        /// <returns></returns>
        public JsonResult CargarTransferenciaPotencia(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            DateTime fechaInicio = objFecha.AnioAct.Fecha_Inicial;
            DateTime fechaFin = objFecha.AnioAct.Fecha_Final;

            _servicioSiosein2.ListarTransfPotenciaEjecMensual(objFecha, out List<Tuple<DateTime, decimal?, decimal?, decimal?>> lstData,
                                                out List<Tuple<string, decimal, decimal, decimal, decimal?>> lstData_);

            var model = new Siosein2Model
            {
                Resultados = new List<string> {
                    _servicioSiosein2.GenerarGHtmlMaxDemandaPFirmerYPFirmeRemunerable(lstData, fechaInicio, fechaFin),
                    _servicioSiosein2.GenerarGHtmlPotenciaConsumida(lstData_)
                },
                Grafico = _servicioSiosein2.GraficoTransferenciaPotencia(lstData, fechaInicio, fechaFin)
            };

            return Json(model);
        }

        #endregion

        #region 8.3.  Valorizacion de las transferencias de Potencia (Soles)

        public ActionResult ValorizacionTransfPotencia(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexValorizacionTransfPotencia, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// carga lista transferencias de energia
        /// </summary>
        /// <returns></returns>
        public JsonResult CargarValorizacionTransfPotencia(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            _servicioSiosein2.ListarValorizacion(objFecha, out List<VtpSaldoEmpresaDTO> lstVtpSaldosEmpresa, out List<VtpSaldoEmpresaDTO> lstVtpPeriodos,
                                        out List<VtpSaldoEmpresaDTO> lstVtpSaldosAntEmpresa);

            var model = new Siosein2Model
            {
                Resultado = _servicioSiosein2.GenerarRHtmlValorizacionTransfPotencia(lstVtpSaldosEmpresa, lstVtpPeriodos, lstVtpSaldosAntEmpresa)
            };

            return Json(model);
        }

        #endregion

        #region 8.4.  Potencia Firme por Empresas (MW)

        public ActionResult PotenciaFirmeEmpresas(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexPotenciaFirmeEmpresas, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// carga lista Potencia Firme por Empresas
        /// </summary>
        /// <returns></returns>
        public JsonResult CargarPotenciaFirmeEmpresas(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            DateTime fechaInicio = objFecha.AnioAct.Fecha_Inicial;
            DateTime fechaFin = objFecha.AnioAct.Fecha_Final;
            _servicioSiosein2.ListarPotenciaFirmeEjecMensual(objFecha, out List<VtpIngresoPotefrDTO> listaIngresoPotefr, out List<VtpIngresoPotefrDetalleDTO> listaIngresoPot);

            var model = new Siosein2Model
            {
                Resultado = _servicioSiosein2.GenerarRHtmlPotenciaFirmeEmpresas(listaIngresoPotefr, listaIngresoPot, fechaInicio, fechaFin),
                NRegistros = 1
            };

            return Json(model);
        }

        #endregion

        #endregion

        #region 9. COMPENSACION A TRANSMISORAS

        #region 9.1 Compensacion a Transmisoras por Peaje de conexion y transmision, Sistema principal y Sistema Garantizado de Transmision

        public ActionResult CompensacionPeajeConexTransmision(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexCompensacionPeajeConexTransmision, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Reporte compensación a transmisoras por peaje de conexión y transmision, sistema principal y sistema garantizado de transmisión  
        /// </summary>
        /// <returns></returns>
        public JsonResult CargarCompensacionPeajeConexTransmision(int codigoVersion, int tip)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            DateTime fechaFin = objFecha.AnioAct.Fecha_Final;
            _servicioSiosein2.ListarCompensacionATransmisora(objFecha, out List<VtpPeajeEmpresaPagoDTO> listaPeajeActual, out _);

            var model = new Siosein2Model();
            switch (tip)
            {
                case 1://TABLA HTML
                    model.Resultado = _servicioSiosein2.GenerarRHtmlCompensacionPeajeConexTransmision(listaPeajeActual);
                    break;
                case 2://GRAFICO
                    model.Grafico = _servicioSiosein2.CargarGraficoCompensacionPeajeConexTransmision(listaPeajeActual, fechaFin);
                    break;
            }

            return Json(model);
        }

        #endregion

        #region 9.2 Porcentaje de Compensacion a Transmisoras por Peaje de conexion y transmision

        public ActionResult PorcentajeCompPeajeConexTransmision(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexPorcentajeCompPeajeConexTransmision, codigoVersion);

            return View(model);
        }

        #endregion

        #region 9.3 Compensacion a Transmisoras por Ingreso tarifario del Sistema principal y Sistema Garantizado de Transmision

        public ActionResult CompensacionIngresoTarifario(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexCompensacionIngresoTarifario, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Reportes compensación a transmisoras por ingreso tarifario del sistema principal y sistema garantizado de transmisión
        /// </summary>
        /// <returns></returns>
        public JsonResult CargarCompensacionIngresoTarifario(int codigoVersion, int tip)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            DateTime fechaFin = objFecha.AnioAct.Fecha_Final;
            _servicioSiosein2.ListarPorcentajeCompensacionATransmisora(objFecha, out List<VtpIngresoTarifarioDTO> lstVtpIngresoAct, out _);

            var model = new Siosein2Model();
            switch (tip)
            {
                case 1://Tabla HTML
                    model.Resultado = _servicioSiosein2.GenerarRHtmlCompensacionIngresoTarifario(lstVtpIngresoAct);
                    break;
                case 2://Grafico
                    model.Grafico = _servicioSiosein2.GenerearGWebCompensacionIngresoTarifario(lstVtpIngresoAct, fechaFin);
                    break;
            }
            model.NRegistros = lstVtpIngresoAct.Count;

            return Json(model);
        }

        #endregion

        #region 9.4 Porcentaje de Compensacion por Ingreso y tarifario

        public ActionResult PorcentajeCompIngresoTarifario(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexPorcentajeCompIngresoTarifario, codigoVersion);

            return View(model);
        }

        #endregion

        #endregion

        #region 10. Eventos y Fallas que ocasionaron interrupcion

        public ActionResult EventoFallaSuministroElect(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexEventoFallaSuministroElect, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// cargar eventos de fallas siministro de energ para los anexos
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult GetEventoFallaSuministroElect(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeEjecutivoMensual.IndexEventoFallaSuministroElect,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioSiosein2.ListarDataVersionEventoFallaSuministroElect(objFiltro);

            Siosein2Model model = new Siosein2Model
            {
                Resultado = _servicioSiosein2.GenerarRHtmlEventoFallaSuministroElect(objReporte.ListaEventos)
            };

            return Json(model);
        }

        #endregion

        #region 11. Fallas por tipo de equipo y causa segun clasificacion

        public ActionResult FallaTipoequipoCausa(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexFallaTipoequipoCausa, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Evento, Falla de Suministros de Energ
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult GetFallaTipoequipoCausa(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeEjecutivoMensual.IndexFallaTipoequipoCausa,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioPR5.ListarDataVersionEventoFallaSuministroEnerg(objFiltro);

            Siosein2Model model = new Siosein2Model
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

        #region 11.2. Energía interrumpida (MWh) por fallas en las diferentes zonas del sistema eléctrico.

        public ActionResult EnergiaInterumpidaFallasZonas(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexEnergiaInterumpidaFallasZonas, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Retornar JsonResult de Energía interrumpida (MWh) por fallas en las diferentes zonas del sistema eléctrico.
        /// </summary>
        /// <returns></returns>
        public JsonResult GetEnergiaInterumpidaFallasZonas(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeEjecutivoMensual.IndexEventoFallaSuministroElect,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioSiosein2.ListarDataVersionEventoFallaSuministroElect(objFiltro);

            Siosein2Model model = new Siosein2Model
            {
                Resultado = _servicioSiosein2.GenerarRHtmlEnergiaInterumpidaFallasZonas(objReporte.ListaEventos, objFecha.FechaInicial, objFecha.FechaFinal),
                Grafico = _servicioSiosein2.GenerarGWebEnergiaInterumpidaFallasZonas(objReporte.ListaEventos, objFecha.FechaInicial, objFecha.FechaFinal)
            };

            return Json(model);
        }

        #endregion

        #region 12. EMPRESAS INTEGRANTES DEL COES

        #region 12.1. EVOLUCIÓN DE INTEGRANTES DEL COES

        public ActionResult EvolucionIntegrantesCoes(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexEvolucionIntegrantesCoes, codigoVersion);

            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            //reporte                        
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeEjecutivoMensual.IndexEvolucionIntegrantesCoes,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioSiosein2.ListarDataVersionEvolucionIntegrantesCoes(objFiltro);
            ViewBag.ListaEvolucionIntegr = objReporte.LstEvolIntgrCoes;

            return View(model);
        }

        #endregion

        #region 12.2. INGRESO DE EMPRESAS INTEGRANTES AL COES

        public ActionResult IngresoEmprIntegrAlCoes(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexIngresoEmprIntegrAlCoes, codigoVersion);

            return View(model);
        }

        [HttpPost]
        public JsonResult ConsultarIngresoEmprIntegrAlCoes(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            DateTime fechaInicio = ConstantesSiosein2.FechaInicioEmpresaIntegrante;
            DateTime fechaFin = objFecha.AnioAct.Fecha_Final;
            //reporte                        
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeEjecutivoMensual.IndexIngresoEmprIntegrAlCoes,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioSiosein2.ListarDataVersionIngresoEmprIntegrAlCoes(objFiltro);
            Siosein2Model model = new Siosein2Model
            {
                Resultado = _servicioSiosein2.GenerarRHtmlIngresoEmprIntegrAlCoes(fechaInicio, fechaFin, objReporte.ListEmpresasEvl)
            };

            return Json(model);
        }

        #endregion

        #region 12.3. RETIRO DE EMPRESAS INTEGRANTES DEL COES

        public ActionResult RetiroEmprIntegrDelCoes(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexRetiroEmprIntegrDelCoes, codigoVersion);

            return View(model);
        }

        [HttpPost]
        public JsonResult ConsultarRetiroEmprIntegrDelCoes(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            DateTime fechaInicio = ConstantesSiosein2.FechaInicioEmpresaIntegrante;
            DateTime fechaFin = objFecha.AnioAct.Fecha_Final;
            //reporte                        
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeEjecutivoMensual.IndexRetiroEmprIntegrDelCoes,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioSiosein2.ListarDataVersionRetiroEmprIntegrDelCoes(objFiltro);
            Siosein2Model model = new Siosein2Model
            {
                Resultado = _servicioSiosein2.GenerarRHtmlIngresoEmprIntegrAlCoes(fechaInicio, fechaFin, objReporte.ListEmpresasEvl)
            };

            return Json(model);
        }

        #endregion

        #region 12.4. CAMBIO DE DENOMINACIÓN Y FUSIÓN DE EMPRESAS INTEGRANTES DEL COES

        public ActionResult CambioDenomFusionEmprIntegrCoes(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();

            Siosein2Model model = GetModelGenericoIndex(ConstantesInformeEjecutivoMensual.IndexCambioDenomFusionEmprIntegrCoes, codigoVersion);

            return View(model);
        }

        [HttpPost]
        public JsonResult ConsultarCambioDenomFusionEmprIntegrCoes(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha(codigoVersion);
            DateTime fechaInicio = ConstantesSiosein2.FechaInicioEmpresaIntegrante;
            DateTime fechaFin = objFecha.AnioAct.Fecha_Final;
            //reporte                        
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeEjecutivoMensual.IndexCambioDenomFusionEmprIntegrCoes,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioSiosein2.ListarDataVersionCambioDenomFusionEmprIntegrCoes(objFiltro);
            Siosein2Model model = new Siosein2Model
            {
                Resultado = _servicioSiosein2.GenerarRHtmlConsultarCambioDenomFusionEmprIntegrCoes(fechaInicio, fechaFin, objReporte.ListRiHistorico)
            };

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
                    var objVersion = _servicioSiosein.GetByIdSiVersion(idVersion);
                    DateTime dFechaPeriodo = objVersion.Versfechaperiodo;
                    string fileName = string.Format(ConstantesPR5ReportesServicio.ArchivoInformeEjecutivoMensualPDF, EPDate.f_NombreMes(dFechaPeriodo.Month), dFechaPeriodo.Year, idVersion);

                    //string fileName = string.Format(ConstantesPR5ReportesServicio.ArchivoInformeSemanalPDF, idVersion);

                    if (!FileServer.VerificarExistenciaDirectorio(ConstantesPR5ReportesServicio.PathArchivosInformeEjecutivoMensualPDF, string.Empty))
                        FileServer.CreateFolder(ConstantesPR5ReportesServicio.PathArchivosInformeEjecutivoMensualPDF, string.Empty, string.Empty);

                    if (FileServer.VerificarExistenciaFile(ConstantesPR5ReportesServicio.PathArchivosInformeEjecutivoMensualPDF, fileName,
                        string.Empty))
                        FileServer.DeleteBlob(ConstantesPR5ReportesServicio.PathArchivosInformeEjecutivoMensualPDF + fileName,
                            string.Empty);

                    FileServer.UploadFromStream(file.InputStream, ConstantesPR5ReportesServicio.PathArchivosInformeEjecutivoMensualPDF, fileName,
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
            var objVersion = _servicioSiosein.GetByIdSiVersion(idVersion);
            DateTime dFechaPeriodo = objVersion.Versfechaperiodo;
            string fileName = string.Format(ConstantesPR5ReportesServicio.ArchivoInformeEjecutivoMensualPDF, EPDate.f_NombreMes(dFechaPeriodo.Month), dFechaPeriodo.Year, idVersion);
            Stream stream = FileServer.DownloadToStream(ConstantesPR5ReportesServicio.PathArchivosInformeEjecutivoMensualPDF + fileName,
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

            string folder = _servicioPR5.GetCarpetaInformeEjecutivoMensual();
            var objVersion = _servicioPR5.GetByIdSiVersion(idVersion);

            string nombreArchivo = _servicioSiosein2.GetNombreArchivoInformeEjecutivoMensual(objVersion.Versfechaperiodo, idVersion);

            //if (tipo == "E") nombreArchivo = servicio.GetNombreArchivoInformeSemanal("", fechaInicio, fechaFin, idVersion);
            //if (tipo == "L") nombreArchivo = servicio.GetNombreArchivoLogInformeSemanal(fechaInicio, fechaFin, idVersion);

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
            var objVersion = _servicioSiosein.GetByIdSiVersion(idVersion);
            DateTime dFechaPeriodo = objVersion.Versfechaperiodo;
            string fileName = string.Format(ConstantesPR5ReportesServicio.ArchivoInformeEjecutivoMensualPDF, EPDate.f_NombreMes(dFechaPeriodo.Month), dFechaPeriodo.Year, idVersion);
            byte[] stream = FileServer.DownloadToArrayByte(ConstantesPR5ReportesServicio.PathArchivosInformeEjecutivoMensualPDF + fileName,
                string.Empty);
            string mimeType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "inline; filename=" + fileName);
            return File(stream, mimeType);
        }

        #endregion

        #region UTIL

        public JsonResult ListarGraficosReporte(int reporcodi)
        {
            Siosein2Model model = new Siosein2Model();

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
            Siosein2Model model = new Siosein2Model();

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
            Siosein2Model model = new Siosein2Model();

            try
            {
                string folder = _servicioPR5.GetCarpetaInformeEjecutivoMensual();
                var objVersion = _servicioSiosein.GetByIdSiVersion(idVersion);
                DateTime fechaInicio = objVersion.Versfechaperiodo;
                DateTime fechaFin = fechaInicio.AddDays(6);
                string nombreArchivo = _servicioSiosein2.GetNombreArchivoInformeEjecutivoMensual(fechaInicio, idVersion);

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

        public ActionResult VistaPortal()
        {
            return View();
        }

        public ActionResult ListaDistribucion()
        {
            return View();
        }

    }
}