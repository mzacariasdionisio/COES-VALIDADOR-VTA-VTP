using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.CostoOportunidad.Helper;
using COES.MVC.Intranet.Areas.CostoOportunidad.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.CostoOportunidad;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.CostoOportunidad.Controllers
{
    public class ProcesoController : BaseController
    {
        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        private readonly CostoOportunidadAppServicio servicio = new CostoOportunidadAppServicio();
        private readonly ServiceReferenceCostoOportunidad.CostoOportunidadServicioClient coService = new ServiceReferenceCostoOportunidad.CostoOportunidadServicioClient();

        /// <summary>
        /// Pagina inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ProcesoModel model = new ProcesoModel();
            model.ListaPeriodo = this.servicio.GetByCriteriaCoPeriodos(-1);
            return View(model);
        }

        /// <summary>
        /// Permite tener las versiones de un periodo
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerVersiones(int idPeriodo)
        {
            try
            {
                return Json(this.servicio.GetByCriteriaCoVersions(idPeriodo));
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite obtener los datos
        /// </summary>
        /// <param name="idVersion"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ObtenerDatos(int idVersion)
        {
            ProcesoModel model = new ProcesoModel();
            try
            {
                string[][] datosPrograma = null;
                string[][] datosRAUp = null;
                string[][] datosRADown = null;
                string[][] datosRAEjecUp = null;
                string[][] datosRAEjecDown = null;
                string[][] datosProgramaSinReserva = null;

                this.servicio.ObtenerInsumosProcesoFinal(idVersion, out datosPrograma, out datosRAUp, out datosRADown,
                    out datosRAEjecUp, out datosRAEjecDown, out datosProgramaSinReserva);

                model.DatosPrograma = datosPrograma;
                model.DatosProgramaSinReserva = datosProgramaSinReserva;
                model.DatosRAProgramadaUp = datosRAUp;
                model.DatosRAProgramadaDown = datosRADown;
                model.DatosRAEjecutadaUp = datosRAEjecUp;
                model.DatosRAEjecutadaDown = datosRAEjecDown;
                model.Indicador = 1;

                var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
                var result = new ContentResult
                {
                    Content = serializer.Serialize(model),
                    ContentType = "application/json"
                };

                return result;
            }
            catch
            {
                return Json(-1);
            }
        }

        [HttpPost]
        public ActionResult ObtenerResultado(int idVersion)
        {
            ProcesoModel model = new ProcesoModel();
            try
            {
                string[][] datosDespacho = null;
                string[][] datosDespachoSinR = null;
                int[][] coloresDespacho = null;
                int[][] coloresDespachoSinR = null;

                this.servicio.ObtenerResultadoProceso(idVersion, out datosDespacho, out datosDespachoSinR, out coloresDespacho, out coloresDespachoSinR);

                model.DatosDespacho = datosDespacho;
                model.DatosDespachoSinR = datosDespachoSinR;
                model.ColoresDespacho = coloresDespacho;
                model.ColoresDespachoSinR = coloresDespachoSinR;

                var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
                var result = new ContentResult
                {
                    Content = serializer.Serialize(model),
                    ContentType = "application/json"
                };

                return result;
            }
            catch
            {
                return Json(-1);
            }
        }

        public JsonResult ExportarInsumo(int idVersion)
        {
            try
            {
                ProcesoModel model = new ProcesoModel();
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCOportunidad.RutaExportacion;
                string file = ConstantesCOportunidad.ArchivoReporteInsumo;

                string[][] datosPrograma = null;
                string[][] datosRAUp = null;
                string[][] datosRADown = null;
                string[][] datosRAEjecUp = null;
                string[][] datosRAEjecDown = null;
                string[][] datosProgramaSinReserva = null;

                this.servicio.ObtenerInsumosProcesoFinal(idVersion, out datosPrograma, out datosRAUp, out datosRADown,
                    out datosRAEjecUp, out datosRAEjecDown, out datosProgramaSinReserva);

                model.DatosPrograma = datosPrograma;
                model.DatosProgramaSinReserva = datosProgramaSinReserva;
                model.DatosRAProgramadaUp = datosRAUp;
                model.DatosRAProgramadaDown = datosRADown;
                model.DatosRAEjecutadaUp = datosRAEjecUp;
                model.DatosRAEjecutadaDown = datosRAEjecDown;

                CostoOportunidadExcel.ExportarInsumoProceso(model, path, file);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarInsumo()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesCOportunidad.RutaExportacion + ConstantesCOportunidad.ArchivoReporteInsumo;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, Constantes.AppExcel, ConstantesCOportunidad.ArchivoReporteInsumo);
        }

        public JsonResult ExportarResultado(int idVersion)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCOportunidad.RutaExportacion;
                string file = ConstantesCOportunidad.ArchivoReporteResultado;

                ProcesoModel model = new ProcesoModel();

                string[][] datosDespacho = null;
                string[][] datosDespachoSinR = null;
                int[][] coloresDespacho = null;
                int[][] coloresDespachoSinR = null;

                this.servicio.ObtenerResultadoProceso(idVersion, out datosDespacho, out datosDespachoSinR, out coloresDespacho, out coloresDespachoSinR);

                model.DatosDespacho = datosDespacho;
                model.DatosDespachoSinR = datosDespachoSinR;
                model.ColoresDespacho = coloresDespacho;
                model.ColoresDespachoSinR = coloresDespachoSinR;

                CostoOportunidadExcel.ExportarResultadoProceso(model, path, file);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarResultado()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesCOportunidad.RutaExportacion + ConstantesCOportunidad.ArchivoReporteResultado;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, Constantes.AppExcel, ConstantesCOportunidad.ArchivoReporteResultado);
        }

        /// <summary>
        /// Validar la existencia del proceso de calculo
        /// </summary>
        /// <param name="idVersion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarPeriodo(int idVersion)
        {
            return Json(this.servicio.ValidarProcesoPeriodo(idVersion));
        }

        /// <summary>
        /// Permite ejecutar el proceso de los costos de oportunidad
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="option">0: Proceso normal, 1: Uso de alfas y betas diarios</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Procesar(int idVersion, int option)
        {
            try
            {
                CoVersionDTO version = this.servicio.GetByIdCoVersion(idVersion);
                //return Json(this.servicio.ProcesarCalculo(idVersion, (DateTime)version.Coverfecinicio, (DateTime)version.Coverfecfin, base.UserName, option));
                return Json(this.coService.ProcesarCalculo(idVersion, (DateTime)version.Coverfecinicio, (DateTime)version.Coverfecfin, base.UserName, option));
                
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite abrir la ventana de reproceso
        /// </summary>
        /// <param name="idVersion"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Reprocesar(int idVersion)
        {
            ProcesoModel model = new ProcesoModel();
            CoVersionDTO version = this.servicio.GetByIdCoVersion(idVersion);
            model.FechaInicio = ((DateTime)version.Coverfecinicio).ToString(Constantes.FormatoFecha);
            model.FechaFin = ((DateTime)version.Coverfecfin).ToString(Constantes.FormatoFecha);
            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar la pantalla de envío a liquidaciones
        /// </summary>
        /// <param name="idVersion"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Liquidacion(int idVersion)
        {
            ProcesoModel model = new ProcesoModel();
            CoVersionDTO version = this.servicio.GetByIdCoVersion(idVersion);
            CoPeriodoDTO periodo = this.servicio.GetByIdCoPeriodo((int)version.Copercodi);
            model.PeriodoNombre = periodo.Copernomb;
            model.VersionNombre = version.Coverdesc;
            model.IdPeriodoTrn = -1;
            model.ListaPeriodoTrn = (new Servicios.Aplicacion.Transferencias.PeriodoAppServicio()).ListPeriodo();

            if (periodo.Coperanio != null && periodo.Copermes != null)
            {
                Dominio.DTO.Transferencias.PeriodoDTO periodoTrn = model.ListaPeriodoTrn.Where(x => x.AnioCodi == periodo.Coperanio
                && x.MesCodi == periodo.Copermes).FirstOrDefault();

                if (periodoTrn != null)
                {
                    model.IdPeriodoTrn = periodoTrn.PeriCodi;
                }
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite tener las versiones de un periodo de vcr
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerRecalculoTrn(int idPeriodo)
        {
            try
            {
                return Json((new Servicios.Aplicacion.CompensacionRSF.CompensacionRSFAppServicio()).ListVcrRecalculos(idPeriodo));
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite ejecutar el reproceso
        /// </summary>
        /// <param name="idVersio"></param>
        /// <param name="indicador"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></paramejecutarreproceso
        /// <param name="indicadorDatos"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EjecutarReproceso(int idVersion, int indicador, string fechaInicio, string fechaFin, int indicadorDatos,
            int option, int importarSP7)
        {
            try
            {
                DateTime fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                int result = this.servicio.EjecutarReproceso(idVersion, indicador, fecInicio, fecFin, indicadorDatos, base.UserName, option, importarSP7);
                //int result = this.coService.EjecutarReproceso(idVersion, indicador, fecInicio, fecFin, indicadorDatos, base.UserName, option, importarSP7);
                

                return Json(result);
            }
            catch
            {
                return Json(-1);
            }
        }



        /// <summary>
        /// Permite enviar los datos a liquidación
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EjecutarLiquidacion(int idVersion, int idPeriodoTrn, int idVersionTrn)
        {
            try
            {
                this.servicio.EnviarLiquidacion(idVersion, base.UserName, idPeriodoTrn, idVersionTrn);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite mostrar el listado de envíos realizados
        /// </summary>
        /// <param name="idVersion"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult EnvioLiquidacion(int idVersion)
        {
            ProcesoModel model = new ProcesoModel();
            model.ListaEnvios = this.servicio.GetByCriteriaCoEnvioliquidacions(idVersion);

            return PartialView(model);
        }

        /// <summary>
        /// Permite obtener la reserva asignada en bloques distintos
        /// </summary>
        /// <param name="idVersion"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ReservaAsignada(int idVersion)
        {
            ProcesoModel model = new ProcesoModel();
            model.ListaRADetalle = this.servicio.ObtenerDetalleRAEjecutada(idVersion);
            return PartialView(model);
        }
    }
}