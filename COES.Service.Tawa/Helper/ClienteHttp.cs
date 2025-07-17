using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace COES.Service.Tawa.Helper
{
    public class ClienteHttp
    {
        public async Task<int> EjecutarServicioTawa()
        {
            ResultadoProceso result = new ResultadoProceso();
            EventLogger logger = new EventLogger();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["PathServicioTawa"]);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync("grabarserviciotawa");
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();

                        result = JsonConvert.DeserializeObject<ResultadoProceso>(data);

                        if (result.EjecutarServicioTawa == 1)
                        {
                            logger.Info("Proceso " + "EjecutarServicioTawa" + " ejecutado correctamente.");
                        }
                        else if (result.EjecutarServicioTawa == 2)
                        {
                            logger.Info("Proceso EjecutarServicioTawa no encontró archivos en el servidor.");
                        }
                        else logger.Error("Error al ejecutar método: EjecutarServicioTawa - Result " + result.EjecutarServicioTawa);
                    }
                }
            }
            catch (Exception)
            {
                logger.Error("Error al ejecutar método: " + "EjecutarServicioTawa");
                return -1;
            }

            return result.EjecutarServicioTawa;
        }


    }
}
