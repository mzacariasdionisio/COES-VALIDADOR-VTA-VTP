using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.ReservaFriaNodoEnergetico;
using COES.WebAPI.Reserva.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace COES.WebAPI.Reserva.Controllers
{
    public class ReservaController : ApiController
    {

        ReservaFriaNodoEnergeticoAppServicio service = new ReservaFriaNodoEnergeticoAppServicio();

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ReservaController));

        /// <summary>
        /// Lista la reserva ejecutada diaria
        /// </summary>
        /// <param name="dtFecha">Fecha en la cual se quiere buscar</param>
        /// <returns></returns>
        [ResponseType(typeof(List<CostosVariables>))]
        public IHttpActionResult GetReservaDiariaEjecutada(DateTime dtFecha)
        {
            List<ReservaDTO> lista = this.service.ObtenerReservaDiariaEjecutada(dtFecha);
            try
            {
                return Ok(

                    (from campos in lista
                     select new
                     {
                         URS = campos.URS,
                         Empresa = campos.Empresa,
                         Central = campos.Central,
                         TipoEquipo = campos.TipoEquipo,
                         Equipo = campos.Equipo,
                         Tipo = campos.Tipo,
                         FechaHoraInicio = campos.FechaHoraInicio,
                         FechaHoraFin = campos.FechaHoraFin,
                         ValorReserva = campos.ValorReserva,
                         Hora = campos.Hora
                     })
           );


            }
            catch (Exception ex)
            {
                log.Error("GetReservaDiariaEjecutada", ex);
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
