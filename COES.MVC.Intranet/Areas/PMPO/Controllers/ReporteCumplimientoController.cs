using COES.MVC.Intranet.Areas.PMPO.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PMPO;
using log4net;
using System;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PMPO.Controllers
{
    /// <summary>
    /// Controladora del reporte de cumplimiento
    /// </summary>
    public class ReporteCumplimientoController : BaseController
    {
        readonly ProgramacionAppServicio pmpo = new ProgramacionAppServicio();

        #region Declaración de variables

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            RemisionModel model = new RemisionModel();
            model.Mes = pmpo.GetMesElaboracionDefecto().ToString(ConstantesAppServicio.FormatoMes);

            return View(model);
        }

        /// <summary>
        /// Reporte de cumplimiento
        /// </summary>
        /// <param name="mesElaboracion"></param>
        /// <param name="estadoCumplimiento"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaReporteCumplimiento(string mesElaboracion, string estadoCumplimiento)
        {
            RemisionModel model = new RemisionModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha1Mes = DateTime.ParseExact(mesElaboracion, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                string url = Url.Content("~/");
                model.HtmlReporte = pmpo.GenerarHtmlReporteCumplimiento(fecha1Mes, estadoCumplimiento);
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
        public JsonResult ListaHistorialCumplimiento(string mesElaboracion)
        {
            RemisionModel model = new RemisionModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha1Mes = DateTime.ParseExact(mesElaboracion, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                model.ListaReporteOsinerg = pmpo.ListarReportOsinerg(fecha1Mes);
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

        /// <summary>
        /// Reporte de cumplimiento
        /// </summary>
        /// <param name="mesElaboracion"></param>
        /// <param name="estadoCumplimiento"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarReporte(string mesElaboracion, string estadoCumplimiento)
        {
            RemisionModel model = new RemisionModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha1Mes = DateTime.ParseExact(mesElaboracion, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                string logo = @"logocoes.png";
                string ruta = AppDomain.CurrentDomain.BaseDirectory + @"Content\Images\";

                int idRpt = pmpo.GuardarPdfCumplimiento(ruta, logo, fecha1Mes, estadoCumplimiento, base.UserName);

                model.Resultado = idRpt.ToString();
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

        public FileResult DowloadFilePdf(int id)
        {
            byte[] buffer = pmpo.GetBufferArchivoRptOsinergminFinal(id, out string filename);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }
    }
}
