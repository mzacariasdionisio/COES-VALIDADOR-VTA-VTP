using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Intranet.Areas.AporteIntegrantes.Models;
using COES.Servicios.Aplicacion.CalculoPorcentajes;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System.Reflection;

namespace COES.MVC.Intranet.Areas.AporteIntegrantes.Controllers
{
    public class PresupuestoController : BaseController
    {
        // GET: /AporteIntegrantes/Presupuesto/
        public PresupuestoController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        
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
        
        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        CalculoPorcentajesAppServicio servicioCalculoPorcentajes = new CalculoPorcentajesAppServicio();
        Funcion servicioFuncion = new Funcion();

        public ActionResult Index()
        {
            base.ValidarSesionUsuario();
            BaseModel model = new BaseModel();
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name);
            return View(model);
        }

        [HttpPost]
        public ActionResult Lista(string nombre)
        {
            BaseModel model = new BaseModel();
            Log.Info("Lista Presupuestos - ListCaiPresupuestos");
            model.ListaPresupuesto = this.servicioCalculoPorcentajes.ListCaiPresupuestos();
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, User.Identity.Name);
            model.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, User.Identity.Name);
            return PartialView(model);
        }


        public ActionResult New() {

            BaseModel model = new BaseModel();
            model.EntidadPresupuesto = new CaiPresupuestoDTO();
            if (model.EntidadPresupuesto == null)
            {
               return HttpNotFound();
            }
            model.EntidadPresupuesto.Caiprscodi = 0;
            //model.Entidad.Caiprsnombre = "Mensual";
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            TempData["Mescodigo"] = new SelectList(this.servicioFuncion.ObtenerMes(), "value", "Text", model.EntidadPresupuesto.Caiprsnromeses);
            TempData["Aniocodigo"] = new SelectList(this.servicioFuncion.ObtenerAnio(), "value", "Text", model.EntidadPresupuesto.Caiprsanio);

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(BaseModel modelo) 
        {
            //registrar=true, actualizar=false
            bool bRegistrar = false;
            if(ModelState.IsValid){
                if (modelo.EntidadPresupuesto.Caiprscodi == 0) 
                {
                    bRegistrar = true;
                }
                /*mas validaciones*/

                if (bRegistrar)
                {
                    CaiPresupuestoDTO dto = new CaiPresupuestoDTO();
                    //dto.Caiprscodi = modelo.Entidad.Caiprscodi;//en el repository se crea el codigo de forma auto
                    dto.Caiprsanio = modelo.EntidadPresupuesto.Caiprsanio;
                    dto.Caiprsmesinicio = modelo.EntidadPresupuesto.Caiprsmesinicio;
                    dto.Caiprsnromeses = modelo.EntidadPresupuesto.Caiprsnromeses;
                    dto.Caiprsnombre = modelo.EntidadPresupuesto.Caiprsnombre;
                    dto.Caiprsusucreacion = User.Identity.Name;
                    dto.Caiprsusumodificacion = User.Identity.Name;
                    /*Inserta el registro*/
                    Log.Info("Insertar registro - SaveCaiPresupuesto");
                    this.servicioCalculoPorcentajes.SaveCaiPresupuesto(dto);
                }
                else 
                { 
                    /*actualiza*/
                    CaiPresupuestoDTO dto = new CaiPresupuestoDTO();
                    dto.Caiprscodi = modelo.EntidadPresupuesto.Caiprscodi;//para el actualizar si necesita del codigo
                    dto.Caiprsanio = modelo.EntidadPresupuesto.Caiprsanio;
                    dto.Caiprsmesinicio = modelo.EntidadPresupuesto.Caiprsmesinicio;
                    dto.Caiprsnromeses = modelo.EntidadPresupuesto.Caiprsnromeses;
                    dto.Caiprsnombre = modelo.EntidadPresupuesto.Caiprsnombre;
                    dto.Caiprsusumodificacion = User.Identity.Name;
                    /*Actualizo el presupuesto*/
                    Log.Info("Actualiza registro - UpdateCaiPresupuesto");
                    this.servicioCalculoPorcentajes.UpdateCaiPresupuesto(dto);
                }
                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return RedirectToAction("Index");
            }
            //Error
            modelo.sError = "Se ha producido un error al insertar la información";
            modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            TempData["Mescodigo"] = new SelectList(this.servicioFuncion.ObtenerMes(), "Value", "Text", modelo.EntidadPresupuesto.Caiprsmesinicio);
            TempData["Aniocodigo"] = new SelectList(this.servicioFuncion.ObtenerAnio(), "Value", "Text", modelo.EntidadPresupuesto.Caiprsanio);
            return PartialView(modelo); 
        }

        public ActionResult Edit(int caiprscodi) 
        {
            BaseModel modelo = new BaseModel();
            Log.Info("Entidad Presupuesto - GetByIdCaiPresupuesto");
            modelo.EntidadPresupuesto = this.servicioCalculoPorcentajes.GetByIdCaiPresupuesto(caiprscodi);

            if (modelo.EntidadPresupuesto == null)
            {
                return HttpNotFound();
            }
            modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            TempData["Mescodigo"] = new SelectList(this.servicioFuncion.ObtenerMes(), "Value", "Text", modelo.EntidadPresupuesto.Caiprsmesinicio);
            TempData["Aniocodigo"] = new SelectList(this.servicioFuncion.ObtenerAnio(), "Value", "Text", modelo.EntidadPresupuesto.Caiprsanio);
            
            return PartialView(modelo); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string Delete(int id = 0)
        {
            string salida = "";
            try
            {
                Log.Info("Elimina registro - DeleteCaiPresupuesto");
                this.servicioCalculoPorcentajes.DeleteCaiPresupuesto(id);
                salida = "true";
            }
            catch
            {
                salida = "false";
            }
            return salida;
        }
            
        public ActionResult View(int id=0) 
        {
            BaseModel modelo = new BaseModel();
            Log.Info("Entidad Presupuesto - GetByIdCaiPresupuesto");
            modelo.EntidadPresupuesto = this.servicioCalculoPorcentajes.GetByIdCaiPresupuesto(id);
            return PartialView(modelo);
        }
    }
}
