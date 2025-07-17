using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using log4net;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Areas.TransfPotencia.Models;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Controllers
{
    public class ActualizarTrasEmpFusionController : BaseController
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ActualizarTrasEmpFusionController));
        private static string NombreControlador = "ActualizarTrasEmpFusionController";

        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        public ActionResult Index()
        {
            base.ValidarSesionUsuario();
            ActualizarTrasEmpFusionModel model = new ActualizarTrasEmpFusionModel();
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            return View(model);
        }

        [HttpPost]
        public ActionResult ListaSaldosSobrantes(int? pericodi)
        {
            ActualizarTrasEmpFusionModel model = new ActualizarTrasEmpFusionModel();
            model.ListaSaldosSobrantes = new ActualizarTrasEmpFusionAppServicio().GetListaSaldosSobrantesVTP(pericodi, User.Identity.Name);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult TrasladarSaldos(int? pericodi, string items = "")
        {
            var resultado = new ActualizarTrasEmpFusionAppServicio().SaveOrUpdateSaldos(pericodi, items, User.Identity.Name);
            TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
            return Json("");
        }
        [HttpPost]
        public JsonResult TrasladarSaldosVTP(int? pericodi, string items = "")
        {
            var resultado = new ActualizarTrasEmpFusionAppServicio().SaveOrUpdateSaldosVTP(pericodi, items, User.Identity.Name);
            TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
            return Json("");
        }

        [HttpPost]
        public ActionResult ListaSaldosNoIdentificados(int? pericodi)
        {
            ActualizarTrasEmpFusionModel model = new ActualizarTrasEmpFusionModel
            {
                ListaSaldosNoIdentificados = new ActualizarTrasEmpFusionAppServicio().GetListaSaldosNoIdentificadosVTP(pericodi)
            };
            return PartialView(model);
        }
    }
}
