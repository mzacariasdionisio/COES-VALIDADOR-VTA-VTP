using COES.Framework.Base.Tools;
using COES.MVC.Intranet.App_Start;
using COES.MVC.Intranet.Areas.Hidrologia.Helper;
using COES.MVC.Intranet.Areas.Hidrologia.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Hidrologia;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Hidrologia.Controllers
{
    [ValidarSesion]
    public class CumplimientoController : Controller
    {
        //inicio modificado
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(CumplimientoController));
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("CumplimientoController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("CumplimientoController", ex);
                throw;
            }
        }
        //fin modificado

        //
        // GET: /Hidrologia/Cumplimiento/
        private HidrologiaAppServicio logic;
        private GeneralAppServicio logicGeneral;
        public CumplimientoController()
        {
            logic = new HidrologiaAppServicio();
            logicGeneral = new GeneralAppServicio();
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Index de inicio de controller Cumplimiento
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            HidrologiaModel model = new HidrologiaModel();
            model.ListaEmpresas = logicGeneral.ObtenerEmpresasHidro();
            model.FechaInicio = DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            int nroSemana = EPDate.f_numerosemana(DateTime.Now);
            model.NroSemana = nroSemana;
            model.Dia = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
            model.ListaFormato = logic.ListMeFormatos().Where(x => x.Modcodi == ConstantesHidrologia.IdModulo).ToList(); //lista de todos los formatos para hidrologia
            model.ListaLectura = logic.ListMeLecturas().Where(x => x.Origlectcodi == ConstantesHidrologia.IdOrigenHidro).ToList();
            List<int> listaFormatCodi = new List<int>();
            List<int> listaFormatPeriodo = new List<int>();
            foreach (var reg in model.ListaFormato)
            {
                listaFormatCodi.Add(reg.Formatcodi);
                listaFormatPeriodo.Add((int)reg.Formatperiodo);
            }
            model.ListaSemanas = HelperHidrologia.GetListaSemana(DateTime.Now.Year);
            model.StrFormatCodi = String.Join(",", listaFormatCodi);
            model.StrFormatPeriodo = String.Join(",", listaFormatPeriodo);
            return View(model);
        }

        /// <summary>
        /// Devuelve vista parcial para mostrar listado de cumplimiento
        /// </summary>
        /// <param name="sEmpresas"></param>
        /// <param name="idFormato"></param>
        /// <param name="fIni"></param>
        /// <param name="fFin"></param>
        /// <param name="mes1"></param>
        /// <param name="mes2"></param>
        /// <param name="semana1"></param>
        /// <param name="semana2"></param>
        /// <returns></returns>
        public PartialViewResult Lista(string sEmpresas, int idFormato, string fIni, string fFin, string mes1, string mes2, string semana1, string semana2)
        {
            DateTime fechaIni = DateTime.MinValue;
            DateTime fechaFin = DateTime.MinValue;
            var formato = logic.GetByIdMeFormato(idFormato);
            fechaIni = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes1, semana1, fIni, Constantes.FormatoFecha);
            fechaFin = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes2, semana2, fFin, Constantes.FormatoFecha);
            HidrologiaModel model = new HidrologiaModel();
            model.Resultado = logic.GeneraViewCumplimiento(sEmpresas, fechaIni, fechaFin, idFormato, (int)formato.Formatperiodo);
            model.NombreFortmato = formato.Formatnombre;
            return PartialView(model);
        }

        /// <summary>
        /// Lista los formatos de acuerdo a la lectura seleccionada
        /// </summary>
        /// <param name="idLectura"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarFormatosXLectura(string sLectura)
        {
            HidrologiaModel model = new HidrologiaModel();
            string sEmpresa = "-1";
            var entitys = logic.GetByModuloLecturaMeFormatosMultiple(ConstantesHidrologia.IdModulo, sLectura, sEmpresa);
            SelectList lista = new SelectList(entitys, "Formatcodi", "Formatnombre");
            return Json(lista);
        }

        /// <summary>
        /// Lista de Semana por Año
        /// </summary>
        /// <param name="idAnho"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarSemanas(string idAnho)
        {
            BusquedaModel model = new BusquedaModel();
            List<TipoInformacion> entitys = HelperHidrologia.GetListaSemana(Int32.Parse(idAnho));
            SelectList list = new SelectList(entitys, "IdTipoInfo", "NombreTipoInfo");
            return Json(list);
        }

        // exporta el reporte general consultado a archivo excel
        [HttpPost]
        public JsonResult GenerarReporteCumplimiento(string sEmpresas, int idFormato, string fIni, string fFin, string mes1, string mes2, string semana1, string semana2)
        {
            int indicador = 1;
            DateTime fechaIni = DateTime.MinValue;
            DateTime fechaFin = DateTime.MinValue;
            var formato = logic.GetByIdMeFormato(idFormato);
            fechaIni = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes1, semana1, fIni, Constantes.FormatoFecha);
            fechaFin = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes2, semana2, fFin, Constantes.FormatoFecha);
            HidrologiaModel model = new HidrologiaModel();
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;


            try
            {
                logic.GeneraExcelCumplimiento(sEmpresas, fechaIni, fechaFin, idFormato, formato.Formatnombre, (int)formato.Formatperiodo, ruta + ConstantesHidrologia.NombreArchivoCumplimiento,
                    ruta + Constantes.NombreLogoCoes);
                indicador = 1;

            }
            catch
            {
                indicador = -1;
            }
            return Json(indicador);
        }

        /// <summary>
        /// Permite exportar el reporte general
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarReporte()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = ConstantesHidrologia.NombreArchivoCumplimiento;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        #region REPORTE de cumplimiento de la Extranet de Hidrología
        public ActionResult Reporte()
        {
            HidrologiaModel model = new HidrologiaModel();
            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
            model.ListaEmpresas = logicGeneral.ObtenerEmpresasHidro();

            return View(model);
        }

        [HttpPost]
        public PartialViewResult ViewReporteCumplimientoByEmpresa(int empresacodi, string mes)
        {
            HidrologiaModel model = new HidrologiaModel();
            model.ListaFormatoCOES = new List<FormatoCoes>();

            int idOrigen = ConstHidrologia.IdOrigenHidro;
            var listaAreas = logic.ListAreaXFormato(idOrigen).OrderByDescending(x => x.Areacode).ToList();
            if (listaAreas != null && listaAreas.Count > 0)
            {
                foreach (var a in listaAreas)
                {
                    FormatoCoes f = new FormatoCoes();
                    f.Areacodi = a.Areacode;
                    f.Areaname = a.Areaname.Trim();
                    f.Areaname = f.Areaname.Replace("Sub Direccion de", "");
                    f.Areaname = f.Areaname.Replace("Sub Direccion ", "");

                    model.ListaFormatoCOES.Add(f);
                }
            }

            //fechas
            DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, mes, "", "", "");
            DateTime fechaInicio = fechaProceso;
            DateTime fechaFin = fechaProceso.AddMonths(1).AddDays(-1);

            foreach (var fcoes in model.ListaFormatoCOES)
            {
                string htmlSegunFormato = logic.GeneraViewReporteCumplimientoHidrologia(fcoes.Areacodi.ToString(), empresacodi.ToString(), mes, fechaInicio, fechaFin);
                fcoes.html = htmlSegunFormato;
            }

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult ExportarExcelReporteCumplimiento(string mes, bool estado)
        {
            HidrologiaModel model = new HidrologiaModel();
            try
            {
                var listaEmpresa = logicGeneral.ObtenerEmpresasHidro();

                //fechas
                DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, mes, "", "", "");
                DateTime fechaInicio = fechaProceso;
                DateTime fechaFin = fechaProceso.AddMonths(1).AddDays(-1);

                model.Resultado = logic.GenerarFileExcelReporteCumplimientoExtranet(listaEmpresa, mes, fechaInicio, fechaFin, estado);
                model.TituloReporteXLS = string.Format("Reporte de Cumplimiento Extranet Hidrología_{0}.xlsx", mes.Replace(" ", "_"));
            }
            catch (Exception ex)
            {
                log.Error("CumplimientoController", ex);
                model.Resultado = "-1";
            }
            return Json(model);
        }

        [HttpGet]
        public virtual ActionResult DescargarExcelReporteCumplimiento()
        {
            string strArchivoTemporal = Request["archivo"];
            string strNombreArchivo = Request["nombre"];
            byte[] buffer = null;

            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);
                System.IO.File.Delete(strArchivoTemporal);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
        }

        #endregion

    }
}
