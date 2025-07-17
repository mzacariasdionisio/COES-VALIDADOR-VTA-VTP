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
    public class InformesAnualController : BaseController
    {
        private readonly SIOSEINAppServicio _serviceSiosein = new SIOSEINAppServicio();
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

        #region Menu Informe Anual

        /// <summary>
        /// index reportes anuales de informes 
        /// de operacion del sein
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuInformeAnual(string anio)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            SioseinModel model = new SioseinModel();

            int anioConsulta = DateTime.Today.Year - 1;
            if (!string.IsNullOrEmpty(anio)) anioConsulta = Convert.ToInt32(anio);

            model.Anho = anioConsulta.ToString();

            return View(model);
        }

        /// <summary>
        /// lista de menu para anual y mensual
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarMenu()
        {
            SioseinModel model = new SioseinModel();

            List<SiMenureporteDTO> listaItems = _servicioPR5.GetListaAdmReporte(ConstantesSioSein.ReptipcodiAnual);
            model.Menu = UtilAnexoAPR5.ListaMenuHtml(listaItems);

            return Json(model);
        }

        #endregion

        #region Útil

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
                return RedirectToAction("MenuAnual", "Siosein/InformesAnual", new { area = string.Empty });
            }
        }

        /// <summary>
        /// Permite retornar informacion basica a mostrar en la vista
        /// </summary>
        /// <returns></returns>
        private SioseinModel GetModelGenericoIndex(int repcodi, int verscodi)
        {
            SioseinModel model = new SioseinModel();

            //version seleccionada
            SiVersionDTO objVersion = _serviceSiosein.GetByIdSiVersion(verscodi);
            List<SiVersionDTO> listaVersion = _serviceSiosein.ListaVersionByFecha(objVersion.Versfechaperiodo, ConstantesPR5ReportesServicio.ReptipcodiInformeAnual);
            model.ListaVersion = listaVersion;
            model.Verscodi = verscodi;

            model.Anho = objVersion.Versfechaperiodo.Year.ToString();

            //numeral seleccionado
            model.Repcodi = repcodi;
            model.Idnumeral = repcodi;
            model.Tiporeporte = ConstantesSiosein.TipoInformeAnual;

            SiMenureporteDTO objItem = _servicioPR5.GetByIdMenuReporte(repcodi);
            model.TituloWeb = objItem.Mreptituloweb;

            model.Url = Url.Content("~/");

            return model;
        }

        private FechasPR5 GetObjetFechaUltimoMes(int codigoVersion)
        {
            SiVersionDTO objVersion = _serviceSiosein.GetByIdSiVersion(codigoVersion);

            FechasPR5 objFecha = UtilInfMensual.ObtenerFechasInformesAnualUltimoMes(objVersion.Versfechaperiodo);

            return objFecha;
        }

        private FechasPR5 GetObjetFecha12Meses(int codigoVersion)
        {
            SiVersionDTO objVersion = _serviceSiosein.GetByIdSiVersion(codigoVersion);

            FechasPR5 objFecha = UtilInfMensual.ObtenerFechasInformesAnual12Meses(objVersion.Versfechaperiodo);

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
                ListaDetalleProduccion = _serviceSiosein.ListarDetalleResumenProduccionGeneracion(fechaInicial, fechaFinal, filtroRER)
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
                ListaDetalleProduccion = _serviceSiosein.ListarDetalleResumenProduccionGeneracionMD(fechaInicial, fechaFinal, fechaMD)
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
                ListaDetalleInterconexion = _serviceSiosein.ListarDetalleResumenInterconexion(fechaInicial, fechaFinal)
            };

            return Json(model);
        }

        [HttpPost]
        public JsonResult CargarDetalleTablaInterconexionMD(string sFechaMaximaDemanda)
        {
            DateTime fechaMD = DateTime.ParseExact(sFechaMaximaDemanda, ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);

            SioseinModel model = new SioseinModel
            {
                ListaDetalleInterconexion = _serviceSiosein.ListarDetalleResumenInterconexionMD(fechaMD)
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

                _serviceSiosein.GenerarRptExcelResumenProduccionGeneracion(ruta, fechaInicial, fechaFinal, filtroRER, out string nameFile);

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

                _serviceSiosein.GenerarRptExcelResumenProduccionGeneracionMD(ruta, fechaInicial, fechaFinal, fechaMD, out string nameFile);

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

                _serviceSiosein.GenerarRptExcelResumenDetalleInterconexion(ruta, fechaInicial, fechaFinal, out string nameFile);

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

                _serviceSiosein.GenerarRptExcelResumenDetalleInterconexionMD(ruta, fechaInicial, fechaFinal, fechaMD, out string nameFile);

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
            GestorModel model = new GestorModel();

            try
            {
                this.ValidarSesionJsonResult();
                DateTime dFechaPeriodo = new DateTime(Convert.ToInt32(fechaPeriodo), 1, 1);

                //Validar existencia de pdf
                model.ListaVersion = _servicioPR5.ListaVersionByFechaInformeAnual(dFechaPeriodo);
                foreach (var item in model.ListaVersion)
                {
                    string fileName = string.Format(ConstantesPR5ReportesServicio.ArchivoInformeAnualPDF, dFechaPeriodo.Year, item.Verscodi);
                    if (FileServer.VerificarExistenciaFile(ConstantesPR5ReportesServicio.PathArchivosInformeAnualPDF, fileName,
                        string.Empty))
                        item.TienePdf = true;
                }

                //Obtener las fechas que se usan para los cálculos
                FechasPR5 objFecha = UtilSemanalPR5.ObtenerFechasInformeSemanal(dFechaPeriodo, dFechaPeriodo.AddDays(6));
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
            GestorModel model = new GestorModel();
            try
            {
                this.ValidarSesionJsonResult();
                DateTime dFechaPeriodo = new DateTime(Convert.ToInt32(fechaPeriodo), 1, 1);
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
                        Tmrepcodi = ConstantesPR5ReportesServicio.ReptipcodiInformeAnual,
                        Versmotivo = motivo,
                        Versfechaversion = DateTime.Now,
                        Versusucreacion = base.UserName,
                        Versfeccreacion = DateTime.Now
                    };

                    int verscodi = _servicioPR5.SaveSiVersion(objVersion);

                    //Cargar datos de energia y potencia instalada
                    _serviceSiosein.GenerarInsumoInfMensual(new DateTime(dFechaPeriodo.Year, 12, 1), base.UserName);

                    //Guardar versión
                    _serviceSiosein.GuardarVersionInfAnual(verscodi, dFechaPeriodo);

                    //Genera Excel                    
                    _serviceSiosein.GenerarArchivoExcelGeneralAnual(verscodi, out string fileName);
                    _serviceSiosein.MoverArchivoInformeAnualFileServer(fileName);
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

        #region Excel individual

        /// <summary>
        /// Permite la exportacion en archivo excel del informe anual
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult GenerarReporteAnual(int reporcodi, int codigoVersion)
        {
            var model = new SioseinModel();

            try
            {
                this.ValidarSesionJsonResult();

                string directorioTemporal = AppDomain.CurrentDomain.BaseDirectory + ConstantesPR5ReportesServicio.PathArchivoExcel;
                FileInfo plantillaExcel = new FileInfo(string.Concat(directorioTemporal, ConstantesPR5ReportesServicio.PantillaExcelInformeAnual));

                //var nombreArchivoExcelExport = _serviceSiosein.GenerarArchivoExcelInformeAnual(codigoVersion, plantillaExcel, reporcodi);
                var nombreArchivoExcelExport = _serviceSiosein.GenerarArchivoExcelIndividualAnual(codigoVersion, reporcodi);
                model.Resultado = nombreArchivoExcelExport;
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
        /// Permite descargar archivo excel del Reporte  Mensual
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

        #region 1. RESUMEN

        #region 1.1 Producción de energía

        public ActionResult IndexAnualProdEnergia(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();
            SioseinModel model = GetModelGenericoIndex(ConstantesInformeAnual.IndexAnualProduccionEnergia, codigoVersion);
            return View(model);
        }

        public JsonResult CargarResumenRelevante(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFecha12Meses(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeAnual.IndexAnualProduccionEnergia,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _serviceSiosein.ListarDataVersionResumenRelevanteMensual(objFiltro);

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

        #endregion

        #region 2. OFERTA DE GENERACIÓN ELÉCTRICA DEL SEIN    

        #region 2.1 Ingreso en Operación Comercial

        public ActionResult IndexAnualIngresoOpComercSEIN(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();
            SioseinModel model = GetModelGenericoIndex(ConstantesInformeAnual.IndexAnualIngresoOpComercSEIN, codigoVersion);
            model.NroMostrar = (int)ConstantesPR5ReportesServicio.TipoOperacion.Ingreso;
            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Ingreso Operacion Anual Comercial SEIN
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaIngresoOpComercSEIN(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFechaUltimoMes(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            int tipoOperacion = 1; // Es Ingreso de operacion
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeAnual.IndexAnualIngresoOpComercSEIN,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = this._serviceSiosein.ListarDataVersionIngresoOpComercSEINMensual(objFiltro, tipoOperacion);
            GraficoWeb graficoPieOp = _serviceSiosein.GenerarGraficoPiePorTipoRer(objReporte.Tabla, objFecha.AnioAct.Fecha_01Enero, tipoOperacion);

            SioseinModel model = new SioseinModel
            {
                Resultado = UtilSemanalPR5.GenerarListadoIngresoRetiroOpComercialSeinHtml(objReporte.Tabla, tipoOperacion),
                Graficos = new List<GraficoWeb>()
                {
                    graficoPieOp,
                    objReporte.Grafico,

                }
            };

            return Json(model);
        }

        #endregion

        #region 2.2. Retiro de Operación Comercial

        public ActionResult IndexAnualRetiroOpComercSEIN(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();
            SioseinModel model = GetModelGenericoIndex(ConstantesInformeAnual.IndexAnualRetiroOpComercSEIN, codigoVersion);
            model.NroMostrar = (int)ConstantesPR5ReportesServicio.TipoOperacion.Retiro;
            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Ingreso Operacion Anual Comercial SEIN
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaRetiroOpComercSEIN(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFechaUltimoMes(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            int tipoOperacion = 2; // Es retiro de operacion
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeAnual.IndexAnualRetiroOpComercSEIN,
                Verscodi = codigoVersion
            };
            InfSGIReporteVersionado objReporte = this._serviceSiosein.ListarDataVersionIngresoOpComercSEINMensual(objFiltro, tipoOperacion);
            GraficoWeb graficoPieOp = _serviceSiosein.GenerarGraficoPiePorTipoRer(objReporte.Tabla, objFecha.AnioAct.Fecha_01Enero, tipoOperacion);

            SioseinModel model = new SioseinModel
            {
                Resultado = UtilSemanalPR5.GenerarListadoIngresoRetiroOpComercialSeinHtml(objReporte.Tabla, tipoOperacion),
                Graficos = new List<GraficoWeb>()
                {
                    graficoPieOp,
                    objReporte.Grafico,

                }
            };

            return Json(model);
        }

        #endregion

        #region 2.3 Potencia instalada en el SEIN

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexAnualPotenciaInstSEIN(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();
            SioseinModel model = GetModelGenericoIndex(ConstantesInformeAnual.IndexAnualPotenciaInstSEIN, codigoVersion);

            model.NroMostrar = (int)ConstantesPR5ReportesServicio.TipoOperacion.Ingreso;
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
            FechasPR5 objFecha = GetObjetFechaUltimoMes(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeAnual.IndexAnualPotenciaInstSEIN,
                Verscodi = codigoVersion
            };
            InfSGIReporteVersionado objReporte = this._serviceSiosein.ListarDataVersionPotenciaInstSEIN(objFiltro);
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

        #region 3. PRODUCCION DE LA ENERGIA ELECTRICA EN EL SEIN

        #region 3.1. Producción por tipo de generación
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexAnualProdTipoGen(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();
            SioseinModel model = GetModelGenericoIndex(ConstantesInformeAnual.IndexAnualProdTipoGen, codigoVersion);
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
            FechasPR5 objFecha = GetObjetFechaUltimoMes(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeAnual.IndexAnualProdTipoGen,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _serviceSiosein.ListarDataVersionReporteProduccionXTgeneracionInfMensual(objFiltro);
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

        #region 3.2 Variación interanual

        public ActionResult IndexAnualVarInteranual(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();
            SioseinModel model = GetModelGenericoIndex(ConstantesInformeAnual.IndexAnualVariacionInteranual, codigoVersion);
            return View(model);
        }

        #endregion

        #region 3.3. Producción por tipo de Recurso Energético

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexAnualProdTipoRecurso(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();
            SioseinModel model = GetModelGenericoIndex(ConstantesInformeAnual.IndexAnualProdTipoRecurso, codigoVersion);
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
            FechasPR5 objFecha = GetObjetFechaUltimoMes(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeAnual.IndexAnualProdTipoRecurso,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _serviceSiosein.ListarDataProduccionXTipoRecursoEnergetico(objFiltro);
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

        #region 3.4. Por Producción RER

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexAnualProdRER(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();
            SioseinModel model = GetModelGenericoIndex(ConstantesInformeAnual.IndexAnualProdRER, codigoVersion);
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
            FechasPR5 objFecha = GetObjetFechaUltimoMes(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeAnual.IndexAnualProdRER,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _serviceSiosein.ListarDataVersionProduccionRERInfMensual(objFiltro);
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

        #region 3.5. Factor de planta RER


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexAnualFactorPlantaRER(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            SioseinModel model = GetModelGenericoIndex(ConstantesInformeAnual.IndexAnualFactorPlantaRER, codigoVersion);

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
            FechasPR5 objFecha = GetObjetFechaUltimoMes(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeAnual.IndexAnualFactorPlantaRER,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _serviceSiosein.ListarDataVersionFactorPlantaCentralesRER(objFiltro);
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

        #region 3.6. Participación de la producción por empresas Integrantes

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexAnualParticipacionEmpresas(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();
            SioseinModel model = GetModelGenericoIndex(ConstantesInformeAnual.IndexAnualProduccionEmpresas, codigoVersion);
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
            FechasPR5 objFecha = GetObjetFechaUltimoMes(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeAnual.IndexAnualProduccionEmpresas,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _serviceSiosein.ListarDataVersionProduccionEmpresasIntegrantesInfMensua(objFiltro);
            SioseinModel model = new SioseinModel
            {
                Resultado = UtilSemanalPR5.ReporteProduccionEmpresasIntegrantesHtml(objReporte.Tabla),
                Grafico = objReporte.Grafico
            };

            return Json(model);
        }

        #endregion

        #endregion

        #region 4. MÁXIMA POTENCIA COINCIDENTE A NIVEL DE GENERACION DEL SEIN

        #region 4.1. MÁXIMA POTENCIA COINCIDENTE POR TIPO DE GENERACIÓN (MW)

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexAnuaMaxPotenciaTipoGeneracion(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();
            SioseinModel model = GetModelGenericoIndex(ConstantesInformeAnual.IndexAnualMaxDemandaTipoGeneracion, codigoVersion);
            return View(model);
        }

        /// <summary>
        /// Cargar Maxima Demanda Tipo de Generacion Semanal
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarMaximaDemandaTipoGeneracionAnual(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFechaUltimoMes(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeAnual.IndexAnualMaxDemandaTipoGeneracion,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _serviceSiosein.ListarDataVersionCargarMaximaDemandaTipoGeneracionMen(objFiltro);
            List<SioseinModel> listaModel = new List<SioseinModel>();
            SioseinModel model;

            //Máxima demanda coincidente de potencia (MW) por tipo de generación en el SEIN.
            model = new SioseinModel
            {
                Resultado = UtilSemanalPR5.ListarMaximaDemandaTipoGeneracionSemanalHtml(objReporte.Tabla)
            };
            listaModel.Add(model);

            //Comparación de la máxima demanda coincidente de potencia(MW) por tipo de generación en el SEIN
            model = new SioseinModel
            {
                Grafico = objReporte.GraficoCompMD
            };
            listaModel.Add(model);

            return Json(listaModel);
        }

        #endregion

        #region 4.2. PARTICIPACIÓN DE LAS EMPRESAS INTEGRANTES EN LA MÁXIMA POTENCIA COINCIDENTE

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexAnualMaxPotenciaPorEmpresa(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();
            SioseinModel model = GetModelGenericoIndex(ConstantesInformeAnual.IndexAnualMaxDemandaPorEmpresa, codigoVersion);
            return View(model);
        }

        /// <summary>
        /// Cargar Maxima Demanda X Empresa Semanal
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarMaximaDemandaXEmpresaAnual(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFechaUltimoMes(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeAnual.IndexAnualMaxDemandaPorEmpresa,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _serviceSiosein.ListarDataVersionCargarMaximaDemandaXEmpresaInfMen(objFiltro);

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

        #region 5. HIDROLOGÍA

        #region 5.1. EVOLUCIÓN DE LOS VOLÚMENES ALMACENADOS

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexAnualEvolVolAlmacenados(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();
            SioseinModel model = GetModelGenericoIndex(ConstantesInformeAnual.IndexAnualEvolVolAlmacenados, codigoVersion);
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

            FechasPR5 objFecha = GetObjetFechaUltimoMes(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            //reporte                        
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeAnual.IndexAnualEvolVolAlmacenados,
                Verscodi = codigoVersion
            };
            InfSGIReporteVersionado objReporte = _servicioPR5.ListarDataVersionVolumenUtilEmbalsesLagunas(objFiltro);
            /// Output          
            TablaReporte dataTabla = UtilSemanalPR5.ObtenerDataTablaVolumenUtilEmbalsesLagunas(objFecha, objReporte.ListaPtoEmbalsesLagunas, objReporte.ListaDataXPto);
            model.Resultado = UtilSemanalPR5.GenerarRHtmlVolumenUtilEmbalsesLagunas(objFecha, dataTabla);
            objReporte = _servicioPR5.ListarDataVersionEvolucionVolumenUtilSemanal(objFiltro);

            model.Graficos = new List<GraficoWeb>();
            model.Graficos.AddRange(objReporte.ListaGrafico);

            return Json(model);
        }

        #endregion

        #region 5.2. EVOLUCIÓN PROMEDIO MENSUAL DE CAUDALES (m3/s)

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexAnualEvolPromCaudales(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();
            SioseinModel model = GetModelGenericoIndex(ConstantesInformeAnual.IndexAnualEvolCaudales, codigoVersion);
            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Promedio Mensual de Caudales para el Informe Anual
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaPromMensualCaudales(int codigoVersion)
        {
            SioseinModel model = new SioseinModel();

            FechasPR5 objFecha = GetObjetFechaUltimoMes(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeAnual.IndexAnualEvolCaudales,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioPR5.ListarDataVersionCuadroCaudalSemanal(objFiltro);
            List<string> listaHtml = new List<string>();
            foreach (var objTabla in objReporte.ListaTabla)
            {
                listaHtml.Add(UtilSemanalPR5.GenerarRHtmlPromedioMensualCaudalesSemanal(objFecha, objTabla));
            }
            model.Resultado = listaHtml[0];
            model.Resultado2 = listaHtml[1];
            objReporte = _servicioPR5.ListarDataVersionEvolucionCaudales(objFiltro);
            model.Graficos = objReporte.ListaGrafico;

            return Json(model);
        }

        #endregion

        #endregion

        #region 6. COSTOS MARGINALES PROMEDIO PONDERADO MENSUAL DEL SEIN 

        #region 6.1 EVOLUCIÓN MENSUAL DE LOS COSTOS MARGINALES PROMEDIO PONDERA DEL SEIN BARRA STA ROSA

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexAnualCostosMarginalesProm(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();
            SioseinModel model = GetModelGenericoIndex(ConstantesInformeAnual.IndexAnualEvolCostosMarginales, codigoVersion);
            return View(model);
        }

        /// <summary>
        /// Genera reporte Costos Marginales promedio mensuales en el SEIN
        /// </summary>
        /// <param name="fechaInicioM"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ConsultaAnualCostosMarginalesBarraStaRosa(int codigoVersion)
        {
            FechasPR5 objFecha = GetObjetFechaUltimoMes(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                Mrepcodi = ConstantesInformeAnual.IndexAnualEvolCostosMarginales,
                Verscodi = codigoVersion,
                ObjFecha = objFecha
            };

            InfSGIReporteVersionado objReporte = _serviceSiosein.ListarDataCostosMargianlesStaRosaInfAnual(objFiltro);
            var model = new SioseinModel
            {
                Resultado = _serviceSiosein.ListaReporteCostosMargStaRosaAnualGenHTML(objReporte.Tabla),
                Grafico = objReporte.Grafico
            };

            return Json(model);
        }

        #endregion

        #endregion

        #region 7. HORAS DE CONGESTIÓN EN LAS PRINCIPALES EQUIPOS DE TRANSMISIÓN DEL SEIN (Horas)

        #region 7.1 POR ÁREA OPERATIVA
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexAnualHorasCongestionAreaOpe(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();
            SioseinModel model = GetModelGenericoIndex(ConstantesInformeAnual.IndexAnualHorasCongestionAreaOpe, codigoVersion);
            return View(model);
        }

        /// <summary>
        /// Cargar Horas de Congestion por Area Operativa Anual
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarHorasCongestionAreaOpe(int codigoVersion)
        {
            SioseinModel model = new SioseinModel();

            FechasPR5 objFecha = GetObjetFechaUltimoMes(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeAnual.IndexAnualHorasCongestionAreaOpe,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _servicioPR5.ListarDataVersionHorasCongestionPorArea(objFiltro);
            model.Resultado = UtilSemanalPR5.ListaHorasCongestionDeEquiposTransmisioHTML(objFecha, objReporte.Tabla);
            model.Grafico = objReporte.Grafico;

            return Json(model);
        }

        #endregion

        #endregion

        #region 8. INTERCAMBIOS INTERNACIONES DE ENERGIA Y POTENCIA EN BARRA FRONTERA (GWH/MW)

        #region 8.1 Intercambios Internacionales de energía y potencia

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexAnualIntercambiosIntEnergPot(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();
            SioseinModel model = GetModelGenericoIndex(ConstantesInformeAnual.IndexAnualInterInternacionales, codigoVersion);
            return View(model);
        }

        /// <summary>
        /// Cargar Intercambios Internacionales
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarIntercambiosIntEnergPot(int codigoVersion)
        {
            SioseinModel model = new SioseinModel();

            FechasPR5 objFecha = GetObjetFechaUltimoMes(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeAnual.IndexAnualInterInternacionales,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _serviceSiosein.ListarDataVersionIntercambiosIntEnergPot(objFiltro);

            model.Resultado = UtilInfMensual.ListarTablaInterconexionInternacionalAnualHTML(objFecha, objReporte.Tabla);
            model.Resultado2 = UtilInfMensual.ListarTablaVariacionInterconexionInternacionalAnualHTML(objFecha, objReporte.TablaVar);

            model.Grafico = objReporte.Grafico;

            return Json(model);
        }

        #endregion

        #endregion

        #region 9. ANEXOS

        #region 9.1 PRODUCCIÓN DE ELECTRICIDAD ANUAL POR EMPRESA Y TIPO DE GENERACIÓN EN EL SEIN
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexAnualProdElectricidad(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();
            SioseinModel model = GetModelGenericoIndex(ConstantesInformeAnual.IndexAnualProdElectricidad, codigoVersion);
            return View(model);
        }

        /// <summary>
        /// Cargar Producción de Electricidad Anual por Empresa y tipo de Generación en el Sein
        /// </summary>
        /// <param name="codigoVersion"></param>        
        /// <returns></returns>
        public JsonResult CargarProdElectAnualEmpresasTipoGen(int codigoVersion)
        {
            SioseinModel model = new SioseinModel();
            FechasPR5 objFecha = GetObjetFecha12Meses(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeAnual.IndexAnualProdElectricidad,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = this._serviceSiosein.ListarDataVersionProduccionElectricidad(objFiltro);
            model.Resultado = UtilInfMensual.ListarResumenProduccionAnualHtml(objReporte.Tabla);
            return Json(model);
        }

        #endregion

        #region 9.2 MÁXIMA POTENCIA COINCIDENTE ANUAL

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexAnualMaxPotenciaCoincidente(int codigoVersion)
        {
            if (!EsOpcionValida()) return RedireccionarOpcionValida();
            SioseinModel model = GetModelGenericoIndex(ConstantesInformeAnual.IndexAnualMaxPotenciaCoincidente, codigoVersion);
            return View(model);
        }

        /// <summary>
        /// Cargar Máxima Potencia Coincidente Anual
        /// </summary>
        /// <param name="codigoVersion"></param>        
        /// <returns></returns>
        public JsonResult CargarMaximaPotenciaCoincidente(int codigoVersion)
        {
            SioseinModel model = new SioseinModel();

            FechasPR5 objFecha = GetObjetFecha12Meses(codigoVersion);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeAnual.IndexAnualMaxPotenciaCoincidente,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = _serviceSiosein.ListarDataVersionMaximaPotenciaCoincidente(objFiltro);
            model.Resultado = UtilInfMensual.ListarResumenMaximaDemandaAnualHtml(objReporte.Tabla);

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
                    var objVersion = _servicioPR5.GetByIdSiVersion(idVersion);
                    DateTime dFechaPeriodo = objVersion.Versfechaperiodo;
                    string fileName = string.Format(ConstantesPR5ReportesServicio.ArchivoInformeAnualPDF, dFechaPeriodo.Year, idVersion);

                    if (!FileServer.VerificarExistenciaDirectorio(ConstantesPR5ReportesServicio.PathArchivosInformeAnualPDF, string.Empty))
                        FileServer.CreateFolder(ConstantesPR5ReportesServicio.PathArchivosInformeAnualPDF, string.Empty, string.Empty);

                    if (FileServer.VerificarExistenciaFile(ConstantesPR5ReportesServicio.PathArchivosInformeAnualPDF, fileName,
                        string.Empty))
                        FileServer.DeleteBlob(ConstantesPR5ReportesServicio.PathArchivosInformeAnualPDF + fileName,
                            string.Empty);

                    FileServer.UploadFromStream(file.InputStream, ConstantesPR5ReportesServicio.PathArchivosInformeAnualPDF, fileName,
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
            var objVersion = _servicioPR5.GetByIdSiVersion(idVersion);
            DateTime dFechaPeriodo = objVersion.Versfechaperiodo;
            string fileName = string.Format(ConstantesPR5ReportesServicio.ArchivoInformeAnualPDF, dFechaPeriodo.Year, idVersion);
            Stream stream = FileServer.DownloadToStream(ConstantesPR5ReportesServicio.PathArchivosInformeAnualPDF + fileName,
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
            string folder = _servicioPR5.GetCarpetaInformeAnual();
            var objVersion = _servicioPR5.GetByIdSiVersion(idVersion);
            string nombreArchivo = UtilInfMensual.GetNombreArchivoInformeAnual(objVersion.Versfechaperiodo, idVersion);
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
            var objVersion = _servicioPR5.GetByIdSiVersion(idVersion);
            DateTime dFechaPeriodo = objVersion.Versfechaperiodo;
            string fileName = string.Format(ConstantesPR5ReportesServicio.ArchivoInformeAnualPDF, dFechaPeriodo.Year, idVersion);
            byte[] stream = FileServer.DownloadToArrayByte(ConstantesPR5ReportesServicio.PathArchivosInformeAnualPDF + fileName,
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
                string folder = _servicioPR5.GetCarpetaInformeAnual();
                var objVersion = this._serviceSiosein.GetByIdSiVersion(idVersion);
                DateTime fechaInicio = objVersion.Versfechaperiodo;
                DateTime fechaFin = fechaInicio.AddDays(6);
                string nombreArchivo = UtilInfMensual.GetNombreArchivoInformeAnual(fechaInicio, idVersion);

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