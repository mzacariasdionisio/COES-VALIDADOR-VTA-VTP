using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace COES.Service.Proceso.Helper
{
    public class ClienteHttp
    {
        public async Task<List<SiProcesoDTO>> ObtenerProcesos()
        {
            List<SiProcesoDTO> procesos = new List<SiProcesoDTO>();
            int nroBloque = int.Parse(ConfigurationManager.AppSettings[Constantes.BloqueProceso]);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[Constantes.PathServicioListaProceso]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                HttpResponseMessage response = await client.GetAsync("ObtenerTareasProgramadasXBloque/?bloque=" + nroBloque);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    procesos = JsonConvert.DeserializeObject<List<SiProcesoDTO>>(data);                  
                }                
            }
            return procesos;
        }

        public async Task<ResultadoProceso> EjecutarProceso(int idProceso, string method, EventLogger logger)
        {
            string horaInicioEjecucion = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            string horaFinEjecucion = string.Empty;
            ResultadoProceso result = new ResultadoProceso();
            try
            {               
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[Constantes.PathServiciosProceso]);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync(method + "/?prcscodi=" + idProceso);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<ResultadoProceso>(data);

                        horaFinEjecucion = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                        if (result.Resultado == Constantes.EstadoExitoso)
                        {                            
                            logger.Info("Proceso " + method + " ejecutado correctamente. Inicio: " + horaInicioEjecucion + ", Fin: " + horaFinEjecucion);
                        }
                        else
                        {
                            logger.Error("Proceso " + method + " con errores. Inicio: " + horaInicioEjecucion + 
                                ", Fin: " + horaFinEjecucion  + ". Detalle error: " + result.Mensaje);
                        }
                    }
                }
            }
            catch(Exception){
                horaFinEjecucion = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                logger.Error("Error al ejecutar método: " + method + ". Inicio: " + horaInicioEjecucion + ", Fin: " + horaFinEjecucion);
            }

            return result;
        }


    }
}
