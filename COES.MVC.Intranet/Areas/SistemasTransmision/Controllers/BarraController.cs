using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.SistemasTransmision.Models;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
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
    public class BarraController : BaseController
    {
        //
        // GET: /SistemasTransmision/Barra/

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        SistemasTransmisionAppServicio servicioSistemasTransmision = new SistemasTransmisionAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        BarraAppServicio servicioBarra = new BarraAppServicio();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New(int stpercodi = 0, int strecacodi = 0)
        {
            BarraModel modelo = new BarraModel();
            modelo.EntidadBarra = new BarraDTO();

            modelo.Entidad = new StBarraDTO();
            modelo.EntidadPeriodo = new StPeriodoDTO();
            modelo.EntidadStRecalculo = new StRecalculoDTO();

            modelo.EntidadStRecalculo = this.servicioSistemasTransmision.GetByIdStRecalculo(strecacodi);


            if (modelo.Entidad == null)
            {
                return HttpNotFound();
            }


            modelo.Entidad.Stbarrcodi = 0;
            modelo.ListaBarra = this.servicioBarra.ListBarras();
            modelo.Entidad.Strecacodi = modelo.EntidadStRecalculo.Strecacodi;
            modelo.EntidadPeriodo.Stpercodi = stpercodi;
            modelo.EntidadStRecalculo.Strecacodi = strecacodi;
            modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            return PartialView(modelo);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(BarraModel modelo)
        {
            //modelo.ListaBarra = this.servicioBarra.ListBarras();
            modelo.ListaSTBarra = this.servicioSistemasTransmision.ListStBarra(modelo.Entidad.Strecacodi);
            foreach (var item in modelo.ListaSTBarra)
            {
                if(modelo.Entidad.Barrcodi == item.Barrcodi){
                    modelo.ListaBarra = (new BarraAppServicio()).ListBarras();
                    modelo.sError = "La barra seleccionada ya se encuentra registrada.";
                    modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                    return PartialView(modelo);
                }
            }
            if (ModelState.IsValid)
            {
                modelo.Entidad.Stbarrusucreacion = User.Identity.Name;
                modelo.Entidad.Stbarrfeccreacion = DateTime.Now;
                modelo.EntidadBarra = new BarraDTO();

                if (modelo.Entidad.Stbarrcodi == 0)
                {
                    this.servicioSistemasTransmision.SaveStBarra(modelo.Entidad);
                }
                else
                {
                    this.servicioSistemasTransmision.UpdateStBarra(modelo.Entidad);
                }
                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                //return RedirectToAction("Index", "Empresa", new { stpercodi = modelo.EntidadPeriodo.Stpercodi, strecacodi = modelo.Entidad.Strecacodi });2
                return new RedirectResult(Url.Action("Index", "Empresa", new { stpercodi = modelo.EntidadPeriodo.Stpercodi, strecacodi = modelo.Entidad.Strecacodi }) + "#paso3");
            }

            modelo.ListaBarra = (new BarraAppServicio()).ListBarras();
            modelo.sError = "Se ha producido un error al insertar la información";
            modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return PartialView(modelo);
        }

        [HttpPost]
        public ActionResult List(int strecacodi)
        {
            BarraModel model = new BarraModel();
            model.ListaSTBarra = this.servicioSistemasTransmision.ListStBarra(strecacodi);
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            return PartialView(model);
        }

        [HttpPost, ActionName("DeleteBAR")]
        [ValidateAntiForgeryToken]
        public string DeleteBAR(int id)
        {
            if (id > 0)
            {
                BarraModel model = new BarraModel();
                model.Entidad = this.servicioSistemasTransmision.GetByIdStBarra(id);
                this.servicioSistemasTransmision.DeleteDstEleDet(model.Entidad.Barrcodi, model.Entidad.Strecacodi);
                this.servicioSistemasTransmision.DeleteStBarra(id);
                
                return "true";
            }
            return "False";
        }

        public ActionResult Edit(int id, int barrcodi = 0)
        {
            BarraModel model = new BarraModel();
            model.EntidadBarra = (new BarraAppServicio()).GetByIdBarra(barrcodi);
            model.EntidadPeriodo = new StPeriodoDTO();
            model.EntidadStRecalculo = new StRecalculoDTO();

            model.Entidad = this.servicioSistemasTransmision.GetByIdStBarra(id);
            if (model.Entidad == null)
            {
                return HttpNotFound();
            }

            if (barrcodi != 0)
            {
                model.Entidad.Barrcodi = model.EntidadBarra.BarrCodi;
            }
            int IdRecalculo = model.Entidad.Strecacodi;
            model.EntidadStRecalculo = this.servicioSistemasTransmision.GetByIdStRecalculo(IdRecalculo);
            int IdPeriodo = model.EntidadStRecalculo.Stpercodi;
            model.EntidadPeriodo.Stpercodi = IdPeriodo;

            model.ListaBarra = this.servicioBarra.ListBarras();
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);

            return PartialView(model);
        }

    }
}