using COES.Base.Tools;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.DemandaMaxima.Models;
using COES.Servicios.Aplicacion.DemandaMaxima;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Mediciones;
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
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.DemandaMaxima.Helper;
using log4net;
using System.Reflection;


namespace COES.MVC.Intranet.Areas.DemandaMaxima.Controllers
{
    public class InformacionController : BaseController
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
        public InformacionController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>  
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public int IdFormato = Int32.Parse(ConfigurationManager.AppSettings["IdFormatoPR16"]);

        FormatoMedicionAppServicio logic = new FormatoMedicionAppServicio();
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

            //- alpha.JDEL - Inicio 02/06/2016: Cambio para atender el requerimiento.
            //model.ListaPeriodo = this.servicio.ListaPeriodoReporte(ConfigurationManager.AppSettings["FechaInicioCumplimiento"]);//Fecha de inicio del despliqgue de la aplicación
            model.ListaPeriodo = this.servicio.ListaPeriodoReporte(ConfigurationManager.AppSettings["FechaInicioCumplimientoPR16"]);//Fecha de inicio del despliqgue de la aplicación
            //- JDEL Fin

            //- pr16.HDT - Inicio 01/04/2018: Cambio para atender el requerimiento.
            model.ListaPeriodoSicli = this.servicio.ListaPeriodoSicli();
            //- HDT Fin

            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");

            int tipoEmp = 0;
            if (model.ListaTipoempresa.Count != 1)
            {
                tipoEmp = model.ListaTipoempresa[0].Tipoemprcodi;
            }
            //var oUl=model.ListaTipoempresa.FirstOrDefault(t => t.Tipoemprcodi == 4);
            //model.ListaTipoempresa.Remove(oUl);
            model.ListaEmpresasCumplimiento = this.servicio.ListaEmpresasPorTipoCumplimiento(tipoEmp, IdFormato);

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

        //- pr16.HDT - 01/04/2018: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener la máxima demanda para el periodo proporcionado.
        /// </summary>
        /// <param name="periodoInicial"></param>
        /// <returns></returns>
        public JsonResult obtenerMaximaDemanda(string periodoInicial)
        {
            string[] formats = { Constantes.FormatoFecha };
            DateTime dti = DateTime.ParseExact(periodoInicial, formats, new CultureInfo("en-US"), DateTimeStyles.None);
            DateTime dtf = dti;

            DateTime fec_ini = new DateTime();
            DateTime fec_fin = new DateTime();

            fec_ini = new DateTime(dti.Year, dti.Month, 1);
            fec_fin = new DateTime(dtf.Year, dtf.Month, 1).AddMonths(1).AddDays(-1);

            DemandadiaDTO entity = this.servicio.ObtenerDatosMaximaDemanda(fec_ini, fec_fin);

            return Json(entity);
        }


        /// <summary>
        /// Metodo para obtener la lista de información cada 15 min
        /// </summary>
        /// <param name="nroPagina"></param>
        /// <param name="empresas"></param>
        /// <param name="tipos"></param>
        /// <param name="ini"></param>
        /// <param name="periodoSicli"></param>
        /// <param name="cumpli"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListarReporteInformacion15min(string empresas, string tipos, string ini, string periodoSicli, int max, int nroPagina)
        {
            DemandaMaximaModel model = new DemandaMaximaModel();
            ViewBag.max = max;

            string diasMaxDemanda = "";
            //- pr16.JDEL - Inicio 07/07/2016: Cambio para actualizar el codido de lectura de pr16 y alpha.
            string lectCodiPR16 = ConfigurationManager.AppSettings["IdLecturaPR16"];
            string lectCodiAlpha = ConfigurationManager.AppSettings["IdLecturaAlphaPR16"];
            //- JDEL Fin
            string[] formats = { Constantes.FormatoFecha };
            DateTime dti = DateTime.ParseExact(ini, formats, new CultureInfo("en-US"), DateTimeStyles.None);

            if (max == 1)
            {
                //Simulando la obtencion de la máxima demanda

                //- pr16.HDT - Inicio 01/04/2018: Cambio para atender el requerimiento.
                //DateTime dtf = DateTime.ParseExact(periodoSicli, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                DateTime dtf = dti;
                //- HDT Fin

                DateTime fec_ini = new DateTime();
                DateTime fec_fin = new DateTime();

                List<DemandadiaDTO> listDemanda = new List<DemandadiaDTO>();
                for (int i = dti.Month; i <= dtf.Month; i++)
                {
                    fec_ini = new DateTime(dti.Year, i, 1);
                    fec_fin = new DateTime(dtf.Year, i, 1).AddMonths(1).AddDays(-1);
                    DemandadiaDTO entity = this.servicio.ObtenerDatosMaximaDemanda(fec_ini, fec_fin);
                    if (diasMaxDemanda == "")
                    { diasMaxDemanda = diasMaxDemanda + "'" + entity.FechaMD + "'"; }
                    else
                    { diasMaxDemanda = diasMaxDemanda + ", '" + entity.FechaMD + "'"; }
                    listDemanda.Add(entity);
                }
                model.ListaDemandadia = listDemanda;
            }

            List<MeMedicion96DTO> listInfo = new List<MeMedicion96DTO>();

            int regIni, regFin;
            int numeroDias = System.DateTime.DaysInMonth(dti.Year, dti.Month);

            regIni = (nroPagina - 1) * numeroDias + 1;
            regFin = nroPagina * numeroDias;

            List<MeMedicion96DTO> listReporteInformacion = this.servicio.ListaReporteInformacion15min(IdFormato, ini, periodoSicli, empresas, tipos, diasMaxDemanda, lectCodiPR16, lectCodiAlpha, regIni, regFin);

            foreach (var entity in listReporteInformacion)
            {
                if (max == 1)
                {
                    foreach (var item in model.ListaDemandadia)
                    {
                        if (entity.FechaFila.ToString(Constantes.FormatoFecha) == item.FechaMD)
                        {
                            entity.HP = "H" + item.IndiceMDHP;
                            entity.HFP = "H" + item.IndiceMDHFP;
                        }
                    }
                }
                listInfo.Add(entity);
            }
            model.tipoEmpresa = tipos;
            model.ListaReporteInformacion15min = listInfo;
            model.registros = listInfo.Count().ToString();
            return View(model);
        }

        /// <summary>
        /// Metodo para obtener la lista de información cada 30 min
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="tipos"></param>
        /// <param name="ini"></param>
        /// <param name="periodoSicli"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListarReporteInformacion30min(string empresas, string tipos, string ini, string periodoSicli, int max, int nroPagina)
        {
            DemandaMaximaModel model = new DemandaMaximaModel();

            //- pr16.JDEL - Inicio 07/07/2016: Cambio para actualizar el codido de lectura de pr16 y alpha.
            string lectCodiPR16 = ConfigurationManager.AppSettings["IdLecturaPR16"];
            //- JDEL Fin


            //- pr16.JDEL - Inicio 21/07/2016: Cambio para atender el requerimiento.
            //string lectCodiAlpha = ConfigurationManager.AppSettings["IdLecturaAlpha"];
            string lectCodiAlpha = ConfigurationManager.AppSettings["IdLecturaAlphaPR16"];
            //- JDEL Fin

            int regIni, regFin;
            regIni = (nroPagina - 1) * Constantes.PageSizeDemandaMaxima + 1;
            regFin = nroPagina * Constantes.PageSizeDemandaMaxima;

            List<MeMedicion48DTO> listReporteInformacion30min = this.servicio.ListaReporteInformacion30min(IdFormato
                                                                                                         , ConfigurationManager.AppSettings["FechaInicioCumplimientoPR16"]
                                                                                                         , lectCodiPR16
                                                                                                         , empresas
                                                                                                         , tipos
                                                                                                         , ini
                                                                                                         , periodoSicli
                                                                                                         , lectCodiAlpha
                                                                                                         , regIni
                                                                                                         , regFin);
            model.tipoEmpresa = tipos;
            model.ListaReporteInformacion30min = listReporteInformacion30min;
            model.registros = listReporteInformacion30min.Count().ToString();
            return View(model);
        }

        /// <summary>
        /// Permite pintar la vista del paginado
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string empresas, string tipos, string ini, string periodoSicli, int max, string nivel)
        {
            Paginacion model = new Paginacion();

            string diasMaxDemanda = "";
            //- pr16.JDEL - Inicio 07/07/2016: Cambio para actualizar el codido de lectura de pr16 y alpha.
            string lectCodiPR16 = ConfigurationManager.AppSettings["IdLecturaPR16"];
            string lectCodiAlpha = ConfigurationManager.AppSettings["IdLecturaAlphaPR16"];
            //- JDEL Fin

            if (max == 1)
            {
                //Simulando la obtencion de la máxima demanda
                string[] formats = { Constantes.FormatoFecha };
                DateTime dti = DateTime.ParseExact(ini, formats, new CultureInfo("en-US"), DateTimeStyles.None);

                //- pr16.HDT - Inicio 01/04/2018: Cambio para atender el requerimiento.
                //DateTime dtf = DateTime.ParseExact(periodoSicli, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                DateTime dtf = dti;
                //- HDT Fin

                DateTime fec_ini = new DateTime();
                DateTime fec_fin = new DateTime();

                List<DemandadiaDTO> listDemanda = new List<DemandadiaDTO>();
                for (int i = dti.Month; i <= dtf.Month; i++)
                {
                    fec_ini = new DateTime(dti.Year, i, 1);
                    fec_fin = new DateTime(dtf.Year, i, 1).AddMonths(1).AddDays(-1);
                    DemandadiaDTO entity = this.servicio.ObtenerDatosMaximaDemanda(fec_ini, fec_fin);
                    if (diasMaxDemanda == "")
                    { diasMaxDemanda = diasMaxDemanda + "'" + entity.FechaMD + "'"; }
                    else
                    { diasMaxDemanda = diasMaxDemanda + ", '" + entity.FechaMD + "'"; }
                    listDemanda.Add(entity);
                }
            }

            int nroRegistros = this.servicio.ListaReporteInformacion15minCount(IdFormato, ini, periodoSicli, empresas, tipos, diasMaxDemanda, lectCodiPR16, lectCodiAlpha);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeDemandaMaxima;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }


        /// <summary>
        /// Exportar en Excel Reporte de información
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="tipos"></param>
        /// <param name="ini"></param>
        /// <param name="periodoSicli"></param>
        /// <param name="nivel"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public JsonResult Exportar(string empresas, string tipos, string ini, string periodoSicli, string nivel, int max)
        {
            try
            {
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + ConstantesDemandaMaxima.PathLogo;
                string file = this.servicio.GenerarFormatoRepInformacion(nivel, empresas, tipos, ini, periodoSicli, max, AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga, pathLogo);
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
