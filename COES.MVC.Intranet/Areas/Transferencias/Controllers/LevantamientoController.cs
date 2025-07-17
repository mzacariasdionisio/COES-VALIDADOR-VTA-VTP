//using COES.MVC.Intranet.Models;
using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Transferencias;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class LevantamientoController : Controller
    {
        //
        // GET: /Transferencias/Levantamiento/
        public ActionResult Index(int iPericodi = 0)
        {
            PeriodoModel model = new PeriodoModel();
            model.ListaPeriodos = (new PeriodoAppServicio()).BuscarPeriodo("");
            TempData["Periodocodigo"] = new SelectList(model.ListaPeriodos, "Pericodi", "Perinombre", iPericodi);
            Session["iTrmVersion"] = 0;
            return View();
        }

        /// <summary>
        /// Permite cargar versiones deacuerdo al periodo
        /// </summary>
        /// <returns></returns>
        public JsonResult GetVersion(int pericodi)
        {
            RecalculoModel modelRecalculo = new RecalculoModel();
            modelRecalculo.ListaRecalculo = (new RecalculoAppServicio()).ListRecalculos(pericodi);
            Session["iTrmVersion"] = modelRecalculo.ListaRecalculo[0].RecaCodi;
            return Json(modelRecalculo.ListaRecalculo);
        }

        [HttpPost]
        public ActionResult Lista(DateTime? fecha, string corrinf, int pericodi = 0, int vers = 0) 
        {
            if (vers == 0 && !Session["iTrmVersion"].Equals(null)) vers = Convert.ToInt32(Session["iTrmVersion"].ToString());
            LevantamientoModel model = new LevantamientoModel();
            if (corrinf.Equals("Ambos")) corrinf = null;
            model.ListaTramites = (new TramiteAppServicio()).BuscarTramite(fecha, corrinf, pericodi, vers);
            model.bEditar = (new Funcion()).ValidarPermisoEditar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            return PartialView(model);
        }

        public ActionResult View(int id)
        {
            LevantamientoModel model = new LevantamientoModel();
            model.Entidad = (new TramiteAppServicio()).GetByIdTramite(id);
            return PartialView(model);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(TramiteLevantamientoModel  modelo)
        {
            if (ModelState.IsValid)
            {
                modelo.Entidad.UsuaCoesCodi = User.Identity.Name;
                modelo.Entidad.TramCodi = (new TramiteAppServicio()).SaveOrUpdateTramite(modelo.Entidad);
                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                //return RedirectToAction("Index");
                return Redirect("index?iPericodi=" + modelo.Entidad.PeriCodi);
            }
            modelo.ListaTipoTramites = (new TipoTramiteAppServicio()).ListTipoTramite();
            TempData["Tipotramitecodigo"] = new SelectList(modelo.ListaTipoTramites, "Tipotramcodi", "Tipotramnombre", modelo.Entidad.TipoTramcodi);
            PeriodoModel modelPeriodo = new PeriodoModel();
            modelPeriodo.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(modelo.Entidad.PeriCodi);
            TempData["iPeriCodi"] = modelo.Entidad.PeriCodi;
            TempData["Periodonombre"] = modelPeriodo.Entidad.PeriNombre;
            RecalculoModel reca = new RecalculoModel();
            reca.Entidad = new RecalculoDTO();
            reca.Entidad = (new RecalculoAppServicio()).GetByIdRecalculo((int)modelo.Entidad.PeriCodi, modelo.Entidad.TramVersion);
            TempData["NombreVersion"] = reca.Entidad.RecaNombre;
            EmpresaModel modelEmpresa = new EmpresaModel();
            modelEmpresa.Entidad = (new EmpresaAppServicio()).GetByIdEmpresa((Int32)modelo.Entidad.EmprCodi);
            TempData["EmprNombre"] = modelEmpresa.Entidad.EmprNombre;
            return PartialView(modelo); 
        }


        public ActionResult Edit(int id)
        {
            TramiteLevantamientoModel modelo = new TramiteLevantamientoModel();
            modelo.Entidad = new TramiteDTO();
            modelo.Entidad = (new TramiteAppServicio()).GetByIdTramite(id);
            if (modelo.Entidad == null)
            {
                return HttpNotFound();
            }
            modelo.EntidadTipoTramite = (new TipoTramiteAppServicio()).GetByIdTipoTramite(modelo.Entidad.TipoTramcodi);
            TempData["Tipotramnombre"] = modelo.EntidadTipoTramite.TipoTramNombre;

            PeriodoModel modelPeriodo = new PeriodoModel();
            modelPeriodo.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(modelo.Entidad.PeriCodi);
            TempData["iPeriCodi"] = modelo.Entidad.PeriCodi;
            TempData["Periodonombre"] = modelPeriodo.Entidad.PeriNombre;
            
            RecalculoModel reca = new RecalculoModel();
            reca.Entidad = new RecalculoDTO();
            reca.Entidad = (new RecalculoAppServicio()).GetByIdRecalculo((int)modelo.Entidad.PeriCodi, modelo.Entidad.TramVersion);
            TempData["NombreVersion"] = reca.Entidad.RecaNombre;

            EmpresaModel modelEmpresa = new EmpresaModel();
            modelEmpresa.Entidad = (new EmpresaAppServicio()).GetByIdEmpresa((Int32)modelo.Entidad.EmprCodi);
            TempData["EmprNombre"] = modelEmpresa.Entidad.EmprNombre;

            TempData["var"] = "1";
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            return PartialView(modelo); 
        }

    }
}
