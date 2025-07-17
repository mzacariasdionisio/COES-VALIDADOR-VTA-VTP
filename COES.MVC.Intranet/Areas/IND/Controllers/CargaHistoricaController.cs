using COES.MVC.Intranet.Areas.IND.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Indisponibilidades;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Ind.Controllers
{
    public class CargaHistoricaController : BaseController
    {
        private readonly INDAppServicio _indAppServicio;

        #region Declaración de variables

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

        public CargaHistoricaController()
        {
            log4net.Config.XmlConfigurator.Configure();
            _indAppServicio = new INDAppServicio();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        #region Hidro

        public ActionResult IndexHistoricoHidro()
        {
            var model = new IndisponibilidadesModel();
            model.UsarLayoutModulo = true;
            try
            {
                base.ValidarSesionJsonResult();

                _indAppServicio.CrearPeriodoRecalculoPorDefectoHistorico();

                model.MesFin = _indAppServicio.GetPeriodoMaxHistHidro().ToString(ConstantesAppServicio.FormatoMes);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult HandsonCargaHistoricoHidro(int? irptcodi = 0)
        {
            var model = new IndisponibilidadesModel();
            model.UsarLayoutModulo = true;
            try
            {
                base.ValidarSesionJsonResult();

                model.ListaReporte = _indAppServicio.ListarVersionesHistorico(ConstantesIndisponibilidades.ReportePR25HistoricoHidro);

                model.Cuadro = _indAppServicio.GetByIdIndCuadro(ConstantesIndisponibilidades.ReportePR25HistoricoHidro);
                if (irptcodi.GetValueOrDefault(0) <= 0) model.Cuadro.PeriodoFinHistoricoDesc = _indAppServicio.GetPeriodoMaxHistHidro().ToString("MMM yyyy");

                model.IndReporte = _indAppServicio.GetByIdIndReporte(irptcodi.Value);

                string url = Url.Content("~/");
                model.Resultado2 = _indAppServicio.GenerarHtmlListadoVerHistorico(url, model.ListaReporte);

                model.HandsonCargaHistorica = _indAppServicio.GenerarHandsonCargarHistoricaHidro(irptcodi ?? 0);
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

        [HttpPost]
        public JsonResult ExportarCargaHistoricoHidro(string stringJson)
        {
            var model = new IndisponibilidadesModel();
            model.UsarLayoutModulo = true;
            try
            {
                base.ValidarSesionJsonResult();

                var lstData = JsonConvert.DeserializeObject<List<CargaHistorica>>(stringJson);

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string nameFile = _indAppServicio.GenerarArchivoExcelCargaHistoricoHidro(ruta, lstData);

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
        public JsonResult GuardarCargaHistoricoHidro(string stringJson)
        {
            var model = new IndisponibilidadesModel();
            model.UsarLayoutModulo = true;
            try
            {
                base.ValidarSesionJsonResult();

                var lstData = JsonConvert.DeserializeObject<List<CargaHistorica>>(stringJson);
                int irptcodi = _indAppServicio.GuardarCargaHistoricoHidro(lstData, User.Identity.Name);

                model.Resultado = irptcodi.ToString();
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
        public JsonResult UploadCargaHistoricoHidro()
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();

                Stream stremExcel = (Request.Files.Count >= 1) ? Request.Files[0].InputStream : null;

                if (stremExcel != null)
                {
                    var file = Request.Files[0];
                    model.NombreArchivoUpload = file.FileName;

                    if (model.NombreArchivoUpload.IndexOf(".XLSX", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        model.Resultado2 = _indAppServicio.ImportarCargaHistoricoHidro(stremExcel);

                        model.Resultado = "1";
                    }
                    else
                    {
                        throw new ArgumentException("Archivo no válido. La extensión del archivo debe terminar en .xlsx");
                    }
                }
                else
                {
                    throw new ArgumentException("No existe archivo o no ha seleccionado recálculo.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Mensaje = ex.Message;
                model.Resultado = "-1";
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult GuardarFechaMaxHidro(string mes)
        {
            var model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();

                _indAppServicio.UpdateSiParametroValor(ConstantesIndisponibilidades.SiparvcodiHidro, base.UserName, mes);

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

        #region Termico

        public ActionResult IndexHistoricoTermico()
        {
            var model = new IndisponibilidadesModel();
            model.UsarLayoutModulo = true;
            try
            {
                base.ValidarSesionJsonResult();

                _indAppServicio.CrearPeriodoRecalculoPorDefectoHistorico();

                model.MesFin = _indAppServicio.GetPeriodoMaxHistTermo().ToString(ConstantesAppServicio.FormatoMes);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult HandsonCargaHistoricoTermico(int? irptcodi = 0)
        {
            var model = new IndisponibilidadesModel();
            model.UsarLayoutModulo = true;
            try
            {
                base.ValidarSesionJsonResult();

                model.ListaReporte = _indAppServicio.ListarVersionesHistorico(ConstantesIndisponibilidades.ReportePR25HistoricoTermo);

                model.Cuadro = _indAppServicio.GetByIdIndCuadro(ConstantesIndisponibilidades.ReportePR25HistoricoTermo);
                if (irptcodi.GetValueOrDefault(0) <= 0) model.Cuadro.PeriodoFinHistoricoDesc = _indAppServicio.GetPeriodoMaxHistTermo().ToString("MMM yyyy");

                model.IndReporte = _indAppServicio.GetByIdIndReporte(irptcodi.Value);

                string url = Url.Content("~/");
                model.Resultado2 = _indAppServicio.GenerarHtmlListadoVerHistorico(url, model.ListaReporte);

                model.HandsonCargaHistorica = _indAppServicio.GenerarHandsonCargarHistoricaTermica(irptcodi ?? 0);
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

        [HttpPost]
        public JsonResult ExportarCargaHistoricoTermico(string stringJson)
        {
            var model = new IndisponibilidadesModel();
            model.UsarLayoutModulo = true;
            try
            {
                base.ValidarSesionJsonResult();

                var lstData = JsonConvert.DeserializeObject<List<CargaHistorica>>(stringJson);

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string nameFile = _indAppServicio.GenerarArchivoExcelCargaHistoricoTermico(ruta, lstData);

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
        public JsonResult GuardarCargaHistoricoTermico(string stringJson)
        {
            var model = new IndisponibilidadesModel();
            model.UsarLayoutModulo = true;
            try
            {
                base.ValidarSesionJsonResult();

                var lstData = JsonConvert.DeserializeObject<List<CargaHistorica>>(stringJson);

                int irptcodi = _indAppServicio.GuardarCargaHistoricoTermico(lstData, User.Identity.Name);
                model.Resultado = irptcodi.ToString();
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
        public JsonResult UploadCargaHistoricoTermo()
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();

                Stream stremExcel = (Request.Files.Count >= 1) ? Request.Files[0].InputStream : null;

                if (stremExcel != null)
                {
                    var file = Request.Files[0];
                    model.NombreArchivoUpload = file.FileName;

                    if (model.NombreArchivoUpload.IndexOf(".XLSX", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        model.Resultado2 = _indAppServicio.ImportarCargaHistoricoTermo(stremExcel);

                        model.Resultado = "1";
                    }
                    else
                    {
                        throw new ArgumentException("Archivo no válido. La extensión del archivo debe terminar en .xlsx");
                    }
                }
                else
                {
                    throw new ArgumentException("No existe archivo o no ha seleccionado recálculo.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Mensaje = ex.Message;
                model.Resultado = "-1";
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult GuardarFechaMaxTermo(string mes)
        {
            var model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();

                _indAppServicio.UpdateSiParametroValor(ConstantesIndisponibilidades.SiparvcodiTermo, base.UserName, mes);

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

        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, ConstantesAppServicio.ExtensionExcel, nombreArchivo);
        }


    }
}