using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia;
using COES.MVC.Intranet.Areas.TransfPotencia.Models;
using COES.MVC.Intranet.Areas.TransfPotencia.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Configuration;
using System.Globalization;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Controllers
{
    public class RepaRecaPeajeController : BaseController
    {
        // GET: /Transfpotencia/RepaRecaPeaje/
        //[CustomAuthorize]

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();
        ConstantesTransfPotencia libFuncion = new ConstantesTransfPotencia();

        /// <summary>
        /// Muestra la pantalla inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int pericodi = 0, int recpotcodi=0)
        {
            base.ValidarSesionUsuario();
            Session["pericodi"] = pericodi;
            Session["recpotcodi"] = recpotcodi;
            RepaRecaPeajeModel model = new RepaRecaPeajeModel();
            model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi); 
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name); 
            return View(model);
        }

        /// <summary>
        /// Muestra la lista de datos de la RecalculoPotencia
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Lista()
        {
            RepaRecaPeajeModel model = new RepaRecaPeajeModel();
            model.Pericodi = Convert.ToInt32(Session["pericodi"].ToString());
            model.Recpotcodi = Convert.ToInt32(Session["recpotcodi"].ToString());
            model.ListaRepaRecaPeaje = this.servicioTransfPotencia.GetByCriteriaVtpRepaRecaPeajes(model.Pericodi, model.Recpotcodi); //Lista todas la lista de la tabla RecalculoPotencia incluido el atributo Nombre area
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, User.Identity.Name);
            model.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, User.Identity.Name);
            return PartialView(model);
        }

        /// <summary>
        /// Muestra un registro 
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        public ActionResult View(int pericodi = 0, int recpotcodi = 0, int rrpecodi = 0)
        {
            RepaRecaPeajeModel model = new RepaRecaPeajeModel();
            model.Entidad = this.servicioTransfPotencia.GetByIdVtpRepaRecaPeaje(pericodi, recpotcodi, rrpecodi);
            return PartialView(model);
        }

        /// <summary>
        /// Prepara una vista para ingresar un nuevo registro
        /// </summary>
        /// <returns></returns>
        public ActionResult New(int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();
            RepaRecaPeajeModel model = new RepaRecaPeajeModel();
            model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            model.Entidad = new VtpRepaRecaPeajeDTO();
            if (model.Entidad == null)
            {
                return HttpNotFound();
            }
            model.Entidad.Pericodi = model.EntidadRecalculoPotencia.Pericodi;
            model.Entidad.Recpotcodi = model.EntidadRecalculoPotencia.Recpotcodi;
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name); 
          
            return PartialView(model); 
        }

        /// <summary>
        /// Prepara una vista para editar un nuevo registro
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        public ActionResult Edit(int pericodi = 0, int recpotcodi = 0, int rrpecodi = 0)
        {
            base.ValidarSesionUsuario();
            RepaRecaPeajeModel model = new RepaRecaPeajeModel();
            model.Entidad = this.servicioTransfPotencia.GetByIdVtpRepaRecaPeaje(pericodi, recpotcodi, rrpecodi);
            if (model.Entidad == null)
            {
                return HttpNotFound();
            }
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name); 
            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar los datos del formulario
        /// </summary>
        /// <param name="model">Contiene los datos del regitsro a grabar</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(RepaRecaPeajeModel model)
        {
            base.ValidarSesionUsuario();
            if (ModelState.IsValid)
            {
                model.Entidad.Rrpeusumodificacion = User.Identity.Name;
              if (model.Entidad.Rrpecodi == 0)
                {   //Crear registro
                    model.Entidad.Rrpeusucreacion = User.Identity.Name;
                    this.servicioTransfPotencia.SaveVtpRepaRecaPeaje(model.Entidad);
                    TempData["sMensajeExito"] = ConstantesTransfPotencia.MensajeOkInsertarReistro;
                }
                else
                {   //Editar registro
                    this.servicioTransfPotencia.UpdateVtpRepaRecaPeaje(model.Entidad);
                    TempData["sMensajeExito"] = ConstantesTransfPotencia.MensajeOkEditarReistro;
                }

              return RedirectToAction("Index", new { pericodi = model.Entidad.Pericodi, recpotcodi = model.Entidad.Recpotcodi });
            }
            //Error
            model.sError = ConstantesTransfPotencia.MensajeErrorGrabarReistro;
            model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(model.Entidad.Pericodi, model.Entidad.Recpotcodi); 
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name); 
            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar un registro de forma definitiva en la base de datos
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int pericodi = 0, int recpotcodi = 0, int rrpecodi = 0)
        {
            base.ValidarSesionUsuario();
            RepaRecaPeajeModel model = new RepaRecaPeajeModel();
            this.servicioTransfPotencia.DeleteByCriteriaRRPEVtpRepaRecaPeajeDetalle(pericodi, recpotcodi, rrpecodi);
            this.servicioTransfPotencia.DeleteVtpRepaRecaPeaje(pericodi, recpotcodi, rrpecodi);
            return "true";
        }

    }
}
