using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.Eventos;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Helper;

namespace COES.MVC.Intranet.Areas.Eventos.Controllers
{
    public class ObservacionesController : Controller
    {
        public ActionResult Index()
        {
            AnalisisFallasAppServicio appAnalisisFallas = new AnalisisFallasAppServicio();
            ViewBag.EmpresaObservacion = appAnalisisFallas.ObtenerEmpresasObservacion();

            ViewBag.FechaInicio = DateTime.Now.AddDays(-30).ToString(Constantes.FormatoFecha);
            ViewBag.FechaFin = DateTime.Now.AddDays(1).ToString(Constantes.FormatoFecha);

            return View();
        }
        public JsonResult ConsultarObservacion(EmpresaObservacionDTO obj)
        {
            AnalisisFallasAppServicio appAnalisisFallas = new AnalisisFallasAppServicio();
            JsonResult jRespuesta;
            List<EmpresaObservacionDTO> Lista = appAnalisisFallas.ConsultarObservacion(obj);
            jRespuesta = Json(Lista, JsonRequestBehavior.AllowGet);
            return jRespuesta;
        }

    }
}
