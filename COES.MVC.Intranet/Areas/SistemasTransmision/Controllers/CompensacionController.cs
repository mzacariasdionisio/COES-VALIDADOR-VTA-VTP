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
    public class CompensacionController : BaseController
    {
        //
        // GET: /SistemasTransmision/Compensacion/
        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        SistemasTransmisionAppServicio servicioSistemasTransmision = new SistemasTransmisionAppServicio();
        
        public ActionResult Index(int id)
        {
            CompensacionModel model = new CompensacionModel();
            model.ListaCompensacion = this.servicioSistemasTransmision.GetBySisTransStCompensacions(id);
            model.EntidadSistema = this.servicioSistemasTransmision.GetByIdStSistematrans(id);
            if (model.EntidadSistema != null)
            {
                model.EntidadRecalculo = this.servicioSistemasTransmision.GetByIdStRecalculo(model.EntidadSistema.Strecacodi);

            }
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return View(model);
        }

        [HttpPost]
        public ActionResult lista(int id)
        {
            CompensacionModel model = new CompensacionModel();
            model.ListaCompensacion = this.servicioSistemasTransmision.GetBySisTransStCompensacions(id);
            model.EntidadSistema = this.servicioSistemasTransmision.GetByIdStSistematrans(id);
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            return PartialView(model);
        }

        public ActionResult View(int id)
        {
            CompensacionModel model = new CompensacionModel();
            model.EntidadCompensacion = this.servicioSistemasTransmision.GetByIdStCompensacion(id);
            return PartialView(model);
        }

        public ActionResult New(int id)
        {
            CompensacionModel model = new CompensacionModel();
            model.EntidadCompensacion = new StCompensacionDTO();
            model.ListaBarra = (new BarraAppServicio()).ListBarras();
            if(model.EntidadCompensacion == null){
                return HttpNotFound();
            }
            model.EntidadCompensacion.Stcompcodi = 0;
            model.EntidadCompensacion.Sistrncodi = id;
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(CompensacionModel model)
        {
            if (ModelState.IsValid)
            {
                model.EntidadCompensacion.Sstcmpusucreacion = User.Identity.Name;
                model.EntidadCompensacion.Sstcmpusumodificacion = User.Identity.Name;
                model.EntidadCompensacion.Sstcmpfeccreacion = DateTime.Now;
                model.EntidadCompensacion.Sstcmpfecmodificacion = DateTime.Now;
                if(model.EntidadCompensacion.Stcompcodi == 0){
                    model.ListaCompensacion = this.servicioSistemasTransmision.ListStCompensacions();
                    foreach (var item in model.ListaCompensacion)
                    {
                        if (model.EntidadCompensacion.Stcompcodelemento == item.Stcompcodelemento)
                        {
                            model.ListaBarra = (new BarraAppServicio()).ListBarras();

                            model.sError = "Codigo de elemento ya existe";
                            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                            return PartialView(model);
                        }
                    }
                    this.servicioSistemasTransmision.SaveStCompensacion(model.EntidadCompensacion);
                }
                else
                {
                    this.servicioSistemasTransmision.UpdateStCompensacion(model.EntidadCompensacion);
                }
                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return RedirectToAction("Index", new { id = model.EntidadCompensacion.Sistrncodi });
            }

            model.ListaBarra = (new BarraAppServicio()).ListBarras();

            model.sError = "Se ha producido un error al insertar la información";
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return PartialView(model);
        }

        public ActionResult Edit(int id)
        {
            CompensacionModel model = new CompensacionModel();
            model.EntidadCompensacion = this.servicioSistemasTransmision.GetByIdStCompensacion(id);
            model.ListaBarra = (new BarraAppServicio()).ListBarras();
            if (model.EntidadCompensacion == null)
            {
                return HttpNotFound();
            }
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            return PartialView(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int id)
        {
            if (id > 0)
            {
                CompensacionModel model = new CompensacionModel();
                this.servicioSistemasTransmision.DeleteStCompensacion(id);
                return "true";
            }
            return "False";
        }
    }
}