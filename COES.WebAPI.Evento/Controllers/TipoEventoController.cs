using COES.Dominio.DTO.Sic;

using COES.Servicios.Aplicacion.Eventos;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace COES.WebAPI.Controllers
{
    public class TipoEventoController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(EventoController));
        EventoAppServicio service = new EventoAppServicio();
        /// <summary>
        /// Permite listar los tipos de eventos
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetObtenerTipoEvento(DateTime fecha)
        {
            try
            {
                return Ok(
                               this.service.ObtenerAnalisisFallaCompleto(fecha)
                            );
            }
            catch (Exception ex)
            {
                log.Error("GetTipoEvento", ex);
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
