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
using System.Web.Http.Description;

namespace COES.WebAPI.Controllers
{
    public class EventoController : ApiController
    {
        EventoAppServicio service = new EventoAppServicio();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(EventoController));


        // <summary>
        /// ListarTupoEvent
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public IHttpActionResult GetListarTipoEvento()
        {
            try
            {
                return Ok(
              this.service.ListarTipoEvento()
           );
            }
            catch (Exception ex)
            {
                log.Error("GetListarTipoEvento", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);

            }

        }
         
        /// <summary>
        /// Obtener Causa Evento
        /// </summary>
        /// <param name="idTipoEvento"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerCausaEvento(int? idTipoEvento)
        {
            try
            {
                return Ok(
                              this.service.ObtenerCausaEvento(idTipoEvento)
                           );
            }
            catch (Exception ex)
            {
                log.Error("GetObtenerCausaEvento", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }

        }

        /// <summary>
        /// Buscar eventos con restricción de 30 días 
        /// </summary>
        /// <param name="idTipoEvento">0</param>
        /// <param name="fechaInicio">01/01/2019 00:00:00</param>
        /// <param name="fechaFin">02/02/2019 00:00:00</param>
        /// <param name="version">-1</param>
        /// <param name="turno"></param>
        /// <param name="idTipoEmpresa">0</param>
        /// <param name="idEmpresa">0</param>
        /// <param name="idTipoEquipo">0</param>
        /// <param name="indInterrupcion">-1</param>
        /// <param name="campo">EVENINI</param>
        /// <param name="orden">desc</param>
        /// <param name="areaoperativa">-1</param>
        /// <param name="todosaseg">-1</param>
        /// <returns></returns>
        public IHttpActionResult GetBuscarEventos(int? idTipoEvento, DateTime fechaInicio, DateTime fechaFin,
            string version, string turno, int? idTipoEmpresa, int? idEmpresa, int? idTipoEquipo, string indInterrupcion,
            string campo, string orden, string areaoperativa, int todosaseg)
        {
            int nroPage = 1;
            int nroFilas = int.MaxValue;
            TimeSpan tiempoDiferencia = (fechaFin - fechaInicio);


            if (tiempoDiferencia.Days <= 30)
            {
                try
                {
                    return Ok(
                     this.service.BuscarEventos(idTipoEvento, fechaInicio, fechaFin, version, turno, idTipoEmpresa,
                     idEmpresa, idTipoEquipo, indInterrupcion, nroPage, nroFilas, campo, orden, areaoperativa, todosaseg)
           );
                }
                catch (Exception ex)
                {
                    log.Error("GetBuscarEventos", ex);
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
        /// Obtener numero de filas del evento
        /// </summary>
        /// <param name="idTipoEvento"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="version"></param>
        /// <param name="turno"></param>
        /// <param name="idTipoEmpresa"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idTipoEquipo"></param>
        /// <param name="indInterrupcion"></param>
        /// <param name="areaOperativa"></param>
        /// <param name="todosaseg"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerNroFilasEvento(int? idTipoEvento, DateTime fechaInicio, DateTime fechaFin,
            string version, string turno, int? idTipoEmpresa, int? idEmpresa, int? idTipoEquipo, string indInterrupcion,
            string areaOperativa, int todosaseg)
        {
            try
            {
                return Ok(this.service.ObtenerNroFilasEvento(idTipoEvento, fechaInicio,
                          fechaFin, version, turno, idTipoEmpresa, idEmpresa, idTipoEquipo, indInterrupcion, areaOperativa, todosaseg)
                            );
            }
            catch (Exception ex)
            {
                log.Error("GetObtenerNroFilasEvento", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }

        }
        /// <summary>
        /// Obtener Exportar Eventos
        /// </summary>
        /// <param name="idTipoEvento"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="version"></param>
        /// <param name="turno"></param>
        /// <param name="idTipoEmpresa"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idTipoEquipo"></param>
        /// <param name="indInterrupcion"></param>
        /// <param name="indicador"></param>
        /// <param name="areaOperativa"></param>
        /// <returns></returns>
        public IHttpActionResult GetExportarEventos(int? idTipoEvento, DateTime fechaInicio, DateTime fechaFin,
            string version, string turno, int? idTipoEmpresa, int? idEmpresa, int? idTipoEquipo, string indInterrupcion, int indicador, string areaOperativa)
        {
            try
            {
                return Ok(
               this.service.ExportarEventos(idTipoEvento, fechaInicio, fechaFin, version, turno, idTipoEmpresa,
                idEmpresa, idTipoEquipo, indInterrupcion, indicador, areaOperativa)
            );
            }
            catch (Exception ex)
            {
                log.Error("GetExportarEventos", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }

        }
        /// <summary>
        /// Obtener resumen de Evento
        /// </summary>
        /// <param name="idEvento"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerResumenEvento(int idEvento)
        {
            try
            {
                return Ok(
               this.service.ObtenerResumenEvento(idEvento)
            );
            }
            catch (Exception ex)
            {
                log.Error("GetObtenerResumenEvento", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }

        }

        /// <summary>
        /// Obtener Evento
        /// </summary>
        /// <param name="Eventoid"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerEvento(int Eventoid)
        {
            try
            {
                return Ok(
                               this.service.ObtenerEvento(Eventoid)
                            );
            }
            catch (Exception ex )
            {
                log.Error("GetObtenerEvento", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }

        }

        /// <summary>
        /// Obtener informe por evento
        /// </summary>
        /// <param name="Evento">Identificador del evento</param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerInformePorEvento(int Evento)
        {
            try
            {
                return Ok(
                               this.service.ObtenerInformePorEvento(Evento)
                            );
            }
            catch (Exception ex)
            {
                log.Error("GetObtenerInformePorEvento", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }

        }

        /// <summary>
        /// Obtener Area por Empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFamilia"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerAreaPorEmpresa(int? idEmpresa, string idFamilia)
        {
            try
            {
                return Ok(
                               this.service.ObtenerAreaPorEmpresa(idEmpresa, idFamilia)
                            );
            }
            catch (Exception ex)
            {
                log.Error("GetObtenerAreaPorEmpresa", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }

        }
        /// <summary>
        ///   Buscar Equipo Evento
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idArea"></param>
        /// <param name="idFamilia"></param>
        /// <param name="filtro"></param>
        /// <param name="nroPagina"></param>
        /// <param name="nroFilas"></param>
        /// <returns></returns>
        public IHttpActionResult GetBuscarEquipoEvento(int? idEmpresa, int? idArea, string idFamilia, string filtro, int nroPagina, int nroFilas)
        {
            try
            {
                return Ok(
               this.service.BuscarEquipoEvento(idEmpresa, idArea, idFamilia, filtro, nroPagina, nroFilas)
            );
            }
            catch (Exception ex)
            {
                log.Error("GetBuscarEquipoEvento", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }

        }
        /// <summary>
        /// Obtener Nro de filas Busqueda Equipo
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idArea"></param>
        /// <param name="idFamilia"></param>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerNroFilasBusquedaEquipo(int? idEmpresa, int? idArea, string idFamilia, string filtro)
        {
            try
            {
                return Ok(
              this.service.ObtenerNroFilasBusquedaEquipo(idEmpresa, idArea, idFamilia, filtro)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetObtenerNroFilasBusquedaEquipo", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }

        }      

        //}
        /// <summary>
        /// Listar Empresas por tipo
        /// </summary>
        /// <param name="idTipoEmpresa"></param>
        /// <returns></returns>
        public IHttpActionResult GetListarEmpresasPorTipo(int idTipoEmpresa)
        {
            try
            {
                return Ok(
               this.service.ListarEmpresasPorTipo(idTipoEmpresa)
            );
            }
            catch (Exception ex)
            {
                log.Error("GetListarEmpresasPorTipo", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }

        }

          /// <summary>
        /// Lee las horas de operación por fecha
        /// </summary>
        /// <param name="fechaO"></param>
        /// <returns></returns>
        public IHttpActionResult GetHorasOperacion(DateTime fechaO)
        {
            try
            {
                return Ok(
                               this.service.GetByDetalleHO(fechaO)
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
        /// <summary>
        /// Obtener reporte de fallos por fecha
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>

        public IHttpActionResult GetEventoFecha(DateTime fecha)
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