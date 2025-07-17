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
    public class EquipoController : ApiController
    {
        private EquipamientoAppServicio ServicioEquipamiento = new EquipamientoAppServicio();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(EquipoController));
    
        /// <summary>
        /// Método que consulta los equipos electricos del SEIN 
        /// </summary>        
        /// <returns></returns>           
        [ResponseType(typeof(List<DatoEquipo>))]
        public IHttpActionResult Get() 
        {
            List<EqEquipoDTO> listaEquipos = this.ServicioEquipamiento.BasicListEqEquipos();
            try
            {
                return Ok(
                  (from campos in listaEquipos
                   select new
                   {
                       EQUICODI = campos.Equicodi,
                       EQUIABREV = campos.Equiabrev,                       
                       EQUINOMB = campos.Equinomb,
                       AREACODI = campos.Areacodi,
                       EMPRCODI = campos.Emprcodi,
                       FAMCODI = campos.Famcodi,                       
                       EQUIESTADO = campos.Equiestado                       
                   })
                    );
            }
            catch (Exception ex)
            {
                log.Error("GetListarEquipos", ex);
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
