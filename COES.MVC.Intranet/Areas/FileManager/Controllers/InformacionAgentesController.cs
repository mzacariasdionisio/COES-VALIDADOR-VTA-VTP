using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Intranet.Areas.Equipamiento.Controllers;
using COES.MVC.Intranet.Areas.FileManager.ViewModels;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.InformacionAgentes;
using log4net;

namespace COES.MVC.Intranet.Areas.FileManager.Controllers
{
    public class InformacionAgentesController : Controller
    {
        //
        // GET: /FileManager/InformacionAgentes/
        GeneralAppServicio appGeneral = new GeneralAppServicio();
        InformacionAgentesAppServicio appInformacionAgentes = new InformacionAgentesAppServicio();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(InformacionAgentesController));

        public InformacionAgentesController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("InformacionAgentesController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("InformacionAgentesController", ex);
                throw;
            }
        }

        public ActionResult Index()
        {
            var modelo = new InformacionAgentesViewModel
            {
                FechaInicial = DateTime.Today.AddDays(-30),
                FechaFinal = DateTime.Today,
                ListaEmpresa = appGeneral.ListarEmpresasPorTipo("-1"),
                NroPagina = 1
            };
            return View(modelo);
        }

        [HttpPost]
        public PartialViewResult Paginado(int iEmpresa, string sFechaInicio, string sFechaFin)
        {
            var modelo = new InformacionAgentesViewModel();
            DateTime fechaInicio = DateTime.ParseExact(sFechaInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(sFechaFin, "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(sFechaFin);
            int nroRegistros = appInformacionAgentes.TotalListarArchivosPorFiltro(iEmpresa, fechaInicio, fechaFin);
            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                modelo.NroPaginas = nroPaginas;
                modelo.NroMostrar = Constantes.NroPageShow;
                modelo.IndicadorPagina = true;
            }
            return PartialView(modelo);
        }

        [HttpPost]
        public PartialViewResult ListadoArchivos(int iEmpresa, string sFechaInicio, string sFechaFin, int nroPagina)
        {
            var modelo = new InformacionAgentesViewModel();
            DateTime fechaInicio = DateTime.ParseExact(sFechaInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(sFechaFin, "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(sFechaFin);
            var ListaResultados = appInformacionAgentes.ListarArchivosPorFiltro(iEmpresa, fechaInicio, fechaFin, nroPagina, Constantes.PageSizeEvento);
            foreach (var item in ListaResultados)
            {
                string link = string.Empty;
                if (string.IsNullOrEmpty(item.Archiruta) || string.IsNullOrWhiteSpace(item.Archiruta))
                {
                    item.Archiruta = link;
                    continue;
                }
                if (item.Archiruta.ToUpperInvariant().Trim().StartsWith("HTTP"))
                {
                    link = "<a href='" + item.Archiruta + "' target='_blank'>Ver Archivo</a>";
                    link = link.Replace("'", "\"");
                    item.Archiruta = link;
                }
            }
            modelo.ListaResultados = ListaResultados;
            return PartialView(modelo);
        }
    }
}
