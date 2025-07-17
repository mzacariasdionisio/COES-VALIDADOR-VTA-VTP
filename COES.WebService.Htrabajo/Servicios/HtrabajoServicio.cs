using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Despacho;
using COES.Servicios.Aplicacion.Despacho.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Migraciones;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using COES.WebService.Htrabajo.Contratos;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using static COES.Servicios.Aplicacion.Migraciones.Helper.UtilCdispatch;

namespace COES.WebService.Htrabajo.Servicios
{
    /// <summary>
    /// Implementa los contratos de los servicios
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class HtrabajoServicio : IHtrabajoServicio
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(HtrabajoServicio));

        public HtrabajoServicio()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Funcion que hace llamado a la ejecución automática cada media hora
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="hMediaHora"></param>
        /// <returns></returns>
        public async Task<int> SubirArchivoFTPPronosticoRER(string fecha, string hMediaHora)
        {
            try
            {
                HTrabajoAppServicio htrabajoServicio = new HTrabajoAppServicio();

                //obtener hora a procesar
                var strFecha = fecha.Split('-').Select(x => Convert.ToInt32(x)).ToArray();
                DateTime fechaHora = new DateTime(strFecha[2], strFecha[1], strFecha[0], 0, 0, 0);
                int h = Convert.ToInt32(hMediaHora);
                DateTime fechaMediaHora = fechaHora.AddMinutes(h * 30);

                //Url del servidor web que descarga archivo onedrive y sube a servidor sftp
                string urlClienteCore = ConfigurationManager.AppSettings[ConstantesDespacho.KeyHtrabajoRERUrlClienteCore];

                //variables
                string pathArchivosClienteCore = ConfigurationManager.AppSettings[ConstantesDespacho.KeyHtrabajoRERPathArchivosClienteCore];
                string pathArchivosTemporal = ConfigurationManager.AppSettings[ConstantesDespacho.KeyHtrabajoRERPathArchivosTemporal];

                EliminarArchivoTemporal(pathArchivosTemporal, "Htrabajo_");
                EliminarArchivoTemporal(pathArchivosTemporal, "Measurement_");

                string nombreArchivoHTrabajo = string.Format("Htrabajo_generación_{0}.xlsm", fechaHora.ToString(ConstantesAppServicio.FormatoFechaYMD2));
                string nombreArchivoCSV = string.Format("Measurements_{0}.csv", fechaMediaHora.ToString(ConstantesDespacho.FormatoFechaHoraCsv));

                //Ejecución del proceso
                List<ErrorHtrabajo> listaerror = new List<ErrorHtrabajo>();
                var tipoError = 0;
                var nombreArchivoObs = "";

                using (var client = new HttpClient())
                {
                    //1. Descargar Htrabajo de onedrive
                    try
                    {
                        client.BaseAddress = new Uri(urlClienteCore);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage resDescarga = await client.GetAsync(string.Format("Home/1/{0}", nombreArchivoHTrabajo));

                        if (resDescarga.IsSuccessStatusCode)
                        {
                            var response = resDescarga.Content.ReadAsStringAsync().Result;

                            int resultado = JsonConvert.DeserializeObject<int>(response);

                            //descarga exitosa del archivo Onedrive a 
                            if (resultado == 1)
                            {
                                //eliminar archivo existente htrabajo
                                FileServer.DescargarCopia("", pathArchivosTemporal, nombreArchivoHTrabajo, pathArchivosClienteCore, false);
                            }
                            else
                            {
                                tipoError = 1; //Error archivo en el Onedrive
                                nombreArchivoObs = nombreArchivoHTrabajo;
                                listaerror.Add(new ErrorHtrabajo { Tipo = "ERROR", Descripcion = "No se pudo descargar archivo Onedrive." });
                            }
                        }
                        else
                        {
                            log.Error("Descargar Htrabajo de onedrive" + "\n" + resDescarga.ReasonPhrase + "\n");

                            tipoError = 1; //Error archivo en el Onedrive
                            nombreArchivoObs = nombreArchivoHTrabajo;
                            listaerror.Add(new ErrorHtrabajo { Tipo = "ERROR", Descripcion = "No se pudo conectar con el servidor COES que realiza la descarga del archivo Onedrive." });
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Descargar Htrabajo de onedrive", ex);

                        tipoError = 1; //Error archivo en el Onedrive
                        nombreArchivoObs = nombreArchivoHTrabajo;
                        listaerror.Add(new ErrorHtrabajo { Tipo = "ERROR", Descripcion = "No se pudo descargar archivo Onedrive." });
                    }

                    if (tipoError == 0)
                    {
                        //2. Generar archivo csv
                        try
                        {
                            decimal umbralIni = Convert.ToDecimal(ConfigurationManager.AppSettings[ConstantesDespacho.KeyHtrabajoRERUmbralIni]);
                            decimal umbralFin = Convert.ToDecimal(ConfigurationManager.AppSettings[ConstantesDespacho.KeyHtrabajoRERUmbralFin]);

                            htrabajoServicio.ProcesarArchivoGeneracionRER(pathArchivosTemporal, nombreArchivoHTrabajo, nombreArchivoCSV, fechaHora, h, umbralIni, umbralFin, out List<ErrorHtrabajo> listaObs);
                            listaerror.AddRange(listaObs);

                            //Copiar archivo temporal al fileserver del ClienteCore
                            FileServer.CopiarFileAlterFinal(pathArchivosTemporal, "", nombreArchivoCSV, pathArchivosClienteCore);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Generar archivo csv", ex);

                            tipoError = 4; // Otros errores en ejecución
                            nombreArchivoObs = nombreArchivoCSV;
                            listaerror.Add(new ErrorHtrabajo { Tipo = "ERROR", Descripcion = "No se pudo generar el archivo CSV." });
                        }

                        if (tipoError == 0)
                        {
                            //3. Subir archivo csv a ftp
                            try
                            {
                                HttpResponseMessage resSubida = await client.GetAsync(string.Format("Home/2/{0}", nombreArchivoCSV));

                                if (resSubida.IsSuccessStatusCode)
                                {
                                    var response = resSubida.Content.ReadAsStringAsync().Result;

                                    int resultado = JsonConvert.DeserializeObject<int>(response);

                                    //subida exitosa a SFTP
                                    if (resultado == 1)
                                    {
                                        //proceso correcto, eliminar los temporales
                                        EliminarArchivoTemporal(pathArchivosTemporal, "Htrabajo_");
                                        EliminarArchivoTemporal(pathArchivosTemporal, "Measurement_");
                                    }
                                    else
                                    {
                                        tipoError = 3; //Error archivo FTP
                                        nombreArchivoObs = nombreArchivoCSV;
                                        listaerror.Add(new ErrorHtrabajo { Tipo = "ERROR", Descripcion = "No se pudo cargar el archivo al servidor FTP / SFTP." });
                                    }
                                }
                                else
                                {
                                    log.Error("Subir archivo csv a ftp" + "\n" + resSubida.ReasonPhrase + "\n");

                                    tipoError = 3; //Error archivo FTP
                                    nombreArchivoObs = nombreArchivoCSV;
                                    listaerror.Add(new ErrorHtrabajo { Tipo = "ERROR", Descripcion = "No se pudo conectar con el servidor COES que realiza la subida del archivo FTP / SFTP." });
                                }

                            }
                            catch (Exception ex)
                            {
                                log.Error("Subir archivo csv a ftp", ex);

                                tipoError = 3; // Error al cargar archivo al servidor FTP / SFPT
                                listaerror.Add(new ErrorHtrabajo { Tipo = "ERROR", Descripcion = "Error al cargar archivo al servidor FTP / SFPT." });
                            }
                        }
                    }
                }

                // ENVIAR NOTIFICACIONES DE RESULTADO
                if (!listaerror.Any(x => x.Tipo == "ERROR"))
                {
                    //ENVIAR NOTIFICACIONES DE RESULTADO EXITOSO
                    if (listaerror.Any(x => x.Tipo == "ALERTA"))
                    {
                        tipoError = 2; // exito con alerta
                        htrabajoServicio.EnviarNotificacionEnvio(tipoError, listaerror, fechaMediaHora);
                    }
                    else
                    {
                        tipoError = 0; // exito sin alertas
                        htrabajoServicio.EnviarNotificacionEnvio(tipoError, null, fechaMediaHora);
                    }

                    return 1;
                }
                else
                {
                    //NOTIFICACIONES DE ERROR
                    htrabajoServicio.EnviarNotificacionEnvio(tipoError, null, fechaMediaHora, nombreArchivoObs);

                    return 0;
                }
            }
            catch (Exception ex)
            {
                log.Error("SubirArchivoFTPPronosticoRER", ex);

                return -1;
            }
        }

        public async Task<int> EjecutarCargaFTPPronRER()
        {
            log.Info(ConstantesDespacho.PrcsmetodoEjecutarCargaFTPPronRER);
            try
            {
                (new HTrabajoAppServicio()).ObtenerMediaHoraAProcesar(DateTime.Now, out string fechaRER, out int hRER);
                await Task.Factory.StartNew(() => CallWebServiceHtrabajo(fechaRER, hRER));
                
                return 1;

            }
            catch (Exception ex)
            {
                log.Error("PrcsmetodoEjecutarCargaFTPPronRER", ex);
                return 0;
            }
        }

        private async Task<int> CallWebServiceHtrabajo(string fecha, int hMediaHora)
        {
            //obtener hora a procesar
            var strFecha = fecha.Split('-').Select(x => Convert.ToInt32(x)).ToArray();
            DateTime fechaHora = new DateTime(strFecha[2], strFecha[1], strFecha[0], 0, 0, 0);
            int h = Convert.ToInt32(hMediaHora);
            DateTime fechaMediaHora = fechaHora.AddMinutes(h * 30);
            log.Info("CallWebServiceHtrabajo: Ejecución de tarea programada con fecha => " + fecha + " y mediahora => " + hMediaHora);

            try
            {
                string baseUrl = ConfigurationManager.AppSettings[ConstantesDespacho.KeyHtrabajoRERUrlWS];

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage resProceso = await client.GetAsync(string.Format("HtrabajoServicio.svc/SubirArchivoFTPPronosticoRER/{0}/{1}", fecha, hMediaHora));

                    if (resProceso.IsSuccessStatusCode)
                    {
                        var response = resProceso.Content.ReadAsStringAsync().Result;
                        log.Info("CallWebServiceHtrabajo: Respuesta del servicio (" + string.Format(baseUrl + "HtrabajoServicio.svc/SubirArchivoFTPPronosticoRER/{0}/{1}", fecha, hMediaHora) + ") => " + response);

                        return JsonConvert.DeserializeObject<int>(response);
                    }
                    else
                    {
                        log.Error("Error al conectarse al Servidor web COES. " + "\n" + resProceso.ReasonPhrase + "\n");
                        (new HTrabajoAppServicio()).EnviarNotificacionEnvio(5, null, fechaMediaHora);
                        return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("CallWebServiceHtrabajo - ERROR - " + fecha + " - " + hMediaHora, ex);
                (new HTrabajoAppServicio()).EnviarNotificacionEnvio(5, null, fechaMediaHora);
                return -1;
            }
        }

        private static void EliminarArchivoTemporal(string carpetaTemporal, string prefijoArchivo)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(carpetaTemporal);
            if (directoryInfo.Exists)
            {
                foreach (FileInfo file in directoryInfo.GetFiles().OrderBy(x => x.Name))
                {
                    if (file.Name.ToLower().StartsWith(prefijoArchivo.ToLower()))
                        File.Delete(file.FullName);
                }
            }
        }

        private async Task<int> CargarHtrabajoEnSicoes(string fecha)
        {
            try
            {
                HTrabajoAppServicio htrabajoServicio = new HTrabajoAppServicio();

                //obtener hora a procesar
                var strFecha = fecha.Split('-').Select(x => Convert.ToInt32(x)).ToArray();
                DateTime fechaHora = new DateTime(strFecha[2], strFecha[1], strFecha[0], 0, 0, 0);

                //Url del servidor web que descarga archivo onedrive y sube a servidor sftp
                string urlClienteCore = ConfigurationManager.AppSettings[ConstantesDespacho.KeyHtrabajoRERUrlClienteCore];

                //variables
                string pathArchivosFileServer = ConfigurationManager.AppSettings[ConstantesDespacho.KeyHtrabajoRERPathArchivosClienteCore]; // Pathfileserver
                string pathArchivosTemporal = ConfigurationManager.AppSettings[ConstantesDespacho.KeyHtrabajoRERPathArchivosTemporal]; //pathtemporal

                EliminarArchivoTemporal(pathArchivosTemporal, "CDispatch_");

                string nombreArchivoHTrabajo = string.Format("Htrabajo_generación_{0}.xlsm", fechaHora.ToString(ConstantesAppServicio.FormatoFechaYMD2));
                string nombreArchivoRename = nombreArchivoHTrabajo.Replace("Htrabajo", "CDispatch");
                //Ejecución del proceso
                List<ErrorHtrabajo> listaerror = new List<ErrorHtrabajo>();
                var tipoError = 0;
                var nombreArchivoObs = "";

                using (var client = new HttpClient())
                {
                    //1. Descargar Htrabajo de onedrive
                    try
                    {
                        client.BaseAddress = new Uri(urlClienteCore);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage resDescarga = await client.GetAsync(string.Format("Home/3/{0}", nombreArchivoHTrabajo));

                        if (resDescarga.IsSuccessStatusCode)
                        {
                            var response = resDescarga.Content.ReadAsStringAsync().Result;

                            int resultado = JsonConvert.DeserializeObject<int>(response);

                            //descarga exitosa del archivo Onedrive a 
                            if (resultado == 1)
                            {
                                //copiar archivo htrabajo al fileserver
                                FileServer.CopiarFileAlterFinal(pathArchivosTemporal, "", nombreArchivoRename, pathArchivosFileServer);
                            }
                            else
                            {
                                tipoError = 1; //Error archivo en el Onedrive
                                nombreArchivoObs = nombreArchivoHTrabajo;
                                listaerror.Add(new ErrorHtrabajo { Tipo = "ERROR", Descripcion = "No se pudo descargar archivo Onedrive." });
                            }
                        }
                        else
                        {
                            log.Error("Descargar Htrabajo de onedrive" + "\n" + resDescarga.ReasonPhrase + "\n");

                            tipoError = 1; //Error archivo en el Onedrive
                            nombreArchivoObs = nombreArchivoHTrabajo;
                            listaerror.Add(new ErrorHtrabajo { Tipo = "ERROR", Descripcion = "No se pudo conectar con el servidor COES que realiza la descarga del archivo Onedrive." });
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Descargar Htrabajo de onedrive", ex);

                        tipoError = 1; //Error archivo en el Onedrive
                        nombreArchivoObs = nombreArchivoHTrabajo;
                        listaerror.Add(new ErrorHtrabajo { Tipo = "ERROR", Descripcion = "No se pudo descargar archivo Onedrive." });
                    }

                    if (tipoError == 0)
                    {
                        //variables flag carga hojas
                        HtFiltro flagCarga = new HtFiltro();
                        flagCarga.FlagCDispatchCargaActiva = ConfigurationManager.AppSettings["CDispatchFlagCargaActiva"].ToString() == "S" ? true : false;
                        flagCarga.FlagCDispatchCargaReactiva = ConfigurationManager.AppSettings["CDispatchFlagCargaReactiva"].ToString() == "S" ? true : false;
                        flagCarga.FlagCDispatchCargaHidrologia = ConfigurationManager.AppSettings["CDispatchFlagCargaHidrologia"].ToString() == "S" ? true : false;
                        flagCarga.FlagCDispatchCargaReprograma = ConfigurationManager.AppSettings["CDispatchFlagCargaReprograma"].ToString() == "S" ? true : false;

                        //2. Grabar Cdispach
                        // lectocodi = 6 (DESPACHO EJECUTADO)
                        try
                        {
                            htrabajoServicio.GuardarCdispatch(ConstantesDespacho.LectcodiEjecutadoHisto, flagCarga, pathArchivosTemporal, nombreArchivoRename, out List<HtError> listaErrorValidacion);

                            if (listaErrorValidacion.Any())
                            {

                                foreach (var error in listaErrorValidacion)
                                {
                                    listaerror.Add(new ErrorHtrabajo { Tipo = "ALERTA", Ptomedicion = error.Ptomedicion, Posicion= error.Posicion, Descripcion = error.Descripcion});
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            log.Error("Guardar Cdispatch", ex);

                            tipoError = 3; // error al guardar Cdispatch
                            nombreArchivoObs = nombreArchivoRename;
                            listaerror.Add(new ErrorHtrabajo { Tipo = "ERROR", Descripcion = "No se pudo guardar el archivo Cdispatch." });
                        }

                        // lectocodi = 5 (DESPACHO REPROGRAMA)
                        try
                        {
                            htrabajoServicio.GuardarCdispatch(ConstantesDespacho.LectcodiReprogDiario, flagCarga, pathArchivosTemporal, nombreArchivoRename, out List<HtError> listaErrorValidacion);

                            if (listaErrorValidacion.Any())
                            {
                                foreach (var error in listaErrorValidacion)
                                {
                                    listaerror.Add(new ErrorHtrabajo { Tipo = "ALERTA", Ptomedicion = error.Ptomedicion, Posicion = error.Posicion, Descripcion = error.Descripcion });
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            log.Error("Guardar Cdispatch", ex);

                            tipoError = 3; // error al guardar Cdispatch
                            nombreArchivoObs = nombreArchivoRename;
                            listaerror.Add(new ErrorHtrabajo { Tipo = "ERROR", Descripcion = "No se pudo guardar el archivo Cdispatch." });
                        }
                    }
                }

                //3. Enviar notificaciones del resultado
                if (!listaerror.Any(x => x.Tipo == "ERROR"))
                {
                    //ENVIAR NOTIFICACIONES DE RESULTADO EXITOSO
                    if (listaerror.Any(x => x.Tipo == "ALERTA"))
                    {
                        tipoError = 2; // exito con alerta
                        htrabajoServicio.EnviarNotificacionCdisptch(1, listaerror, fechaHora);
                    }
                    else
                    {
                        tipoError = 0; // exito sin alertas
                        htrabajoServicio.EnviarNotificacionCdisptch(0, null, fechaHora);
                    }

                    return 1;
                }
                else
                {
                    //NOTIFICACIONES DE ERROR
                    htrabajoServicio.EnviarNotificacionCdisptch(2, listaerror, fechaHora);

                    return 0;
                }
            }
            catch (Exception ex)
            {
                log.Error("CargarHtrabajoEnSicoes", ex);

                return -1;
            }
        }

        /// <summary>
        /// Ejecución de proceso automático cada 30minutos de Cargar Htrabajo en Sicoes segun la fecha actual
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public async Task<int> EjecutarCargaHtrabajoEnSicoes(string fecha)
        {
            log.Info(ConstantesDespacho.PrcsmetodoEjecutarCargaHtrabajoSicoes);
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(ConstantesDespacho.PrcscodiEjecutarCargaHtrabajo);
            try
            {
                if(fecha == DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaWS))
                {
                    (new HTrabajoAppServicio()).ObtenerFechaAProcesar(DateTime.Now, out string fechaProceso);
                    fecha = fechaProceso;
                }

                var respuesta = await Task.Factory.StartNew(() => CargarHtrabajoEnSicoes(fecha));

                if(respuesta.Result != -1)
                {
                    ActualizarFinLog(prcslgcodi, DateTime.Now, "E", ""); //Actualizar log exitoso
                    return 1;
                }
                else
                {
                    ActualizarFinLog(prcslgcodi, DateTime.Now, "F", "Error al CargarHtrabajoEnSicoes");
                    return 0;
                }

            }
            catch (Exception ex)
            {
                log.Error("PrcsmetodoEjecutarCargaHtrabajoEnSicoes", ex);
                //Actualizar log fallido
                ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
                return 0;
            }
        }

        #region Utilidad
        public int AgregarInicioLog(int prcscodi)
        {
            SiProcesoLogDTO log = new SiProcesoLogDTO();
            log.Prcscodi = prcscodi;
            log.Prcslgfecha = DateTime.Today;
            log.Prcslginicio = DateTime.Now;
            log.Prcslgestado = "P";

            int logcodi = FactorySic.GetSiProcesoLogRepository().Save(log);

            return logcodi;
        }

        public void ActualizarFinLog(int prcslgcodi, DateTime fechaFin, string estado, string mensaje)
        {
            SiProcesoLogDTO log = FactorySic.GetSiProcesoLogRepository().GetById(prcslgcodi);

            if (log != null)
            {
                log.Prcslgfin = DateTime.Now;
                log.Prcslgestado = estado;

                FactorySic.GetSiProcesoLogRepository().Update(log);
            }
        }

        #endregion
    }
}
