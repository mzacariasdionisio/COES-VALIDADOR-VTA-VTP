using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Globalization;
using COES.Servicios.Aplicacion.DPODemanda;
using COES.MVC.Intranet.Areas.Admin.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.DPODemanda.Helper;
using COES.MVC.Intranet.Areas.DemandaPO.Models;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;
using log4net.Repository.Hierarchy;
using COES.Dominio.DTO.Sic;

using COES.Servicios.Aplicacion.CortoPlazo;
using COES.MVC.Intranet.Helper;
using log4net;
using COES.MVC.Intranet.Areas.CortoPlazo.Controllers;
using System.Threading;

namespace COES.MVC.Intranet.Areas.DemandaPO.Controllers
{
    public class CargaDatosController : Controller
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ResultadoController));

        /// <summary>
        /// Instancia del Web Service de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        DemandaPOAppServicio servDemandaPO = new DemandaPOAppServicio();


        /// <summary>
        /// Creación de la instancia del servicio correspondiente
        /// </summary>
        CortoPlazoAppServicio servicio = new CortoPlazoAppServicio();

        /// <summary>
        /// Lista de Empresa del usuario
        /// </summary>
        public List<COES.MVC.Intranet.SeguridadServicio.EmpresaDTO> ListaEmpresa
        {
            get
            {
                return (Session[ConstantesAdmin.SesionEmpresa] != null) ?
                    (List<COES.MVC.Intranet.SeguridadServicio.EmpresaDTO>)Session[ConstantesAdmin.SesionEmpresa] :
                    new List<COES.MVC.Intranet.SeguridadServicio.EmpresaDTO>();
            }
            set { Session[ConstantesAdmin.SesionEmpresa] = value; }
        }

        /// <summary>
        /// Pagina inicial del modulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Procesa los archivo Raw x Minuto de todo el DIA
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarAutomaticamentePorMinuto(string strFecProcess)
        {
            string sResultado = "1";

            try
            {
                DateTime parseFecha = DateTime.ParseExact(strFecProcess, ConstantesDpo.FormatoFechaMedicionRaw, CultureInfo.InvariantCulture);
                DateTime hoy = new DateTime(parseFecha.Year, parseFecha.Month, parseFecha.Day, 0, 5, 0); // 00:05
                DateTime hoyInicio = hoy.AddMinutes(-4); //Inicia en el minuto 00:01
                DateTime hoyFin = hoy.AddHours(23).AddMinutes(55); //termina en el minuto 00:00 del día siguiente
                int diaProceso = hoy.Day;
                int iTipo = 1;
                // PASO 1: Eliminamos los archivos dpo_estimadorraw_files TIPO = 1 and fecha entre hoyInicio, hoyFin
                servDemandaPO.DeleteREstimadorRawFileByDiaProceso(hoyInicio, hoyFin, iTipo);

                // PASO 2: Delete DPO_ESTIMADOR_RAW_LOG WHERE TIPO = 1 and fecha entre hoyInicio, hoyFin
                servDemandaPO.DeleteREstimadorRawLogByDiaProceso(hoyInicio, hoyFin, iTipo);

                // PASO 3: Delete DPO_ESTIMADOR_RAW_TMP WHERE fecha entre hoyInicio, hoyFin
                servDemandaPO.DeleteREstimadorRawTemporalByDiaProceso(hoyInicio, hoyFin);

                // PASO 4: Delete DPO_ESTIMADOR_RAW_[sufAnioMes] WHERE fecha entre hoyInicio, hoyFin
                string sufAnioMes = hoy.ToString("yyyyMM"); // Se le asignara a la tabla correspondiente del año mes
                hoyInicio = hoyInicio.AddMinutes(-1); // debe eliminar desde 00:00 [Recordar que las 23:00 le corresponde al bloque final del dia]
                servDemandaPO.DeleteREstimadorRawByDiaProceso(sufAnioMes, hoyInicio, hoyFin.AddMinutes(-60));
                string sProcedimiento = "Manual";
                //PASO 5: Insertamos todos los TNA del día 
                for (int i = 0; i < 1440; i += 5)
                {
                    parseFecha = hoy.AddMinutes(i);
                    sResultado = servDemandaPO.EjecutaProcesoLecturaRawPorMinuto(parseFecha, diaProceso, sProcedimiento);
                }
            }
            catch(Exception ex)
            {
                return Json(ex.Message);
            }
            

            return Json(sResultado);
        }

        /// <summary>
        /// Procesa los archivo Raw x 30 Minutos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarAutomaticamentePor30Minutos(string strFecProcess)
        {
            string sResultado = "1";

            try
            {
                sResultado = servDemandaPO.EjecutaJobGeneracionRawsIEOD(strFecProcess);
                if (!sResultado.Equals("1"))
                {
                    return Json(sResultado);
                }

                DateTime parseFecha = DateTime.ParseExact(strFecProcess, ConstantesDpo.FormatoFechaMedicionRaw, CultureInfo.InvariantCulture);
                DateTime hoy = new DateTime(parseFecha.Year, parseFecha.Month, parseFecha.Day, 0, 30, 0);
                DateTime hoyInicio = hoy; //Inicia en el minuto 00:30
                DateTime hoyFin = hoy.AddHours(23).AddMinutes(30); //termina en el minuto 00:00 del día siguiente
                int iTipo = 2;
                
                // Eliminamos los archivos dpo_estimadorraw_files TIPO = 1 and del día
                servDemandaPO.DeleteREstimadorRawFileByDiaProceso(hoyInicio, hoyFin, iTipo);

                // PASO 1: Delete DPO_ESTIMADOR_RAW_LOG WHERE TIPO = 2 and fecha = diaProceso
                servDemandaPO.DeleteREstimadorRawLogByDiaProceso(hoyInicio, hoyFin, iTipo);

                // PASO 2: Ejecutamos la carga de archivos IEOD en los intervalos de 30 minutos.
                for (int i = 0; i < 1440; i += 30)
                {
                    parseFecha = hoy.AddMinutes(i);
                    sResultado = servDemandaPO.EjecutaJobProcesoAutomaticoRawPor30Minutos(parseFecha);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }


            return Json(sResultado);
        }

        /// <summary>
        /// Muestra estados en la barra de estado del proceso x1M
        /// </summary>
        /// <returns>res</returns>
        public JsonResult consultarEstados(string fecProceso)
        {
            DateTime fechaProceso = DateTime.ParseExact(fecProceso, ConstantesDpo.FormatoFecha, CultureInfo.InvariantCulture);
            object res = new object();
            res = this.servDemandaPO.ObtenerMatrizBarraEstadoO(fechaProceso);
            return Json(res);
        }

        /// <summary>
        /// Ejecuta el proceso manual de carga
        /// </summary>
        /// <param name="fechaImportacion">Fecha de los archivos a procesar</param>
        /// <param name="direccion">Dirección de los archivos a procesar</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarManualmente(string fechaImportacion, string direccion)
        {
            string direccionValida = (!direccion.EndsWith(@"\")) ? direccion + @"\" : direccion;
            object res = this.servDemandaPO.EjecutaProcesoManualRawDemandaCPSco(fechaImportacion, direccionValida);
            return Json(res);
        }

        /// <summary>
        /// Función que se encarga de exportar a excel los datos de los archivos Raw no procesados
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarExcelRawsNoProcesados()
        {
            string fileName = string.Empty;
            string path = string.Empty;
           
            List<DpoEstimadorRawFilesDTO> listaRawsNoProcesadosXMinuto = new List<DpoEstimadorRawFilesDTO>();
            List<DpoEstimadorRawFilesDTO> listaRawsNoProcesadosIeod = new List<DpoEstimadorRawFilesDTO>();

            listaRawsNoProcesadosXMinuto = servDemandaPO.ListFilesRawPorMinuto();
            listaRawsNoProcesadosIeod = servDemandaPO.ListFilesRawIeod();

            if (listaRawsNoProcesadosXMinuto.Count == 0 && listaRawsNoProcesadosIeod.Count == 0)
            {
                return Json("-2");
            }
            else
            {
                path = AppDomain.CurrentDomain.BaseDirectory + ConstantesDpo.RutaReportes;
                fileName = ConstantesDpo.NombreReporteExcelRaws;
                servDemandaPO.ExportarToExcelRawsNoProcesados(listaRawsNoProcesadosXMinuto, listaRawsNoProcesadosIeod, path, fileName);

                return Json(fileName);
            }
        }

        [HttpPost]
        public JsonResult exportarDatosxHora(
            string regFecha, int regHora,
            int selTipo)
        {
            object res;
            try
            {
                int valid = 1;
                string response = this.servDemandaPO.CargaDatosReporteDatosxHora(
                    regFecha, regHora, selTipo);
                res = new { valid, response };
            }
            catch (Exception ex)
            {
                int valid = -1;
                string response = $"Error: {ex.Message}";
                res = new { valid, response };
                return Json(res);
            }

            return Json(res);
        }

        /// <summary>
        /// Función que se encarga de descargar el archivo generado al vuelo en la raiz de la aplicación
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar(string file)
        {
            int formato = 1;
            string fecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesDpo.RutaReportes + file;
            string app = (formato == 1) ? ConstantesDpo.AppExcel : (formato == 2) ? ConstantesDpo.AppPdf : ConstantesDpo.AppWord;
            return File(path, app, fecha + "_" + file);
        }

    }
}