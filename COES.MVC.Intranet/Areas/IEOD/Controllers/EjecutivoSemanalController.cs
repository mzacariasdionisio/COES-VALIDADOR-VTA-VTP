using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.IEOD.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Siosein2;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.IEOD.Controllers
{
    public class EjecutivoSemanalController : BaseController
    {
        readonly PR5ReportesAppServicio servicio = new PR5ReportesAppServicio();

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

        #region Versiones

        [HttpPost]
        public JsonResult ListadoVersion(string fechaPeriodo)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();

            try
            {
                this.ValidarSesionJsonResult();

                DateTime dFechaPeriodo = DateTime.ParseExact(fechaPeriodo, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                //Validar existencia de pdf
                model.ListaVersion = servicio.ListaVersionByFechaEjecutivoSemanal(dFechaPeriodo);
                foreach (var item in model.ListaVersion)
                {
                    string fileName = string.Format(ConstantesPR5ReportesServicio.ArchivoInformeEjecutivoSemanalPDF, item.Verscodi);
                    if (FileServer.VerificarExistenciaFile(ConstantesPR5ReportesServicio.PathArchivosInformeEjecutivoSemanalPDF, fileName,
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
            PublicacionIEODModel model = new PublicacionIEODModel();

            try
            {
                this.ValidarSesionJsonResult();

                //Validación
                DateTime dFechaPeriodo = DateTime.ParseExact(fechaPeriodo, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                motivo = (motivo ?? "").Trim();
                if (motivo == null || motivo.Trim() == string.Empty)
                {
                    throw new Exception("Debe ingresar motivo.");
                }

                if (servicio.EsValidoNuevaVersionSemanal(ConstantesPR5ReportesServicio.ReptipcodiEjecutivoSemanal, dFechaPeriodo))
                {
                    //Guardar registro
                    SiVersionDTO objVersion = new SiVersionDTO()
                    {
                        Versfechaperiodo = dFechaPeriodo,
                        Mprojcodi = ConstantesPR5ReportesServicio.MprojcodiIEOD,
                        Tmrepcodi = ConstantesPR5ReportesServicio.ReptipcodiEjecutivoSemanal,
                        Versmotivo = motivo,
                        Versfechaversion = DateTime.Now,
                        Versusucreacion = base.UserName,
                        Versfeccreacion = DateTime.Now
                    };
                    int verscodi = servicio.SaveSiVersion(objVersion);

                    //Guardar versión
                    servicio.GuardarVersionEjecSemanal(verscodi, dFechaPeriodo);

                    //Generar log
                    servicio.GenerarArchivoExceLogSemanal(verscodi, ConstantesPR5ReportesServicio.ReptipcodiEjecutivoSemanal, out string fileNameLog);
                    servicio.MoverArchivoEjecutivoSemanalFileServer(fileNameLog);

                    ////Genera Excel
                    servicio.GenerarArchivoExcelTodoEjecutivoSemanal(verscodi, out string fileName);
                    servicio.MoverArchivoEjecutivoSemanalFileServer(fileName);

                    model.Resultado = verscodi.ToString();
                }
                else
                {
                    throw new ArgumentException("No se puede generar una nueva versión: No existe diferencias con la versión anterior");
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

        #region Menu Informe

        //
        // GET: /IEOD/ReporteInformes/MenuInformeSemanal/
        public ActionResult MenuEjecutivoSemanal(string fecha)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            BusquedaIEODModel model = new BusquedaIEODModel();

            DateTime finicio = EPDate.f_fechainiciosemana(DateTime.Today.AddDays(-10));
            finicio = finicio.AddDays(5); //buscar el jueves
            if (!string.IsNullOrEmpty(fecha)) finicio = DateTime.ParseExact(fecha.Replace("-", "/"), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            Tuple<int, int> anioSemIni = EPDate.f_numerosemana_y_anho(finicio);
            model.AnhoIni = anioSemIni.Item2.ToString();
            model.SemanaIni = anioSemIni.Item1.ToString();
            model.FechaInicio = finicio.ToString(ConstantesAppServicio.FormatoFecha);
            model.ListaSemanasIni = ListarSemanasEjecutivoSemanalByAnio(anioSemIni.Item2);

            return View(model);
        }

        /// <summary>
        /// Cargar menu de opciones
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult CargarMenu()
        {
            BusquedaIEODModel model = new BusquedaIEODModel();

            List<SiMenureporteDTO> listaItems = servicio.GetListaAdmReporte(ConstantesPR5ReportesServicio.ReptipcodiEjecutivoSemanal);
            model.Menu = UtilAnexoAPR5.ListaMenuHtml(listaItems);

            return Json(model);
        }

        /// <summary>
        /// Carga lista de Semanas
        /// </summary>
        /// <param name="idAnho"></param>
        /// <returns></returns>
        public PartialViewResult CargarSemana(int idAnho, string tipoSem)
        {
            PublicacionIEODModel model = new PublicacionIEODModel
            {
                ListaSemanas = ListarSemanasEjecutivoSemanalByAnio(idAnho),
                TipoSemana = tipoSem
            };

            return PartialView(model);
        }

        /// <summary>
        /// Listar la lista de semanas
        /// </summary>
        /// <param name="idAnho"></param>
        /// <returns></returns>
        private List<TipoInformacionPR5> ListarSemanasEjecutivoSemanalByAnio(int idAnho)
        {
            List<TipoInformacionPR5> entitys = new List<TipoInformacionPR5>();

            int nsemanas = EPDate.TotalSemanasEnAnho(idAnho, FirstDayOfWeek.Saturday);
            DateTime dtfecha = EPDate.f_fechainiciosemana(idAnho, 1);

            for (int i = 1; i <= nsemanas; i++)
            {
                DateTime dtfechaIniSem = dtfecha.AddDays(7 * (i - 1)).AddDays(5);
                DateTime dtfechaFinSem = dtfechaIniSem.AddDays(6);

                Tuple<int, int> anioSem1 = EPDate.f_numerosemana_y_anho(dtfechaIniSem);
                Tuple<int, int> anioSem2 = EPDate.f_numerosemana_y_anho(dtfechaFinSem);

                TipoInformacionPR5 reg = new TipoInformacionPR5
                {
                    IdTipoInfo = i,
                    NombreTipoInfo = string.Format("Sem{0}({1})-{2} (JUE {3} a MIE {4})", anioSem1.Item1, anioSem2.Item1, anioSem1.Item2, dtfechaIniSem.ToString(ConstantesAppServicio.FormatoFecha), dtfechaFinSem.ToString(ConstantesAppServicio.FormatoFecha)),
                    FechaIniSem = dtfechaIniSem.ToString(ConstantesAppServicio.FormatoFecha),
                    FechaFinSem = dtfechaFinSem.ToString(ConstantesAppServicio.FormatoFecha)
                };

                entitys.Add(reg);
            }

            return entitys;
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
                return RedirectToAction("MenuEjecutivoSemanal", "IEOD/EjecutivoSemanal", new { area = string.Empty });
            }
        }

        /// <summary>
        /// Obtener numero formateado de 3 digitos
        /// </summary>
        /// <returns></returns>
        public static NumberFormatInfo GenerarNumberFormatInfo()
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";
            return nfi;
        }

        /// <summary>
        /// Setear valores para cada index
        /// </summary>
        /// <param name="model"></param>
        /// <param name="repcodi"></param>
        private BusquedaIEODModel GetModelGenericoIndex(int repcodi, int verscodi)
        {
            BusquedaIEODModel model = new BusquedaIEODModel();

            //version seleccionada
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(verscodi);
            List<SiVersionDTO> listaVersion = servicio.ListaVersionByFecha(objVersion.Versfechaperiodo, ConstantesPR5ReportesServicio.ReptipcodiEjecutivoSemanal);
            model.ListaVersion = listaVersion;
            model.Verscodi = verscodi;

            //Fechas
            DateTime finicio = objVersion.Versfechaperiodo;
            DateTime ffin = finicio.AddDays(6);

            Tuple<int, int> anioSemIni = EPDate.f_numerosemana_y_anho(finicio);
            Tuple<int, int> anioSemFin = EPDate.f_numerosemana_y_anho(ffin);
            model.AnhoIni = anioSemIni.Item2.ToString();
            model.SemanaIni = anioSemIni.Item1.ToString();
            model.SemanaFin = anioSemFin.Item1.ToString();
            model.FechaInicio = finicio.ToString(ConstantesAppServicio.FormatoFecha);

            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(finicio, ffin);

            //numeral seleccionado
            model.Tiporeporte = ConstantesPR5ReportesServicio.TIPO_EJECUTIVO_SEM;
            model.Idnumeral = repcodi;
            model.Reporcodi = repcodi;

            SiMenureporteDTO objItem = servicio.GetByIdMenuReporte(repcodi);
            model.TituloWeb = objItem.Mreptituloweb
                + "<span class='filtro_version_desc'></span>"
                + "<br/>"
                + "<span class='filtro_fecha_desc'>" + model.FiltroFechaDesc + "</span>";

            model.Url = Url.Content("~/");

            return model;
        }

        /// <summary>
        /// Descripción de los filtros fecha
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        private string GetDescripcionFiltroFecha(DateTime fechaInicial, DateTime fechaFinal)
        {
            return UtilAnexoAPR5.GetDescripcionFiltroFechaEjecutivoSemanal(fechaInicial, fechaFinal);
        }

        /// <summary>
        /// Devuelve el valor por empresa
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public decimal GetValorCeldaProdEmpresas(List<MeMedicion48DTO> lista, int emprcodi)
        {
            decimal valorCelda = 0.0m;
            var obj = lista.Find(x => x.Emprcodi == emprcodi);
            if (obj != null)
            {
                decimal? valor = obj.Meditotal;

                if (valor != null)
                {
                    valorCelda = (decimal)valor;
                }
            }
            return valorCelda;
        }

        [HttpPost]
        public JsonResult CargarDetalleTablaResumen(string sfechaInicial, string sfechaFinal, string filtroRER)
        {
            DateTime fechaInicial = DateTime.ParseExact(sfechaInicial, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(sfechaFinal, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            filtroRER = !string.IsNullOrEmpty(filtroRER) ? filtroRER : "-1";

            PublicacionIEODModel model = new PublicacionIEODModel
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

            PublicacionIEODModel model = new PublicacionIEODModel
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

            PublicacionIEODModel model = new PublicacionIEODModel
            {
                ListaDetalleInterconexion = servicio.ListarDetalleResumenInterconexion(fechaInicial, fechaFinal)
            };

            return Json(model);
        }

        [HttpPost]
        public JsonResult CargarDetalleTablaInterconexionMD(string sFechaMaximaDemanda)
        {
            DateTime fechaMD = DateTime.ParseExact(sFechaMaximaDemanda, ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);

            PublicacionIEODModel model = new PublicacionIEODModel
            {
                ListaDetalleInterconexion = servicio.ListarDetalleResumenInterconexionMD(fechaMD)
            };

            return Json(model);
        }

        [HttpPost]
        public JsonResult GenerarArchivoExcelDetalleTablaResumen(string sfechaInicial, string sfechaFinal, string filtroRER)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();

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
            PublicacionIEODModel model = new PublicacionIEODModel();

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
            PublicacionIEODModel model = new PublicacionIEODModel();

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
            PublicacionIEODModel model = new PublicacionIEODModel();

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

        #region EXPORTACIÓN A EXCEL

        /// <summary>
        /// Permite la exportacion en archivo excel del ejecutivo semanal
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult GenerarEjecutivoSemanal(int reporcodi, int codigoVersion)
        {
            var model = new PublicacionIEODModel();

            try
            {
                this.ValidarSesionJsonResult();

                //Utilizar la carpeta de Intervenciones debido a que ya está configurado para ser 'leido' por el plugin office
                string subcarpetaDestino = ConstantesPR5ReportesServicio.RutaReportes;
                string directorioDestino = AppDomain.CurrentDomain.BaseDirectory + subcarpetaDestino;

                servicio.GenerarArchivoExcelEjecutivoSemanal(codigoVersion, directorioDestino, reporcodi, out string nombreArchivo);

                model.Resultado = nombreArchivo;
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

        //
        // GET: /IEOD/ReporteInformes/IndexResumenRelevante
        public ActionResult IndexResumenRelevante(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesInformeSemanalPR5.IndexResumenRelevante, codigoVersion);

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
            //filtros
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);
            DateTime fechaInicial = objVersion.Versfechaperiodo;
            DateTime fechaFinal = fechaInicial.AddDays(6);

            FechasPR5 objFecha = UtilSemanalPR5.ObtenerFechasEjecutivoSemanal(fechaInicial, fechaFinal);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesInformeSemanalPR5.IndexResumenRelevante,
                Verscodi = codigoVersion
            };
            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionResumenRelevante(objFiltro);

            List<PublicacionIEODModel> listaModel = new List<PublicacionIEODModel>();
            PublicacionIEODModel model;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //RESUMEN RELEVANTE
            model = new PublicacionIEODModel
            {
                FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal),
                Resultado = UtilSemanalPR5.ReporteResumenSemanalHtml(objReporte.ObjTexto)
            };
            listaModel.Add(model);

            //Grafico participacion de Recursos energéticos
            model = new PublicacionIEODModel
            {
                Grafico = objReporte.GraficoPieSemAct_AnioAct
            };
            listaModel.Add(model);

            model = new PublicacionIEODModel
            {
                Grafico = objReporte.GraficoPieSemAct_Anio1Ant
            };
            listaModel.Add(model);

            return Json(listaModel);
        }

        #endregion

        #region 1. OFERTA DE GENERACIÓN ELÉCTRICA EN EL SEIN

        #region 1.1. Ingreso en Operación Comercial al SEIN

        //
        // GET: /IEOD/ReporteInformes/IndexIngresoOpComercSEIN
        public ActionResult IndexIngresoOpComercSEIN(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesEjecutivoSemanalPR5.IndexIngresoOpComercSEIN, codigoVersion);

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
            PublicacionIEODModel model = new PublicacionIEODModel();

            //filtros
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);
            DateTime fechaInicial = objVersion.Versfechaperiodo;
            DateTime fechaFinal = fechaInicial.AddDays(6);

            FechasPR5 objFecha = UtilSemanalPR5.ObtenerFechasEjecutivoSemanal(fechaInicial, fechaFinal);
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesEjecutivoSemanalPR5.IndexIngresoOpComercSEIN,
                Verscodi = codigoVersion
            };
            int tipoOperacion = 1; // Es Ingreso de operacion
            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionIngresoOpComercSEIN(objFiltro, tipoOperacion);

            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);
            model.Resultado = UtilSemanalPR5.GenerarListadoIngresoRetiroOpComercialSeinHtml(objReporte.Tabla, tipoOperacion);
            model.Grafico = objReporte.Grafico;

            return Json(model);
        }

        #endregion

        #region 1.2. Retiro de Operación Comercial
        //
        // GET: /IEOD/ReporteInformes/IndexRetiroOpComercSEIN
        public ActionResult IndexRetiroOpComercSEIN(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesEjecutivoSemanalPR5.IndexRetiroOpComercSEIN, codigoVersion);

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
            PublicacionIEODModel model = new PublicacionIEODModel();

            //filtros
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);
            DateTime fechaInicial = objVersion.Versfechaperiodo;
            DateTime fechaFinal = fechaInicial.AddDays(6);

            FechasPR5 objFecha = UtilSemanalPR5.ObtenerFechasEjecutivoSemanal(fechaInicial, fechaFinal);
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesEjecutivoSemanalPR5.IndexRetiroOpComercSEIN,
                Verscodi = codigoVersion
            };
            int tipoOperacion = 2; // Es Retiro de operacion
            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionIngresoOpComercSEIN(objFiltro, tipoOperacion);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);
            model.Resultado = UtilSemanalPR5.GenerarListadoIngresoRetiroOpComercialSeinHtml(objReporte.Tabla, tipoOperacion);
            model.Grafico = objReporte.Grafico;

            return Json(model);
        }

        #endregion

        #endregion

        #region 2. MATRIZ ELÉCTRICA DE GENERACIÓN DEL SEIN (GWh)

        #region 2.1. Producción por tipo de Generación

        //
        // GET: /IEOD/ReporteInformes/IndexProdTipoGen
        public ActionResult IndexProdTipoGen(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesEjecutivoSemanalPR5.IndexProdTipoGen, codigoVersion);

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
            //filtros
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);
            DateTime fechaInicial = objVersion.Versfechaperiodo;
            DateTime fechaFinal = fechaInicial.AddDays(6);

            FechasPR5 objFecha = UtilSemanalPR5.ObtenerFechasEjecutivoSemanal(fechaInicial, fechaFinal);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesEjecutivoSemanalPR5.IndexProdTipoGen,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionProdTipoGen(objFiltro);

            List<PublicacionIEODModel> listaModel = new List<PublicacionIEODModel>();
            PublicacionIEODModel model = new PublicacionIEODModel
            {
                Resultado = UtilSemanalPR5.ListaReporteProduccionTipoGenHTML(objReporte.Tabla),
                FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal)
            };
            listaModel.Add(model);

            model = new PublicacionIEODModel
            {
                Grafico = objReporte.GraficoComp
            };
            listaModel.Add(model);

            model = new PublicacionIEODModel
            {
                Grafico = objReporte.GraficoProdSem
            };
            listaModel.Add(model);

            return Json(listaModel);
        }

        #endregion

        #region 2.2. Producción por tipo de Recurso Energético
        //
        // GET: /IEOD/ReporteInformes/IndexProdTipoRecurso
        public ActionResult IndexProdTipoRecurso(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesEjecutivoSemanalPR5.IndexProdTipoRecurso, codigoVersion);

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
            //filtros
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);
            DateTime fechaInicial = objVersion.Versfechaperiodo;
            DateTime fechaFinal = fechaInicial.AddDays(6);

            FechasPR5 objFecha = UtilSemanalPR5.ObtenerFechasEjecutivoSemanal(fechaInicial, fechaFinal);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesEjecutivoSemanalPR5.IndexProdTipoRecurso,
                Verscodi = codigoVersion
            };

            this.servicio.CargarReporteProduccionXTipoRecursoEjecSem(objFiltro, out TablaReporte dataTabla, out GraficoWeb graficoCompProd, out GraficoWeb graficoEvoSem, out GraficoWeb graficoParticSem);

            List<PublicacionIEODModel> listaModel = new List<PublicacionIEODModel>();
            PublicacionIEODModel model = new PublicacionIEODModel
            {
                Resultado = UtilSemanalPR5.ReporteProdXTipoRecursoEnergeticoHtml(dataTabla),
                FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal)
            };
            listaModel.Add(model);

            model = new PublicacionIEODModel
            {
                Grafico = graficoCompProd
            };
            listaModel.Add(model);

            model = new PublicacionIEODModel
            {
                Grafico = graficoEvoSem
            };
            listaModel.Add(model);

            model = new PublicacionIEODModel
            {
                Grafico = graficoParticSem
            };
            listaModel.Add(model);

            return Json(listaModel);
        }

        #endregion

        #region 2.3. Producción RER

        //
        // GET: /IEOD/ReporteInformes/IndexProdRER
        public ActionResult IndexProdRER(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesEjecutivoSemanalPR5.IndexProdRER, codigoVersion);

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
            //filtros
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);
            DateTime fechaInicial = objVersion.Versfechaperiodo;
            DateTime fechaFinal = fechaInicial.AddDays(6);

            FechasPR5 objFecha = UtilSemanalPR5.ObtenerFechasEjecutivoSemanal(fechaInicial, fechaFinal);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesEjecutivoSemanalPR5.IndexProdRER,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionProduccionRER(objFiltro);

            List<PublicacionIEODModel> listaModel = new List<PublicacionIEODModel>();
            PublicacionIEODModel model = new PublicacionIEODModel
            {
                Resultado = UtilSemanalPR5.ReporteProduccionRERHTML(objReporte.Tabla),
                FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal)
            };
            listaModel.Add(model);

            model = new PublicacionIEODModel
            {
                Grafico = objReporte.ListaGrafico[0]
            };
            listaModel.Add(model);

            model = new PublicacionIEODModel
            {
                Grafico = objReporte.ListaGrafico[1]
            };
            listaModel.Add(model);

            model = new PublicacionIEODModel
            {
                Grafico = objReporte.ListaGrafico[2]
            };
            listaModel.Add(model);

            return Json(listaModel);
        }

        #endregion

        #region 2.4. Factor de planta de las centrales RER

        //
        // GET: /IEOD/ReporteInformes/IndexFactorPlantaRER
        public ActionResult IndexFactorPlantaRER(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesEjecutivoSemanalPR5.IndexFactorPlantaRER, codigoVersion);

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
            //filtros
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);
            DateTime fechaInicial = objVersion.Versfechaperiodo;
            DateTime fechaFinal = fechaInicial.AddDays(6);

            FechasPR5 objFecha = UtilSemanalPR5.ObtenerFechasEjecutivoSemanal(fechaInicial, fechaFinal);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesEjecutivoSemanalPR5.IndexFactorPlantaRER,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionFactorPlantaCentralesRER(objFiltro);

            List<PublicacionIEODModel> listaModel = new List<PublicacionIEODModel>();

            // Producción de energía eléctrica (GWh) y factor de planta de las centrales con recursos energético renovables en el SEIN.
            PublicacionIEODModel model = new PublicacionIEODModel
            {
                Resultado = UtilSemanalPR5.ReporteFactorPlantaCentralesRERHtml(objReporte.Tabla),
                FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal)
            };
            listaModel.Add(model);

            //Factor de planta de las centrales RER  Acumulado al FECHA_FIN
            model = new PublicacionIEODModel
            {
                Grafico = objReporte.Grafico
            };
            listaModel.Add(model);

            //Producción de energía eléctrica (GWh) y factor de planta de las centrales con recursos energético renovables por tipo de generación en el SEIN - semana operativa N
            foreach (var reg in objReporte.ListaGrafico)
            {
                model = new PublicacionIEODModel
                {
                    Grafico = reg
                };
                listaModel.Add(model);
            }

            return Json(listaModel);
        }

        #endregion

        #region 2.5. Participación de la producción por empresas Integrantes

        //
        // GET: /IEOD/ReporteInformes/IndexParticipacionEmpresas
        public ActionResult IndexParticipacionEmpresas(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesEjecutivoSemanalPR5.IndexParticipacionEmpresas, codigoVersion);

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
            //filtros
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);
            DateTime fechaInicial = objVersion.Versfechaperiodo;
            DateTime fechaFinal = fechaInicial.AddDays(6);

            FechasPR5 objFecha = UtilSemanalPR5.ObtenerFechasEjecutivoSemanal(fechaInicial, fechaFinal);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesEjecutivoSemanalPR5.IndexParticipacionEmpresas,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionProduccionEmpresasIntegrantesInfSem(objFiltro);

            PublicacionIEODModel model = new PublicacionIEODModel
            {
                Resultado = UtilSemanalPR5.ReporteProduccionEmpresasIntegrantesHtml(objReporte.Tabla),
                FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal),
                Grafico = objReporte.Grafico
            };

            return Json(model);
        }

        #endregion

        #endregion

        #region 3. MÁXIMA DEMANDA COINCIDENTE DE POTENCIA EN EL SEIN (MW)

        #region 3.1. Máxima demanda Por tipo de generación
        //
        // GET: /IEOD/ReporteInformes/CargarMaximaDemandaTipoGeneracionSemanalmandaTipoGeneracionEjecut
        public ActionResult IndexMaximaDemandaTipoGeneracionEjecut(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesEjecutivoSemanalPR5.IndexMaximaDemandaTipoGeneracionEjecut, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Maxima Demanda Tipo de Generacion
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarMaximaDemandaTipoGeneracionSemanal(int codigoVersion)
        {
            //filtros
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);
            DateTime fechaInicial = objVersion.Versfechaperiodo;
            DateTime fechaFinal = fechaInicial.AddDays(6);

            FechasPR5 objFecha = UtilSemanalPR5.ObtenerFechasEjecutivoSemanal(fechaInicial, fechaFinal);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesEjecutivoSemanalPR5.IndexMaximaDemandaTipoGeneracionEjecut,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionMaximaDemandaTipoGeneracionSemanal(objFiltro);

            List<PublicacionIEODModel> listaModel = new List<PublicacionIEODModel>();
            PublicacionIEODModel model;

            //Máxima demanda coincidente de potencia (MW) por tipo de generación en el SEIN.
            model = new PublicacionIEODModel
            {
                FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal),

                Resultado = UtilSemanalPR5.ListarMaximaDemandaTipoGeneracionSemanalHtml(objReporte.Tabla)
            };
            listaModel.Add(model);

            //Comparación de la máxima demanda coincidente de potencia(MW) por tipo de generación en el SEIN, en la semana operativa 36 - (37) de los años 2015, 2016 y 2017
            model = new PublicacionIEODModel
            {
                Grafico = objReporte.GraficoCompMD
            };
            listaModel.Add(model);

            //Diagrama de carga del despacho en el día de máxima demanda por recurso energético duranta la semana operativa 36-2017
            model = new PublicacionIEODModel
            {
                Grafico = objReporte.GraficoCargaDespacho
            };
            listaModel.Add(model);

            //MÁXIMA DEMANDA Y VARIACIÓN SEMANAL
            //Evolución semanal de la máxima demanda y comparación de la variación semanal para los años 2015,2016,2017 
            model = new PublicacionIEODModel
            {
                Grafico = objReporte.GraficoVarSem
            };
            listaModel.Add(model);

            return Json(listaModel);
        }

        #endregion

        #region 3.2. PARTICIPACIÓN DE LAS EMPRESAS INTEGRANTES EN LA MÁXIMA DEMANDA COINCIDENTE (MW)

        //
        // GET: /IEOD/ReporteInformes/MaximaDemandaXEmpresaEjecut
        public ActionResult IndexMaximaDemandaXEmpresaEjecut(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesEjecutivoSemanalPR5.IndexMaximaDemandaXEmpresaEjecut, codigoVersion);

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
            //filtros
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);
            DateTime fechaInicial = objVersion.Versfechaperiodo;
            DateTime fechaFinal = fechaInicial.AddDays(6);

            FechasPR5 objFecha = UtilSemanalPR5.ObtenerFechasEjecutivoSemanal(fechaInicial, fechaFinal);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesEjecutivoSemanalPR5.IndexMaximaDemandaXEmpresaEjecut,
                Verscodi = codigoVersion
            };
            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionMaximaDemandaXEmpresaSemanalEjecSem(objFiltro);

            PublicacionIEODModel model = new PublicacionIEODModel
            {
                //Cuadro N° 9: Participación de las empresas generadoras del COES en la máxima demanda coincidente (MW) durante la semana operativa
                Resultado = UtilSemanalPR5.ReporteMaximaDemandaXEmpresaSemanalHtml(objReporte.Tabla),
                FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal),

                //Gráfico N° 17: Comparación de la máxima demanda coincidente  (MW) de las empresas generadoras del COES durante la semana operativa N de los años  Y1 - Y2
                Grafico = objReporte.Grafico
            };

            return Json(model);
        }

        #endregion

        #region 3.3. EVOLUCIÓN DE LA DEMANDA POR ÁREAS OPERATIVAS DEL SEIN (GWh)

        //
        // GET: /IEOD/ReporteInformes/DemandaXAreaOpeEjecut
        public ActionResult IndexDemandaXAreaOpeEjecut(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesEjecutivoSemanalPR5.IndexDemandaXAreaOpeEjecut, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Demanda X Area Operativa Semanal
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarDemandaXAreaOpeSemanal(int codigoVersion)
        {
            //filtros
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);
            DateTime fechaInicial = objVersion.Versfechaperiodo;
            DateTime fechaFinal = fechaInicial.AddDays(6);

            FechasPR5 objFecha = UtilSemanalPR5.ObtenerFechasEjecutivoSemanal(fechaInicial, fechaFinal);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesEjecutivoSemanalPR5.IndexDemandaXAreaOpeEjecut,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = servicio.ListarDataEvolucionDemandaXAreaOperativaSEIN(objFiltro);

            List<PublicacionIEODModel> listaModel = new List<PublicacionIEODModel>();
            //Evolucion de la Demanda por áreas operativas del SEIN
            PublicacionIEODModel model = new PublicacionIEODModel
            {
                Resultado = UtilSemanalPR5.ReporteDemandaXAreaOperSemanalHtml(objReporte.Tabla),
                FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal)
            };
            listaModel.Add(model);

            //Comparación de la máxima demanda coincidente de potencia (MW) por área operativa en el SEIN, en la semana operativa36 de los años 2015,2016 y 2017
            model = new PublicacionIEODModel
            {
                Grafico = objReporte.GraficoCompMD
            };
            listaModel.Add(model);

            //Variación acumulada por áreas
            model = new PublicacionIEODModel
            {
                Grafico = objReporte.GraficoVarSem
            };
            listaModel.Add(model);

            return Json(listaModel);
        }

        #endregion

        #endregion

        #region 4. HIDROLOGÍA PARA LA OPERACIÓN DEL SEIN

        #region 4.1. Volumen útil de los embalses y lagunas (Mm3)

        //
        // GET: /IEOD/ReporteInformes/IndexVolUtilEmbLag
        public ActionResult IndexVolUtilEmbLag(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesEjecutivoSemanalPR5.IndexVolUtilEmbLag, codigoVersion);

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
            PublicacionIEODModel model = new PublicacionIEODModel();

            //filtros
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);
            DateTime fechaInicial = objVersion.Versfechaperiodo;
            DateTime fechaFinal = fechaInicial.AddDays(6);

            FechasPR5 objFecha = UtilSemanalPR5.ObtenerFechasEjecutivoSemanal(fechaInicial, fechaFinal);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesEjecutivoSemanalPR5.IndexVolUtilEmbLag,
                Verscodi = codigoVersion
            };
            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionVolumenUtilEmbalsesLagunas(objFiltro);

            TablaReporte dataTabla = UtilSemanalPR5.ObtenerDataTablaVolumenUtilEmbalsesLagunas(objFecha, objReporte.ListaPtoEmbalsesLagunas, objReporte.ListaDataXPto);
            dataTabla.ListaItem = servicio.ListarItemFromSiMenureporte();
            model.Resultado = UtilSemanalPR5.GenerarRHtmlVolumenUtilEmbalsesLagunas(objFecha, dataTabla);

            return Json(model);
        }

        #endregion

        #region 4.2. Evolucion de volumenes de embalses y lagunas (Mm3)

        //
        // GET: /IEOD/ReporteInformes/IndexVolUtilEmbLag
        public ActionResult IndexEvolucionVolEmbLag(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesEjecutivoSemanalPR5.IndexEvolucionVolEmbLag, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Grafico de Volumenes de Embales y Lagunas
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoVolEmbalesLagunas(int codigoVersion)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();

            //filtros
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);
            DateTime fechaInicial = objVersion.Versfechaperiodo;
            DateTime fechaFinal = fechaInicial.AddDays(6);

            FechasPR5 objFecha = UtilSemanalPR5.ObtenerFechasEjecutivoSemanal(fechaInicial, fechaFinal);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;

            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesEjecutivoSemanalPR5.IndexEvolucionVolEmbLag,
                Verscodi = codigoVersion
            };
            /// Output          
            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionEvolucionVolumenUtilSemanal(objFiltro);

            model.Graficos = new List<GraficoWeb>();
            model.Graficos.AddRange(objReporte.ListaGrafico);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            return Json(model);
        }

        #endregion

        #region 4.3. Promedio semanal de los caudales (m3/s)

        //
        // GET: /IEOD/ReporteInformes/IndexPromCaudales
        public ActionResult IndexPromCaudales(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesEjecutivoSemanalPR5.IndexPromCaudales, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Promedio semanal de Caudales para Ejecutivo Semanal
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaPromCaudales(int codigoVersion)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();

            //filtros
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);
            DateTime fechaInicial = objVersion.Versfechaperiodo;
            DateTime fechaFinal = fechaInicial.AddDays(6);

            //reporte
            FechasPR5 objFecha = UtilSemanalPR5.ObtenerFechasEjecutivoSemanal(fechaInicial, fechaFinal);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesEjecutivoSemanalPR5.IndexPromCaudales,
                Verscodi = codigoVersion
            };
            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionCuadroCaudalSemanal(objFiltro);
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

        #region 4.4. Evolución de los caudales

        //
        // GET: /IEOD/ReporteInformes/IndexSemPromMensualCaudales
        public ActionResult IndexEvolucionCaudalesEjecut(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesEjecutivoSemanalPR5.IndexEvolucionCaudalesEjecut, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Promedio semanal de Caudales
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaEvolucionCaudales(int codigoVersion)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();

            //filtros
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);
            DateTime fechaInicial = objVersion.Versfechaperiodo;
            DateTime fechaFinal = fechaInicial.AddDays(6);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            //reporte
            FechasPR5 objFecha = UtilSemanalPR5.ObtenerFechasInformeSemanal(fechaInicial, fechaFinal);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesEjecutivoSemanalPR5.IndexEvolucionCaudalesEjecut,
                Verscodi = codigoVersion
            };
            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionEvolucionCaudales(objFiltro);

            model.Graficos = objReporte.ListaGrafico;

            return Json(model);
        }

        #endregion

        #endregion

        #region 5. COSTO DE OPERACIÓN EJECUTADO ACUMULADO SEMANAL DEL SEIN (Millones de S/.)

        //5.1. Evolución  de los Costos de Operacion Ejecutado semanal (US$/MWh)
        //
        // GET: /IEOD/ReporteInformes/IndexEvolCostosOperacionEjecutados
        public ActionResult IndexEvolCostosOperacionEjecutados(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesEjecutivoSemanalPR5.IndexEvolCostosOperacionEjecutados, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Genera listado de los costos de operación
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaEvolCostosOperacionEjecutadosAcum(int codigoVersion)
        {
            //filtros
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);
            DateTime fechaInicial = objVersion.Versfechaperiodo;
            DateTime fechaFinal = fechaInicial.AddDays(6);

            //reporte
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = UtilSemanalPR5.ObtenerFechasEjecutivoSemanal(fechaInicial, fechaFinal, ConstantesPR5ReportesServicio.TipoVistaIndividual),
                Mrepcodi = ConstantesEjecutivoSemanalPR5.IndexEvolCostosOperacionEjecutados,
                Verscodi = codigoVersion
            };
            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionCostosOperacionEjecutado(objFiltro);

            //Salidas
            List<PublicacionIEODModel> listaModel = new List<PublicacionIEODModel>();

            PublicacionIEODModel model = new PublicacionIEODModel
            {
                Resultado = UtilSemanalPR5.ListarReporteEvolucionCOEjecutivoHTML(objReporte.Tabla),
                FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal)
            };
            listaModel.Add(model);

            model = new PublicacionIEODModel
            {
                Grafico = objReporte.Grafico
            };
            listaModel.Add(model);

            return Json(listaModel);
        }

        #endregion

        #region 6. COSTOS MARGINALES PROMEDIO NODALES DIARIOS DEL SEIN (US$/MWh)

        //6.1. Evolución  de los Costos Marginales Promedio semanal (US$/MWh)
        //
        // GET: /IEOD/ReporteInformes/IndexEvolCostosMarginalesProm
        public ActionResult IndexEvolCostosMarginalesProm(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesEjecutivoSemanalPR5.IndexEvolCostosMarginalesProm, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Evolucion de Costos Marginales Promedio
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaEvolCostosMarginalesProm(int codigoVersion)
        {
            //filtros
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);
            DateTime fechaInicial = objVersion.Versfechaperiodo;
            DateTime fechaFinal = fechaInicial.AddDays(6);

            //reporte
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = UtilSemanalPR5.ObtenerFechasEjecutivoSemanal(fechaInicial, fechaFinal, ConstantesPR5ReportesServicio.TipoVistaIndividual),
                Mrepcodi = ConstantesEjecutivoSemanalPR5.IndexEvolCostosMarginalesProm,
                Verscodi = codigoVersion
            };
            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionCostosMarginalesPromSantaRosaEjec(objFiltro);

            //Salidas
            PublicacionIEODModel model = new PublicacionIEODModel
            {
                Resultado = UtilSemanalPR5.GenerarRHtmlEvolucionCMGbarra(objReporte.Tabla, ConstantesEjecutivoSemanalPR5.IndexEvolCostosMarginalesProm),
                Grafico = objReporte.Grafico,
                FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal)
            };

            return Json(model);
        }

        #endregion

        #region 7. FLUJOS DE INTERCONEXIÓN EN ÁREAS OPERATIVAS DEL SEIN (MW)

        //
        // GET: /IEOD/ReporteInformes/FlujoMaximoInterconexionesEjecut
        public ActionResult IndexFlujoMaximoInterconexionesEjecut(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesEjecutivoSemanalPR5.IndexFlujoMaximoInterconexionesEjecut, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista Flujo Maximo Interconexiones
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaFlujoMaximoInterconexiones(int codigoVersion)
        {
            PublicacionIEODModel model = new PublicacionIEODModel();

            //filtros
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);
            DateTime fechaInicial = objVersion.Versfechaperiodo;
            DateTime fechaFinal = fechaInicial.AddDays(6);
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            FechasPR5 objFecha = UtilSemanalPR5.ObtenerFechasEjecutivoSemanal(fechaInicial, fechaFinal, ConstantesPR5ReportesServicio.TipoVistaIndividual);

            //reporte           
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                Mrepcodi = ConstantesEjecutivoSemanalPR5.IndexFlujoMaximoInterconexionesEjecut,
                Verscodi = codigoVersion,
                ObjFecha = objFecha
            };

            InfSGIReporteVersionado objReporte = this.servicio.ListarDataVersionPotenciaMaxTransmitidaxPuntos(objFiltro);

            model.ResultadosHtml = new List<string>();
            model.Graficos = new List<GraficoWeb>();

            model.ResultadosHtml = objReporte.ResultadosHtml;
            model.Graficos = objReporte.ListaGrafico;
            return Json(model);
        }

        #endregion

        #region 8. HORAS DE CONGESTIÓN EN LAS PRINCIPALES EQUIPOS DE TRANSMISIÓN DEL SEIN (Horas)

        //
        // GET: /IEOD/ReporteInformes/HorasCongestionAreaOpeEjecut
        public ActionResult IndexHorasCongestionAreaOpeEjecut(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesEjecutivoSemanalPR5.IndexHorasCongestionAreaOpeEjecut, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Horas de Congestion por Area Operativa Semanal
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarHorasCongestionAreaOpe(int codigoVersion)
        {
            //filtros
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);
            DateTime fechaInicial = objVersion.Versfechaperiodo;
            DateTime fechaFinal = fechaInicial.AddDays(6);

            FechasPR5 objFecha = UtilSemanalPR5.ObtenerFechasEjecutivoSemanal(fechaInicial, fechaFinal);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesEjecutivoSemanalPR5.IndexHorasCongestionAreaOpeEjecut,
                Verscodi = codigoVersion
            };

            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionHorasCongestionPorArea(objFiltro);
            PublicacionIEODModel model = new PublicacionIEODModel
            {
                FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal),
                Resultado = UtilSemanalPR5.ListaHorasCongestionDeEquiposTransmisioHTML(objFecha, objReporte.Tabla),
                Grafico = objReporte.Grafico
            };

            return Json(model);
        }

        #endregion

        #region 9. CONSUMO DE COMBUSTIBLE EN EL SEIN

        //
        // GET: /IEOD/ReporteInformes/ConsumoCombustibleSemanalEjecut
        public ActionResult IndexConsumoCombustibleEjecut(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesEjecutivoSemanalPR5.IndexConsumoCombustibleEjecut, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista de Consumo Combustible Semanal
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaConsumoCombustibleSemanal(int codigoVersion)
        {
            //filtros
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);
            DateTime fechaInicial = objVersion.Versfechaperiodo;
            DateTime fechaFinal = fechaInicial.AddDays(6);

            //reporte
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = UtilSemanalPR5.ObtenerFechasEjecutivoSemanal(fechaInicial, fechaFinal, ConstantesPR5ReportesServicio.TipoVistaIndividual),
                Mrepcodi = ConstantesEjecutivoSemanalPR5.IndexConsumoCombustibleEjecut,
                Verscodi = codigoVersion
            };
            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionConsumoCombustible(objFiltro);

            //Salidas
            PublicacionIEODModel model = new PublicacionIEODModel
            {
                FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal),
                Resultado = UtilSemanalPR5.ListarReporteConsumoCombustibleSemanalHtml(fechaInicial, fechaFinal, objReporte.ListaRptDia, objReporte.ListaTotal),
                Graficos = new List<GraficoWeb>()
            {
                UtilSemanalPR5.GraficoMedIntervaloConsumoCombustibleSemanal(objReporte.ListaRptDia, ConstantesPR5ReportesServicio.EstcomcodiGaseoso,objReporte.ListaFenerg, fechaInicial, fechaFinal, out _, out _),
                UtilSemanalPR5.GraficoMedIntervaloConsumoCombustibleSemanal(objReporte.ListaRptDia, ConstantesPR5ReportesServicio.EstcomcodiLiquido,objReporte.ListaFenerg, fechaInicial, fechaFinal, out List<SerieDuracionCarga> lstGrafico2, out _),
                UtilSemanalPR5.GraficoMedIntervaloConsumoCombustibleSemanal3(objReporte.ListaRptDia ,ConstantesPR5ReportesServicio.EstcomcodiSolido,objReporte.ListaFenerg, fechaInicial, fechaFinal),
            },
                ListaGrafico = lstGrafico2
            };

            return Json(model);
        }

        #endregion

        #region 10. INTERCAMBIOS INTERNACIONALES

        //
        // GET: /IEOD/ReporteInformes/IntercambioInternacionalesEjecut
        public ActionResult IndexIntercambioInternacionalesEjecut(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesEjecutivoSemanalPR5.IndexIntercambioInternacionalesEjecut, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Lista Intercambio Internacionales Semanal
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarListaIntercambioInternacionalesSemanal(int codigoVersion)
        {
            //filtros
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);
            DateTime fechaInicial = objVersion.Versfechaperiodo;
            DateTime fechaFinal = fechaInicial.AddDays(6);

            FechasPR5 objFecha = UtilSemanalPR5.ObtenerFechasEjecutivoSemanal(fechaInicial, fechaFinal);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesEjecutivoSemanalPR5.IndexIntercambioInternacionalesEjecut,
                Verscodi = codigoVersion
            };
            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionIntercambioInternacionales(objFiltro);

            PublicacionIEODModel model = new PublicacionIEODModel
            {
                FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal),
                Resultado = UtilSemanalPR5.ListarIntercambioInternacionalesHtml(objReporte.Tabla, fechaInicial, fechaFinal),
                Grafico = objReporte.Grafico
            };

            return Json(model);
        }

        #endregion

        #region 11. EVENTOS Y FALLAS QUE OCASIONARON INTERRUPCIÓN Y DISMINUCIÓN DE SUMINISTRO ELÉCTRICO

        //
        // GET: /IEOD/ReporteInformes/EventoFallaSuministroEnergEjecut
        public ActionResult IndexEventoFallaSuministroEnergEjecut(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesEjecutivoSemanalPR5.IndexEventoFallaSuministroEnergEjecut, codigoVersion);

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
            PublicacionIEODModel model = new PublicacionIEODModel();

            //filtros
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);
            DateTime fechaInicial = objVersion.Versfechaperiodo;
            DateTime fechaFinal = fechaInicial.AddDays(6);

            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = UtilSemanalPR5.ObtenerFechasEjecutivoSemanal(fechaInicial, fechaFinal, ConstantesPR5ReportesServicio.TipoVistaIndividual),
                Mrepcodi = ConstantesEjecutivoSemanalPR5.IndexEventoFallaSuministroEnergEjecut,
                Verscodi = codigoVersion
            };
            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionEventoFallaSuministroEnerg(objFiltro);

            model.Resultado = UtilSemanalPR5.GenerarRHtmlFallasXFamiliaYCausa(objReporte.Tabla);

            model.Graficos = new List<GraficoWeb>()
            {
                objReporte.GraficoEveFallaXTipo, objReporte.GraficoEveFallaXFam, objReporte.GraficoEveEnergXFam
            };

            return Json(model);
        }

        #endregion

        #region 12. Anexos

        #region Listado DE EVENTOS Y FALLAS

        //11.2 Listado Detalles de los  EVENTOS
        public ActionResult IndexEventoDetalleEvento(int codigoVersion)
        {
            if (!this.EsOpcionValida()) return this.RedireccionarOpcionValida();

            BusquedaIEODModel model = GetModelGenericoIndex(ConstantesEjecutivoSemanalPR5.IndexEventoDetalleEvento, codigoVersion);

            return View(model);
        }

        /// <summary>
        /// Cargar Evento, Falla de Suministros de Energ
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public JsonResult CargarDetalleEventos(int codigoVersion)
        {

            PublicacionIEODModel model = new PublicacionIEODModel();

            //filtros
            SiVersionDTO objVersion = servicio.GetByIdSiVersion(codigoVersion);
            DateTime fechaInicial = objVersion.Versfechaperiodo;
            DateTime fechaFinal = fechaInicial.AddDays(6);

            FechasPR5 objFecha = UtilSemanalPR5.ObtenerFechasEjecutivoSemanal(fechaInicial, fechaFinal);
            objFecha.TipoVistaReporte = ConstantesPR5ReportesServicio.TipoVistaIndividual;
            InfSGIFiltroReporte objFiltro = new InfSGIFiltroReporte()
            {
                ObjFecha = objFecha,
                Mrepcodi = ConstantesEjecutivoSemanalPR5.IndexEventoDetalleEvento,
                Verscodi = codigoVersion
            };
            InfSGIReporteVersionado objReporte = servicio.ListarDataVersionDetalleEventos(objFiltro);

            var reptiCodi = ConstantesPR5ReportesServicio.ReptipcodiEjecutivoSemanal;
            model.FiltroFechaDesc = this.GetDescripcionFiltroFecha(fechaInicial, fechaFinal);

            model.Resultado = UtilSemanalPR5.GenerarHTMLDetalleEventos(reptiCodi, objReporte.ListaEventos, fechaInicial, fechaFinal);

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
                    string fileName = string.Format(ConstantesPR5ReportesServicio.ArchivoInformeEjecutivoSemanalPDF, idVersion);

                    if (!FileServer.VerificarExistenciaDirectorio(ConstantesPR5ReportesServicio.PathArchivosInformeEjecutivoSemanalPDF, string.Empty))
                        FileServer.CreateFolder(ConstantesPR5ReportesServicio.PathArchivosInformeEjecutivoSemanalPDF, string.Empty, string.Empty);

                    if (FileServer.VerificarExistenciaFile(ConstantesPR5ReportesServicio.PathArchivosInformeEjecutivoSemanalPDF, fileName,
                        string.Empty))
                        FileServer.DeleteBlob(ConstantesPR5ReportesServicio.PathArchivosInformeEjecutivoSemanalPDF + fileName,
                            string.Empty);

                    FileServer.UploadFromStream(file.InputStream, ConstantesPR5ReportesServicio.PathArchivosInformeEjecutivoSemanalPDF, fileName,
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
        /// Permite descargar el archivo de interrupciones
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarArchivoPDF(int idVersion)
        {
            string fileName = string.Format(ConstantesPR5ReportesServicio.ArchivoInformeEjecutivoSemanalPDF, idVersion);
            Stream stream = FileServer.DownloadToStream(ConstantesPR5ReportesServicio.PathArchivosInformeEjecutivoSemanalPDF + fileName,
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
        public virtual ActionResult DescargarArchivoExcel(int idVersion, string tipo)
        {
            string folder = this.servicio.GetCarpetaEjecutivoSemanal();
            var objVersion = this.servicio.GetByIdSiVersion(idVersion);
            DateTime fechaInicio = objVersion.Versfechaperiodo;
            DateTime fechaFin = fechaInicio.AddDays(6);
            string nombreArchivo = "";
            if (tipo == "E") nombreArchivo = this.servicio.GetNombreArchivoEjecutivoSemanal("", fechaInicio, fechaFin, idVersion);
            if (tipo == "L") nombreArchivo = this.servicio.GetNombreArchivoLogEjecutivoSemanal(fechaInicio, fechaFin, idVersion);

            Stream stream = FileServer.DownloadToStream(folder + nombreArchivo,
                string.Empty);

            return File(stream, Constantes.AppExcel, nombreArchivo);
        }

        /// <summary>
        /// Permite descargar pdf
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual FileContentResult DownloadArchivoPDF(int idVersion)
        {
            string fileName = string.Format(ConstantesPR5ReportesServicio.ArchivoInformeEjecutivoSemanalPDF, idVersion);
            byte[] stream = FileServer.DownloadToArrayByte(ConstantesPR5ReportesServicio.PathArchivosInformeEjecutivoSemanalPDF + fileName,
                string.Empty);
            string mimeType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "inline; filename=" + fileName);
            return File(stream, mimeType);
        }

        #endregion

        #region UTIL

        public JsonResult ListarGraficosReporte(int reporcodi)
        {
            BusquedaIEODModel model = new BusquedaIEODModel();

            try
            {
                base.ValidarSesionJsonResult();

                List<MeReporteDTO> listaGraficosReporte = servicio.ListMeReportes().Where(x => x.Reporesgrafico == Constantes.SI).ToList();
                var ListaGraficosVisibles = servicio.ListSiMenureporteGraficos().Where(x => x.Mrepcodi == reporcodi).ToList();
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
            BusquedaIEODModel model = new BusquedaIEODModel();

            try
            {
                base.ValidarSesionJsonResult();

                List<int> listaReporcodisVisible = new List<int>();
                if (!string.IsNullOrEmpty(reporcodisVisible))
                    listaReporcodisVisible = reporcodisVisible.Split(',').Select(x => int.Parse(x)).ToList();
                servicio.GuardarListaGraficosVisibles(ireporcodi, listaReporcodisVisible);

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
            PublicacionIEODModel model = new PublicacionIEODModel();

            try
            {
                string folder = this.servicio.GetCarpetaEjecutivoSemanal();
                var objVersion = this.servicio.GetByIdSiVersion(idVersion);
                DateTime fechaInicio = objVersion.Versfechaperiodo;
                DateTime fechaFin = fechaInicio.AddDays(6);
                string nombreArchivo = this.servicio.GetNombreArchivoEjecutivoSemanal("", fechaInicio, fechaFin, idVersion);

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
