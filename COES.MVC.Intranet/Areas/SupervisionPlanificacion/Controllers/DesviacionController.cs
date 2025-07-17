using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.SupervisionPlanificacion.Helpers;
using COES.MVC.Intranet.Areas.SupervisionPlanificacion.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.General;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.SupervisionPlanificacion.Controllers
{
    public class DesviacionController : Controller
    {
        private DateTime fechaExcel;
        private static List<DesviacionDTO> list;

        /// <summary>
        /// Pagina de inicio
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            DesviacionModel model = new DesviacionModel();
            model.FechaConsulta = DateTime.Now.ToString(Constantes.FormatoFecha);
            list = null;
            return View(model);
        }

        /// <summary>
        /// Grilla de elementos cargados
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Grilla(string fecha)
        {
            DesviacionModel model = new DesviacionModel();
           
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            model.ListaDesviaciones = (new DesviacionAppServicio()).ListarDesviacion(fechaConsulta);
            model.Indicador = Constantes.NO;
            if (model.ListaDesviaciones.Count > 0) model.Indicador = Constantes.SI;

            return PartialView(model);
        }

        /// <summary>
        /// Permite cargar un archivo al servidor
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload(int? chunk, string name)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = file.FileName;
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(path + Constantes.ArchivoDesviacion);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// Permite procesar el archivo cargado en un directorio
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public JsonResult ProcesarArchivo(string fecha)
        {
            DesviacionModel model = new DesviacionModel();
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + Constantes.ArchivoDesviacion;
                List<string> errores = new List<string>();
                List<DesviacionDTO> list = (new RpdHelper()).LeerDesdeFormato(path, out errores);

                if (list != null)
                {
                    if (errores.Count == 0)
                    {
                        DateTime fechaProceso = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        (new DesviacionAppServicio()).SaveDesviacion(list, fechaProceso, User.Identity.Name);
                    }
                    else
                    {
                        return Json(2);
                    }
                }
                else
                {
                    return Json(0);
                }

                return Json(1);
            }
            catch
            {
                list = null;
                return Json(-1);
            }
        }
    }
}
