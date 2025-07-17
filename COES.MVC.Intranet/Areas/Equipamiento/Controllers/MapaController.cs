using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class SiteMapController : Controller
    {
        /// <summary>
        /// Muestra la pantalla inicial del formulario
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            MapaEquipoModel model = new MapaEquipoModel();
            model.ListaEmpresas = (new GeneralAppServicio()).ListadoComboEmpresasPorTipo(-2);
            model.ListaTipoEquipo = (new EquipamientoAppServicio()).ListEqFamilias().Where(t => t.Famcodi > 0).
                OrderBy(t => t.Famnomb).ToList();

            return View(model);
        }

    }
}
