using COES.Servicios.Aplicacion.Hidrologia;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace COES.WebAPI.HIdrología.Controllers
{
    public class HidrologiaController : ApiController
    {
        HidrologiaAppServicio service = new HidrologiaAppServicio();

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(HidrologiaController));


        //public EventoController()
        //{
        //    log4net.Config.XmlConfigurator.Configure();
        //}

      /// <summary>
      /// Lista equipos por familia
      /// </summary>
      /// <param name="idFamilia"></param>
      /// <returns></returns>
        public IHttpActionResult GetListarEquiposXFamilia(int idFamilia)
        {
            try
            {
                return Ok(
              this.service.ListarEquiposXFamilia(idFamilia)
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
        /// Lista mis lecturas
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetListMeLecturas()
        {
            try
            {
                return Ok(
              this.service.ListMeLecturas()
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
        /// Listar familias 
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetListarFamilia()
        {
            try
            {
                return Ok(
              this.service.ListarFamilia()
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
        /// Listar si tipo informacion
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetListSiTipoinformacions()
        {
            try
            {
                return Ok(
              this.service.ListSiTipoinformacions()
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
        /// Obtener mi lectura por codigo
        /// </summary>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public IHttpActionResult GetByIdMeLectura(int lectcodi)
        {
            try
            {
                return Ok(
              this.service.ListarEquiposXFamilia(lectcodi)
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
        /// Obtener lista med 24 hidrologia
        /// </summary>
        /// <param name="lectocodi"></param>
        /// <param name="origlect"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <returns></returns>
        public IHttpActionResult GetListaMed24Hidrologia(int lectocodi, int origlect, string idsEmpresa, string idsCuenca, string idsFamilia, DateTime fechaInicio, DateTime fechaFin, string idsPtoMedicion)
        {
            try
            {
                return Ok(
              this.service.ListaMed24Hidrologia(lectocodi, origlect, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion)
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
        /// Obtener reporte hidrologia
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idLectura"></param>
        /// <param name="opcion"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <param name="unidad"></param>
        /// <param name="numeroMedicion"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerReporteHidrologia(string idsEmpresa, string idsCuenca, string idsFamilia, string idsPtoMedicion, DateTime fechaInicio, DateTime fechaFin, int idLectura, int opcion, int rbDetalleRpte, int unidad, int numeroMedicion)
        {
            try
            {
                return Ok(
              this.service.ObtenerReporteHidrologia(idsEmpresa,idsCuenca,idsFamilia,idsPtoMedicion,fechaInicio,fechaFin,idLectura,opcion,rbDetalleRpte,unidad,numeroMedicion)
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
        /// Listar tipo punto medicion
        /// </summary>
        /// <param name="origlectcodi"></param>
        /// <returns></returns>
        public IHttpActionResult GetListMeTipopuntomedicions(string origlectcodi)
        {
            try
            {
                return Ok(
              this.service.ListMeTipopuntomedicions(origlectcodi)
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
        /// obtener nro de filas med1 hidrologia
        /// </summary>
        /// <param name="lectocodi"></param>
        /// <param name="idOrigenLectura"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerNroFilasMed1Hidrologia(int lectocodi, int idOrigenLectura, string idsEmpresa, string idsCuenca, string idsFamilia, DateTime fechaInicio, DateTime fechaFin, string idsPtoMedicion)
        {
            try
            {
                return Ok(
              this.service.ObtenerNroFilasMed1Hidrologia(lectocodi, idOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion)
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
        /// Listar med1 hidrologia 
        /// </summary>
        /// <param name="lectocodi"></param>
        /// <param name="origlect"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <returns></returns>
        public IHttpActionResult GetListaMed1Hidrologia(int lectocodi, int origlect, string idsEmpresa, string idsCuenca, string idsFamilia,
            DateTime fechaInicio, DateTime fechaFin, string idsPtoMedicion)
        {
            try
            {
                return Ok(
              this.service.ListaMed1Hidrologia(lectocodi, origlect, idsEmpresa, idsCuenca, idsFamilia,
                fechaInicio, fechaFin, idsPtoMedicion)
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
        /// Generar lista detallada de med1 promedio
        /// </summary>
        /// <param name="idLectura"></param>
        /// <param name="IdOrigenLectura"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <returns></returns>
        public IHttpActionResult GetGenerarListaDetalladaMed1Promedio(int idLectura, int IdOrigenLectura, string idsEmpresa, string idsCuenca, string idsFamilia,
            DateTime fechaInicio, DateTime fechaFin, string idsPtoMedicion, int rbDetalleRpte)
        {
            try
            {
                return Ok(
              this.service.GenerarListaDetalladaMed1Promedio(idLectura,IdOrigenLectura,idsEmpresa,idsCuenca,idsFamilia,fechaInicio,fechaFin,idsPtoMedicion,rbDetalleRpte)
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
        /// Generar lista detalle med1 hmax
        /// </summary>
        /// <param name="idLectura"></param>
        /// <param name="IdOrigenLectura"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <returns></returns>
        public IHttpActionResult GetGenerarListaDetalladaMed1Hmax(int idLectura, int IdOrigenLectura, string idsEmpresa, string idsCuenca, string idsFamilia,
            DateTime fechaInicio, DateTime fechaFin, string idsPtoMedicion, int rbDetalleRpte)
        {
            try
            {
                return Ok(
              this.service.GenerarListaDetalladaMed1Hmax(idLectura,IdOrigenLectura,idsEmpresa,idsCuenca,idsFamilia,fechaInicio,fechaFin,idsPtoMedicion,rbDetalleRpte)
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
        /// Generar lista med1 prom anual
        /// </summary>
        /// <param name="idLectura"></param>
        /// <param name="IdOrigenLectura"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <returns></returns>
        public IHttpActionResult GetGenerarListaMed1PromAnual(int idLectura, int IdOrigenLectura, string idsEmpresa, string idsCuenca, string idsFamilia, DateTime fechaInicio,
                                                                      DateTime fechaFin, string idsPtoMedicion)
        {
            try
            {
                return Ok(
              this.service.GenerarListaMed1PromAnual(idLectura,IdOrigenLectura,idsEmpresa,idsCuenca,idsFamilia,fechaInicio,fechaFin,idsPtoMedicion)
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
        /// Generar lista med1anual H maximo
        /// </summary>
        /// <param name="idLectura"></param>
        /// <param name="IdOrigenLectura"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <returns></returns>
        public IHttpActionResult GetGenerarListaMed1AnualHmaximo(int idLectura, int IdOrigenLectura, string idsEmpresa, string idsCuenca, string idsFamilia, DateTime fechaInicio,
                                                                      DateTime fechaFin, string idsPtoMedicion)
        {
            try
            {
                return Ok(
              this.service.GenerarListaMed1AnualHmaximo(idLectura,IdOrigenLectura,idsEmpresa,idsCuenca,idsFamilia,fechaInicio,fechaFin,idsPtoMedicion)
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
        /// Generar lista detallada med 24 promedio
        /// </summary>
        /// <param name="idLectura"></param>
        /// <param name="IdOrigenLectura"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <returns></returns>
        public IHttpActionResult GetGenerarListaDetalladaMed24Promedio(int idLectura, int IdOrigenLectura, string idsEmpresa, string idsCuenca, string idsFamilia, DateTime fechaInicio,
                                                                      DateTime fechaFin, string idsPtoMedicion, int rbDetalleRpte)
        {
            try
            {
                return Ok(
              this.service.GenerarListaDetalladaMed24Promedio(idLectura,IdOrigenLectura,idsEmpresa,idsCuenca,idsFamilia,fechaInicio,fechaFin,idsPtoMedicion,rbDetalleRpte)
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
        /// Generar lista med24h maximo
        /// </summary>
        /// <param name="idLectura"></param>
        /// <param name="IdOrigenLectura"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <returns></returns>
        public IHttpActionResult GetGenerarListaMed24Hmaximo(int idLectura, int IdOrigenLectura, string idsEmpresa, string idsCuenca, string idsFamilia, DateTime fechaInicio,
                                                                      DateTime fechaFin, string idsPtoMedicion, int rbDetalleRpte)
        {
            try
            {
                return Ok(
              this.service.GenerarListaMed24Hmaximo(idLectura,IdOrigenLectura,idsEmpresa,idsCuenca,idsFamilia,fechaInicio,fechaFin,idsPtoMedicion,rbDetalleRpte)
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
        /// Generar lista med24 promedio anual
        /// </summary>
        /// <param name="idLectura"></param>
        /// <param name="IdOrigenLectura"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <returns></returns>
        public IHttpActionResult GetGenerarListaMed24PromAnual(int idLectura, int IdOrigenLectura, string idsEmpresa, string idsCuenca, string idsFamilia,
            DateTime fechaInicio, DateTime fechaFin, string idsPtoMedicion)
        {
            try
            {
                return Ok(
              this.service.GenerarListaMed24PromAnual(idLectura,IdOrigenLectura,idsEmpresa,idsCuenca,idsFamilia,fechaInicio,fechaFin,idsPtoMedicion)
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
        /// Generar lista med 24 anual x Maximo
        /// </summary>
        /// <param name="idLectura"></param>
        /// <param name="IdOrigenLectura"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <returns></returns>
        public IHttpActionResult GetGenerarListaMed24AnualHmaximo(int idLectura, int IdOrigenLectura, string idsEmpresa, string idsCuenca,
            string idsFamilia, DateTime fechaInicio, DateTime fechaFin, string idsPtoMedicion)
        {
            try
            {
                return Ok(
              this.service.GenerarListaMed24AnualHmaximo(idLectura,IdOrigenLectura,idsEmpresa,idsCuenca,idsFamilia,fechaInicio,fechaFin,idsPtoMedicion)
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
        /// Obtener listado de med 24 hidrologia tiempo real
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="idOrigenLectura"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsTipoPtoMedicion"></param>
        /// <returns></returns>
        public IHttpActionResult GetListaMed24HidrologiaTiempoReal(int reporcodi, int idOrigenLectura, string idsEmpresa, DateTime fechaInicio,
            DateTime fechaFin, string idsTipoPtoMedicion)
        {
            try
            {
                return Ok(
              this.service.ListaMed24HidrologiaTiempoReal(reporcodi,idOrigenLectura,idsEmpresa,fechaInicio, fechaFin, idsTipoPtoMedicion)
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
        /// Obtener reporte hidrologia tiempo real
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsTipoPtoMed"></param>
        /// <param name="tipoReporte"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerReporteHidrologiaTiempoReal(string idsEmpresa, DateTime fechaIni, DateTime fechaFin, string idsTipoPtoMed, int tipoReporte)
        {
            try
            {
                return Ok(
              this.service.ObtenerReporteHidrologiaTiempoReal(idsEmpresa, fechaIni, fechaFin, idsTipoPtoMed,tipoReporte)
       
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
        /// obtener lista de encabezado reporptomeds 
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsTipoPtoMed"></param>
        /// <returns></returns>
        public IHttpActionResult GetListarEncabezadoMeReporptomeds(int reporcodi, string idsEmpresa, string idsTipoPtoMed)
        {
            try
            {
                return Ok(
              this.service.ListarEncabezadoMeReporptomeds(reporcodi, idsEmpresa, idsTipoPtoMed)
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
        /// Obtener nro de filas hidrologia tiempo real
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="idOrigenLectura"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsTipoPtoMedicion"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerNroFilasHidrologiaTiempoReal(int reporcodi, int idOrigenLectura, string idsEmpresa, DateTime fechaInicio,
            DateTime fechaFin, string idsTipoPtoMedicion)
        {
            try
            {
                return Ok(
              this.service.ObtenerNroFilasHidrologiaTiempoReal(reporcodi, idOrigenLectura, idsEmpresa, fechaInicio, fechaFin, idsTipoPtoMedicion)
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
        /// Obtener reportehidrologia qnvoltipo2
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <param name="unidad"></param>
        /// <param name="idLectura"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerReporteHidrologiaQnVolTipo2(string idsEmpresa, DateTime fechaInicio, DateTime fechaFin, int rbDetalleRpte, int unidad,
            int idLectura, string idsCuenca, string idsFamilia, string idsPtoMedicion)
        {
            try
            {
                return Ok(
              this.service.ObtenerReporteHidrologiaQnVolTipo2(idsEmpresa,fechaInicio,fechaFin,rbDetalleRpte,unidad,idLectura,idsCuenca,idsFamilia,idsPtoMedicion)
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
        /// Obtener nro de filas descargaras vert
        /// </summary>
        /// <param name="idFormato"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerNroFilasDescargVert(int idFormato, string idsEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                return Ok(
              this.service.ObtenerNroFilasDescargVert(idFormato,idsEmpresa,fechaInicio,fechaFin)
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
        /// obtener reoirte descarga vertimiento
        /// </summary>
        /// <param name="idFormato"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="nroPagina"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerReporteDescargaVertimiento(int idFormato, string idsEmpresa, DateTime fechaIni, DateTime fechaFin, int nroPagina, int pageSize)
        {
            try
            {
                return Ok(
              this.service.ObtenerReporteDescargaVertimiento(idFormato,idsEmpresa,fechaIni,fechaFin,nroPagina,pageSize)
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
        /// Obtener lista med intervalos desga ber¿¿
        /// </summary>
        /// <param name="formatCodi"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public IHttpActionResult GetListaMedIntervaloDescargaVert(int formatCodi, string idsEmpresa, DateTime fechaInicio,
            DateTime fechaFin)
        {
            try
            {
                return Ok(
              this.service.ListaMedIntervaloDescargaVert(formatCodi, idsEmpresa, fechaInicio,fechaFin)
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
        /// Lista Pronostico hidrologia
        /// </summary>
        /// <param name="reportecodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public IHttpActionResult GetListaPronosticoHidrologia(int reportecodi, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                return Ok(
              this.service.ListaPronosticoHidrologia(reportecodi,fechaInicio,fechaFin)
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
        /// Lista historia hidrologia
        /// </summary>
        /// <param name="reportecodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public IHttpActionResult GetListaHistoricoHidrologia(int reportecodi, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                return Ok(
              this.service.ListaHistoricoHidrologia(reportecodi,fechaInicio,fechaFin)
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
        /// Lista pronostico hidrologia por punto calculado y fecha
        /// </summary>
        /// <param name="reportecodi"></param>
        /// <param name="ptocalculadocodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public IHttpActionResult GetListaPronosticoHidrologiaByPtoCalculadoAndFecha(int reportecodi, int ptocalculadocodi, DateTime fecha)
        {
            try
            {
                return Ok(
              this.service.ListaPronosticoHidrologiaByPtoCalculadoAndFecha(reportecodi,ptocalculadocodi,fecha)
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
        /// Obtener lista encabezado powel
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <returns></returns>
        public IHttpActionResult GetListarEncabezadoPowel(int reporcodi)
        {
            try
            {
                return Ok(
              this.service.ListarEncabezadoPowel(reporcodi)
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
        /// Lista semanal Owel
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="reportecodi"></param>
        /// <returns></returns>
        public IHttpActionResult ListaSemanalPOwel(DateTime fechaInicio, DateTime fechaFin, int reportecodi)
        {
            try
            {
                return Ok(
              this.service.ListaSemanalPOwel(fechaInicio,fechaFin,reportecodi)
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
        /// Obtener numero formato info
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetGenerarNumberFormatInfo()
        {
            try
            {
                return Ok(
              this.service.GenerarNumberFormatInfo()
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
        
    }
}


