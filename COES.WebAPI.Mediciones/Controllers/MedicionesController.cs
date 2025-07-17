using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Migraciones;
using COES.WebAPI.Mediciones.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace COES.WebAPI.Mediciones
{
    /// <summary>
    /// Servicio para consulta de Información de Despacho de la pagina web https://www.coes.org.pe/appintranet/ en la pestaña de Programación-> Despacho -> CDispatch
    /// </summary>
    public class MedicionesController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(MedicionesController));
        MigracionesAppServicio service = new MigracionesAppServicio();

        /// <summary>
        /// Obtener historico medicion 48 mediante la pagina web https://www.coes.org.pe/appintranet/ en la pestaña de Programación-> Despacho -> CDispatch
        /// </summary>
        /// <param name="lectcodi">Programación semanal 3, Programación diaria = 4, Reprogramación diaria 5, ejecutado = 6</param>
        /// <param name="fechaIni">Fecha de inicio</param>
        /// <param name="fechaFin">No debe de exceder los 7 días con respecto a la fecha de inicio</param>    
        /// <returns></returns>
        [ResponseType(typeof(List<Demanda48>))]
        public IHttpActionResult GetObtenerHistoricoMedicion48(int lectcodi, DateTime fechaIni, DateTime fechaFin)
        {
            TimeSpan tiempoDiferencia = (fechaFin - fechaIni);
            if (tiempoDiferencia.Days <= 7)
            {
                try
                {
                    var listaMe48 = this.service.GetListaMedicion48WebApiMediciones(lectcodi, fechaIni, fechaFin);

                    return Ok(
                            from campos in listaMe48
                            select new
                            {
                                Medifecha = campos.Medifecha,
                                //Tipoinfocodi=campos.Tipoinfocodi,
                                //Tipoinfodesc=campos.Tipoinfodesc,
                                Ptomedicodi = campos.Ptomedicodi,
                                NombreEquipo = campos.Equinomb,
                                CodigoEquipo = campos.Equicodi,
                                CodigoEmpresa = campos.Emprcodi,
                                NombreEmpresa = campos.Emprnomb,
                                TensionEquipo = campos.Equitension,
                                CodigoUbicacion = campos.Areacodi,
                                NombreUbicacion = campos.Areanomb,
                                Tipoinfoabrev = campos.Tipoinfoabrev,
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
                                t1 = campos.T1,
                                t2 = campos.T2,
                                t3 = campos.T3,
                                t4 = campos.T4,
                                t5 = campos.T5,
                                t6 = campos.T6,
                                t7 = campos.T7,
                                t8 = campos.T8,
                                t9 = campos.T9,
                                t10 = campos.T10,
                                t11 = campos.T11,
                                t12 = campos.T12,
                                t13 = campos.T13,
                                t14 = campos.T14,
                                t15 = campos.T15,
                                t16 = campos.T16,
                                t17 = campos.T17,
                                t18 = campos.T18,
                                t19 = campos.T19,
                                t20 = campos.T20,
                                t21 = campos.T21,
                                t22 = campos.T22,
                                t23 = campos.T23,
                                t24 = campos.T24,
                                t25 = campos.T25,
                                t26 = campos.T26,
                                t27 = campos.T27,
                                t28 = campos.T28,
                                t29 = campos.T29,
                                t30 = campos.T30,
                                t31 = campos.T31,
                                t32 = campos.T32,
                                t33 = campos.T33,
                                t34 = campos.T34,
                                t35 = campos.T35,
                                t36 = campos.T36,
                                t37 = campos.T37,
                                t38 = campos.T38,
                                t39 = campos.T39,
                                t40 = campos.T40,
                                t41 = campos.T41,
                                t42 = campos.T42,
                                t43 = campos.T43,
                                t44 = campos.T44,
                                t45 = campos.T45,
                                t46 = campos.T46,
                                t47 = campos.T47,
                                t48 = campos.T48,
                            }
                            );
                }
                catch (Exception ex)
                {
                    log.Error("GetObtenerHistoricoMedicion48", ex);
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
                    Content = new StringContent("No debe de exceder los 7 días con respecto a la fecha de inicio", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.BadRequest
                };
                throw new HttpResponseException(response);
            }       

        }

        /// <summary>
        /// Leer puntos de medición
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(List<MedicionDTO>))]
        public IHttpActionResult GetLeerPtoMedicion()
        {
            List<MePtomedicionDTO> lista = this.service.ListarPtoMedicionDespacho();

            try
            {
                return Ok(

              from campos in lista
              select new
              {
                  Ptomedicodi = campos.Ptomedicodi,
                  Ptomedielenomb = campos.Ptomedielenomb,
                  Ptomedidesc = campos.Ptomedidesc,
                  Equicodi = campos.Equicodi,                  
                  Lastdate = campos.Lastdate
              }
           );
            }
            catch (Exception ex)
            {
                log.Error("GetLeerPtoMedicion", ex);
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