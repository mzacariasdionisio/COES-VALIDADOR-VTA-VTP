using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.IndicadoresSup.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Siosein2;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.IndicadoresSup.Controllers
{
    public class NumeralController : BaseController
    {
        Siosein2AppServicio servicio = new Siosein2AppServicio();

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

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

        public NumeralController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #region Versión Numerales

        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            NumeralModel model = new NumeralModel();
            int mes = DateTime.Now.AddMonths(-1).Month;
            string periodo = (mes > 9) ? "" : "0" + mes.ToString() + " " + DateTime.Now.AddMonths(-1).Year.ToString();
            model.Fecha = periodo;
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public PartialViewResult Lista(string fecha)
        {
            NumeralModel model = new NumeralModel();
            int imes = Int32.Parse(fecha.Substring(0, 2));
            int ianho = Int32.Parse(fecha.Substring(3, 4));
            DateTime dtFecha = new DateTime(ianho, imes, 1);
            model.TablaNum1 = servicio.GetHtmlReporteNumeral1(1, dtFecha, servicio.ObtenerUltimaVersionNumeral(dtFecha, 1));
            model.TablaNum2 = servicio.GetHtmlReporteNumeral2(2, dtFecha, servicio.ObtenerUltimaVersionNumeral(dtFecha, 2));
            model.TablaNum3 = servicio.GetHtmlReporteNumeral3(3, dtFecha, servicio.ObtenerUltimaVersionNumeral(dtFecha, 3));
            model.TablaNum4 = servicio.GetHtmlReporteNumeral4(4, dtFecha, servicio.ObtenerUltimaVersionNumeral(dtFecha, 4));
            model.TablaNum5 = servicio.GetHtmlReporteNumeral5(5, dtFecha, servicio.ObtenerUltimaVersionNumeral(dtFecha, 5));
            model.TablaNum6 = servicio.GetHtmlReporteNumeral6(6, dtFecha, servicio.ObtenerUltimaVersionNumeral(dtFecha, 6));
            model.TablaNum7 = servicio.GetHtmlReporteNumeral7(7, dtFecha, servicio.ObtenerUltimaVersionNumeral(dtFecha, 7));
            model.TablaNum8 = servicio.GetHtmlReporteNumeral8(8, dtFecha, servicio.ObtenerUltimaVersionNumeral(dtFecha, 8));
            model.TablaNum9 = servicio.GetHtmlReporteNumeral9(9, dtFecha, servicio.ObtenerUltimaVersionNumeral(dtFecha, 9));
            model.TablaNum10 = servicio.GetHtmlReporteNumeral10(10, dtFecha, servicio.ObtenerUltimaVersionNumeral(dtFecha, 10));
            model.TablaNum11 = servicio.GetHtmlReporteNumeral11(11, dtFecha, servicio.ObtenerUltimaVersionNumeral(dtFecha, 11));
            return PartialView(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="numeral"></param>
        /// <returns></returns>
        public PartialViewResult ListaNumeral(int verncodi, string fecha, int numeral)
        {
            NumeralModel model = new NumeralModel();
            int imes = Int32.Parse(fecha.Substring(0, 2));
            int ianho = Int32.Parse(fecha.Substring(3, 4));
            DateTime dtFecha = new DateTime(ianho, imes, 1);
            switch (numeral)
            {
                case 1: model.TablaNum1 = servicio.GetHtmlReporteNumeral1(1, dtFecha, verncodi); break;
                case 2: model.TablaNum2 = servicio.GetHtmlReporteNumeral2(2, dtFecha, verncodi); break;
                case 3: model.TablaNum3 = servicio.GetHtmlReporteNumeral3(3, dtFecha, verncodi); break;
                case 4: model.TablaNum4 = servicio.GetHtmlReporteNumeral4(4, dtFecha, verncodi); break;
                case 5: model.TablaNum5 = servicio.GetHtmlReporteNumeral5(5, dtFecha, verncodi); break;
                case 6: model.TablaNum6 = servicio.GetHtmlReporteNumeral6(6, dtFecha, verncodi); break;
                case 7: model.TablaNum7 = servicio.GetHtmlReporteNumeral7(7, dtFecha, verncodi); break;
                case 8: model.TablaNum8 = servicio.GetHtmlReporteNumeral8(8, dtFecha, verncodi); break;
                case 9: model.TablaNum9 = servicio.GetHtmlReporteNumeral9(9, dtFecha, verncodi); break;
                case 10: model.TablaNum10 = servicio.GetHtmlReporteNumeral10(10, dtFecha, verncodi); break;
                case 11: model.TablaNum11 = servicio.GetHtmlReporteNumeral11(11, dtFecha, verncodi); break;
            }
            return PartialView(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexGenerarVersion()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            NumeralModel model = new NumeralModel();
            model.ListaNumeral = servicio.GetByCriteriaSpoNumhistoria();
            int mes = DateTime.Now.AddMonths(-1).Month;
            string periodo = (mes > 9) ? mes + " " + DateTime.Now.AddMonths(-1).Year : "0" + mes.ToString() + " " + DateTime.Now.AddMonths(-1).Year;
            model.Fecha = periodo;
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="numeral"></param>
        /// <returns></returns>
        public PartialViewResult ListaGenerarVersion(string fecha, int numeral)
        {
            int imes = Int32.Parse(fecha.Substring(0, 2));
            int ianho = Int32.Parse(fecha.Substring(3, 4));
            DateTime dtFecha = new DateTime(ianho, imes, 1);

            VersionNumeralModel model = new VersionNumeralModel();
            var lista = servicio.GetByCriteriaSpoVersionnums(dtFecha, numeral);
            model.IdNumeral = numeral;
            model.ListaVersionNumeral = lista;

            string url = Url.Content("~/");
            model.Url = url;

            return PartialView(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="numeral"></param>
        /// <returns></returns>
        public JsonResult GeneraVersion(string periodo, int numeral)
        {
            ReporteNumeralModel model = new ReporteNumeralModel();
            try
            {
                base.ValidarSesionJsonResult();

                //servicio.GenerarScriptDatablase(1);
                int imes = Int32.Parse(periodo.Substring(0, 2));
                int ianho = Int32.Parse(periodo.Substring(3, 4));
                DateTime dtFecha = new DateTime(ianho, imes, 1);
                string usuario = this.UserName;

                string mens = string.Empty;
                switch (numeral)
                {
                    case 1: mens = servicio.Generarnumeral51(dtFecha, usuario, numeral); break;//Completado
                    case 2: mens = servicio.Generarnumeral52(dtFecha, usuario, numeral); break;//Completado
                    case 3:
                    case 4: mens = servicio.Generarnumeral53_54(dtFecha, usuario, numeral); break;//Completado
                    case 5: mens = servicio.GenerarNumeral55(dtFecha, usuario, numeral); break;//Completado
                    case 6: mens = servicio.GenerarNumeral56(dtFecha, usuario, numeral); break;
                    case 7: mens = servicio.GenerarNumeral57(dtFecha, usuario, numeral); break;//Completado
                    case 8: mens = servicio.GenerarNumeral58(dtFecha, usuario, numeral); break;//Completado(Falta costo marginal real sancionado)
                    case 9: mens = servicio.GenerarNumeral59(dtFecha, usuario, numeral); break;//Completado
                    case 10: mens = servicio.GenerarNumeral510(dtFecha, usuario, numeral); break;
                        //case 11: mens = servicio.GenerarNumeral511(dtFecha, usuario, numeral); break;
                }

                model.Resultado = "1";
                model.Mensaje = mens;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);
                model.Mensaje = e.Message;
                model.StrDetalle = e.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Excel web del numeral 5.11 de la Energía mensual
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="numeral"></param>
        /// <returns></returns>
        public JsonResult GenerarExcelwebNumeral511(string periodo, int numeral, int verncodi)
        {
            ReporteNumeralModel model = new ReporteNumeralModel();
            try
            {
                base.ValidarSesionJsonResult();

                int imes = Int32.Parse(periodo.Substring(0, 2));
                int ianho = Int32.Parse(periodo.Substring(3, 4));
                DateTime dtFecha = new DateTime(ianho, imes, 1);

                if (numeral == 11)
                {
                    model.ListaPrecalculoEnergiaForzada = verncodi == 0 ? servicio.GetEnergiaForzada(dtFecha) : servicio.ListarSpoNumeralGenforzadaByVersion(verncodi);
                    model.Resultado = "1";
                }
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);
                model.Resultado = "-1";
                model.Mensaje = e.Message;
                model.StrDetalle = e.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult EnvioEnergiaForzada(string periodo, int numeral, List<SpoNumeralGenforzadaDTO> listaEnergiaForzada)
        {
            ReporteNumeralModel model = new ReporteNumeralModel();
            try
            {
                base.ValidarSesionJsonResult();

                int imes = Int32.Parse(periodo.Substring(0, 2));
                int ianho = Int32.Parse(periodo.Substring(3, 4));
                DateTime dtFecha = new DateTime(ianho, imes, 1);
                string usuario = this.UserName;

                if (numeral == 11)
                {
                    model.Mensaje = servicio.GenerarNumeral511(dtFecha, usuario, numeral, listaEnergiaForzada);

                    model.Resultado = "1";
                }
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = "Se produjo un error: " + ex.Message;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="numeral"></param>
        /// <returns></returns>
        public JsonResult Generadetalleversion(string periodo, int numeral)
        {

            ReporteNumeralModel model = new ReporteNumeralModel();

            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string nameFile = "RptDetalleNumerales_5." + numeral + ConstantesAppServicio.ExtensionExcel;

                int imes = Int32.Parse(periodo.Substring(0, 2));
                int ianho = Int32.Parse(periodo.Substring(3, 4));
                DateTime dtFecha = new DateTime(ianho, imes, 1);

                servicio.GenerarDetalleNumeral(ruta + nameFile, numeral, dtFecha);

                model.Resultado = nameFile;
                model.NroEstadoNum = 1;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);
                model.Mensaje = e.Message;
                model.StrDetalle = e.StackTrace;
                model.NroEstadoNum = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="numeral"></param>
        /// <returns></returns>
        public JsonResult GeneradetalleversionPruebaConsultor(string periodo, int numeral)
        {

            ReporteNumeralModel model = new ReporteNumeralModel();

            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string nameFile = "RptDetalleNumerales_5." + numeral + ConstantesAppServicio.ExtensionExcel;

                int imes = Int32.Parse(periodo.Substring(0, 2));
                int ianho = Int32.Parse(periodo.Substring(3, 4));
                DateTime dtFecha = new DateTime(ianho, imes, 1);

                servicio.GenerarDetalleNumeral2(ruta + nameFile, numeral, dtFecha);

                model.Resultado = nameFile;
                model.NroEstadoNum = 1;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);
                model.Mensaje = e.Message;
                model.StrDetalle = e.StackTrace;
                model.NroEstadoNum = -1;
            }

            return Json(model);
        }

        #endregion

        #region Reporte Numerales

        public ActionResult IndexGenerarReporte()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ReporteNumeralModel model = new ReporteNumeralModel();
            var mes = DateTime.Now.AddMonths(-1);
            string periodo = mes.ToString("MM yyyy");
            model.Fecha = periodo;
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public PartialViewResult ListaGenerarReporte(string fecha)
        {
            int imes = Int32.Parse(fecha.Substring(0, 2));
            int ianho = Int32.Parse(fecha.Substring(3, 4));
            DateTime dtFecha = new DateTime(ianho, imes, 1);
            ReporteNumeralModel model = new ReporteNumeralModel();
            var lista = servicio.GetByCriteriaSpoVersionrep(dtFecha);
            model.ListaVersionReporte = lista.OrderBy(x => x.Verrnro).ToList();
            model.ListaNumeral = servicio.ListaEstadoNumeral(dtFecha);
            if (model.ListaNumeral.Count > 0)
            {
                model.NroEstadoNum = model.ListaNumeral.Count(x => x.Idestado == 2);
            }
            return PartialView(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="numeral"></param>
        /// <returns></returns>
        public JsonResult GenerarReporte(string fecha, int cc)
        {
            ReporteNumeralModel model = new ReporteNumeralModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (cc >= 11)//Todos los numerales validados
                {
                    DateTime dtFecha = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);
                    var versionReporte = new SpoVersionrepDTO()
                    {
                        Verrusucreacion = this.UserName,
                        Verrfeccreacion = DateTime.Now,
                        Verrestado = Acciones.Grabar,
                        Verrfechaperiodo = dtFecha
                    };

                    var verrcodi = servicio.SaveSpoVersionrep(versionReporte);

                    var listaNumeralValidado = servicio.ListaEstadoNumeral(dtFecha);
                    //Detalle del reporte
                    if (verrcodi.HasValue)
                    {
                        foreach (var numeral in listaNumeralValidado)
                        {
                            var versionReporteNumeral = new SpoVerrepnumDTO()
                            {
                                Verrcodi = verrcodi.Value,
                                Verncodi = numeral.Verncodi
                            };
                            servicio.SaveSpoVerrepnum(versionReporteNumeral);
                        }
                    }

                    model.Resultado = "1";
                }
                else { model.Resultado = "Faltan numerales por verificar"; }
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);
                model.Mensaje = e.Message;
                model.StrDetalle = e.StackTrace;
                model.NroEstadoNum = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 
        public JsonResult ValidaVersionReporte(int id)
        {
            ReporteNumeralModel model = new ReporteNumeralModel();

            try
            {
                base.ValidarSesionJsonResult();

                var spoVersionnum = new SpoVersionrepDTO()
                {
                    Verrcodi = id,
                    Verrestado = 2,
                    Verrfecmodificacion = DateTime.Now,
                    Verrusumodificacion = this.UserName
                };

                servicio.UpdateEstadoSpoVersionrep(spoVersionnum);
                model.NroEstadoNum = 1;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);
                model.Mensaje = e.Message;
                model.StrDetalle = e.StackTrace;
                model.NroEstadoNum = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="verrcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ConsultarDetalleReporte(int verrcodi)
        {
            ReporteNumeralModel model = new ReporteNumeralModel();
            try
            {
                base.ValidarSesionJsonResult();
                model.Resultado = servicio.ListaDetalleReporteNumeralesHtml(verrcodi);
                model.NroEstadoNum = 1;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);
                model.Mensaje = e.Message;
                model.StrDetalle = e.StackTrace;
                model.NroEstadoNum = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// Valida versión de numerales
        /// </summary>
        /// <param name="verncodi"></param>
        /// <returns></returns>
        public JsonResult ValidaVersionNumeral(int verncodi)
        {
            var model = new ReporteNumeralModel();

            try
            {
                base.ValidarSesionJsonResult();
                var spoVersionnum = new SpoVersionnumDTO()
                {
                    Verncodi = verncodi,
                    Vernestado = 2,
                    Vernfecmodificacion = DateTime.Now,
                    Vernusumodificacion = this.UserName
                };

                servicio.UpdateEstadoSpoVersionnum(spoVersionnum);
                model.Resultado = "1";
                model.Mensaje = "Numeral validado correctamente";
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);
                model.Mensaje = e.Message;
                model.StrDetalle = e.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fecha"></param>
        /// <returns></returns> 
        public JsonResult ExportaVersionReporte(int verrcodi, string fecha)
        {
            ReporteNumeralModel model = new ReporteNumeralModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime perido = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string nameFile = "RptNumerales" + ConstantesAppServicio.ExtensionExcel;

                var listaRepNumDetalle = servicio.GetByVersionReporteSpoVerrepnums(verrcodi);
                servicio.GenerarReporteExcelNumerales(ruta + nameFile, perido, listaRepNumDetalle);

                model.Resultado = nameFile;
                model.NroEstadoNum = 1;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);
                model.Mensaje = e.Message;
                model.StrDetalle = e.StackTrace;
                model.NroEstadoNum = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// Exporta versión del numeral
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult ExportaVersionNumeral(int verncodi, string fecha, int numeral)
        {
            ReporteNumeralModel model = new ReporteNumeralModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string nameFile = "RptNumerales_5." + numeral + ConstantesAppServicio.ExtensionExcel;

                int imes = Int32.Parse(fecha.Substring(0, 2));
                int ianho = Int32.Parse(fecha.Substring(3, 4));
                DateTime dtFecha = new DateTime(ianho, imes, 1);

                servicio.GenerarExcelNumeral(ruta + nameFile, numeral, dtFecha, verncodi);

                model.Resultado = nameFile;
                model.NroEstadoNum = 1;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);
                model.Mensaje = e.Message;
                model.StrDetalle = e.StackTrace;
                model.NroEstadoNum = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// //Descarga el archivo excel exportado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["fi"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string fullPath = ruta + nombreArchivo;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, ConstantesAppServicio.ExtensionExcel, nombreArchivo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarDatosBase(string fecha)
        {
            ReporteNumeralModel model = new ReporteNumeralModel();
            try
            {
                base.ValidarSesionJsonResult();

                int imes = Int32.Parse(fecha.Substring(0, 2));
                int ianho = Int32.Parse(fecha.Substring(3, 4));
                DateTime dtFecha = new DateTime(ianho, imes, 1);
                servicio.GenerarCarpetaDatosBase(dtFecha);
                model.Resultado = "1";
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);
                model.Mensaje = e.Message;
                model.StrDetalle = e.StackTrace;
                model.NroEstadoNum = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// Descarga el reporte excel del servidor
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult RareaCarpetaDatosBase(string fechita)
        {
            DateTime fecha = Convert.ToDateTime(fechita);
            const string FolderTemporal = "Areas\\IndicadoresSup\\DatosBase\\";
            string nombreCarpeta = string.Empty;
            nombreCarpeta = ExtensionMethod.NombreMes(fecha).ToUpper() + "_" + fecha.Year;
            string nombreArchivo = nombreCarpeta + ".zip";
            string ruta = AppDomain.CurrentDomain.BaseDirectory + FolderTemporal;
            string rutaCarpeta = ruta + nombreCarpeta;
            string rutaArchivo = ruta + nombreArchivo;
            ZipFile.CreateFromDirectory(rutaCarpeta, rutaArchivo);
            return File(rutaArchivo, Constantes.AppZip, nombreArchivo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ViewPopUpGenerarDatosBase()
        {
            ReporteNumeralModel model = new ReporteNumeralModel();

            var mes = DateTime.Now.AddMonths(-3);
            string periodo = mes.ToString("MM yyyy");
            model.Fecha = periodo;

            return PartialView(model);
        }

        #endregion
    }
}