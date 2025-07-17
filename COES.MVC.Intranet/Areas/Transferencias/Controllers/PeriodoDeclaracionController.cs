using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Controllers;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Transferencias;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.IO;
using System.Globalization;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Configuration;
using COES.MVC.Intranet.SeguridadServicio;
using COES.MVC.Intranet.Areas.PMPO.Controllers;


namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class PeriodoDeclaracionController : BaseController
    {


        public ActionResult Index()
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();


            PeriodoDeclaracionModel model = new PeriodoDeclaracionModel();
            model.bNuevo = (new Funcion()).ValidarPermisoNuevo(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);

            model.bNuevo = true;
            model.bEditar = true;
            model.bGrabar = true;
            return View(model);
        }


        public ActionResult Lista()
        {
            PeriodoDeclaracionModel model = new PeriodoDeclaracionModel();
            model.ListaPeriodos = new PeriodoDeclaracionAppServicio().GetListaPeriodoDeclaracion();
            return PartialView(model);
        }

        public ActionResult New()
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            int Idperiodo =  new PeriodoDeclaracionAppServicio().GetListaPeriodoDeclaracion().Max(x => (int)x.PeridcCodi);
            PeriodoDeclaracionDTO periodoDTO = new PeriodoDeclaracionAppServicio().GetBydId(Idperiodo);

            int ultimoPeriodo = 0;
            int ultimoMes = 0;

            if (periodoDTO!=null)
            {
                ultimoPeriodo = Convert.ToInt32(periodoDTO.PeridcAnio);
                ultimoMes = Convert.ToInt32(periodoDTO.PeridcMes);
            }

            if (ultimoMes==12)
            {
                ultimoPeriodo = ultimoPeriodo + 1;
                ultimoMes = 1;
            } else
            {
                ultimoMes = ultimoMes + 1;
            }

            var anios = (new Funcion()).ObtenerAnio().Where(x => (Convert.ToInt32(x.Value) >= ultimoPeriodo));

            PeriodoDeclaracionModel modelo = new PeriodoDeclaracionModel();
            modelo.Entidad = new PeriodoDeclaracionDTO();
            modelo.Entidad.PeridcCodi = 0;
            modelo.Entidad.PeridcFecRegi = System.DateTime.Now;
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            TempData["Mescodigo"] = new SelectList((new Funcion()).ObtenerMes(), "Value", "Text", ultimoMes);
            TempData["Aniocodigo"] = new SelectList(anios, "Value", "Text", ultimoPeriodo);
            return View(modelo);
        }

        public ActionResult Edit(int id)
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            PeriodoDeclaracionModel modelo = new PeriodoDeclaracionModel();
            modelo.Entidad = new PeriodoDeclaracionAppServicio().GetBydId(id);
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            TempData["Mescodigo"] = new SelectList((new Funcion()).ObtenerMes(), "Value", "Text", modelo.Entidad.PeridcFecRegi.Value.Month);
            TempData["Aniocodigo"] = new SelectList((new Funcion()).ObtenerAnio(), "Value", "Text", modelo.Entidad.PeridcFecRegi.Value.Year);
            return View(modelo);
        }

        public ActionResult Save(PeriodoDeclaracionModel modelo)
        {
            string mensajeError = "Se ha producido un error al insertar la información";


            ResultadoDTO<PeriodoDeclaracionDTO> resultado = new PeriodoDeclaracionAppServicio().SaveUpdate(modelo.Entidad);
            if (resultado.EsCorrecto >= 0)
            {
                TempData["sMensajeExito"] = "Se ha registrado correctamente.";
                mensajeError = "";
            }
            else
            {
                mensajeError = resultado.Mensaje;
            }

            modelo.sError = mensajeError;
            return Json(modelo,JsonRequestBehavior.AllowGet);
        }
    }
}
