using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.Servicios.Aplicacion.Equipamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class ManiobraController : Controller
    {
        /// <summary>
        /// Instancia para el manejo de datos
        /// </summary>
        EquipamientoAppServicio servicio = new EquipamientoAppServicio();

        /// <summary>
        /// Carga la primera pantalla
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ManiobraModel model = new ManiobraModel();
            model.ListaFamilia = this.servicio.ObtenerFamiliasProcManiobras();
            return View(model);
        }

        /// <summary>
        /// Permite listar los equipos con maniobras
        /// </summary>
        /// <param name="famCodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listar(int famCodi)
        {
            ManiobraModel model = new ManiobraModel();          
            int propCodi = this.ObtenerCodigoPropiedad(famCodi);
            model.ListaEquipos = this.servicio.ObtenerEquiposProcManiobras(famCodi, propCodi);
            return PartialView(model);
        }

        /// <summary>
        /// Permite actualizar el procedimiento de maniobras
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <param name="idFamilia"></param>
        /// <param name="enlace"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarEnlace(int idEquipo, int idFamilia, string enlace)
        {
            try
            {
                int propcodi = this.ObtenerCodigoPropiedad(idFamilia);
                this.servicio.ActualizarProcedimientoManiobra(propcodi, idEquipo, enlace, User.Identity.Name);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite obtener el codigo de la propiedad
        /// </summary>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        private int ObtenerCodigoPropiedad(int famcodi)
        {
            int propcodi = 0;
            switch (famcodi)
            {
                case 6: { propcodi = 1806; break; }
                case 14: { propcodi = 1807; break; }
                case 8: { propcodi = 1808; break; }
                case 17: { propcodi = 1809; break; }
                case 7: { propcodi = 1810; break; }
                case 10: { propcodi = 1811; break; }
                case 9: { propcodi = 1812; break; }
                case 12: { propcodi = 1813; break; }
                default: { break; }
            }

            return propcodi;
        }
    }
}
