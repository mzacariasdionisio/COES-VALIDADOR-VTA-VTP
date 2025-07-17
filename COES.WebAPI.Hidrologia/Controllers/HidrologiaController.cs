using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Hidrologia;
using COES.WebAPI.Hidrologia.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace COES.WebAPI.HIdrología.Controllers
{
    public class HidrologiaController : ApiController
    {
        HidrologiaAppServicio service = new HidrologiaAppServicio();

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(HidrologiaController));
                
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
                log.Error("GetListarEquiposXFamilia", ex);
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
                log.Error("GetByIdMeLectura", ex);
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
                log.Error("GetListaMed24Hidrologia", ex);
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
              this.service.ObtenerReporteHidrologia(idsEmpresa, idsCuenca, idsFamilia, idsPtoMedicion, fechaInicio, fechaFin, idLectura, opcion, rbDetalleRpte, unidad, numeroMedicion)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetObtenerReporteHidrologia", ex);
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
                log.Error("GetListMeTipopuntomedicions", ex);
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
                log.Error("GetObtenerNroFilasMed1Hidrologia", ex);
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
        /// <param name="codilecto"></param>
        /// <param name="origlect"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <returns></returns>
        public IHttpActionResult GetListaMed1Hidrologia(int codilecto, int origlect, string idsEmpresa, string idsCuenca, string idsFamilia,DateTime fechaInicio, DateTime fechaFin, string idsPtoMedicion)
        {
            try
            {
                return Ok(
              this.service.ListaMed1Hidrologia(codilecto, origlect, idsEmpresa, idsCuenca, idsFamilia,
                fechaInicio, fechaFin, idsPtoMedicion)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetListaMed1Hidrologia", ex);
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
        /// <param name="Lecturaid"></param>
        /// <param name="IdOrigenLectura"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <returns></returns>
        public IHttpActionResult GetGenerarListaDetalladaMed1Promedio(int Lecturaid, int IdOrigenLectura, string idsEmpresa, string idsCuenca, string idsFamilia,
            DateTime fechaInicio, DateTime fechaFin, string idsPtoMedicion, int rbDetalleRpte)
        {
            try
            {
                return Ok(
              this.service.GenerarListaDetalladaMed1Promedio(Lecturaid, IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetGenerarListaDetalladaMed1Promedio", ex);
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
        /// <param name="IdOrigenLectura"></param>
        /// <param name="idLectura"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <returns></returns>
        public IHttpActionResult GetGenerarListaDetalladaMed1Hmax(int IdOrigenLectura, int idLectura, string idsEmpresa, string idsCuenca, string idsFamilia,
            DateTime fechaInicio, DateTime fechaFin, string idsPtoMedicion, int rbDetalleRpte)
        {
            try
            {
                return Ok(
              this.service.GenerarListaDetalladaMed1Hmax(IdOrigenLectura,idLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetGenerarListaDetalladaMed1Hmax", ex);
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
        public IHttpActionResult GetGenerarListaMed1PromAnual(string idsEmpresa,int idLectura, int IdOrigenLectura, string idsCuenca, string idsFamilia, DateTime fechaInicio,
                                                                      DateTime fechaFin, string idsPtoMedicion)
        {
            try
            {
                return Ok(
              this.service.GenerarListaMed1PromAnual(idLectura, IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetGenerarListaMed1PromAnual", ex);
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
        public IHttpActionResult GetGenerarListaMed1AnualHmaximo(string idsCuenca,int idLectura, int IdOrigenLectura, string idsEmpresa, string idsFamilia, DateTime fechaInicio,
                                                                      DateTime fechaFin, string idsPtoMedicion)
        {
            try
            {
                return Ok(
              this.service.GenerarListaMed1AnualHmaximo(idLectura, IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetGenerarListaMed1AnualHmaximo", ex);
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
        public IHttpActionResult GetGenerarListaDetalladaMed24Promedio(string idsFamilia,int idLectura, int IdOrigenLectura, string idsEmpresa, string idsCuenca,DateTime fechaInicio,
                                                                      DateTime fechaFin, string idsPtoMedicion, int rbDetalleRpte)
        {
            try
            {
                return Ok(
              this.service.GenerarListaDetalladaMed24Promedio(idLectura, IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetGenerarListaDetalladaMed24Promedio", ex);
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
        public IHttpActionResult GetGenerarListaMed24Hmaximo(DateTime fechaInicio, DateTime fechaFin,int idLectura, int IdOrigenLectura, string idsEmpresa, string idsCuenca, string idsFamilia,string idsPtoMedicion, int rbDetalleRpte)
        {
            try
            {
                return Ok(
              this.service.GenerarListaMed24Hmaximo(idLectura, IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetGenerarListaMed24Hmaximo", ex);
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
        public IHttpActionResult GetGenerarListaMed24PromAnual(DateTime fechaFin, DateTime fechaInicio, int idLectura, int IdOrigenLectura, string idsEmpresa, string idsCuenca, string idsFamilia,string idsPtoMedicion)
        {
            try
            {
                return Ok(
              this.service.GenerarListaMed24PromAnual(idLectura, IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetGenerarListaMed24PromAnual", ex);
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
              this.service.GenerarListaMed24AnualHmaximo(idLectura, IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetGenerarListaMed24AnualHmaximo", ex);
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
        /// <param name="codirepor"></param>
        /// <param name="idOrigenLectura"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsTipoPtoMedicion"></param>
        /// <returns></returns>
        public IHttpActionResult GetListaMed24HidrologiaTiempoReal(int codirepor, int idOrigenLectura, string idsEmpresa, DateTime fechaInicio,
            DateTime fechaFin, string idsTipoPtoMedicion)
        {
            try
            {
                return Ok(
              this.service.ListaMed24HidrologiaTiempoReal(codirepor, idOrigenLectura, idsEmpresa, fechaInicio, fechaFin, idsTipoPtoMedicion)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetListaMed24HidrologiaTiempoReal", ex);
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
              this.service.ObtenerReporteHidrologiaTiempoReal(idsEmpresa, fechaIni, fechaFin, idsTipoPtoMed, tipoReporte)

           );
            }
            catch (Exception ex)
            {
                log.Error("GetObtenerReporteHidrologiaTiempoReal", ex);
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
                log.Error("GetListarEncabezadoMeReporptomeds", ex);
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
        /// <param name="codigorepor"></param>
        /// <param name="idOrigenLectura"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsTipoPtoMedicion"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerNroFilasHidrologiaTiempoReal(int codigorepor, int idOrigenLectura, string idsEmpresa, DateTime fechaInicio,
            DateTime fechaFin, string idsTipoPtoMedicion)
        {
            try
            {
                return Ok(
              this.service.ObtenerNroFilasHidrologiaTiempoReal(codigorepor, idOrigenLectura, idsEmpresa, fechaInicio, fechaFin, idsTipoPtoMedicion)
                );
            }
            catch (Exception ex)
            {
                log.Error("GetObtenerNroFilasHidrologiaTiempoReal", ex);
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
              this.service.ObtenerReporteHidrologiaQnVolTipo2(idsEmpresa, fechaInicio, fechaFin, rbDetalleRpte, unidad, idLectura, idsCuenca, idsFamilia, idsPtoMedicion)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetObtenerReporteHidrologiaQnVolTipo2", ex);
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
              this.service.ObtenerNroFilasDescargVert(idFormato, idsEmpresa, fechaInicio, fechaFin)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetObtenerNroFilasDescargVert", ex);
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
              this.service.ObtenerReporteDescargaVertimiento(idFormato, idsEmpresa, fechaIni, fechaFin, nroPagina, pageSize)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetObtenerReporteDescargaVertimiento", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// Obtener lista med intervalos descargad vert
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
              this.service.ListaMedIntervaloDescargaVert(formatCodi, idsEmpresa, fechaInicio, fechaFin)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetListaMedIntervaloDescargaVert", ex);
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
        /// <param name="codigoreporte"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public IHttpActionResult GetListaPronosticoHidrologia(int codigoreporte, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                return Ok(
              this.service.ListaPronosticoHidrologia(codigoreporte, fechaInicio, fechaFin)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetListaPronosticoHidrologia", ex);
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
              this.service.ListaHistoricoHidrologia(reportecodi, fechaInicio, fechaFin)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetListaHistoricoHidrologia", ex);
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
              this.service.ListaPronosticoHidrologiaByPtoCalculadoAndFecha(reportecodi, ptocalculadocodi, fecha)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetListaPronosticoHidrologiaByPtoCalculadoAndFecha", ex);
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
                log.Error("GetListarEncabezadoPowel", ex);
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
              this.service.ListaSemanalPOwel(fechaInicio, fechaFin, reportecodi)
           );
            }
            catch (Exception ex)
            {
                log.Error("ListaSemanalPOwel", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// Lista medidores de hidrología
        /// </summary>        
        /// <param name="fecha">Fecha en la cual se buscara los medidores de hidrología</param>
        /// <returns></returns>
        [ResponseType(typeof(List<Demanda48DTO>))]
        public IHttpActionResult GetMedidoresHidrologia(DateTime fecha)
        {
            List<MeMedicion48DTO> lista = this.service.LeerMedidoresHidrologia(fecha);
            try
            {
                return Ok(

              from campos in lista
              select new
              {
                  Ptomedicodi = campos.Ptomedicodi,
                  Tipoinfocodi = campos.Tipoinfocodi,
                  h1 = campos.H1,
                  h2 = campos.H2,
                  h3 = campos.H3,
                  h4 = campos.H4,
                  h5 = campos.H5,
                  h6 = campos.H6,
                  h7 = campos.H7,
                  h8 = campos.H8,
                  h9 = campos.H9,
                  h10 = campos.H10,
                  h11 = campos.H11,
                  h12 = campos.H12,
                  h13 = campos.H13,
                  h14 = campos.H14,
                  h15 = campos.H15,
                  h16 = campos.H16,
                  h17 = campos.H17,
                  h18 = campos.H18,
                  h19 = campos.H19,
                  h20 = campos.H20,
                  h21 = campos.H21,
                  h22 = campos.H22,
                  h23 = campos.H23,
                  h24 = campos.H24,
                  h25 = campos.H25,
                  h26 = campos.H26,
                  h27 = campos.H27,
                  h28 = campos.H28,
                  h29 = campos.H29,
                  h30 = campos.H30,
                  h31 = campos.H31,
                  h32 = campos.H32,
                  h33 = campos.H33,
                  h34 = campos.H34,
                  h35 = campos.H35,
                  h36 = campos.H36,
                  h37 = campos.H37,
                  h38 = campos.H38,
                  h39 = campos.H39,
                  h40 = campos.H40,
                  h41 = campos.H41,
                  h42 = campos.H42,
                  h43 = campos.H43,
                  h44 = campos.H44,
                  h45 = campos.H45,
                  h46 = campos.H46,
                  h47 = campos.H47,
                  h48 = campos.H48
              }
           );
            }
            catch (Exception ex)
            {
                log.Error("LeerMedidoresHidrologia", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }
        }


        /// <summary>
        /// Lista punto de mediciones de hidrología
        /// </summary>               
        /// <returns></returns>
        [ResponseType(typeof(List<MedicionDTO>))]
        public IHttpActionResult GetPtoMedicionHidrologia()
        {
            List<MePtomedicionDTO> lista = this.service.LeerPtoMedicionHidrologia();
            try
            {
                return Ok(

              from campos in lista
              select new
              {
                  ptomedicodi = campos.Ptomedicodi,
                  ptomedidesc = campos.Ptomedidesc,
                  tipoinfocodi = campos.Tipoinfocodi,
                  emprnomb = campos.Emprnomb
              }
           );
            }
            catch (Exception ex)
            {
                log.Error("LeerPtoMedicionHidrologia", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// Obtiene la lista de datos de hidrología
        /// </summary>               
        /// <returns></returns>
        [ResponseType(typeof(List<Medicion_DTO>))]
        public IHttpActionResult GetDatosHidrologia(DateTime dtFechaInicio, DateTime dtFechaFin)
        {
            List<MePtomedicionDTO> lista = this.service.ObtenerDatosHidrologia(dtFechaInicio, dtFechaFin);
            try
            {
                return Ok(

              from campos in lista
              select new
              {
                  medifecha = campos.Medifecha,                 
                  ptomedidesc = campos.Ptomedidesc,
                  tipoinfodesc = campos.Tipoinfodesc,
                  h1 = campos.H1
              }
           );
            }
            catch (Exception ex)
            {
                log.Error("ObtenerDatosHidrologia", ex);
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


