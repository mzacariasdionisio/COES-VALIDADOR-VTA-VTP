using COES.Base.Tools;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.DemandaMaxima.Models;
using COES.Servicios.Aplicacion.DemandaMaxima;
using COES.Servicios.Aplicacion.Helper;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using System.Net;
using COES.Servicios.Aplicacion.DemandaMaxima.Helper;
using log4net;
using System.Reflection;

namespace COES.MVC.Intranet.Areas.DemandaMaxima.Controllers
{
    public class AmpliacionPlazoController : BaseController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(AmpliacionPlazoController));
        public int IdFormato = Int32.Parse(ConfigurationManager.AppSettings["IdFormatoPR16"]);

        DemandaMaximaAppServicio servicio = new DemandaMaximaAppServicio();

        public AmpliacionPlazoController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("AmpliacionPlazoController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("AmpliacionPlazoController", ex);
                throw;
            }
        }
        /// <summary>
        /// Carga principal de la pantalla
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            AmpliacionPlazoModel model = new AmpliacionPlazoModel();
            //model.ListaEmpresas = new List<SiEmpresaDTO>();
            model.ListaEmpresas = this.servicio.ListaEmpresasAmpliacionPlazo().OrderBy(t=>t.Emprrazsocial).ToList();
            model.FechaInicio = DateTime.Now.ToString("dd/MM/yyyy");

            ViewBag.hfCantidadRegistros = "0";

            return View(model);
        }

        [HttpPost]
        public ActionResult ListarEmpresasAmpliacionPlazo(int empresa, string periodo, int nroPagina)
        {
            AmpliacionPlazoModel model = new AmpliacionPlazoModel();

            int regIni = 0;
            int regFin = 0;

            regIni = (nroPagina - 1) * ConstantesDemandaMaxima.PageSizeDemandaUsuario + 1;
            regFin = nroPagina * ConstantesDemandaMaxima.PageSizeDemandaUsuario;

            var fechaPeriodo = DateTime.MinValue;
            if(!string.IsNullOrEmpty(periodo))
            {
                var fecha = periodo.Split('/');
                fechaPeriodo = new DateTime(Convert.ToInt32(fecha[1]), Convert.ToInt16(fecha[0]), 1);
            }

            model.ListaEmpresasPlazo = this.servicio.GetListaAmpliacionFiltro(fechaPeriodo, empresa, IdFormato, regIni, regFin);

            return View(model);
        }

        public ActionResult GuardarAmpliacionPlazo(string empresas, string periodo, string fechaAmpliacion)        
        {
            base.ValidarSesionUsuario();
            COES.Servicios.Aplicacion.Hidrologia.HidrologiaAppServicio servicioHidrologia = new COES.Servicios.Aplicacion.Hidrologia.HidrologiaAppServicio();

            var fecha = periodo.Split('/');
            var fechaAmpliacionFormato = fechaAmpliacion.Split('/');
            var fechaAmpliacionPlazo = new DateTime(Convert.ToInt32(fechaAmpliacionFormato[2]), Convert.ToInt16(fechaAmpliacionFormato[1]), Convert.ToInt16(fechaAmpliacionFormato[0]));

            var fechaPeriodo = new DateTime(Convert.ToInt32(fecha[1]), Convert.ToInt16(fecha[0]), 1);

            string[] empresasId = empresas.Split(',');
            foreach (var empresa in empresasId)
            {
                MeAmpliacionfechaDTO ampliacion = new MeAmpliacionfechaDTO();
                ampliacion.Lastuser = User.Identity.Name;
                ampliacion.Lastdate = DateTime.Now;
                ampliacion.Amplifecha = fechaPeriodo;
                ampliacion.Formatcodi = IdFormato;
                ampliacion.Emprcodi = Convert.ToInt32(empresa);
                ampliacion.Amplifechaplazo = fechaAmpliacionPlazo;

                servicioHidrologia.SaveMeAmpliacionfecha(ampliacion);
            }

            return Json(new { success = true, message = "Ok" });
        }
                
        public JsonResult ObtenerListaEmpresas()
        {
            List<SiEmpresaDTO> list = this.servicio.ListaEmpresasAmpliacionPlazo().OrderBy(t => t.Emprrazsocial).ToList();
            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = Int32.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Permite generar la vista del paginado
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(int empresa, string periodo)
        {
            Paginacion model = new Paginacion();

            var fechaPeriodo = DateTime.MinValue;
            if (!string.IsNullOrEmpty(periodo))
            {
                var fecha = periodo.Split('/');
                fechaPeriodo = new DateTime(Convert.ToInt32(fecha[1]), Convert.ToInt16(fecha[0]), 1);
            }

            int nroRegistros = this.servicio.GetListaAmpliacionFiltroCount(fechaPeriodo, empresa, IdFormato);

            if (nroRegistros > ConstantesDemandaMaxima.NroPageShow)
            {
                int pageSize = ConstantesDemandaMaxima.PageSizeDemandaUsuario;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = ConstantesDemandaMaxima.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);

        }
    }
}
