using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net;
using COES.Servicios.Aplicacion.Mediciones;
using System.Web.Http.Description;
using COES.WebAPI.Mediciones.Models;
using COES.Dominio.DTO.Sic;

namespace COES.WebAPI.Mediciones.Controllers
{
    /// <summary>
    /// Servicio que retorna información de Frecuencia.
    /// </summary>
    public class FrecuenciaController : ApiController
    {
        LecturaAppServicio service = new LecturaAppServicio();

         private static readonly ILog log = log4net.LogManager.GetLogger(typeof(FrecuenciaController));
      
        /// <summary>
        /// Método que consulta del registro de frecuencia de la S.E. San Juan 
        /// </summary>
        /// <param name="fecha">Fecha de consulta de la información</param>
        /// <returns></returns>                   
        [ResponseType(typeof(List<Flectura>))]
        public IHttpActionResult Get(DateTime fecha) //List<FrecuenciaGpsDTO>
        {
            //DateTime fecha = DateTime.Today;
            if (DateTime.Today.AddYears(-1) < fecha)
            {
                List<FLecturaDTO> dataLectura = service.GetByCriteriaFLecturas(1, fecha, fecha);
                try
                {
                    return Ok(
                      (from campos in dataLectura
                       select new
                       {
                           Medifecha = campos.Fechahora,
                           H0 = campos.H0,
                           H1 = campos.H1,
                           H2 = campos.H2,
                           H3 = campos.H3,
                           H4 = campos.H4,
                           H5 = campos.H5,
                           H6 = campos.H6,
                           H7 = campos.H7,
                           H8 = campos.H8,
                           H9 = campos.H9,
                           H10 = campos.H10,
                           H11 = campos.H11,
                           H12 = campos.H12,
                           H13 = campos.H13,
                           H14 = campos.H14,
                           H15 = campos.H15,
                           H16 = campos.H16,
                           H17 = campos.H17,
                           H18 = campos.H18,
                           H19 = campos.H19,
                           H20 = campos.H20,
                           H21 = campos.H21,
                           H22 = campos.H22,
                           H23 = campos.H23,
                           H24 = campos.H24,
                           H25 = campos.H25,
                           H26 = campos.H26,
                           H27 = campos.H27,
                           H28 = campos.H28,
                           H29 = campos.H29,
                           H30 = campos.H30,
                           H31 = campos.H31,
                           H32 = campos.H32,
                           H33 = campos.H33,
                           H34 = campos.H34,
                           H35 = campos.H35,
                           H36 = campos.H36,
                           H37 = campos.H37,
                           H38 = campos.H38,
                           H39 = campos.H39,
                           H40 = campos.H40,
                           H41 = campos.H41,
                           H42 = campos.H42,
                           H43 = campos.H43,
                           H44 = campos.H44,
                           H45 = campos.H45,
                           H46 = campos.H46,
                           H47 = campos.H47,
                           H48 = campos.H48,
                           H49 = campos.H49,
                           H50 = campos.H50,
                           H51 = campos.H51,
                           H52 = campos.H52,
                           H53 = campos.H53,
                           H54 = campos.H54,
                           H55 = campos.H55,
                           H56 = campos.H56,
                           H57 = campos.H57,
                           H58 = campos.H58,
                           H59 = campos.H59
                       })
                        );
                }
                catch (Exception ex)
                {
                    log.Error("GetObtenerFrecuencia", ex);
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error en el lado del servidor"));
                }
            }
            else
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "La información solicitada solo del ultimo año"));               
            }            
        }
    }
}
