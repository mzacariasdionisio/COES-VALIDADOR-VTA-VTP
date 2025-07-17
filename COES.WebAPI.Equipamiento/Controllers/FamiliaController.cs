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
    public class FamiliaController : ApiController
    {
        private EquipamientoAppServicio ServicioEquipamiento = new EquipamientoAppServicio();       
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(FamiliaController));
                
        /// <summary>
        /// Método que consulta de los tipos de equipos (Familia)
        /// </summary>
        /// <returns></returns>           
        [ResponseType(typeof(List<DatoFamilia>))]
        public IHttpActionResult Get()
        {
            List<EqFamiliaDTO> listaFamilias = ServicioEquipamiento.ListEqFamilias().OrderBy(f=>f.Famcodi).ToList();
            
            try
            {
                return Ok(
                  (from campos in listaFamilias
                   select new
                   {
                       FAMCODI = campos.Famcodi,
                       FAMABREV = campos.Famabrev,
                       FAMNOMB = campos.Famnomb
                   })
                );
            }
            catch (Exception ex)
            {
                log.Error("GetListarFamilia", ex);
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
