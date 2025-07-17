using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Web.Helper;
using COES.MVC.Intranet.Areas.Web.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Web.Controllers
{
    public class CmVsTarifaController : BaseController
    {
        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        PortalAppServicio servicio = new PortalAppServicio();

        /// <summary>
        /// Muestra la pagina inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Muestra el listado de datos
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Lista()
        {
            CmVsTarifaModel model = new CmVsTarifaModel();
            model.Listado = this.servicio.ListarCmVsTarifa();
            return PartialView(model);
        }


        /// <summary>
        /// Permite cargar el archivo de potencia
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = path + Constantes.ArchivoCmVsTarifa;

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
        /// Permite cargar la potencia desde excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Procesar()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + Constantes.ArchivoCmVsTarifa;
                List<WbCmvstarifaDTO> list = WebHelper.LeerDesdeFormato(path);
                this.servicio.CargarCmVsTarifa(list, base.UserName);
                return Json(1);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}
