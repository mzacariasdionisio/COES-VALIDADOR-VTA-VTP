using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Intervenciones.Helper;
using COES.Servicios.Aplicacion.Monitoreo;
using COES.Servicios.Aplicacion.PMPO;
using COES.Servicios.Aplicacion.TiempoReal;
using COES.Servicios.Aplicacion.YupanaContinuo;

namespace COES.MVC.Intranet
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            MonitoreoAppServicio servMonitoreo = new MonitoreoAppServicio();
            servMonitoreo.VerificarUltimaGeneracionMonitoreoMME();

            YupanaContinuoAppServicio yupanaServicio = new YupanaContinuoAppServicio();
            yupanaServicio.VerificarUltimaSimulacionPendiente();

            ProgramacionAppServicio servPmpo = new ProgramacionAppServicio();
            servPmpo.VerificarUltimaGeneracionSDDP();
            servPmpo.VerificarUltimoProcesoValidacionRemision();

            ScadaSp7AppServicio servScadaSp7 = new ScadaSp7AppServicio();
            servScadaSp7.VerificarUltimaProcesoArchivoEstadistica();

            //Módulo Intervenciones: Eliminar archivos temporales de la vista previa
            FileServer.EliminarArchivosTemporalesCarpetaReporte(AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes, new List<string>() { "coes.png" });
        }
        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            if (app != null &&
                app.Context != null)
            {
                app.Context.Response.Headers.Remove("Server");
            }
        }
    }
}
