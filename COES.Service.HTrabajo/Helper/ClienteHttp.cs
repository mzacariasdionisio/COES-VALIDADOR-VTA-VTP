using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace COES.Service.HTrabajo.Helper
{
    public class ClienteHttp
    {
        public async Task<int> EjecutarProceso()
        {
            ResultadoProceso result = new ResultadoProceso();
            EventLogger logger = new EventLogger();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["PathServicioHtrabajo"]);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync("EjecutarCargaFTPPronRER");
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();

                        result = JsonConvert.DeserializeObject<ResultadoProceso>(data);

                        if (result.EjecutarCargaFTPPronRERResult == 1)
                        {
                            logger.Info("Proceso " + "EjecutarCargaFTPPronRER" + " ejecutado correctamente.");
                        }
                    }
                }
            }
            catch (Exception)
            {
                logger.Error("Error al ejecutar método: " + "EjecutarCargaFTPPronRER");
                return -1;
            }

            return result.EjecutarCargaFTPPronRERResult;
        }


    }
}
