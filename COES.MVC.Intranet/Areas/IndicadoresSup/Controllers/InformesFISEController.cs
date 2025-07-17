using COES.MVC.Intranet.Areas.IndicadoresSup.Models;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Siosein2;
using COES.Servicios.Aplicacion.Transferencias;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.SistemasTransmision;
using System.Linq;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System.IO;

namespace COES.MVC.Intranet.Areas.IndicadoresSup.Controllers
{
    public class InformesFISEController : Controller
    {
        //
        // GET: /Siosein2/InformesFISE/


        private static readonly ILog Log = LogManager.GetLogger(typeof(InformesFISEController));
        private static readonly string NameController = "InformesFISEController";
        private readonly Siosein2AppServicio _servicioSiosein2;
        private readonly PeriodoAppServicio _servicioPeriodo;
        private readonly SistemasTransmisionAppServicio _servicioSistemasTransmision;


        public InformesFISEController()
        {
            _servicioSiosein2 = new Siosein2AppServicio();
            _servicioPeriodo = new PeriodoAppServicio();
            _servicioSistemasTransmision = new SistemasTransmisionAppServicio();
        }


        /// <inheritdoc />
        /// <summary>
        /// Protected de log de errores page
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NameController, ex);
                throw;
            }
        }

        public ActionResult Index()
        {
            var fechaActual = DateTime.Today;

            List<StCompensacionDTO> listaElemento = _servicioSiosein2.ObtenerListaCompensacion(fechaActual);

            var model = new Siosein2Model
            {
                MesAnio = fechaActual.ToString(ConstantesAppServicio.FormatoMes),
                Titulo = "Informes FISE Y Asignación de Responsabilidades de pago de los SST y SCT (PR-35)",
                ListItemSelect = listaElemento.Select(x => new SelectListItem() { Value = x.Stcompcodi.ToString(), Text = x.Stcompnomelemento }).ToList()
            };


            return View(model);
        }

        /// <summary>
        /// carga lista InformesCUCGE
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarInformesFise(string mesanio)
        {
            DateTime fecha = DateTime.ParseExact(ConstantesAppServicio.IniDiaFecha + mesanio.Replace(" ", "/"), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            int pericodi = int.Parse(fecha.Year + fecha.Month.ToString().PadLeft(2, '0')), recpotcodi = 1;
            var datosTrnPeriodo = _servicioPeriodo.GetByAnioMes(pericodi);
            var pericodis = datosTrnPeriodo != null ? datosTrnPeriodo.PeriCodi : (int?)null;

            var model = new Siosein2Model
            {
                Resultados = new List<string>()
                {
                    _servicioSiosein2.ListaDemandaElectricaAfectaFiseHtml(pericodis, recpotcodi),
                }
            };
            model.Resultados.AddRange(_servicioSiosein2.ListaIngresoTarifariosEmpresaPagoHtml(pericodis, recpotcodi));
            model.Resultados.AddRange(_servicioSiosein2.ListaCompensacionIncluidasEnPeajeConexionHtml(pericodis, recpotcodi));

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult CargarListaCompensacion(string mesanio)
        {
            DateTime fecha = DateTime.ParseExact(ConstantesAppServicio.IniDiaFecha + mesanio.Replace(" ", "/"), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            List<StCompensacionDTO> listaElemento = _servicioSiosein2.ObtenerListaCompensacion(fecha);
            var listaSelect = listaElemento.Select(x => new SelectListItem() { Value = x.Stcompcodi.ToString(), Text = x.Stcompnomelemento }).ToList();

            var jsonResult = Json(listaSelect);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult CargarAnexo(string mesanio, int stcompcodi, int numeroAnexo)
        {

            var model = new Siosein2Model();

            try
            {
                DateTime fecha = DateTime.ParseExact(ConstantesAppServicio.IniDiaFecha + mesanio.Replace(" ", "/"), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                var resultReporte = string.Empty;
                switch (numeroAnexo)
                {
                    case 4:
                        resultReporte = _servicioSiosein2.ListaGeneradoresCompensacionHtml(fecha, stcompcodi);
                        break;
                    case 5:
                        resultReporte = _servicioSiosein2.ListaFactorPagoParticipacionHtml(fecha, stcompcodi);
                        break;
                    case 6:
                        resultReporte = _servicioSiosein2.ListaCompensacionMensualHtml(fecha, stcompcodi);
                        break;
                }

                model.Resultado = resultReporte;
                model.TipoEstado = (int)Siosein2Model.Estado.Ok;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);
                model.Resultado = e.Message;
                model.TipoEstado = (int)Siosein2Model.Estado.Error;
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Permite retornar datos de exportación generados del Informe FISE
        /// </summary>
        /// <param name="mesanio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarFise(string mesanio)
        {
            var model = new Siosein2Model();
            try
            {
                DateTime fecha = DateTime.ParseExact(ConstantesAppServicio.IniDiaFecha + mesanio.Replace(" ", "/"), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                string directorioTemporal = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                FileInfo plantillaExcel = new FileInfo(string.Concat(directorioTemporal, ConstantesSiosein2.PantillaReporteFise));

                var nombreArchivo = _servicioSiosein2.GenerarArchivoExcelInformeFise(fecha, plantillaExcel);
                model.Resultado = nombreArchivo;
                model.TipoEstado = (int)Siosein2Model.Estado.Ok;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);
                model.Resultado = e.Message;
                model.TipoEstado = (int)Siosein2Model.Estado.Error; 
            }
            return Json(model);
        }

        /// <summary>
        /// permite Descargar el archivo excel exportado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["fi"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, ConstantesAppServicio.ExtensionExcel, nombreArchivo);
        }

    }
}
