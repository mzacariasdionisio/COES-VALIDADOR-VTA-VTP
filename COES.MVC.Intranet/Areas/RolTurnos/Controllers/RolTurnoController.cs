using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.RolTurnos.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using System;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.RolTurnos.Controllers
{
    public class RolTurnoController : BaseController
    {
        readonly RolTurnosAppServicio servicio = new RolTurnosAppServicio();

        /// <summary>
        /// Muestra la pantalla inicial del rol de turnos
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            RolTurnoModel model = new RolTurnoModel
            {
                Fecha = DateTime.Now.ToString("MM-yyyy")
            };
            return View(model);
        }

        /// <summary>
        /// Permite obtener la data de rol de turnos
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Consultar(string fecha)
        {
            string[] strFecha = fecha.Split(Constantes.CaracterGuion);
            int anio = int.Parse(strFecha[1]);
            int mes = int.Parse(strFecha[0]);
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            EstructuraRolTurno data = this.servicio.ObtenerEstructuraRolTurno(anio, mes, false);
            result.Content = serializer.Serialize(data);
            result.ContentType = "application/json";
            return result;
        }

        /// <summary>
        /// Permite almacenar los datos del rol de turnos
        /// </summary>
        /// <param name="data"></param>
        /// <param name="modalidad"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarRolTurno(string[][] data, string[][] modalidad, string fecha)
        {
            string[] strFecha = fecha.Split(Constantes.CaracterGuion);
            int anio = int.Parse(strFecha[1]);
            int mes = int.Parse(strFecha[0]);
            return Json(this.servicio.GrabarRolTurno(data, modalidad, anio, mes, base.UserName));
        }

        /// <summary>
        /// Permite generar el formato de carga de rol de turnos
        /// </summary>
        /// <param name="fecha"></param>        
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarFormato(string fecha)
        {
            string[] strFecha = fecha.Split(Constantes.CaracterGuion);
            int anio = int.Parse(strFecha[1]);
            int mes = int.Parse(strFecha[0]);
            string path = AppDomain.CurrentDomain.BaseDirectory + RolTurnoHelper.RutaReporte;
            string file = RolTurnoHelper.FormatoCargaRolTurno;

            return Json(this.servicio.GenerarFormatoRolTurno(path, file, anio, mes));
        }

        /// <summary>
        /// Permite descargar el formato de rol de turnos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato(string fecha)
        {
            string[] strFecha = fecha.Split(Constantes.CaracterGuion);
            int anio = int.Parse(strFecha[1]);
            int mes = int.Parse(strFecha[0]);

            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RolTurnoHelper.RutaReporte +
                RolTurnoHelper.FormatoCargaRolTurno;
            return File(fullPath, Constantes.AppExcel,
                string.Format(RolTurnoHelper.ExportacionArchivo, mes.ToString().PadLeft(2, '0') + anio.ToString()));
        }

        /// <summary>
        /// Permite cargar el archivo al servidor
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + RolTurnoHelper.RutaReporte;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = path + RolTurnoHelper.ArchivoImportacionRolTurno;

                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Permite importar los datos de rol de turnos
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Importar(string fecha)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + RolTurnoHelper.RutaReporte;
            string file = RolTurnoHelper.ArchivoImportacionRolTurno;
            string[] strFecha = fecha.Split(Constantes.CaracterGuion);
            int anio = int.Parse(strFecha[1]);
            int mes = int.Parse(strFecha[0]);
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            EstructuraRolTurno data = this.servicio.CargarRolTurnoExcel(path, file, anio, mes);
            result.Content = serializer.Serialize(data);
            result.ContentType = "application/json";
            return result;
        }

        /// <summary>
        /// Permite obtener reporte
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Reporte(string fecha)
        {
            RolTurnoModel model = new RolTurnoModel();

            string[] strFecha = fecha.Split(Constantes.CaracterGuion);
            int anio = int.Parse(strFecha[1]);
            int mes = int.Parse(strFecha[0]);
            model.Reporte = this.servicio.ObtenerReporteRolTurno(anio, mes);

            return PartialView(model);
        }
    }

}