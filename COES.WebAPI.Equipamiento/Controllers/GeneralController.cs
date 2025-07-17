using COES.Servicios.Aplicacion.Hidrologia;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace COES.WebAPI.Equipamiento.Controllers
{
    public class GeneralController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(GeneralController));
        HidrologiaAppServicio service = new HidrologiaAppServicio();

        /// <summary>
        /// Listar familias 
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetListarFamilia()
        {
            try
            {
                return Ok(
              this.service.ListarFamilia()
           );
            }
            catch (Exception ex)
            {
                log.Error("GetListarFamilia", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }
        }


    }
}