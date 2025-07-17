using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.IND.Helper;
using COES.MVC.Intranet.Areas.IND.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Indisponibilidades;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.IND.Controllers
{
    public class RelacionEmpresaController : BaseController
    {
        readonly INDAppServicio indServicio = new INDAppServicio();

        #region Declaración de variables

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public RelacionEmpresaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #endregion


        public ActionResult Index()
        {
            //if (!base.IsValidSesionView()) return base.RedirectToLogin();
            //if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            IndisponibilidadesModel model = new IndisponibilidadesModel();
            model.TienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            model.bNuevo = (new Funcion()).ValidarPermisoNuevo(364, User.Identity.Name);

            return View(model);
        }


        /// <summary>
        /// Muestra vista principal Indisponibilidades de empresas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListadoRelacionEmpresa()
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                //realizar proceso 

                model.ListadoIdnRelacionEmpresa = indServicio.ListIndRelacionEmpresa();
                model.bEditar = (new Funcion()).ValidarPermisoEditar(364, User.Identity.Name);
                model.bEliminar = (new Funcion()).ValidarPermisoEliminar(364, User.Identity.Name);
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Prepara una vista para ingresar un nuevo registro
        /// </summary>
        /// <returns></returns>
        public ActionResult New()
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            model.Entidad = new IndRelacionEmpresaDTO();
            if (model.Entidad == null)
            {
                return HttpNotFound();
            }
            model.Entidad.Relempcodi = 0;
            model.Entidad.Relempfeccreacion = DateTime.Now;
            model.Entidad.Relempfecmodificacion = DateTime.Now;
            model.Entidad.Relempusucreacion = User.Identity.Name;
            model.Entidad.Relempusumodificacion = User.Identity.Name;
            model.bGrabar = (new Funcion()).ValidarPermisoGrabar(364, User.Identity.Name);

            indServicio.ListaEmpresa_Central(out List<SiEmpresaDTO> listaEmpresa, out List<IndRelacionEmpresaDTO> listaCentral);
            model.ListaEmpresa = listaEmpresa;
            model.ListaCentral2 = listaCentral;
            TempData["Emprcodigo"] = new SelectList(model.ListaEmpresa, "Emprcodi", "Emprnomb");
            TempData["Centralcodigo"] = new SelectList(model.ListaCentral2, "equicodicentral", "equinomb");

            return PartialView(model);
        }

        /// <summary>
        /// Prepara una vista para editar un nuevo registro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            indServicio.ListaEmpresa_Central(out List<SiEmpresaDTO> listaEmpresa, out List<IndRelacionEmpresaDTO> listaCentral);
            model.ListaEmpresa = listaEmpresa;
            model.ListaCentral2 = listaCentral;
            TempData["Emprcodigo"] = new SelectList(model.ListaEmpresa, "Emprcodi", "Emprnomb");
            TempData["Centralcodigo"] = new SelectList(model.ListaCentral2, "equicodicentral", "equinomb");
            model.ListaGaseoducto = indServicio.ListGaseoducto();
            model.ListaTipoTecnologia = (new Funcion()).listTipoTecnologia();
            model.ListaGrupo = indServicio.ListarPrGrupoForCN2();

            model.Entidad = indServicio.GetByIdIndRelacionEmpresa(id);
            if (model.Entidad == null)
            {
                return HttpNotFound();
            }
            model.bGrabar = (new Funcion()).ValidarPermisoGrabar(364, User.Identity.Name);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult PostListUnidad(int equicodicentral)
        {

            var unidad = indServicio.ListaUnidad(equicodicentral);
            return Json(unidad, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// Permite grabar los datos del formulario
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(IndisponibilidadesModel model)
        {
            string fecha = DateTime.Now.ToString(ConstantesIndisponibilidades.FormatoFecha);

            if (ModelState.IsValid)
            {
                try
                {
                    model.Entidad.Relempusucreacion = User.Identity.Name;
                    model.Entidad.Relempusumodificacion = User.Identity.Name;
                    model.Entidad.Relempfecmodificacion = DateTime.ParseExact(fecha,ConstantesIndisponibilidades.FormatoFecha,CultureInfo.InvariantCulture);
                    model.Relempcodi = indServicio.SaveOrUpdateIndRelacionEmpresa(model.Entidad);
                    TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Log.Error(NameController, ex);
                    model.Resultado = "-1";
                    model.Mensaje = ex.Message;
                    model.Detalle = ex.StackTrace;
                    TempData["sMensajeError"] = ex.Message;
                }
            }
            //Error
            indServicio.ListaEmpresa_Central(out List<SiEmpresaDTO> listaEmpresa, out List<IndRelacionEmpresaDTO> listaCentral);
            model.ListaEmpresa = listaEmpresa;
            model.ListaCentral2 = listaCentral;
            TempData["Emprcodigo"] = new SelectList(model.ListaEmpresa, "Emprcodi", "Emprnomb");
            TempData["Centralcodigo"] = new SelectList(model.ListaCentral2, "equicodicentral", "equinomb");
            model.ListaGaseoducto = indServicio.ListGaseoducto();
            model.ListaTipoTecnologia = (new Funcion()).listTipoTecnologia();
            model.ListaGrupo = indServicio.ListarPrGrupoForCN2();

            model.Entidad = indServicio.GetByIdIndRelacionEmpresa(model.Entidad.Relempcodi);
            if (model.Entidad == null)
            {
                return HttpNotFound();
            }
            model.bGrabar = (new Funcion()).ValidarPermisoGrabar(364, User.Identity.Name);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult Delete(int id = 0)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            var result = "false";
            try
            {

                model.Relempcodi = indServicio.DeleteIndRelacionEmpresa(id);
                result = "true";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult ActualizarLista()
        {

            indServicio.ActualizarRelacionEmpresa(User.Identity.Name);
            return Json(1);

        }
    }
}
