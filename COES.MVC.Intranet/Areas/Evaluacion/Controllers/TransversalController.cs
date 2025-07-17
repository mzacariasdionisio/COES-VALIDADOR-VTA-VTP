using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.Equipamiento;
using log4net;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.Evaluacion.Models;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Evaluacion;
using COES.Dominio.DTO.Sic;
using System.Collections.Generic;


namespace COES.MVC.Intranet.Areas.Evaluacion.Controllers
{
    public class TransversalController : BaseController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ReleController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        EquipamientoAppServicio servicioEquipamiento = new EquipamientoAppServicio();
        EquipoProteccionAppServicio equipoProteccion = new EquipoProteccionAppServicio();
        ProyectoActualizacionAppServicio proyectoActualzacion = new ProyectoActualizacionAppServicio();
        ConsultaMedidoresAppServicio consultaMedidores = new ConsultaMedidoresAppServicio();
        ReleAppServicio servicioRele = new ReleAppServicio();
        TransversalAppServicio servicioTransversal = new TransversalAppServicio();

        public TransversalController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                log.Fatal(NameController, ex);
                throw;
            }
        }

        /*INICIO CONSULTAR EQUIPO*/
        [AllowAnonymous]
        public ActionResult IndexConsultarEquipo(string codigoId, string pathReturn)
        {
            TransversalModel modelo = new TransversalModel();
            EprEquipoDTO equipo = servicioTransversal.ObtenerCabeceraEquipoPorId(Convert.ToInt32(codigoId));
            modelo.CodigoId = equipo.Equicodi.ToString();
            modelo.Codigo = equipo.Codigo;
            modelo.Ubicacion = equipo.Ubicacion;
            modelo.Empresa = equipo.Empresa;
            modelo.Area = equipo.Area;
            modelo.PathReturn = pathReturn.Replace("-", "/");

            return View(modelo);
        }

        [ActionName("IndexConsultarEquipo"), HttpPost]
        public ActionResult IndexConsultarEquipoPost(TransversalModel datos)
        {
            return View(datos);
        }

        [HttpPost]
        public PartialViewResult ListaConsultarEquipo(string codigoId)
        {
            ListadoTransversalModel model = new ListadoTransversalModel();
            model.ListaConsultarEquipo = servicioTransversal.ListaConsultarEquipo(codigoId).ToList();
            return PartialView("~/Areas/Evaluacion/Views/Transversal/ListaConsultarEquipo.cshtml", model);
        }
        /*FIN CONSULTAR EQUIPO*/

        /*INICIO HISTORIAL DE CAMBIOS*/
        [AllowAnonymous]
        public ActionResult IndexHistorialCambio(string codigoId, string pathReturn)
        {
            TransversalModel modelo = new TransversalModel();
            EprEquipoDTO equipo = servicioTransversal.ObtenerCabeceraEquipoPorId(Convert.ToInt32(codigoId));
            modelo.CodigoId = equipo.Equicodi.ToString();
            modelo.Codigo = equipo.Codigo;
            modelo.Ubicacion = equipo.Ubicacion;
            modelo.Empresa = equipo.Empresa;
            modelo.Area = equipo.Area;
            modelo.PathReturn = pathReturn.Replace("-", "/");

            return View(modelo);
        }

        [ActionName("IndexHistorialCambio"), HttpPost]
        public ActionResult IndexHistorialCambioPost(TransversalModel datos)
        {
            return View(datos);
        }

        [HttpPost]
        public PartialViewResult ListaHistorialCambio(string codigoId)
        {
            ListadoTransversalModel model = new ListadoTransversalModel();
            model.ListaActualizaciones = servicioTransversal.ListaActualizaciones(codigoId).ToList();
            model.ListaPropiedadActualizada = new List<EprEquipoDTO>();

            return PartialView("~/Areas/Evaluacion/Views/Transversal/ListaHistorialCambio.cshtml", model);
        }

        [HttpPost]
        public JsonResult ListaPropiedadesActualizadas(string codigoId, string proyectoId)
        {

            ListadoTransversalModel model = new ListadoTransversalModel();
            try
            {
                model.ListaPropiedadActualizada = servicioTransversal.ListaPropiedadesActualizadas(codigoId, proyectoId).ToList();

                return Json(model);
            }
            catch (Exception ex)
            {
                var stak = ex.StackTrace.ToString();
                var msgError = ex.Message.ToString();
                return Json(model);
            }
        }
        /*FIN HISTORIAL DE CAMBIOS*/

    }
}
