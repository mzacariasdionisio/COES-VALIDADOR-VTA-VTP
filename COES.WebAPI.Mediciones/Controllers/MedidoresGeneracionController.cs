using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Migraciones;
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

namespace COES.WEBAPI.MedidoresGeneracion.Controllers
{
    public class MedidoresGeneracionController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(MedidoresGeneracionController));
        MigracionesAppServicio service = new MigracionesAppServicio();


        ///// <summary>
        ///// Listar empresas Demanda Barra
        ///// </summary>
        ///// <param name="idTipoEmpresa"></param>
        ///// <returns></returns>
        //public IHttpActionResult GetListarEmpresasDemandaBarrra(int idTipoEmpresa)
        //{
        //    try
        //    {
        //        return Ok(
        //      this.service.ListarEmpresasDemandaBarrra(idTipoEmpresa)
        //   );
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("GetListarEmpresasDemandaBarrra", ex);
        //        var response = new HttpResponseMessage(HttpStatusCode.NotFound)
        //        {
        //            Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
        //            StatusCode = HttpStatusCode.InternalServerError
        //        };
        //        throw new HttpResponseException(response);

        //    }

        //}

        ///// <summary>
        ///// Obtener lista Tipo info
        ///// </summary>
        ///// <param name="tipoinfocodi"></param>
        ///// <returns></returns>
        //public IHttpActionResult GetListaTipoInfo(string tipoinfocodi)
        //{
        //    try
        //    {
        //        return Ok(
        //      this.service.GetListaTipoInfo(tipoinfocodi)
        //   );
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("GetListaTipoInfo", ex);
        //        var response = new HttpResponseMessage(HttpStatusCode.NotFound)
        //        {
        //            Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
        //            StatusCode = HttpStatusCode.InternalServerError
        //        };
        //        throw new HttpResponseException(response);

        //    }

        //}

        ///// <summary>
        ///// Obtener Lista de produccion
        ///// </summary>
        ///// <param name="fecIni"></param>
        ///// <param name="lectcodi"></param>
        ///// <param name="tipoinfocodi"></param>
        ///// <returns></returns>
        //public IHttpActionResult GetListaProduccioncco(DateTime fecIni, string lectcodi, int tipoinfocodi)
        //{
        //    try
        //    {
        //        return Ok(
        //      this.service.GetListaProduccioncco(fecIni, lectcodi, tipoinfocodi)
        //   );
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("GetListaProduccioncco", ex);
        //        var response = new HttpResponseMessage(HttpStatusCode.NotFound)
        //        {
        //            Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
        //            StatusCode = HttpStatusCode.InternalServerError
        //        };
        //        throw new HttpResponseException(response);

        //    }

        //}

        ///// <summary>
        ///// Obtener Lista medicion 48 x lectocodi
        ///// </summary>
        ///// <param name="lectcodi"></param>
        ///// <returns></returns>
        //public IHttpActionResult GetListaMedicion48xlectcodi(int lectcodi)
        //{
        //    try
        //    {
        //        return Ok(
        //      this.service.GetListaMedicion48xlectcodi( lectcodi)
        //   );
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("GetListaMedicion48xlectcodi", ex);
        //        var response = new HttpResponseMessage(HttpStatusCode.NotFound)
        //        {
        //            Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
        //            StatusCode = HttpStatusCode.InternalServerError
        //        };
        //        throw new HttpResponseException(response);

        //    }

        //}

        ///// <summary>
        ///// Obtener Lista de mediccion 48
        ///// </summary>
        ///// <param name="fecIni"></param>
        ///// <param name="fecFin"></param>
        ///// <param name="lectcodi"></param>
        ///// <param name="tipoinfocodi"></param>
        ///// <param name="ptomedicodi"></param>
        ///// <returns></returns>
        //public IHttpActionResult GetListaObtenerMedicion48(DateTime fecIni, DateTime fecFin, string lectcodi, int tipoinfocodi, string ptomedicodi)
        //{
        //    try
        //    {
        //        return Ok(
        //      this.service.GetListaObtenerMedicion48(fecIni, fecFin, lectcodi, tipoinfocodi, ptomedicodi)
        //   );
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("GetListaObtenerMedicion48", ex);
        //        var response = new HttpResponseMessage(HttpStatusCode.NotFound)
        //        {
        //            Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
        //            StatusCode = HttpStatusCode.InternalServerError
        //        };
        //        throw new HttpResponseException(response);

        //    }

        //}

        ///// <summary>
        ///// Obtener Lista medicion por puntos
        ///// </summary>
        ///// <param name="ptomedicodi"></param>
        ///// <returns></returns>
        //public IHttpActionResult GetListaMePtomedicionxPtos(string ptomedicodi)
        //{
        //    try
        //    {
        //        return Ok(
        //      this.service.GetListaMePtomedicionxPtos(ptomedicodi)
        //   );
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("GetListaMePtomedicionxPtos", ex);
        //        var response = new HttpResponseMessage(HttpStatusCode.NotFound)
        //        {
        //            Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
        //            StatusCode = HttpStatusCode.InternalServerError
        //        };
        //        throw new HttpResponseException(response);

        //    }

        //}                

        ///// <summary>
        ///// Obtener cmg Corto plazo
        ///// </summary>
        ///// <param name="lectcodi"></param>
        ///// <param name="tipoinfocodi"></param>
        ///// <param name="ptomedicodi"></param>
        ///// <param name="fecha1"></param>
        ///// <param name="fecha2"></param>
        ///// <returns></returns>
        //public IHttpActionResult GetRptCmgCortoPlazo(string lectcodi, int tipoinfocodi, int ptomedicodi, DateTime fecha1, DateTime fecha2)
        //{
        //    try
        //    {
        //        return Ok(
        //      this.service.GetRptCmgCortoPlazo(lectcodi, tipoinfocodi, ptomedicodi, fecha1, fecha2)
        //   );
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("GetRptCmgCortoPlazo", ex);
        //        var response = new HttpResponseMessage(HttpStatusCode.NotFound)
        //        {
        //            Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
        //            StatusCode = HttpStatusCode.InternalServerError
        //        };
        //        throw new HttpResponseException(response);

        //    }

        //}

        ///// <summary>
        ///// Obtener lista punto medicion demanda barra
        ///// </summary>
        ///// <param name="emprcodi"></param>
        ///// <param name="fechaPeriodoIni"></param>
        ///// <param name="fechaPeriodoFin"></param>
        ///// <returns></returns>
        //public IHttpActionResult GetListaPuntoMedicionDemandaBarra(string emprcodi, DateTime fechaPeriodoIni, DateTime fechaPeriodoFin)
        //{
        //    try
        //    {
        //        return Ok(
        //      this.service.GetListaPuntoMedicionDemandaBarra(emprcodi,fechaPeriodoIni,fechaPeriodoFin)
        //   );
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("GetListaPuntoMedicionDemandaBarra", ex);
        //        var response = new HttpResponseMessage(HttpStatusCode.NotFound)
        //        {
        //            Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
        //            StatusCode = HttpStatusCode.InternalServerError
        //        };
        //        throw new HttpResponseException(response);

        //    }

        //}

        ///// <summary>
        ///// Obtener Lista demana Barras
        ///// </summary>
        ///// <param name="ptomedicodi"></param>
        ///// <param name="lectcodi"></param>
        ///// <param name="fecInicio"></param>
        ///// <param name="fecFin"></param>
        ///// <returns></returns>
        //public IHttpActionResult GetListaDemandaBarras(string ptomedicodi, string lectcodi, DateTime fecInicio, DateTime fecFin)
        //{
        //    try
        //    {
        //        return Ok(
        //      this.service.GetListaDemandaBarras(ptomedicodi, lectcodi, fecInicio, fecFin)
        //   );
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("GetListaDemandaBarras", ex);
        //        var response = new HttpResponseMessage(HttpStatusCode.NotFound)
        //        {
        //            Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
        //            StatusCode = HttpStatusCode.InternalServerError
        //        };
        //        throw new HttpResponseException(response);

        //    }

        //}

        ///// <summary>
        /////  Obtener Archivo reporte demana barra
        ///// </summary>
        ///// <param name="idTipoEmpresa"></param>
        ///// <param name="emprcodi"></param>
        ///// <param name="lectcodi"></param>
        ///// <param name="fechaInicio"></param>
        ///// <param name="fechaFin"></param>
        ///// <returns></returns>
        //public IHttpActionResult NombreArchivoReporteDemandaBarra(int idTipoEmpresa, string emprcodi, int lectcodi, DateTime fechaInicio, DateTime fechaFin)
        //{
        //    try
        //    {
        //        return Ok(
        //      this.service.NombreArchivoReporteDemandaBarra(idTipoEmpresa,emprcodi,lectcodi,fechaInicio,fechaFin)
        //   );
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("NombreArchivoReporteDemandaBarra", ex);
        //        var response = new HttpResponseMessage(HttpStatusCode.NotFound)
        //        {
        //            Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
        //            StatusCode = HttpStatusCode.InternalServerError
        //        };
        //        throw new HttpResponseException(response);

        //    }

        //}

        public List<MeMedicion96DTO> LeerMedidores(DateTime fechaInicio)
        {
            return FactorySic.GetMePtomedicionRepository().LeerMedidores(fechaInicio);
        }

        /// <summary>
        ///  Obtener medidodres de un determinado año
        /// </summary>      
        /// <param name="fechaInicio"></param>
        /// <returns></returns>
        [ResponseType(typeof(List<Demanda96DTO>))]
        public IHttpActionResult getMedidores(DateTime fechaInicio)
        {
            List <MeMedicion96DTO> lista = this.service.LeerMedidores(fechaInicio);
            try
            {
                return Ok(
                    from campos in lista
                    select new
                    {
                        Ptomedicodi = campos.Ptomedicodi,                                           
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
                        h48 = campos.H48,
                        h49 = campos.H49,
                        h50 = campos.H50,
                        h51 = campos.H51,
                        h52 = campos.H52,
                        h53 = campos.H53,
                        h54 = campos.H54,
                        h55 = campos.H55,
                        h56 = campos.H56,
                        h57 = campos.H57,
                        h58 = campos.H58,
                        h59 = campos.H59,
                        h60 = campos.H60,
                        h61 = campos.H61,
                        h62 = campos.H62,
                        h63 = campos.H63,
                        h64 = campos.H64,
                        h65 = campos.H65,
                        h66 = campos.H66,
                        h67 = campos.H67,
                        h68 = campos.H68,
                        h69 = campos.H69,
                        h70 = campos.H70,
                        h71 = campos.H71,
                        h72 = campos.H72,
                        h73 = campos.H73,
                        h74 = campos.H74,
                        h75 = campos.H75,
                        h76 = campos.H76,
                        h77 = campos.H77,
                        h78 = campos.H78,
                        h79 = campos.H79,
                        h80 = campos.H80,
                        h81 = campos.H81,
                        h82 = campos.H82,
                        h83 = campos.H83,
                        h84 = campos.H84,
                        h85 = campos.H85,
                        h86 = campos.H86,
                        h87 = campos.H87,
                        h88 = campos.H88,
                        h89 = campos.H89,
                        h90 = campos.H90,
                        h91 = campos.H91,
                        h92 = campos.H92,
                        h93 = campos.H93,
                        h94 = campos.H94,
                        h95 = campos.H95,
                        h96 = campos.H96,
                        tipoinfocodidesc = campos.Tipoinfocodidesc,
                        TptoMediCodi = campos.TptoMediCodi,
                        tptomedinomb = campos.tptomedinomb
                    }
              
           );
            }
            catch (Exception ex)
            {
                log.Error("getMedidores", ex);
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