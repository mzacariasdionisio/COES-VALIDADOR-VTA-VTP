// See https://aka.ms/new-console-template for more information
using COES.Job.EjecutadoDatosCada30Minutos.Helper;
using Newtonsoft.Json;
using System.Net.Http.Headers;

internal class Program
{
    public class Ejecutado
    {
        private static async Task Main(string[] args)
        {
            ResultadoProceso result = new ResultadoProceso();
            EventLogger logger = new EventLogger();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["PathServicioHtrabajo"]);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string fecha = DateTime.Now.ToString("dd-MM-yyyy");
                    HttpResponseMessage response = await client.GetAsync("EjecutarCargaHtrabajoEnSicoes/" + fecha);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();

                        result = JsonConvert.DeserializeObject<ResultadoProceso>(data);

                        if (result.EjecutarCargaFTPPronRERResult == 1)
                        {
                            logger.Info("Proceso " + "EjecutarCargaHtrabajoEnSicoes" + " ejecutado correctamente.");
                        }
                    }
                }
            }
            catch (Exception)
            {
                logger.Error("Error al ejecutar método: " + "EjecutarCargaHtrabajoEnSicoes");
            }
        }

    }
}

