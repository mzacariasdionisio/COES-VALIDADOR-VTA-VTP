using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.WebAPI.Equipamiento.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace COES.WebAPI.Equipamiento.Controllers
{
    public class TipoAreaController : ApiController
    {
        private EquipamientoAppServicio ServicioEquipamiento = new EquipamientoAppServicio();              
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(TipoAreaController));

        /// <summary>
        /// Método que consulta los tipo de ubicaciones (areas)
        /// </summary>        
        /// <returns></returns>           
        [ResponseType(typeof(List<DatoTipoArea>))]
        public IHttpActionResult Get()
        {
            List<EqTipoareaDTO> listaTipoUbicaciones = ServicioEquipamiento.ListEqTipoareas().OrderBy(ta=>ta.Tareacodi).ToList();
            
            try
            {
                return Ok(
                  (from campos in listaTipoUbicaciones
                   select new
                   {                      
                       TAREACODI = campos.Tareacodi,
                       TAREANOMB = campos.Tareanomb
                   })
                    );
            }
            catch (Exception ex)
            {
                log.Error("GetListarTipoArea", ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error en el lado del servidor"));
                //var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                //{
                //    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                //    StatusCode = HttpStatusCode.InternalServerError
                //};
                //throw new HttpResponseException(response);
            }
        }
    }
}
