using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Globalization;
using COES.Servicios.Aplicacion.DPODemanda;
using COES.MVC.Intranet.Areas.Admin.Helper;
using COES.Servicios.Aplicacion.DPODemanda.Helper;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.CortoPlazo;
using log4net;
using COES.MVC.Intranet.Areas.CortoPlazo.Controllers;
using System.Threading;

namespace COES.MVC.Intranet.Areas.DemandaPO.Controllers
{
    public class HiloController : Controller
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
        public ActionResult Hilo()
        {
            return View();
        }

        /// <summary>
        /// Procesa los archivo Raw x Minuto en un día
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
                
                string sProcedimiento = "Manual";
                // PASO 3: TRUNCATE DPO_ESTIMADORRAW_MANUAL 
                if (sProcedimiento.Equals("Manual"))
                {
                    servDemandaPO.TruncateTemporalDpoEstimadorRaw(ConstantesDpo.CargaManualRaw);
                }
                else
                {   // Delete DPO_ESTIMADORRAW_TMP WHERE fecha entre hoyInicio, hoyFin
                    servDemandaPO.DeleteREstimadorRawTemporalByDiaProceso(hoyInicio, hoyFin);
                }

                // PASO 4: Delete DPO_ESTIMADOR_RAW_[sufAnioMes] WHERE fecha entre hoyInicio, hoyFin
                string sufAnioMes = hoy.ToString("yyyyMM"); // Se le asignara a la tabla correspondiente del año mes
                hoyInicio = hoyInicio.AddMinutes(-1); // debe eliminar desde 00:00 [Recordar que las 23:00 le corresponde al bloque final del dia]
                servDemandaPO.DeleteREstimadorRawByDiaProceso(sufAnioMes, hoyInicio, hoyFin.AddMinutes(-60));

                //PASO 5: Insertamos todos los TNA del día 
                for (int i = 0; i < 1440; i += 5)
                {
                    parseFecha = hoy.AddMinutes(i);
                    sResultado = servDemandaPO.EjecutaProcesoLecturaRawPorMinuto(parseFecha, diaProceso, sProcedimiento);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }


            return Json(sResultado);
        }

        /// <summary>
        /// Procesa los archivo Raw x 30 Minutos en un día
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

        /// <summary>
        /// Muestra estados en la barra de estado del proceso x1M
        /// </summary>
        /// <returns>res</returns>
        public JsonResult verificarFaltantesDia(string fecProceso)
        {
            int iResultado = 0;
            DateTime fechaProceso = DateTime.ParseExact(fecProceso, ConstantesDpo.FormatoFecha, CultureInfo.InvariantCulture);
            iResultado = this.servDemandaPO.verificarFaltantesDia(fechaProceso);
            return Json(iResultado);
        }

        public JsonResult completarDia(string fecProceso)
        {
            try
            {
                int iResultado = 0;
                DateTime fechaProceso = DateTime.ParseExact(fecProceso, ConstantesDpo.FormatoFecha, CultureInfo.InvariantCulture);
                //Completa los faltantes de la tabla TMP en caso no se completo la migración
                iResultado = this.servDemandaPO.completarDia(fechaProceso);
                //Consultamos si aun persiste faltantes
                if (this.servDemandaPO.verificarFaltantesDia(fechaProceso) > 0)
                {
                    iResultado = this.servDemandaPO.completarDiaMigrando(fechaProceso);
                }
                return Json(iResultado);
            }
            catch (Exception e)
            {
                return Json(e.StackTrace);
            }
        }

        /// <summary>
        /// Procesa los archivo Raw x Minuto en una Hora
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarAutomaticamentePorHora(string strFecProcessHora)
        {
            string sResultado = "1";

            try
            {
                DateTime parseFecha = DateTime.ParseExact(strFecProcessHora, ConstantesDpo.FormatoFechaMedicionRaw, CultureInfo.InvariantCulture);
                DateTime hora = new DateTime(parseFecha.Year, parseFecha.Month, parseFecha.Day, parseFecha.Hour, 5, 0); // XX:05
                DateTime horaInicio = hora.AddMinutes(-4); //Inicia en el minuto XX:01
                DateTime horaFin = hora.AddMinutes(55); //termina en el minuto (XX+1):00 la hora siguiente
                int diaProceso = hora.Day;
                int iTipo = 1;
                // PASO 1: Eliminamos los archivos dpo_estimadorraw_files TIPO = 1 and fecha entre horaInicio, horaFin
                servDemandaPO.DeleteREstimadorRawFileByDiaProceso(horaInicio, horaFin, iTipo);

                // PASO 2: Delete DPO_ESTIMADOR_RAW_LOG WHERE TIPO = 1 and fecha entre horaInicio, horaFin
                servDemandaPO.DeleteREstimadorRawLogByDiaProceso(horaInicio, horaFin, iTipo);

                // PASO 3: Delete DPO_ESTIMADORRAW_MANUAL WHERE fecha entre horaInicio, horaFin
                servDemandaPO.TruncateTemporalDpoEstimadorRaw(ConstantesDpo.CargaManualRaw);

                // PASO 4: Delete DPO_ESTIMADOR_RAW_[sufAnioMes] WHERE fecha entre horaInicio, horaFin
                string sufAnioMes = hora.ToString("yyyyMM"); // Se le asignara a la tabla correspondiente del año mes
                horaInicio = horaInicio.AddMinutes(-1); // debe eliminar desde XX:00 [Recordar que las 23:00 le corresponde al bloque final del dia]
                servDemandaPO.DeleteREstimadorRawByDiaProceso(sufAnioMes, horaInicio, horaFin.AddMinutes(-60));
                string sProcedimiento = "Manual";
                //PASO 5: Insertamos todos los TNA de la hora
                for (int i = 0; i < 60; i += 5)
                {
                    parseFecha = hora.AddMinutes(i);
                    sResultado = servDemandaPO.EjecutaProcesoLecturaRawPorMinuto(parseFecha, diaProceso, sProcedimiento);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }


            return Json(sResultado);
        }


        /// <summary>
        /// Reconstruye los indices de la tabla dpo_estimadorraw
        /// </summary>
        /// <returns>res</returns>
        public JsonResult updateRAW(string fecProceso)
        {
            string sMensaje = "";
            try
            {
                DateTime fechaProceso = DateTime.ParseExact(fecProceso, ConstantesDpo.FormatoFecha, CultureInfo.InvariantCulture);
                sMensaje = this.servDemandaPO.UpdateRAW(fechaProceso);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
            return Json(sMensaje);
        }

        /// <summary>
        /// Reconstruye los indices de la tabla dpo_estimadorraw_tmp
        /// </summary>
        /// <returns>res</returns>
        public JsonResult updateTMP()
        {
            string sMensaje = "";
            try
            {
                sMensaje = this.servDemandaPO.UpdateTMP();
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
            return Json(sMensaje);
        }

        /// <summary>
        /// Trunca la tabla dpo_estimadorraw_tmp
        /// </summary>
        /// <returns>res</returns>
        public JsonResult DeleteEstimadorRawTemporal(string fecProceso)
        {
            string sMensaje = "";
            try
            {
                //Primer día del mes
                DateTime date = DateTime.ParseExact(fecProceso, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                // Obtener el primer día del mes
                DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1).AddMinutes(1); //2024/02/01 00:01
                // Obtener el último día del mes
                DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddMinutes(-1); //2024/03/01 00:00

                this.servDemandaPO.DeleteREstimadorRawTemporalByDiaProceso(firstDayOfMonth, lastDayOfMonth);

                sMensaje = this.servDemandaPO.DeleteEstimadorRawTemporal();
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
            return Json(sMensaje);
        }

        /// <summary>
        /// Procesa los archivo Raw x Minuto en un día
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarAutomaticamentePorMes(string strFecProcess)
        {
            string sResultado = "1";

            try
            {
                //Primer día del mes
                DateTime dFecha = DateTime.ParseExact(strFecProcess, ConstantesDpo.FormatoFecha, CultureInfo.InvariantCulture); //XX/01/2023
                int iPrimerDia = dFecha.Day; //XX: 1
                //Calculamos el ultimo dia del mes
                int iUltimoDia = new DateTime(dFecha.Year, dFecha.Month, 1).AddMonths(1).AddDays(-1).Day;

                for (int i = iPrimerDia; i <= iUltimoDia; i++)
                {
                    string sFecha = dFecha.ToString("dd/MM/yyyy HH:mm");
                    ProcesarAutomaticamentePorMinuto(sFecha);
                    dFecha = dFecha.AddDays(1);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            return Json(sResultado);
        }

        #region Testeo de carga automatica de TNA

        private Timer timer;

        [HttpPost]
        public ActionResult IniciarTarea()
        {
            // Iniciar el temporizador
            timer = new Timer(EjecutarTarea, null, ObtenerTiempoHastaSiguienteEjecucion(), TimeSpan.FromMinutes(5));
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult DetenerTarea()
        {
            // Detener el temporizador
            timer?.Dispose();
            timer = null;

            return Json(new { success = true });
        }

        private void EjecutarTarea(object state)
        {
            // Ejecutar la función ObtenerDatosRawCada5Mninutos en un nuevo hilo
            Thread thread = new Thread(() =>
            {
                ObtenerDatosRawCada5Mninutos();
            });
            thread.Start();

            // Ejecutar la función ObtenerDatosTNTCada5Mninutos en un nuevo hilo
            //Thread thread2 = new Thread(() =>
            //{
            //    ObtenerDatosTNTCada5Mninutos();
            //});
            //thread2.Start();
        }

        private void ObtenerDatosRawCada5Mninutos()
        {
            try
            {
                // Ejecutamos el servicio: ObtenerDatosRawCada5Mninutos
                Console.WriteLine("Ejecutando ObtenerDatosRawCada5Mninutos en hilo: " + Thread.CurrentThread.ManagedThreadId);
                this.servDemandaPO.ObtenerDatosRawCada5Mninutos();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en ObtenerDatosRawCada5Mninutos: " + ex.Message);
            }
        }

        //private void ObtenerDatosTNTCada5Mninutos()
        //{
        //    try
        //    {
        //        // Ejecutamos el servicio: ObtenerDatosTNTCada5Mninutos
        //        Console.WriteLine("Ejecutando ObtenerDatosTNTCada5Mninutos en hilo: " + Thread.CurrentThread.ManagedThreadId);
        //        this.servDemandaPO.ObtenerDatosTNTAntiguos();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error en ObtenerDatosTNTCada5Mninutos: " + ex.Message);
        //    }
        //}

        private TimeSpan ObtenerTiempoHastaSiguienteEjecucion()
        {
            DateTime now = DateTime.Now;
            int minutosActuales = now.Minute;
            int minutosRedondeados = minutosActuales - (minutosActuales % 5) + 5; // Siguiente múltiplo de 5
            DateTime siguienteEjecucion = now.Date.AddHours(now.Hour).AddMinutes(minutosRedondeados);
            TimeSpan tiempoRestante = siguienteEjecucion - now;
            return tiempoRestante;
        }

        #endregion
    }
}