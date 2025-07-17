using COES.MVC.Intranet.Areas.ReportesFrecuencia.Models;
using COES.Servicios.Aplicacion.ReportesFrecuencia;
using COES.Dominio.DTO.ReportesFrecuencia;
using COES.MVC.Intranet.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ReportesFrecuencia.Controllers
{
    public class ReporteFrecuenciaAuditController : BaseController
    {
        public ActionResult Index(int id)
        {
            ReporteFrecuenciaAuditModel model = new ReporteFrecuenciaAuditModel();
            try
            {
                model.ListaAudit = new ReporteFrecuenciaAuditAppServicio().GetFrecuenciasAudit(new ReporteFrecuenciaParam() { IdGPS = id });
                var Equipo = new EquipoGPSAppServicio().GetBydId(id);
                model.Nombre = Equipo.NombreEquipo;
                model.IdGPS = id;
            }
            catch (Exception ex) { model.sError = ex.Message; }
            return View(model);
        }
        public ActionResult New(int id, string FechaInicial, String FechaFinal)
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            ReporteFrecuenciaAuditModel model = new ReporteFrecuenciaAuditModel();
            var Equipo = new EquipoGPSAppServicio().GetBydId(id);
            model.Nombre = Equipo.NombreEquipo;
            model.FechaInicial = Convert.ToDateTime(FechaInicial);
            model.FechaFinal = Convert.ToDateTime(FechaFinal);
            model.IdGPS = id;
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            ReporteFrecuenciaAuditModel model = new ReporteFrecuenciaAuditModel();
            FrecuenciasAudit Frec = new ReporteFrecuenciaAuditAppServicio().GetFrecuenciaAudit(id);
            var Equipo = new EquipoGPSAppServicio().GetBydId(Frec.IdGPS);
            model.Nombre = Equipo.NombreEquipo;
            model.IdGPS = Frec.IdGPS;
            model.ListaAudit = new List<FrecuenciasAudit>();
            model.ListaAudit.Add(Frec);
            return View(model);
        }

        public ActionResult Save(ReporteFrecuenciaAuditModel model)
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            try
            {
                new ReporteFrecuenciaAuditAppServicio().Grabar(new ReporteFrecuenciaParam() { IdGPS = model.IdGPS, FechaInicial = model.FechaInicial, FechaFinal = model.FechaFinal, Usuario = model.Usuario });
                model.sError = "";
            }
            catch (Exception ex) { model.sError = ex.Message; }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Revertir(ReporteFrecuenciaAuditModel model)
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            try
            {
                new ReporteFrecuenciaAuditAppServicio().Eliminar(model.ID, model.Usuario);
                model.sError = "";
            }
            catch (Exception ex) { model.sError = ex.Message; }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}