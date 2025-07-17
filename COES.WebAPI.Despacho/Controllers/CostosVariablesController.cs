using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.WebAPI.Mediciones.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace COES.WebAPI.Despacho.Controllers
{
    public class CostosVariablesController : ApiController
    {
        DespachoAppServicio service = new DespachoAppServicio();

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(CostosVariablesController));

        /// <summary>
        /// Lista las actualizaciones de costos
        /// </summary>
        /// <param name="dtFechaInicio">Fecha inicio de la busqueda</param>
        /// <param name="dtFechaFin">Fecha fin de la busqueda</param>
        /// <returns></returns>
        [ResponseType(typeof(List<CostosVariablesDTO>))]
        public IHttpActionResult GetActualizacionesCostos(DateTime dtFechaInicio, DateTime dtFechaFin)
        {
            List<ActualizacionCVDTO> lista = this.service.ObtenerActualizacionesCostos(dtFechaInicio, dtFechaFin);
            try
            {
                return Ok(

                    (from campos in lista
                     select new
                     {
                         fecha = campos.Fecha,
                         tipo = campos.Tipo,
                         nombre = campos.Nombre,
                         detalle = campos.Detalle,
                         codigo = campos.Codigo
                     })
              
           );
            }
            catch (Exception ex)
            {
                log.Error("GetActualizacionesCostos", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// Lista las actualizaciones de costos variables
        /// </summary>
        /// <param name="iCodActualizacion">id de la actualización</param>
        /// <returns></returns>
        [ResponseType(typeof(List<CostosVariables>))]
        public IHttpActionResult GetCostosVariablesPorActualizacion(int iCodActualizacion)
        {
            List<CostoVariableDTO> lista = this.service.ObtenerCostosVariablesPorActualizacion(iCodActualizacion);
            try
            {
                return Ok(

                    (from campos in lista
                     select new
                     {
                         MODO_OPERACION = campos.MODO_OPERACION,
                         ESCENARIO = campos.ESCENARIO,
                         PE = campos.PE,
                         EFICBTUKWH = campos.EFICBTUKWH,
                         EFICTERM = campos.EFICTERM,
                         CCOMB = campos.CCOMB,
                         CVC = campos.CVC,
                         CVNC = campos.CVNC
                     })
           );
            }
            catch (Exception ex)
            {
                log.Error("GetActualizacionesCostos", ex);
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
