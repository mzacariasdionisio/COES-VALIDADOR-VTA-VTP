using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Migraciones;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace COES.WEBAPI.MedidoresGeneracion.Controllers
{
    public class MedidoresGeneracionController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(MedidoresGeneracionController));
        MigracionesAppServicio service = new MigracionesAppServicio();

        //public EventoController()
        //{
        //    log4net.Config.XmlConfigurator.Configure();
        //}

      
        /// <summary>
        /// Listar empresas Demanda Barra
        /// </summary>
        /// <param name="idTipoEmpresa"></param>
        /// <returns></returns>
        public IHttpActionResult GetListarEmpresasDemandaBarrra(int idTipoEmpresa)
        {
            try
            {
                return Ok(
              this.service.ListarEmpresasDemandaBarrra(idTipoEmpresa)
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
        /// Obtener lista Tipo info
        /// </summary>
        /// <param name="tipoinfocodi"></param>
        /// <returns></returns>
        public IHttpActionResult GetListaTipoInfo(string tipoinfocodi)
        {
            try
            {
                return Ok(
              this.service.GetListaTipoInfo(tipoinfocodi)
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
        /// Obtener Lista de produccion
        /// </summary>
        /// <param name="fecIni"></param>
        /// <param name="lectcodi"></param>
        /// <param name="tipoinfocodi"></param>
        /// <returns></returns>
        public IHttpActionResult GetListaProduccioncco(DateTime fecIni, string lectcodi, int tipoinfocodi)
        {
            try
            {
                return Ok(
              this.service.GetListaProduccioncco(fecIni, lectcodi, tipoinfocodi)
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
        /// Obtener Producciones html
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="lista"></param>
        /// <returns></returns>
        public IHttpActionResult GetCargarProduccionccoHtml(DateTime fecha, List<MeMedicion48DTO> lista)
        {
            try
            {
                return Ok(
              this.service.CargarProduccionccoHtml(fecha, lista)
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
        /// Obtener Lista medicion 48 x lectocodi
        /// </summary>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public IHttpActionResult GetListaMedicion48xlectcodi(int lectcodi)
        {
            try
            {
                return Ok(
              this.service.GetListaMedicion48xlectcodi( lectcodi)
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
        /// Obtener Lista de mediccion 48
        /// </summary>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        /// <param name="lectcodi"></param>
        /// <param name="tipoinfocodi"></param>
        /// <param name="ptomedicodi"></param>
        /// <returns></returns>
        public IHttpActionResult GetListaObtenerMedicion48(DateTime fecIni, DateTime fecFin, string lectcodi, int tipoinfocodi, string ptomedicodi)
        {
            try
            {
                return Ok(
              this.service.GetListaObtenerMedicion48(fecIni, fecFin, lectcodi, tipoinfocodi, ptomedicodi)
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
        /// Obtener Lista medicion por puntos
        /// </summary>
        /// <param name="ptomedicodi"></param>
        /// <returns></returns>
        public IHttpActionResult GetListaMePtomedicionxPtos(string ptomedicodi)
        {
            try
            {
                return Ok(
              this.service.GetListaMePtomedicionxPtos(ptomedicodi)
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
        /// Obtener informacion demanda semanal mensaul y anual html
        /// </summary>
        /// <param name="data"></param>
        /// <param name="tip"></param>
        /// <returns></returns>
        public IHttpActionResult GetInformacionDemandaSemMesAnioHtml(List<MeMedicion48DTO> data, int tip)
        {
            try
            {
                return Ok(
              this.service.InformacionDemandaSemMesAnioHtml(data,tip)
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
        /// Obtener cmg Corto plazo
        /// </summary>
        /// <param name="lectcodi"></param>
        /// <param name="tipoinfocodi"></param>
        /// <param name="ptomedicodi"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <returns></returns>
        public IHttpActionResult GetRptCmgCortoPlazo(string lectcodi, int tipoinfocodi, int ptomedicodi, DateTime fecha1, DateTime fecha2)
        {
            try
            {
                return Ok(
              this.service.GetRptCmgCortoPlazo(lectcodi, tipoinfocodi, ptomedicodi, fecha1, fecha2)
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
        /// Obtener cmg corto plazo html
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        public IHttpActionResult RptCmgCortoPlazoHtml(List<MeMedicion48DTO> datos)
        {
            try
            {
                return Ok(
              this.service.RptCmgCortoPlazoHtml(datos)
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
        /// Obtener lista punto medicion demanda barra
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="fechaPeriodoIni"></param>
        /// <param name="fechaPeriodoFin"></param>
        /// <returns></returns>
        public IHttpActionResult GetListaPuntoMedicionDemandaBarra(string emprcodi, DateTime fechaPeriodoIni, DateTime fechaPeriodoFin)
        {
            try
            {
                return Ok(
              this.service.GetListaPuntoMedicionDemandaBarra(emprcodi,fechaPeriodoIni,fechaPeriodoFin)
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
        /// Obtener Lista demana Barras
        /// </summary>
        /// <param name="ptomedicodi"></param>
        /// <param name="lectcodi"></param>
        /// <param name="fecInicio"></param>
        /// <param name="fecFin"></param>
        /// <returns></returns>
        public IHttpActionResult GetListaDemandaBarras(string ptomedicodi, string lectcodi, DateTime fecInicio, DateTime fecFin)
        {
            try
            {
                return Ok(
              this.service.GetListaDemandaBarras(ptomedicodi, lectcodi, fecInicio, fecFin)
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
        ///  Obtener Archivo reporte demana barra
        /// </summary>
        /// <param name="idTipoEmpresa"></param>
        /// <param name="emprcodi"></param>
        /// <param name="lectcodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public IHttpActionResult NombreArchivoReporteDemandaBarra(int idTipoEmpresa, string emprcodi, int lectcodi, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                return Ok(
              this.service.NombreArchivoReporteDemandaBarra(idTipoEmpresa,emprcodi,lectcodi,fechaInicio,fechaFin)
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
        /// Obtener por criterio mi lectura
        /// </summary>
        /// <param name="codilect"></param>
        /// <returns></returns>
        public IHttpActionResult GetByCriteriaMeLectura(string codilect)
        {
            try
            {
                return Ok(
              this.service.GetByCriteriaMeLectura(codilect)
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