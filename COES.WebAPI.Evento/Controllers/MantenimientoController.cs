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
    public class MantenimientoController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(EventoController));
        EventoAppServicio service = new EventoAppServicio();
        /// <summary>
        /// Permite listar los tipos de eventos
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// Buscar mantenimientos
        /// </summary>
        /// <param name="idsTipoMantenimiento">Valor de prueba 1</param>
        /// <param name="fechaInicio">Valor de prueba 12/01/2019</param>
        /// <param name="fechaFin">Valor de prueba 12/10/2019</param>
        /// <param name="indispo">Valor de prueba -1</param>
        /// <param name="tiposEmpresa">Valor de prueba 1</param>
        /// <param name="empresas">Valor de prueba -1</param>
        /// <param name="idsTipoEquipo">Valor de prueba 1</param>
        /// <param name="indInterrupcion">Valor de prueba -1</param>
        /// <param name="idstipoMantto">Valor de prueba 1</param>
        /// <returns></returns>
        public IHttpActionResult GetBuscarMantenimientos(string idsTipoMantenimiento, DateTime fechaInicio, DateTime fechaFin, string indispo,
            string tiposEmpresa, string empresas, string idsTipoEquipo, string indInterrupcion, string idstipoMantto)
        {
            TimeSpan tiempoDiferencia = (fechaFin - fechaInicio);
            int nroPagina = 1;
            int nroFilas = int.MaxValue;

            if (tiempoDiferencia.Days <= 7)
            {
                try
                {

                    return Ok(this.service.BuscarMantenimientos(idsTipoMantenimiento, fechaInicio, fechaFin,
                      indispo, tiposEmpresa, empresas, idsTipoEquipo, indInterrupcion, idstipoMantto, nroPagina, nroFilas)
               );

                }
                catch (Exception ex)
                {
                    log.Error("GetBuscarMantenimientos", ex);
                    var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                        StatusCode = HttpStatusCode.InternalServerError
                    };
                    throw new HttpResponseException(response);
                }
            }
            else
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }


        }

        /// <summary>
        /// Obtener numero de Filas Mantenimiento 
        /// </summary>
        /// <param name="TipoMantenimientoids"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="indispo"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="idsTipoEquipo"></param>
        /// <param name="indInterrupcion"></param>
        /// <param name="idstipoMantto"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerNroFilasMantenimiento(string TipoMantenimientoids, DateTime fechaInicio, DateTime fechaFin, string indispo,
             string tiposEmpresa, string empresas, string idsTipoEquipo, string indInterrupcion, string idstipoMantto)
        {
            try
            {
                return Ok(
              this.service.ObtenerNroFilasMantenimiento(TipoMantenimientoids, fechaInicio,
               fechaFin, indispo, tiposEmpresa, empresas, idsTipoEquipo, indInterrupcion, idstipoMantto)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetObtenerNroFilasMantenimiento", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }

        }
        /// <summary>
        /// Obtener Mantoo Equipo Clase Fecha 
        /// </summary>
        /// <param name="Equipoids"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="evenClase"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerManttoEquipoClaseFecha(string Equipoids, string fechaInicio, string fechaFin, int evenClase)
        {
            try
            {
                return Ok(
              this.service.ObtenerManttoEquipoClaseFecha(Equipoids, fechaInicio, fechaFin, evenClase)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetObtenerManttoEquipoClaseFecha", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }

        }
        /// <summary>
        /// Obtener Mantto Equipo sub causa Clase Fecha
        /// </summary>
        /// <param name="idsEquipo"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="evenClase"></param>
        /// <param name="subcausaCodi"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerManttoEquipoSubcausaClaseFecha(string idsEquipo, string fechaInicio, string fechaFin, int evenClase, int subcausaCodi)
        {
            try
            {
                return Ok(
                               this.service.ObtenerManttoEquipoSubcausaClaseFecha(idsEquipo, fechaInicio, fechaFin, evenClase, subcausaCodi)
                            );
            }
            catch (Exception ex)
            {
                log.Error("GetObtenerManttoEquipoSubcausaClaseFecha", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }

        }

        /// <summary>
        /// Obtener Mantto Equipo Padre Clase Fecha
        /// </summary>
        /// <param name="idsEquipo"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="evenClase"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerManttoEquipoPadreClaseFecha(string idsEquipo, string fechaInicio, string fechaFin, int evenClase)
        {
            try
            {
                return Ok(
               this.service.ListarFamilias()
            );
            }
            catch (Exception ex)
            {
                log.Error("GetObtenerManttoEquipoPadreClaseFecha", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }

        }
        /// <summary>
        /// Obtener Mantenimientos programados
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerMantenimientosProgramados(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                return Ok(
                               this.service.ObtenerMantenimientosProgramados(fechaInicio, fechaFin)
                            );
            }
            catch (Exception ex)
            {
                log.Error("GetObtenerMantenimientosProgramados", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }

        }
        /// <summary>
        /// Obtener Mantenimiento PRogramados Movil
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerMantenimientosProgramadosMovil(DateTime fechaInicio, DateTime fechaFin, int tipo)
        {
            try
            {
                return Ok(
                               this.service.ObtenerMantenimientosProgramadosMovil(fechaInicio, fechaFin, tipo)
                            );
            }
            catch (Exception ex)
            {
                log.Error("GetObtenerMantenimientosProgramadosMovil", ex);
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
