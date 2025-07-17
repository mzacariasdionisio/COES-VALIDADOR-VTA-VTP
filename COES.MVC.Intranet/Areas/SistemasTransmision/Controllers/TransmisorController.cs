using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.SistemasTransmision.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.SistemasTransmision;
using COES.Servicios.Aplicacion.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.SistemasTransmision.Controllers
{
    public class TransmisorController : BaseController
    {
        //
        // GET: /SistemasTransmision/Transmisor/

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        SistemasTransmisionAppServicio servicioSistemasTransmision = new SistemasTransmisionAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New(int stpercodi = 0, int strecacodi = 0)
        {
            TransmisorModel modelo = new TransmisorModel();
            modelo.EntidadEmpresa = new EmpresaDTO();
            
            modelo.Entidad = new StTransmisorDTO();
            modelo.EntidadPeriodo = new StPeriodoDTO();
            modelo.EntidadStRecalculo = new StRecalculoDTO();

            modelo.EntidadStRecalculo = this.servicioSistemasTransmision.GetByIdStRecalculo(strecacodi);

            
            if (modelo.Entidad == null)
            {
                return HttpNotFound();
            }


            modelo.Entidad.Stranscodi = 0;
            modelo.ListaEmpresas = this.servicioEmpresa.ListEmpresas();
            modelo.Entidad.Strecacodi = modelo.EntidadStRecalculo.Strecacodi;
            modelo.EntidadPeriodo.Stpercodi = stpercodi;
            modelo.EntidadStRecalculo.Strecacodi = strecacodi;
            modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            return PartialView(modelo);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(TransmisorModel modelo)
        {
            if (ModelState.IsValid)
            {
                modelo.Entidad.Stransusucreacion = User.Identity.Name;
                modelo.Entidad.Stransfeccreacion = DateTime.Now;
                modelo.EntidadEmpresa = new EmpresaDTO();

                if (modelo.Entidad.Stranscodi == 0)
                {
                    this.servicioSistemasTransmision.SaveStTransmisor(modelo.Entidad);                    
                }
                else
                {
                    this.servicioSistemasTransmision.UpdateStTransmisor(modelo.Entidad);
                }

                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return RedirectToAction("Index", "Empresa", new { stpercodi = modelo.EntidadPeriodo.Stpercodi, strecacodi = modelo.Entidad.Strecacodi });
            }

            modelo.ListaEmpresas = (new EmpresaAppServicio()).ListEmpresas();
            modelo.sError = "Se ha producido un error al insertar la información";
            modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return PartialView(modelo);
        }        

        [HttpPost]
        public ActionResult List(int strecacodi)
        {
            TransmisorModel model = new TransmisorModel();
            model.ListaEmpresaTransmisor = this.servicioSistemasTransmision.ListStTransmisors(strecacodi);
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            return PartialView(model);
        }

        [HttpPost, ActionName("DeleteET")]
        [ValidateAntiForgeryToken]
        public string DeleteET(int id)
        {
            if (id > 0)
            {
                TransmisorModel model = new TransmisorModel();
                this.servicioSistemasTransmision.DeleteStTransmisor(id);
                return "true";
            }
            return "False";
        }

        public ActionResult Edit(int id, int emprecodi = 0)
        {
            TransmisorModel model = new TransmisorModel();
            model.EntidadEmpresa = (new EmpresaAppServicio()).GetByIdEmpresa(emprecodi);
            model.EntidadPeriodo = new StPeriodoDTO();
            model.EntidadStRecalculo = new StRecalculoDTO();
            
            model.Entidad = this.servicioSistemasTransmision.GetByIdStTransmisor(id);
            if (model.Entidad == null)
            {
                return HttpNotFound();
            }

            if (emprecodi != 0)
            {
                model.Entidad.Emprcodi = model.EntidadEmpresa.EmprCodi;
            }
            int IdRecalculo = model.Entidad.Strecacodi;
            model.EntidadStRecalculo = this.servicioSistemasTransmision.GetByIdStRecalculo(IdRecalculo);
            int IdPeriodo = model.EntidadStRecalculo.Stpercodi;
            model.EntidadPeriodo.Stpercodi = IdPeriodo;

            model.ListaEmpresas = this.servicioEmpresa.ListEmpresas();
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);

            return PartialView(model);
        }

    }
}
