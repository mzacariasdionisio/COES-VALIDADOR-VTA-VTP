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
    public class CumplimientoController : BaseController
    {
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
        public CumplimientoController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>  
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public int IdFormato = Int32.Parse(ConfigurationManager.AppSettings["IdFormatoPR16"]);


        DemandaMaximaAppServicio servicio = new DemandaMaximaAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        /// <summary>
        /// Carga principal de la pantalla
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            DemandaMaximaModel model = new DemandaMaximaModel();
            model.IdModulo = Modulos.AppMedidoresDistribucion;
            model.ListaTipoempresa = this.servicio.ListTipoEmpresaCumplimiento();
            model.ListaPeriodo = this.servicio.ListaPeriodoReporte(ConfigurationManager.AppSettings["FechaInicioCumplimientoPR16"]);//Fecha de inicio del despliqgue de la aplicación
            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
            return View(model);
        }


        /// <summary>
        /// Metodo para obtener la lista de empresas segun el tipo de empresa.
        /// </summary>
        /// <param name="tipoemprcodi"></param>
        /// <returns></returns>
        public JsonResult ObtenerListaEmpresas(int tipoemprcodi)
        {
            List<SiEmpresaDTO> list = this.servicio.ListaEmpresasPorTipoCumplimiento(tipoemprcodi, IdFormato);
            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = Int32.MaxValue;
            return jsonResult;
            //return Json(list);
        }


        /// <summary>
        /// Método para obtener la lista del reporte de cumplimiento
        /// </summary>
        /// <param name="nroPagina"></param>
        /// <param name="empresas"></param>
        /// <param name="tipos"></param>
        /// <param name="ini"></param>
        /// <param name="fin"></param>
        /// <param name="cumpli"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListarReporteCumplimiento(int nroPagina, string empresas, string tipos, string ini, string fin, string cumpli, string ulcoes, string abreviatura)
        {
            DemandaMaximaModel model = new DemandaMaximaModel();
            model.ListaReporteCumplimiento = this.servicio.ListaReporteCumplimiento(IdFormato, empresas, tipos, ini, fin, cumpli, ulcoes, abreviatura,
                ConfigurationManager.AppSettings["FechaInicioCumplimientoPR16"]);

            var fecInicio = DateTime.ParseExact(ini, "dd/MM/yyyy", null);
            var fecFin = DateTime.ParseExact(fin, "dd/MM/yyyy", null);

            var meses = Math.Abs((fecInicio.Month - fecFin.Month) + 12 * (fecInicio.Year - fecFin.Year));

            model.FechaInicioReporteCumplimiento = fecInicio;
            model.FechaFinReporteCumplimiento = fecFin;
            model.MesesReporteCumplimiento = meses;

            return View(model);
        }

        /// <summary>
        /// Permite pintar la vista del paginado
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado()
        {
            Paginacion model = new Paginacion();

            int pageSize = Constantes.PageSize;
            int nroRegistros = 80;
            int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
            model.NroPaginas = nroPaginas;
            model.NroMostrar = Constantes.NroPageShow;
            model.IndicadorPagina = true;

            return base.Paginado(model);
        }


        /// <summary>
        /// Método para exportar en Excel Reporte de Cumplimiento
        /// </summary>
        /// <param name="nroPagina"></param>
        /// <param name="empresas"></param>
        /// <param name="tipos"></param>
        /// <param name="ini"></param>
        /// <param name="fin"></param>
        /// <param name="cumpli"></param>
        /// <returns></returns>
        public JsonResult Exportar(int nroPagina, string empresas, string tipos, string ini, string fin, string cumpli, string ulcoes, string abreviatura)
        {
            try
            {
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + ConstantesDemandaMaxima.PathLogo;

                //- pr16.JDEL - Inicio 21/07/2016: Cambio para atender el requerimiento.
                //                string file = this.servicio.GenerarFormatoRepCumplimiento(nroPagina, empresas, tipos, ini, fin, cumpli, AppDomain.CurrentDomain.BaseDirectory +
                //    Constantes.RutaCarga, pathLogo);

                string file = this.servicio.GenerarFormatoRepCumplimiento(nroPagina, empresas, tipos, ini, fin, cumpli, ulcoes, abreviatura, AppDomain.CurrentDomain.BaseDirectory +
                    Constantes.RutaCarga, pathLogo);

                //- JDEL Fin

                return Json(file);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Descarga el formato
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar(int formato, string file)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;

            return File(path, app, file);
        }

    }
}
