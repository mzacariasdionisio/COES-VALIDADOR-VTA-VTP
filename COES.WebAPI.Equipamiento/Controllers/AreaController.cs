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
    public class AreaController : ApiController
    {        
        private EquipamientoAppServicio ServicioEquipamiento = new EquipamientoAppServicio();
        
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(AreaController));
                
        /// <summary>
        /// Método que lista la ubicaciones (areas)
        /// </summary>        
        /// <returns>Lista de ubicaciones</returns>           
        [ResponseType(typeof(List<DatoArea>))]
        public IHttpActionResult Get()
        {
            List<EqAreaDTO> listaUbicaciones = ServicioEquipamiento.ListaTodasAreasActivas().Where(a => a.Areacodi > 0).ToList();
           
            try
            {
                return Ok(
                  (from campos in listaUbicaciones
                   select new
                   {
                       AREACODI = campos.Areacodi,
                       AREANOMB = campos.Areanomb,
                       TAREACODI= campos.Tareacodi
                   })
                    );
            }
            catch (Exception ex)
            {
                log.Error("GetListarAreas", ex);
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
