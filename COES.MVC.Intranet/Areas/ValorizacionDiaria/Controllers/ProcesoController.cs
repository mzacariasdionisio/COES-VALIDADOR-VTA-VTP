using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.ValorizacionDiaria.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.ValorizacionDiaria;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ValorizacionDiaria.Controllers
{
    public class ProcesoController : BaseController
    {
        //
        // GET: /ValorizacionDiaria/Proceso/
        ValorizacionDiariaAppServicio servicio = new ValorizacionDiariaAppServicio();
        public ActionResult Index()
        {
            ProcesoModel model = new ProcesoModel();
            model.Empresas = servicio.ObtenerEmpresasMME();
            ViewBag.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View(model);
        }

        public JsonResult EjecutarProceso(string empresas, string fechaInicio, string fechaFin)
        {
            if (string.IsNullOrEmpty(empresas)) empresas = "-2";
            int indicador = 0;
            try
            {
                DateTime starDate = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture );
                DateTime endDate = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                string usuarioEmail = ConfigurationManager.AppSettings["admValorizacionDiariaEmail"];
                foreach (DateTime day in EachDay(starDate, endDate))
                {
                    if (!servicio.ValorizacionDiaria(day, base.UserName, usuarioEmail, empresas))
                    {
                        break;
                    }
                    else
                    {
                        indicador = 1;
                    }
                }
            }
            catch
            {
                indicador = 0;
            }

            return Json(indicador);
        }

        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

    }
}
