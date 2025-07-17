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
using COES.Framework.Base.Core;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PotenciaFirmeRemunerable;
using COES.Dominio.DTO.Sic;
using log4net;
using System.Reflection;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Controllers
{
    public class IngresoPotefrController : BaseController
    {
        // GET: /Transfpotencia/IngresoPotefr/
        //[CustomAuthorize]

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        RecalculoAppServicio servicioRecalculo = new RecalculoAppServicio();
        ConstantesTransfPotencia libFuncion = new ConstantesTransfPotencia();
        PotenciaFirmeRemunerableAppServicio pfrServicio = new PotenciaFirmeRemunerableAppServicio();

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

        public IngresoPotefrController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Muestra la pantalla inicial
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        public ActionResult Index(int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();

            IngresoPotefrModel model = new IngresoPotefrModel();
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            if (model.ListaPeriodos.Count > 0 && pericodi == 0)
            { pericodi = model.ListaPeriodos[0].PeriCodi; }
            if (pericodi > 0)
            {
                model.ListaRecalculoPotencia = this.servicioTransfPotencia.ListByPericodiVtpRecalculoPotencia(pericodi); //Ordenado en descendente
                if (model.ListaRecalculoPotencia.Count > 0 && recpotcodi == 0)
                { recpotcodi = (int)model.ListaRecalculoPotencia[0].Recpotcodi; }
            }

            if (pericodi > 0 && recpotcodi > 0)
            {
                model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            }
            model.Pericodi = pericodi;
            model.Recpotcodi = recpotcodi;
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name);

            var regPeriodo = this.servicioPeriodo.ListPeriodo().Find(x => x.PeriCodi == pericodi);
            var listaPeriodosPfr = pfrServicio.ListPfrPeriodos();
            PfrPeriodoDTO periodoTransferencia = listaPeriodosPfr.Find(x => x.Pfrperanio == regPeriodo.AnioCodi && x.Pfrpermes == regPeriodo.MesCodi);
            if (periodoTransferencia != null)
            {
                model.ListaRecalculopfr = this.pfrServicio.GetByCriteriaPfrRecalculos(periodoTransferencia.Pfrpercodi); //Ordenado en descendente
            }
            else
            {
                model.ListaRecalculopfr = new List<PfrRecalculoDTO>();
            }
            return View(model);
        }

        /// <summary>
        /// Muestra la lista de datos de la Potencia Efectiva, Firme y Firme Remunerable
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Lista(int pericodi, int recpotcodi)
        {
            IngresoPotefrModel model = new IngresoPotefrModel();
            model.ListaIngresoPotefr = this.servicioTransfPotencia.GetByCriteriaVtpIngresoPotefrs(pericodi, recpotcodi);
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, User.Identity.Name);
            model.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, User.Identity.Name);
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name);

            if (pericodi > 0 && recpotcodi > 0)
            {
                model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            }

            return PartialView(model);
        }

        /// <summary>
        /// Muestra un registro 
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        public ActionResult View(int ipefrcodi)
        {
            base.ValidarSesionUsuario();
            IngresoPotefrModel model = new IngresoPotefrModel();
            model.Entidad = this.servicioTransfPotencia.GetByIdVtpIngresoPotefr(ipefrcodi);
            return PartialView(model);
        }

        /// <summary>
        /// Prepara una vista para ingresar un nuevo registro
        /// </summary>
        /// <returns></returns>
        public ActionResult New(int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();
            IngresoPotefrModel model = new IngresoPotefrModel();
            model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            model.Entidad = new VtpIngresoPotefrDTO();
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
        public ActionResult Edit(int ipefrcodi)
        {
            base.ValidarSesionUsuario();
            IngresoPotefrModel model = new IngresoPotefrModel();
            model.Entidad = this.servicioTransfPotencia.GetByIdVtpIngresoPotefr(ipefrcodi);
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
        public ActionResult Save(IngresoPotefrModel model)
        {
            string result;
            base.ValidarSesionUsuario();

            if (ModelState.IsValid)
            {             
                model.Entidad.Ipefrusumodificacion = User.Identity.Name;
                if (model.Entidad.Ipefrcodi == 0)
                {   //Crear registro
                    result = this.servicioTransfPotencia.GetResultSaveVtpIngresoPotefr(model.Entidad.Ipefrdia, model.Entidad.Pericodi, model.Entidad.Recpotcodi);
                    if (result.Equals("true"))
                    {
                        model.Entidad.Ipefrusucreacion = User.Identity.Name;
                        this.servicioTransfPotencia.SaveVtpIngresoPotefr(model.Entidad);
                        TempData["sMensajeExito"] = ConstantesTransfPotencia.MensajeOkInsertarReistro;
                        return RedirectToAction("Index", new { pericodi = model.Entidad.Pericodi, recpotcodi = model.Entidad.Recpotcodi });
                    }
                    else
                    {
                        //Agregar error en funcion
                        model.sError = "Numero de dias incorrectos";
                        model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
                        model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name); 
                        return PartialView(model);
                    }
                }
                else
                {
                    result = this.servicioTransfPotencia.GetResultUpdateVtpIngresoPotefr(model.Entidad.Ipefrcodi, model.Entidad.Ipefrdia, model.Entidad.Pericodi, model.Entidad.Recpotcodi);
                    if (result.Equals("true"))
                    {
                        //Editar registro
                        this.servicioTransfPotencia.UpdateVtpIngresoPotefr(model.Entidad);
                        TempData["sMensajeExito"] = ConstantesTransfPotencia.MensajeOkEditarReistro;
                        return RedirectToAction("Index", new { pericodi = model.Entidad.Pericodi, recpotcodi = model.Entidad.Recpotcodi });
                    }
                    else
                    {
                        //Error
                        //Agregar error en funcion
                        model.sError = "Numero de dias incorrectos";                      
                        model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
                        model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name); 
                        return PartialView(model);
                    }
                }
            }
            //Error
            model.sError = ConstantesTransfPotencia.MensajeErrorGrabarReistro;
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name); 
            return PartialView(model);
        }


        /// <summary>
        /// Permite eliminar un registro de forma definitiva en la base de datos
        /// </summary>
        /// <param name="rrpecodi">Código del Mes de valorización</param>        
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]      
        public string Delete(int pericodi = 0, int recpotcodi = 0, int ipefrcodi = 0)
        {
            base.ValidarSesionUsuario();
            RepaRecaPeajeModel model = new RepaRecaPeajeModel();
            model.Entidad = new VtpRepaRecaPeajeDTO();
            model.Entidad.Pericodi = pericodi;
            model.Entidad.Recpotcodi = recpotcodi;
            //Elimnina detalle 
            this.servicioTransfPotencia.DeleteByCriteriaVtpIngresoPotefrDetalle(ipefrcodi, model.Entidad.Pericodi, model.Entidad.Recpotcodi);
            //Elimina cabezera
            this.servicioTransfPotencia.DeleteVtpIngresoPotefr(ipefrcodi);
            return "true";
        }

        [HttpPost]
        public JsonResult ProcesarCargaPFR(int pfrreccodi, int pericodi, int recpotcodi)
        {
            IngresoPotefrModel model = new IngresoPotefrModel();
            try
            {
                base.ValidarSesionJsonResult();

                // Eliminar cabecera y detalle
                var resultado = servicioTransfPotencia.ProcesarCargaPfr(pfrreccodi, pericodi, recpotcodi, User.Identity.Name);

                if (resultado > 0)
                {
                    model.Resultado = "1";
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }
    }
}
