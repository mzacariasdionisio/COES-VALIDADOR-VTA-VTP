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
    public class CopiarDatosController : BaseController
    {
        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        CostoOportunidadAppServicio servicio = new CostoOportunidadAppServicio();

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

                this.servicio.ObtenerInsumosProceso(idVersion, out datosPrograma, out datosRAUp, out datosRADown,
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

        /// <summary>
        /// Permite realizar la copia de los datos
        /// </summary>
        /// <param name="idVersion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Importar(int idVersion)
        {
            try
            {
                CoVersionDTO version = this.servicio.GetByIdCoVersion(idVersion);
                this.servicio.CopiarDatos(idVersion, (DateTime)version.Coverfecinicio, (DateTime)version.Coverfecfin);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        public JsonResult Exportar(int idVersion)
        {
            try
            {

                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCOportunidad.RutaExportacion;
                string file = ConstantesCOportunidad.ArchivoReporteInsumo;

                ProcesoModel model = new ProcesoModel();

                string[][] datosPrograma = null;
                string[][] datosRAUp = null;
                string[][] datosRADown = null;
                string[][] datosRAEjecUp = null;
                string[][] datosRAEjecDown = null;
                string[][] datosProgramaSinReserva = null;

                this.servicio.ObtenerInsumosProceso(idVersion, out datosPrograma, out datosRAUp, out datosRADown,
                    out datosRAEjecUp, out datosRAEjecDown, out datosProgramaSinReserva);

                model.DatosPrograma = datosPrograma;
                model.DatosProgramaSinReserva = datosProgramaSinReserva;
                model.DatosRAProgramadaUp = datosRAUp;
                model.DatosRAProgramadaDown = datosRADown;
                model.DatosRAEjecutadaUp = datosRAEjecUp;
                model.DatosRAEjecutadaDown = datosRAEjecDown;
                model.Indicador = 1;

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
        public virtual ActionResult Descargar()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesCOportunidad.RutaExportacion + ConstantesCOportunidad.ArchivoReporteInsumo;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, Constantes.AppExcel, ConstantesCOportunidad.ArchivoReporteInsumo);
        }
    }
}