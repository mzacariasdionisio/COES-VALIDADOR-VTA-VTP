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
    public class CentralGeneracionController : BaseController
    {
        //
        // GET: /SistemasTransmision/CentralGeneracion/

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        SistemasTransmisionAppServicio servicioSistemasTransmision = new SistemasTransmisionAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        BarraAppServicio servicioBarra = new BarraAppServicio();
        CentralGeneracionAppServicio servicioCentralGen = new CentralGeneracionAppServicio();

        public ActionResult Index(int id = 0)
        {
            CentralGeneracionModel model = new CentralGeneracionModel();
            Session["sGenrcodi"] = id;
            model.EntidadGenerador = this.servicioSistemasTransmision.GetByIdStGenerador(id);
            if (model.EntidadGenerador != null)
            {
                model.EntidadRecalculo = this.servicioSistemasTransmision.GetByIdStRecalculo(model.EntidadGenerador.Strecacodi);

            }
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Lista()
        {
            int id = Convert.ToInt32(Session["sGenrcodi"].ToString());
            CentralGeneracionModel model = new CentralGeneracionModel();
            model.ListaSTCentralGen = this.servicioSistemasTransmision.ListStCentralgens(id);
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);

            return PartialView(model);
        }

        public ActionResult New(int id)
        {
            CentralGeneracionModel model = new CentralGeneracionModel();
            model.Entidad = new StCentralgenDTO();
            model.EntidadBarra = new BarraDTO();
            model.EntidadCentralGeneracion = new CentralGeneracionDTO();
            if (model.Entidad == null)
            {
                HttpNotFound();
            }

            model.ListaCentralGeneracion = this.servicioCentralGen.ListEmpresaCentralGeneracion();
            model.ListaBarra = this.servicioBarra.ListBarras();

            model.Entidad.Stgenrcodi = id;
            model.Entidad.Stcntgcodi = 0;
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(CentralGeneracionModel modelo)
        {
            if (ModelState.IsValid)
            {

                modelo.Entidad.Stcntgusucreacion = User.Identity.Name;
                modelo.Entidad.Stcntgusumodificacion = User.Identity.Name;
                modelo.Entidad.Stcntgfeccreacion = DateTime.Now;
                modelo.Entidad.Stcntgfecmodificacion = DateTime.Now;
                //modelo.Entidad.Stgenrcodi = Convert.ToInt32(Session["sGenrcodi"].ToString());

                if (modelo.Entidad.Stcntgcodi == 0)
                {
                    this.servicioSistemasTransmision.SaveStCentralgen(modelo.Entidad);
                }
                else
                {
                    this.servicioSistemasTransmision.UpdateStCentralgen(modelo.Entidad);
                }

                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return RedirectToAction("Index", new { id = modelo.Entidad.Stgenrcodi });
            }
            modelo.sError = "Se ha producido un error al insertar la información";
            modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return PartialView(modelo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int id)
        {
            if (id > 0)
            {
                CentralGeneracionModel model = new CentralGeneracionModel();
                this.servicioSistemasTransmision.DeleteStCentralgen(id);
                return "true";
            }
            return "False";
        }

        public ActionResult Edit(int id)
        {
            CentralGeneracionModel model = new CentralGeneracionModel();
            model.Entidad = this.servicioSistemasTransmision.GetByIdStCentralgen(id);
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);

            model.ListaCentralGeneracion = this.servicioCentralGen.ListEmpresaCentralGeneracion();
            model.ListaBarra = this.servicioBarra.ListBarras();

            return PartialView(model);
        }

    }
}