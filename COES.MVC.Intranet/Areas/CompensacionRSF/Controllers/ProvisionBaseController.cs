using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.CompensacionRSF.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CompensacionRSF;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CompensacionRSF.Controllers
{
    public class ProvisionBaseController : BaseController
    {
        // GET: /CompensacionRSF/ProvisionBase/

        public ProvisionBaseController()
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
        CompensacionRSFAppServicio servicioCompensacionRsf = new CompensacionRSFAppServicio();
        CentralGeneracionAppServicio servicioCentral = new CentralGeneracionAppServicio();
        BarraUrsAppServicio servicioBarraUrs = new BarraUrsAppServicio();

        public ActionResult Index()
        {
            base.ValidarSesionUsuario();
            ProvisionBaseModel model = new ProvisionBaseModel();
            Log.Info("ListaCentralGeneracion - ListCentralGeneracion");
            model.ListaCentralGeneracion = this.servicioCentral.ListCentralGeneracion();
            return View(model);
        }

        [HttpPost]
        public ActionResult Lista()
        {
            ProvisionBaseModel model = new ProvisionBaseModel();
            Log.Info("ListaProvisionbase - ListVcrProvisionbasesIndex");
            model.ListaProvisionbase = this.servicioCompensacionRsf.ListVcrProvisionbasesIndex();
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, User.Identity.Name);
            model.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, User.Identity.Name);
            return PartialView(model);
        }

        public JsonResult GetVersion(int equicodi)
        {
            ProvisionBaseModel model = new ProvisionBaseModel();
            Log.Info("ListaURS - ListBarraURSbyEquicodi");
            model.ListaURS = this.servicioBarraUrs.ListBarraURSbyEquicodi(equicodi);
            model.bEjecutar = true;
            return Json(model);
        }

        public ActionResult New()
        {
            base.ValidarSesionUsuario();
            ProvisionBaseModel model = new ProvisionBaseModel();
            model.EntidadProvisionbase = new VcrProvisionbaseDTO();
            if (model.EntidadProvisionbase == null)
            {
                return HttpNotFound();
            }
            model.EntidadCentralGeneracion = new CentralGeneracionDTO();
            Log.Info("ListaCentralGeneracion - ListCentralGeneracion");
            model.ListaCentralGeneracion = this.servicioCentral.ListCentralGeneracion();
            model.EntidadURS = new TrnBarraursDTO();
            Log.Info("ListaURS - ListURS");
            model.ListaURS = this.servicioBarraUrs.ListURS();

            model.EntidadProvisionbase.Vcrpbcodi = 0;
            model.EntidadProvisionbase.Equicodi = 0;
            //model.EntidadProvisionbase.Vcrpbperiodoini = DateTime.Now;
            //model.EntidadProvisionbase.Vcrpbperiodofin = DateTime.Now;
            model.Vcrpbperiodoini = DateTime.Now.ToString("dd/MM/yyyy");
            model.Vcrpbperiodofin = DateTime.Now.ToString("dd/MM/yyyy");

            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            Log.Info("ListaCentralGeneracion - ListCentralGeneracion");
            model.ListaCentralGeneracion = this.servicioCentral.ListCentralGeneracion();
            return PartialView(model);
        }

        public ActionResult Edit(int vcrpbcodi = 0)
        {
            base.ValidarSesionUsuario();
            ProvisionBaseModel model = new ProvisionBaseModel();
            Log.Info("EntidadProvisionbase - GetByIdVcrProvisionbase");
            model.EntidadProvisionbase = this.servicioCompensacionRsf.GetByIdVcrProvisionbase(vcrpbcodi);
            if (model.EntidadProvisionbase == null)
            {
                return HttpNotFound();
            }
            model.Vcrpbperiodoini = model.EntidadProvisionbase.Vcrpbperiodoini.ToString("dd/MM/yyyy");
            if (model.EntidadProvisionbase.Vcrpbperiodofin != null)
            { model.Vcrpbperiodofin = model.EntidadProvisionbase.Vcrpbperiodofin.ToString("dd/MM/yyyy"); }
            Log.Info("ListaCentralGeneracion - ListCentralGeneracion");
            model.ListaCentralGeneracion = this.servicioCentral.ListCentralGeneracion();
            model.EntidadCentralGeneracion = new CentralGeneracionDTO();
            model.EntidadCentralGeneracion.CentGeneCodi = model.EntidadProvisionbase.Equicodi;
            Log.Info("ListaURS - ListURS");
            model.ListaURS = this.servicioBarraUrs.ListURS();
            model.EntidadURS = new TrnBarraursDTO();
            model.EntidadURS.GrupoCodi = model.EntidadProvisionbase.Grupocodi;
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(ProvisionBaseModel model)
        {
            base.ValidarSesionUsuario();
            
            if (ModelState.IsValid)
            {
                if (model.Vcrpbperiodoini != "" && model.Vcrpbperiodoini != null)
                    model.EntidadProvisionbase.Vcrpbperiodoini = DateTime.ParseExact(model.Vcrpbperiodoini, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (model.Vcrpbperiodofin != "" && model.Vcrpbperiodofin != null)
                    model.EntidadProvisionbase.Vcrpbperiodofin = DateTime.ParseExact(model.Vcrpbperiodofin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                Log.Info("EntidadURS - GetByIdUrsGrupoCodi");
                model.EntidadURS = this.servicioBarraUrs.GetByIdUrsGrupoCodi(model.EntidadProvisionbase.Grupocodi);
                if (model.EntidadURS != null)
                {
                    model.EntidadProvisionbase.Gruponomb = model.EntidadURS.GrupoNomb;
                    
                    model.EntidadProvisionbase.Vcrpbusumodificacion = User.Identity.Name;
                    if (model.EntidadProvisionbase.Vcrpbcodi == 0)
                    {
                        model.EntidadProvisionbase.Vcrpbusucreacion = User.Identity.Name;
                        Log.Info("Insertar información - SaveVcrProvisionbase");
                        this.servicioCompensacionRsf.SaveVcrProvisionbase(model.EntidadProvisionbase);
                    }
                    else
                    {
                        Log.Info("Actualizar información - UpdateVcrProvisionbase");
                        this.servicioCompensacionRsf.UpdateVcrProvisionbase(model.EntidadProvisionbase);
                    }
                    TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                    return new RedirectResult(Url.Action("Index", "ProvisionBase"));
                }
            }
            Log.Info("ListaCentralGeneracion - ListCentralGeneracion");
            model.ListaCentralGeneracion = this.servicioCentral.ListCentralGeneracion();
            Log.Info("ListaURS - ListURS");
            model.ListaURS = this.servicioBarraUrs.ListURS();
            model.sError = "Se ha producido un error al insertar la información";
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return PartialView(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int vcrpbcodi = 0)
        {
            base.ValidarSesionUsuario();
            Log.Info("Eliminar el registro - DeleteVcrProvisionbase");
            this.servicioCompensacionRsf.DeleteVcrProvisionbase(vcrpbcodi);
            return "true";
        }

        public ActionResult View(int vcrpbcodi = 0)
        {
            ProvisionBaseModel model = new ProvisionBaseModel();
            Log.Info("EntidadProvisionbase - GetByIdVcrProvisionbaseView");
            model.EntidadProvisionbase = this.servicioCompensacionRsf.GetByIdVcrProvisionbaseView(vcrpbcodi);
            return PartialView(model);
        }


    }
}
