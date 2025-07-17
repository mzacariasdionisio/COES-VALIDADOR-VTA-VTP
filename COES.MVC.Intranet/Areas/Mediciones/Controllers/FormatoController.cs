using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Mediciones.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Mediciones.Controllers
{
    public class FormatoController : Controller
    {
        GeneracionRERAppServicio logic = new GeneracionRERAppServicio();
        
        /// <summary>
        /// Muestra la pantalla inicial 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Muestra el listado de puntos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista()
        {
            FormatoModel model = new FormatoModel();
            model.ListaPuntos = this.logic.GetByCriteriaWbGeneracionrers();
            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar la vista para la creacion de puntos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Agregar()
        {
            FormatoModel model = new FormatoModel();
            model.ListaEmpresa = this.logic.ObtenerPuntosEmpresas();
            return PartialView(model);
        }

        /// <summary>
        /// Permite cargar las centrales de la empresa seleccionada
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarCentrales(int idEmpresa)
        {
            List<WbGeneracionrerDTO> entitys = this.logic.ObtenerPuntosCentrales(idEmpresa);
            SelectList list = new SelectList(entitys, EntidadPropiedad.PtoMediCodi, EntidadPropiedad.EquiNomb);
            return Json(list);
        }

        /// <summary>
        /// Permite cargar las unidades de la central seleccionada
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarUnidades(int ptoCentral)
        {
            List<WbGeneracionrerDTO> entitys = this.logic.ObtenerPuntosUnidades(ptoCentral);
            SelectList list = new SelectList(entitys, EntidadPropiedad.PtoMediCodi, EntidadPropiedad.EquiNomb);
            return Json(list);
        }

        [HttpPost]
        public JsonResult AgregarPunto(int indicador, int ptoCentral, int? ptoUnidad)
        {
            try
            {
                int resultado = this.logic.SaveWbGeneracionrer(indicador, ptoCentral, ptoUnidad, User.Identity.Name);
                return Json(resultado);
            }
            catch
            {
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult EliminarPunto(int ptoMediCodi)
        {
            try
            {
                this.logic.DeleteWbGeneracionrer(ptoMediCodi, User.Identity.Name);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult GrabarConfiguracion(int ptomedicodi, decimal? minimo, decimal? maximo)
        {
            try
            {
                this.logic.GrabarConfiguracion(ptomedicodi, minimo, maximo, User.Identity.Name);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

    }
}
