using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Controllers;
using COES.Dominio.DTO.ReportesFrecuencia;
using COES.Servicios.Aplicacion.ReportesFrecuencia;
using COES.MVC.Intranet.Areas.ReportesFrecuencia.Models;
using COES.MVC.Intranet.Areas.ReportesFrecuencia.Helper;
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
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Eventos;

namespace COES.MVC.Intranet.Areas.ReportesFrecuencia.Controllers
{
    public class AnalisisInformacionController : BaseController
    {
        /// <summary>
        /// Instancia del Web Service de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        PR5ReportesAppServicio servicio = new PR5ReportesAppServicio();

        public ActionResult Index()
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            InformacionFrecuenciaModel model = new InformacionFrecuenciaModel();

            model.bNuevo = true;
            model.bEditar = true;
            model.bGrabar = true;
            return View(model);
        }

        public ActionResult Lista()
        {
            InformacionFrecuenciaModel model = new InformacionFrecuenciaModel();
            model.ListaDesviacionFrecuencia = new InformacionFrecuenciaAppServicio().GetReporteFrecuenciaDesviacion();
            return PartialView(model);
        }

        public ActionResult ReporteEventosFrecuencia()
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            InformacionFrecuenciaModel model = new InformacionFrecuenciaModel();
            //model.ListaInformacionFrecuencia = new InformacionFrecuenciaAppServicio().GetReporteEventosFrecuencia();
            return PartialView(model);
        }

        public ActionResult ListaEventosFrecuencia()
        {
            InformacionFrecuenciaModel model = new InformacionFrecuenciaModel();
            model.ListaInformacionFrecuencia = new InformacionFrecuenciaAppServicio().GetReporteEventosFrecuencia();
            return PartialView(model);
        }

        public ActionResult ReporteSegundosFaltantes()
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            ReporteSegundosFaltantesModel model = new ReporteSegundosFaltantesModel();
            //model.ListaInformacionFrecuencia = new InformacionFrecuenciaAppServicio().GetReporteEventosFrecuencia();
            return PartialView(model);
        }

        public ActionResult EnvioAlertasCorreo()
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            ReporteSegundosFaltantesModel model = new ReporteSegundosFaltantesModel();
            AnalisisFallasAppServicio servAppServicio = new AnalisisFallasAppServicio();

            DateTime fechaProceso = DateTime.Now;
            servAppServicio.GenerarAlertasDatosFrecuencia(fechaProceso, 312);
            return Json("1");
        }

        public ActionResult EnvioAlertasCorreoEventosFrec()
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            ReporteSegundosFaltantesModel model = new ReporteSegundosFaltantesModel();
            AnalisisFallasAppServicio servAppServicio = new AnalisisFallasAppServicio();

            DateTime fechaProceso = DateTime.Now;
            servAppServicio.GenerarAlertasEventosFrecuencia(fechaProceso, 313);
            return Json("1");
        }

        public ActionResult ListaSegundosFaltantes()
        {
            /*InformacionFrecuenciaModel model = new InformacionFrecuenciaModel();
            model.ListaInformacionFrecuencia = new InformacionFrecuenciaAppServicio().GetReporteEventosFrecuencia();
            return PartialView(model);*/
            
            ReporteSegundosFaltantesModel model = new ReporteSegundosFaltantesModel();
            string mensajeError = "";
            List<string> listaFechas = new List<string>();
            List<EquipoGPSDTO> listaGPS = new List<EquipoGPSDTO>();
            List<ReporteSegundosFaltantesDTO> listaReporteSegundosFaltantes = new List<ReporteSegundosFaltantesDTO>();


            ReporteSegundosFaltantesParam param = new ReporteSegundosFaltantesParam();

            DateTime date = DateTime.Now;
            model.FechaInicial = date;
            int intAnio = 0;
            int intMes = 0;
            int intDia = 0;
            intAnio = date.Year;
            intMes = date.Month;
            intDia = date.Day;
            string strAnio = intAnio.ToString();
            string strMes = intMes.ToString();
            string strDia = intDia.ToString();

            if (intMes < 10)
            {
                strMes = "0" + strMes;
            }
            if (intDia < 10)
            {
                strDia = "0" + strDia;
            }

            string strFechaActual = strDia + "/" + strMes + "/" + strAnio;
            DateTime dtfechaActual = DateTime.ParseExact(strFechaActual, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaActualAnt = dtfechaActual.AddDays(-1);
            int intAnioAnt = fechaActualAnt.Year;
            int intMesAnt = fechaActualAnt.Month;
            int intDiaAnt = fechaActualAnt.Day;
            string strAnioAnt = intAnioAnt.ToString();
            string strMesAnt = intMesAnt.ToString();
            string strDiaAnt = intDiaAnt.ToString();
            if (intMesAnt < 10)
            {
                strMesAnt = "0" + strMesAnt;
            }
            if (intDiaAnt < 10)
            {
                strDiaAnt = "0" + strDiaAnt;
              
            }
            string strFechaAnt = strDiaAnt + "/" + strMesAnt + "/" + strAnioAnt;

            //model.FechaInicial = new DateTime(intAnio, intMes, 1, 0, 0, 0);

            param.FechaInicial = strFechaAnt;
            param.FechaFinal = strFechaAnt;
            param.IdGPS = 0;
            //param.IndOficial = model.IndOficial;
            //listaReporteSegundosFaltantes = new ReporteSegundosFaltantesAppServicio().GetReporteSegundosFaltantes(param);
            listaReporteSegundosFaltantes = new ReporteSegundosFaltantesAppServicio().GetReporteTotalSegundosFaltantes(param);
            listaGPS = new EquipoGPSAppServicio().GetListaEquipoGPSPorFiltro(param.IdGPS, param.IndOficial);

            DateTime start = DateTime.ParseExact(param.FechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime end = DateTime.ParseExact(param.FechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            TimeSpan difference = end - start;
            for (int i = 0; i <= difference.Days; i++)
            {
                DateTime fechaInicio = DateTime.ParseExact(param.FechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaActual = fechaInicio.AddDays(i);
                listaFechas.Add(fechaActual.ToString(Constantes.FormatoFecha));
            }

            model.ListaEquipos = listaGPS;
            model.ListaFechas = listaFechas;
            model.ListaReporte = listaReporteSegundosFaltantes;
            return PartialView(model);
        }

        public ActionResult EnvioAlertasCorreoRepSegFaltantes()
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            ReporteSegundosFaltantesModel model = new ReporteSegundosFaltantesModel();
            AnalisisFallasAppServicio servAppServicio = new AnalisisFallasAppServicio();

            DateTime fechaProceso = DateTime.Now;
            servAppServicio.GenerarAlertasRepSegFaltantes(fechaProceso, 314);
            return Json("1");
        }



    }
}