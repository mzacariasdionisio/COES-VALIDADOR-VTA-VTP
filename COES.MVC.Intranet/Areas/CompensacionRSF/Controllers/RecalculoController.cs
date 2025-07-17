using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.CompensacionRSF.Models;
using COES.MVC.Intranet.Areas.TransfPotencia.Helper;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.CompensacionRSF;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CompensacionRSF.Controllers
{
    public class RecalculoController : BaseController
    {
        // GET: /CompensacionRSF/Recalculo/

        public RecalculoController()
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
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        CompensacionRSFAppServicio servicioCompensacionRSFA = new CompensacionRSFAppServicio();
        RecalculoAppServicio servicioRecalculo = new RecalculoAppServicio();

        /// <summary>
        /// Muestra la pantalla inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            base.ValidarSesionUsuario();
            RecalculoModel model = new RecalculoModel();
            Log.Info("Lista de Periodos - ListPeriodo");
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name);
            return View(model);
        }

        /// <summary>
        /// Permite cargar versiones de acuerdo al periodo
        /// </summary>
        /// <returns></returns>       
        public JsonResult GetVersion(int pericodi)
        {
            RecalculoModel model = new RecalculoModel();
            Log.Info("ListaTrnRecalculo - ListRecalculos");
            model.ListaTrnRecalculo = this.servicioRecalculo.ListRecalculos(pericodi);
            Log.Info("ListaVcrSuDeRns - ListVcrVersionDSRNS");
            model.ListaVcrSuDeRns = this.servicioCompensacionRSFA.ListVcrVersionDSRNS(pericodi);
            Log.Info("ListaIncumplimiento - ListVcrIncpl");
            model.ListaIncumplimiento = this.servicioCompensacionRSFA.ListVcrIncpl(pericodi);
            model.bEjecutar = true;
            //Consultamos por el estado del periodo
            PeriodoDTO entidadPeriodo = new PeriodoDTO();
            Log.Info("entidadPeriodo - GetByIdPeriodo");
            entidadPeriodo = servicioPeriodo.GetByIdPeriodo(pericodi);
            if (entidadPeriodo.PeriEstado.Equals("Cerrado"))
            { model.bEjecutar = false; }
            return Json(model);
        }

        [HttpPost]
        public ActionResult Lista()
        {
            RecalculoModel model = new RecalculoModel();
            Log.Info("ListaRecalculo - ListVcrTodoRecalculos");
            model.ListaRecalculo = this.servicioCompensacionRSFA.ListVcrTodoRecalculos();
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, User.Identity.Name);
            model.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, User.Identity.Name);
            return PartialView(model);
        }

        public ActionResult New()
        {
            base.ValidarSesionUsuario();
            RecalculoModel model = new RecalculoModel();
            model.EntidadPeriodo = new PeriodoDTO();
            Log.Info("Lista de Periodos - ListPeriodo");
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            model.EntidadRecalculo = new VcrRecalculoDTO();
            model.EntidadTrnRecalculo = new RecalculoDTO();
            Log.Info("ListaTrnRecalculo - ListRecalculos");
            model.ListaTrnRecalculo = this.servicioRecalculo.ListRecalculos(model.EntidadTrnRecalculo.RecaPeriCodi);
            model.EntidadVcrSuDeRns = new VcrVersiondsrnsDTO();
            Log.Info("ListaVcrSuDeRns - ListVcrVersionDSRNS");
            model.ListaVcrSuDeRns = this.servicioCompensacionRSFA.ListVcrVersionDSRNS(model.EntidadVcrSuDeRns.Pericodi);
            model.EntidadVcrIncumplimiento = new VcrVersionincplDTO();
            Log.Info("ListaIncumplimiento - ListVcrIncpl");
            model.ListaIncumplimiento = this.servicioCompensacionRSFA.ListVcrIncpl(model.EntidadVcrIncumplimiento.Pericodi);

            if (model.EntidadRecalculo == null)
            {
                return HttpNotFound();
            }
            model.EntidadRecalculo.Vcrecacodi = 0;
            model.EntidadRecalculo.Pericodi = 0;
            model.EntidadRecalculo.Vcrecakcalidad = 1;
            model.Vcrecafeccreacion = System.DateTime.Now.ToString("dd/MM/yyyy");
            model.EntidadRecalculo.Vcrecaestado = "Abierto";
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            return PartialView(model);
        }

        /// <summary>
        /// Prepara una vista para editar un nuevo registro
        /// </summary>
        /// <param name="pericodi">Código del periodo</param>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        public ActionResult Edit(int pericodi = 0, int vcrecacodi = 0)
        {
            base.ValidarSesionUsuario();
            RecalculoModel model = new RecalculoModel();
            Log.Info("EntidadRecalculo - GetByIdVcrRecalculoUpDate");
            model.EntidadRecalculo = this.servicioCompensacionRSFA.GetByIdVcrRecalculoUpDate(pericodi, vcrecacodi);
            if (model.EntidadRecalculo == null)
            {
                return HttpNotFound();
            }
            Log.Info("Lista de Periodos - ListPeriodo");
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            Log.Info("ListaTrnRecalculo - ListRecalculos");
            model.ListaTrnRecalculo = this.servicioRecalculo.ListRecalculos(pericodi);
            Log.Info("ListaVcrSuDeRns - ListVcrVersionDSRNS");
            model.ListaVcrSuDeRns = this.servicioCompensacionRSFA.ListVcrVersionDSRNS(pericodi);
            Log.Info("ListaIncumplimiento - ListVcrIncpl");
            model.ListaIncumplimiento = this.servicioCompensacionRSFA.ListVcrIncpl(pericodi);
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(RecalculoModel modelo)
        {
            base.ValidarSesionUsuario();
            Log.Info("ListaRecalculo - ListVcrRecalculosReg");
            modelo.ListaRecalculo = this.servicioCompensacionRSFA.ListVcrRecalculosReg(modelo.EntidadRecalculo.Vcrecacodi);
            modelo.ListaRecalculoPeriodo = this.servicioCompensacionRSFA.ListVcrRecalculos(modelo.EntidadRecalculo.Pericodi);
            Log.Info("Lista de Periodos - ListPeriodo");
            modelo.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            modelo.EntidadTrnRecalculo = new RecalculoDTO();
            Log.Info("ListaTrnRecalculo - ListRecalculos");
            modelo.ListaTrnRecalculo = this.servicioRecalculo.ListRecalculos(modelo.EntidadRecalculo.Pericodi);
            modelo.EntidadVcrSuDeRns = new VcrVersiondsrnsDTO();
            Log.Info("ListaVcrSuDeRns - ListVcrVersionDSRNS");
            modelo.ListaVcrSuDeRns = this.servicioCompensacionRSFA.ListVcrVersionDSRNS(modelo.EntidadVcrSuDeRns.Pericodi);
            modelo.EntidadVcrIncumplimiento = new VcrVersionincplDTO();
            Log.Info("ListaIncumplimiento - ListVcrIncpl");
            modelo.ListaIncumplimiento = this.servicioCompensacionRSFA.ListVcrIncpl(modelo.EntidadVcrIncumplimiento.Pericodi);
            if (modelo.EntidadRecalculo.Vcrecacodi == 0)
            {
                foreach (var item in modelo.ListaRecalculo)
                {
                    if (modelo.EntidadRecalculo.Vcrecanombre == item.Vcrecanombre && modelo.EntidadRecalculo.Pericodi == item.Pericodi)
                    {
                        Log.Info("ListaRecalculo - ListVcrTodoRecalculos");
                        modelo.ListaRecalculo = this.servicioCompensacionRSFA.ListVcrTodoRecalculos();
                        modelo.sError = "El nombre del recalculo seleccionado ya se encuentra registrado";
                        modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                        return PartialView(modelo);
                    }
                }
            }
            if (ModelState.IsValid)
            {
                modelo.EntidadRecalculo.Vcrecausucreacion = User.Identity.Name;
                modelo.EntidadRecalculo.Vcrecafeccreacion = DateTime.Now;
                modelo.EntidadRecalculo.Vcrecausumodificacion = User.Identity.Name;
                modelo.EntidadRecalculo.Vcrecafecmodificacion = DateTime.Now;
                modelo.EntidadRecalculo.Vcrecaversion = modelo.ListaRecalculoPeriodo.Count + 1;
                if (modelo.EntidadRecalculo.Vcrecacodi == 0)
                {
                    Log.Info("Insertar registro - SaveVcrRecalculo");
                    this.servicioCompensacionRSFA.SaveVcrRecalculo(modelo.EntidadRecalculo);
                }
                else
                {
                    Log.Info("Actualizar el registro - UpdateVcrRecalculo");
                    this.servicioCompensacionRSFA.UpdateVcrRecalculo(modelo.EntidadRecalculo);
                }
                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return new RedirectResult(Url.Action("Index", "recalculo"));
            }
            Log.Info("ListaRecalculo - ListVcrTodoRecalculos");
            modelo.ListaRecalculo = this.servicioCompensacionRSFA.ListVcrTodoRecalculos();
            modelo.sError = "Se ha producido un error al insertar la información";
            modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return PartialView(modelo);
        }

        /// <summary>
        /// Permite eliminar un registro de forma definitiva en la base de datos
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int vcrecacodi = 0)
        {
            base.ValidarSesionUsuario();
            Log.Info("Eliminar registro - DeleteVcrRecalculo");
            this.servicioCompensacionRSFA.DeleteVcrRecalculo(vcrecacodi);
            return "true";
        }

        /// <summary>
        /// Muestra un registro 
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <returns></returns>
        public ActionResult View(int pericodi = 0, int vcrecacodi = 0)
        {
            RecalculoModel model = new RecalculoModel();
            Log.Info("EntidadRecalculo - GetByIdVcrRecalculoViewIndex");
            model.EntidadRecalculo = this.servicioCompensacionRSFA.GetByIdVcrRecalculoViewIndex(pericodi, vcrecacodi);
            return PartialView(model);
        }
    }
}
