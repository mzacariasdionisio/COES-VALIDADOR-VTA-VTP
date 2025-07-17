using COES.Servicios.Aplicacion.Hidrologia;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace COES.WEBAPI.MedidoresGeneracion.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class LecturasController :ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(LecturasController));
        HidrologiaAppServicio service = new HidrologiaAppServicio();

        /// <summary>
        /// Lista mis lecturas
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetListMeLecturas()
        {
            try
            {
                return Ok(
              this.service.ListMeLecturas()
           );
            }
            catch (Exception ex)
            {
                log.Error("GetListMeLecturas", ex);
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