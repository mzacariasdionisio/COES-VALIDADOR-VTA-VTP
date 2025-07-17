using COES.Servicios.Aplicacion.Hidrologia;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace COES.WebAPI.General.Controllers
{
    public class InformacionController: ApiController
    {

        HidrologiaAppServicio service = new HidrologiaAppServicio();

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(InformacionController));
        /// <summary>
        /// Listar si tipo informacion
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetListSiTipoinformacions()
        {
            try
            {
                return Ok(
              this.service.ListSiTipoinformacions()
           );
            }
            catch (Exception ex)
            {
                log.Error("GetListSiTipoinformacions", ex);
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