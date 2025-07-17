using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Despacho.Helper;
using COES.MVC.Intranet.Areas.Despacho.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Despacho;
using COES.Servicios.Aplicacion.Despacho.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace COES.MVC.Intranet.Areas.Despacho.Controllers
{
    public class CostoIncrementalController : Controller
    {
        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>  
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(CostoIncrementalController));

        public CostoIncrementalController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("CostoIncrementalController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("CostoIncrementalController", ex);
                throw;
            }
        }

        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        GrupoDespachoAppServicio servicio = new GrupoDespachoAppServicio();
   

        #region Reporte Costo Incremental

        /// <summary>
        /// Reporte de Costo Incremental
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
          
            ReporteModel model = new ReporteModel();

            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
           

            return View(model);
        }


        /// <summary>
        /// Permite pintar la lista de registros de la consulta
        /// </summary>
        /// <param name="fecha">fecha de consulta</param>        
        /// <returns></returns>
        public ActionResult Listado(string fecha)
        {

            DateTime fechaConsulta = DateTime.Now;

            if (fecha != null)
            {
                fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }


            ReporteModel model = new ReporteModel();

            //List<ReporteCostoIncrementalDTO> reporte = this.servicio.ListarCostoIncremental(fechaConsulta);
            List<ReporteCostoIncrementalDTO> reporte = this.servicio.ListarTodosCI(new List<int>(), fechaConsulta);

            List<EntidadReporteModel> datos = new List<EntidadReporteModel>();
            foreach (var item in reporte)
            {
                datos.Add(new EntidadReporteModel()
                {
                    CEC = item.CEC,
                    Cincrem1 = item.Cincrem1.ToString(),
                    Cincrem2 = item.Cincrem2.ToString(),
                    Cincrem3 = item.Cincrem3.ToString(),
                    CV = item.CV,
                    CVC = item.CVC,
                    CVNC = item.CVNC,
                    Empresa = item.Empresa,
                    GrupoModoOperacion = item.GrupoModoOperacion,
                    Pe = item.Pe,
                    Precio = item.Precio,
                    Rendimiento = item.Rendimiento,
                    Tramo1 = item.Tramo1,
                    Tramo2 = item.Tramo2,
                    Tramo3 = item.Tramo3,
                    TipoCombustible = item.TipoCombustible
                });
            }

            model.Lista = datos;

            return View(model);
        }

        /// <summary>
        /// Permite generar los datos del reporte de la consulta para exportar
        /// </summary>
        /// <param name="fecha">fecha de consulta</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoReporte(string fecha)
        {
            int indicador = 1;
            try
            {
                DateTime fechaConsulta = DateTime.Now;

                if (fecha != null)
                {
                    fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                ReporteModel model = new ReporteModel();

                //List<ReporteCostoIncrementalDTO> reporte = this.servicio.ListarCostoIncremental(fechaConsulta);
                List<ReporteCostoIncrementalDTO> reporte = this.servicio.ListarTodosCI(new List<int>(), fechaConsulta);

                List<EntidadReporteModel> datos = new List<EntidadReporteModel>();
                foreach (var item in reporte)
                {
                    datos.Add(new EntidadReporteModel()
                    {
                        CEC = item.CEC,
                        Cincrem1 = item.Cincrem1.ToString(),
                        Cincrem2 = item.Cincrem2.ToString(),
                        Cincrem3 = item.Cincrem3.ToString(),
                        CV = item.CV,
                        CVC = item.CVC,
                        CVNC = item.CVNC,
                        Empresa = item.Empresa,
                        GrupoModoOperacion = item.GrupoModoOperacion,
                        Pe = item.Pe,
                        Precio = item.Precio,
                        Rendimiento = item.Rendimiento,
                        Tramo1 = item.Tramo1,
                        Tramo2 = item.Tramo2,
                        Tramo3 = item.Tramo3,
                        TipoCombustible = item.TipoCombustible
                    });

                }


                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesDespacho.FolderReporte;
                string titulo = "REPORTE COSTO INCREMENTAL";
                ExcelDocumentCostoIncremental.GernerarArchivoEnvios(datos, ruta, titulo, fecha);
                indicador = 1;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                indicador = -1;
            }
            return Json(indicador);
        }

        /// <summary>
        /// Permite exportar los datos del reporte
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult ExportarExcel()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = ConstantesDespacho.NombreReporteEnvios;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesDespacho.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        #endregion

        
    }
}
