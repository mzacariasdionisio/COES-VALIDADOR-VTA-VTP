using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.General;
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
    public class EmpresaController : ApiController
    {
        private GeneralAppServicio servGeneral = new GeneralAppServicio();
               
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(EmpresaController));
                
        /// <summary>
        /// Método que consulta de empresas del SEIN 
        /// </summary>        
        /// <returns></returns>           
        [ResponseType(typeof(List<DatoEmpresa>))]
        public IHttpActionResult Get()
        {

            List<SiEmpresaDTO> dataLectura = this.servGeneral.ObtenerEmpresasCOES();
            try
            {
                return Ok(
                  (from campos in dataLectura
                   select new
                   {
                       EMPRCODI = campos.Emprcodi,
                       EMPRNOMB = campos.Emprnomb
                   })
                    );
            }
            catch (Exception ex)
            {
                log.Error("GetListarEmpresas", ex);
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
