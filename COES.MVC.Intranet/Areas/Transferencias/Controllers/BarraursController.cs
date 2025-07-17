using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class BarraursController : BaseController
    {
        // GET: /Transferencias/Barraurs/

        BarraAppServicio servicioBarra = new BarraAppServicio();
        BarraUrsAppServicio servicioBarraUrs = new BarraUrsAppServicio();
        CentralGeneracionAppServicio servicioCentralGen = new CentralGeneracionAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();

        public ActionResult Index(int id = 0)
        {
            BarraursModel modelo = new BarraursModel();

            modelo.bEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            modelo.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            modelo.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            Session["sBarrcodi"] = id;
            modelo.EntidadBarra = this.servicioBarra.GetByIdBarra(id);
            modelo.IdBarra = id;
            return View(modelo);
        }

        [HttpPost]
        public ActionResult Lista()
        {
            int id = Convert.ToInt32(Session["sBarrcodi"].ToString());            
            BarraursModel modelo = new BarraursModel();
            modelo.ListaBarraURS = this.servicioBarraUrs.ListBarraURS(id);

            modelo.bEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            modelo.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            modelo.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);

            return PartialView(modelo);
        }

        public ActionResult New()
        {
            int id = Convert.ToInt32(Session["sBarrcodi"].ToString());    
            BarraursModel modelo = new BarraursModel();
            modelo.EntidadBarra = this.servicioBarra.GetByIdBarra(id);
            modelo.EntidadBarraURS = new TrnBarraursDTO();
            
            if (modelo.EntidadBarraURS == null)
            {
                return HttpNotFound();
            }

            modelo.ListaBarraURS = this.servicioBarraUrs.ListPrGrupo();
            modelo.listaUnidadGen = this.servicioCentralGen.ListUnidad();
            modelo.listaEmpresas = this.servicioEmpresa.ListEmpresas();

            modelo.bEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            modelo.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            modelo.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            
            return PartialView(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(BarraursModel modelo)
        {
            int id = Convert.ToInt32(Session["sBarrcodi"].ToString());
            modelo.EntidadBarra = this.servicioBarra.GetByIdBarra(id);
            modelo.ListaBarraURS = this.servicioBarraUrs.ListBarraURS(id);
            modelo.listaEmpresas = this.servicioEmpresa.ListEmpresas();
            modelo.listaUnidadGen = this.servicioCentralGen.ListUnidad();
            foreach (var item in modelo.ListaBarraURS)
            {
                if (modelo.EntidadBarraURS.GrupoCodi == item.GrupoCodi)
                {
                    modelo.sError = "La URS seleccionada ya se encuentra registrada.";
                    modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                    modelo.bEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
                    modelo.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
                    modelo.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
                    
                    return PartialView(modelo);
                }
            }

            if (ModelState.IsValid)
            {
                modelo.EntidadBarraURS.BarrCodi = id;
                modelo.EntidadBarraURS.BarUrsUsuCreacion = User.Identity.Name;
                modelo.EntidadBarraURS.BarUrsFecCreacion = DateTime.Now;

                this.servicioBarraUrs.saveBarraUrs(modelo.EntidadBarraURS);

                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";

                return RedirectToAction("Index", new { id = modelo.EntidadBarraURS.BarrCodi });
            }
            //Error
            modelo.sError = "Se ha producido un error al insertar la información";
            AreaModel modelArea = new AreaModel();
            modelArea.ListaAreas = (new AreaAppServicio()).ListAreas();

            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);


            return PartialView();
        }

        [HttpPost, ActionName("DeleteUrs")]
        [ValidateAntiForgeryToken]
        public string DeleteUrs(int id)
        {
            int id2 = Convert.ToInt32(Session["sBarrcodi"].ToString());
            if (id > 0)
            {
                BarraursModel model = new BarraursModel();
                this.servicioBarraUrs.DeleteUrs(id2, id, User.Identity.Name);

                return "true";
            }
            return "False";
        }

    }
    
}
