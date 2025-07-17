using System;
using System.Linq;
using System.Web.Mvc;
using System.Globalization;
using System.Configuration;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.TiempoReal;
using COES.MVC.Intranet.Areas.TiempoReal.Models;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Areas.TiempoReal.Helper;
using log4net;
using System.Reflection;
using COES.Servicios.Aplicacion.Helper;


namespace COES.MVC.Intranet.Areas.TiempoReal.Controllers
{
    public class ScadaSp7Controller : BaseController
    {
        ScadaSp7AppServicio servScadaSp7 = new ScadaSp7AppServicio();

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(ScadaSp7Controller));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public ActionResult Index()
        {
            BusquedaMeScadaSp7Model model = new BusquedaMeScadaSp7Model();
            model.ListaTrZonaSp7 = servScadaSp7.ListTrZonaSp7s();
            model.ListaMeScadaFiltroSp7 = servScadaSp7.ListMeScadaFiltroSp7s();
            model.FechaIni = DateTime.Now.ToString(Constantes.FormatoFechaYYYYMMDD);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFechaYYYYMMDD);

            return View(model);
        }

        /// <summary>
        /// Obtiene la lista de datos SCADA usando filtros
        /// </summary>
        /// <param name="ssee">Indica si los datos son por subestación (falso: filtro personalizado)</param>
        /// <param name="zonaCodi">Código de zona/filtro</param>
        /// <param name="mediFechaIni">Fecha Inicial</param>
        /// <param name="mediFechaFin">Fecha Final</param>
        /// <param name="nroPage"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(bool ssee, int zonaCodi, DateTime mediFechaIni, DateTime mediFechaFin, int nroPage)
        {
            //bool sseeVal = Convert.ToBoolean(ssee);


            BusquedaMeScadaSp7Model model = new BusquedaMeScadaSp7Model();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (mediFechaIni != null)
            {
                fechaInicio = mediFechaIni;
            }

            if (mediFechaFin != null)
            {
                fechaFinal = mediFechaFin;
            }

            model.ListaMeScadaSp7 =
                servScadaSp7.BuscarOperaciones(ssee, zonaCodi, fechaInicio, fechaFinal, nroPage,
                    Constantes.PageSizeEvento).ToList();



            return PartialView(model);
        }


        /// <summary>
        /// Realiza el paginado de la lista
        /// </summary>
        /// <param name="ssee">Indica si los datos son por subestación (falso: filtro personalizado)</param>
        /// <param name="zonaCodi">Código de zona/filtro</param>
        /// <param name="mediFechaIni">Fecha Inicial</param>
        /// <param name="mediFechaFin">Fecha Final</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(bool ssee, int zonaCodi, DateTime mediFechaIni, DateTime mediFechaFin)
        {
            BusquedaMeScadaSp7Model model = new BusquedaMeScadaSp7Model();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (mediFechaIni != null)
            {
                fechaInicio = mediFechaIni;
            }

            if (mediFechaFin != null)
            {
                fechaFinal = mediFechaFin;
            }


            int nroRegistros = servScadaSp7.ObtenerNroFilas(ssee, zonaCodi, fechaInicio, fechaFinal);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeEvento;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return base.PartialView(model);
        }


        /// <summary>
        /// Exporta los datos SCADA cada 15/30 minutos
        /// </summary>
        /// <param name="lect15Min">Indica si los datos se exportarán cada 15 minutos (falso: 30 minutos)</param>
        /// <param name="ssee">Indica si los datos son por subestación (falso: filtro personalizado)</param>
        /// <param name="zonaCodi">Código de zona/filtro</param>
        /// <param name="mediFechaIni">Fecha Inicial</param>
        /// <param name="mediFechaFin">Fecha Final</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(bool lect15Min, bool ssee, int zonaCodi, DateTime mediFechaIni, DateTime mediFechaFin)
        {
            BusquedaMeScadaSp7Model model = new BusquedaMeScadaSp7Model();

            int result = 1;

            try
            {

                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFinal = DateTime.Now;

                if (mediFechaIni != null)
                {
                    fechaInicio = mediFechaIni;
                }

                if (mediFechaFin != null)
                {
                    fechaFinal = mediFechaFin;
                }

                model.ListaMeScadaSp7 =
                    servScadaSp7.BuscarOperacionesReporte(ssee, zonaCodi, fechaInicio, fechaFinal).ToList();
                ;
                
                ExcelDocument.GenerarArchivoScadaSP7(lect15Min, model.ListaMeScadaSp7, mediFechaIni.ToString("dd/MM/yyyy"), mediFechaFin.ToString("dd/MM/yyyy"));

                result = 1;

            }
            catch (Exception ex)
            {
                result = -1;
                Log.Error(NameController, ex);
            }

            return Json(result);

        }


        /// <summary>
        /// Permite exportar el reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel +
                          NombreArchivo.ReporteSCADA;
            return File(file, Constantes.AppExcel, NombreArchivo.ReporteSCADA);

        }

    }
}
