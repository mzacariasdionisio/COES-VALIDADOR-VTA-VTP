using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.Eventos;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;

namespace COES.MVC.Intranet.Areas.Eventos.Controllers
{
    public class RecomendacionesController : BaseController
    {
        public ActionResult Index()
        {
            AnalisisFallasAppServicio appAnalisisFallas = new AnalisisFallasAppServicio();
            ViewBag.EmpresaRecomendacion = appAnalisisFallas.ObtenerEmpresasRecomendacion();

            ViewBag.FechaInicio = DateTime.Now.AddDays(-30).ToString(Constantes.FormatoFecha);
            ViewBag.FechaFin = DateTime.Now.AddDays(1).ToString(Constantes.FormatoFecha);

            ViewBag.grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            //ViewBag.grabar = true;


            return View();
        }
        public JsonResult ObtenerMedidasAdoptadas(int id)
        {
            AnalisisFallasAppServicio appAnalisisFallas = new AnalisisFallasAppServicio();
            JsonResult jRespuesta;
            AnalisisFallaDTO entity = appAnalisisFallas.ObtenerMedidasAdoptadas(id);
            jRespuesta = Json(entity, JsonRequestBehavior.AllowGet);
            return jRespuesta;
        }

        public ActionResult MedidasAdoptadas()
        {
            return View();
        }
        public JsonResult ConsultarRecomendacion(EmpresaRecomendacionDTO obj)//Dominio.DTO.Sic.EventoDTO oEventoDTO
        {
            AnalisisFallasAppServicio appAnalisisFallas = new AnalisisFallasAppServicio();
            JsonResult jRespuesta;
            List<EmpresaRecomendacionDTO> Lista = appAnalisisFallas.ConsultarRecomendacion(obj);
            jRespuesta = Json(Lista, JsonRequestBehavior.AllowGet);
            return jRespuesta;
        }
    }
}
