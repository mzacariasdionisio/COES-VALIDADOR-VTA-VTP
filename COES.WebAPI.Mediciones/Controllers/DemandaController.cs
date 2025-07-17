using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Mediciones;
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

namespace COES.WebAPI.Mediciones.Controllers
{
    /// <summary>
    /// Servicio que retorna información de demanda COES para UL y Dist. URL: http://www.coes.org.pe/Portal/DemandaBarras/consulta/index?tipo=2 http://www.coes.org.pe/Portal/DemandaBarras/consulta/index?tipo=4
    /// </summary>
    public class DemandaController : ApiController
    {
        DemandaBarrasAppServicio service = new DemandaBarrasAppServicio();
        GeneralAppServicio servicio = new GeneralAppServicio();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(DemandaController));

        ///// <summary>
        ///// Obtener empresas por tipo
        ///// </summary>
        ///// <param name="idTipoEmpresa"></param>
        ///// <returns></returns>
        //public IHttpActionResult GetObtenerEmpresaPorTipo(int idTipoEmpresa)
        //{
        //    try
        //    {
        //        return Ok(
        //      this.service.ObtenerEmpresaPorTIpo(idTipoEmpresa)
        //   );
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("GetObtenerEmpresaPorTipo", ex);
        //        var response = new HttpResponseMessage(HttpStatusCode.NotFound)
        //        {
        //            Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
        //            StatusCode = HttpStatusCode.InternalServerError
        //        };
        //        throw new HttpResponseException(response);
        //    }
        //}

        /// <summary>
        /// Método que consulta la Demanda Histórica Diaria, Prevista Diaria, Prevista Semanal
        /// </summary>
        /// <param name="lectcodi">Histórica diaria = 45, Prevista diaria = 46, Prevista semanal = 47 </param>
        /// <param name="tipoempresa">Usuario libres = 4, Distribuidores = 2</param>
        /// <param name="empresas">Si desea una empresa específica digite su codigo en caso contrario dejarlo en blanco</param>      
        /// <param name="fechaInicio">Fecha inicio</param>
        /// <param name="fechaFin">No debe de exceder los 7 días con respecto a la fecha de inicio</param>       
        /// <returns></returns>       
        [ResponseType(typeof(List<Demanda48>))]
        public IHttpActionResult GetObtenerReporte(int lectcodi,int tipoempresa,string empresas,DateTime fechaInicio, DateTime fechaFin)
        {            
            int indicador = 0;            
            
            TimeSpan tiempoDiferencia = (fechaFin - fechaInicio);
            string company;
            List<SiEmpresaDTO> listado = servicio.ListadoComboEmpresasPorTipo(tipoempresa).Where(t=>t.Inddemanda=="S").ToList();
            if (listado.Count() > 0)
            {
                company = string.Join(",", listado.Select(x => x.Emprcodi).ToArray());
            }
            else
            {
                company = "-2";
            }
            string[] companies = company.Split(',');
            if (empresas != null && empresas != "")
            {
                if (companies.Contains(empresas))
                {
                    company = empresas;
                } else
                {
                    company = "-2";
                }
                
            }

            List<MeMedicion48DTO> lista = service.ObtenerReporte2(lectcodi, company, fechaInicio, fechaFin, indicador);           
           
            if (tiempoDiferencia.Days <= 7 && empresas == null)
            {
                try
                {
                    return Ok(
                      (from campos in lista
                       select new
                       {
                         Medifecha = campos.Medifecha,                         
                         Ptomedicodi = campos.Ptomedicodi,
                         NombreEquipo = campos.Equinomb,
                         CodigoEquipo = campos.Equicodi,
                         CodigoEmpresa = campos.Emprcodi,
                         NombreEmpresa = campos.Emprnomb,
                         TensionEquipo = campos.Equitension,
                         CodigoUbicacion = campos.Areacodi,
                         NombreUbicacion  = campos.Areanomb,
                         h1 =  campos.H1,
                         h2  = campos.H2,
                         h3  = campos.H3,
                         h4  = campos.H4,
                         h5  = campos.H5,
                         h6  = campos.H6,
                         h7  = campos.H7,
                         h8  = campos.H8,
                         h9  = campos.H9,
                         h10  = campos.H10,
                         h11  = campos.H11,
                         h12  = campos.H12,
                         h13  = campos.H13,
                         h14  = campos.H14,
                         h15  = campos.H15,
                         h16  = campos.H16,
                         h17  = campos.H17,
                         h18  = campos.H18,
                         h19  = campos.H19,
                         h20  = campos.H20,
                         h21  = campos.H21,
                         h22  = campos.H22,
                         h23  = campos.H23,
                         h24  = campos.H24,
                         h25  = campos.H25,
                         h26  = campos.H26,
                         h27  = campos.H27,
                         h28  = campos.H28,
                         h29  = campos.H29,
                         h30  = campos.H30,
                         h31  = campos.H31,
                         h32  = campos.H32,
                         h33  = campos.H33,
                         h34  = campos.H34,
                         h35  = campos.H35,
                         h36  = campos.H36,
                         h37  = campos.H37,
                         h38  = campos.H38,
                         h39  = campos.H39,
                         h40  = campos.H40,
                         h41  = campos.H41,
                         h42  = campos.H42,
                         h43  = campos.H43,
                         h44  = campos.H44,
                         h45  = campos.H45,
                         h46  = campos.H46,
                         h47  = campos.H47,
                         h48  = campos.H48
                         
                       })
                        );
                }
                catch (Exception ex)
                {
                    log.Error("GetObtenerReporte", ex);
                    var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                        StatusCode = HttpStatusCode.InternalServerError
                    };
                    throw new HttpResponseException(response);
                }
            }
            else if (tiempoDiferencia.Days <= 7 && empresas != null)
            {
                try
                {
                    return Ok(from campos in lista
                              select new
                              {
                                  Medifecha = campos.Medifecha,                                  
                                  Ptomedicodi = campos.Ptomedicodi,
                                  NombreEquipo = campos.Equinomb,
                                  CodigoEquipo = campos.Equicodi,
                                  CodigoEmpresa = campos.Emprcodi,
                                  NombreEmpresa = campos.Emprnomb,
                                  TensionEquipo = campos.Equitension,
                                  CodigoUbicacion = campos.Areacodi,
                                  NombreUbicacion = campos.Areanomb,
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
                    log.Error("GetObtenerReporte", ex);
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
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }
        }

        ///// <summary>
        ///// Obtener nombre de la empresa
        ///// </summary>
        ///// <param name="idEmpresa"></param>
        ///// <returns></returns>
        //public IHttpActionResult GetObtenerNombreEmpresa(int idEmpresa)
        //{
        //    try
        //    {
        //        return Ok(
        //      this.service.ObtenerNombreEmpresa(idEmpresa)
        //   );
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("GetObtenerNombreEmpresa", ex);
        //        var response = new HttpResponseMessage(HttpStatusCode.NotFound)
        //        {
        //            Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
        //            StatusCode = HttpStatusCode.InternalServerError
        //        };
        //        throw new HttpResponseException(response);
        //    }
        //}

        /// <summary>
        /// Obtener demanda programada diaria por áreas
        /// </summary>
        /// <param name="dtFecha">Fecha en la cual se quiere buscar</param>
        /// <returns></returns>
        [ResponseType(typeof(List<Demanda48DTO>))]
        public IHttpActionResult GetDemandaProgramadaDiariaAreas(DateTime dtFecha)
        {
            List<Medicion48DTO> lista = this.service.ObtenerDemandaProgramadaDiariaAreas(dtFecha);
            try
            {
                return Ok(from campos in lista
                          select new
                          {
                              PTOMEDICODI = campos.PTOMEDICODI,
                              PTOMEDIELENOMB = campos.PTOMEDIELENOMB,
                              MEDIFECHA = campos.MEDIFECHA,                            
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
                log.Error("GetDemandaProgramadaDiariaAreas", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// Obtener demanda programada diaria COES
        /// </summary>
        /// <param name="fecha">Fecha en la cual se quiere buscar</param>
        /// <returns></returns>
        [ResponseType(typeof(List<Demanda48DTO>))]
        public IHttpActionResult GetDemandaProgramadaDiariaCOES(DateTime fecha)
        {
            var lista = this.service.ObtenerProgramadaDiariaCOES(fecha);
            try
            {
                return Ok(from campos in lista
                          select new
                          {
                              PTOMEDICODI = campos.PTOMEDICODI,
                              PTOMEDIELENOMB = campos.PTOMEDIELENOMB,
                              MEDIFECHA = campos.MEDIFECHA,
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
                log.Error("GetDemandaProgramadaDiariaCOES", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }
        }


        /// <summary>
        /// Obtener demanda diaria ejecutada por área
        /// </summary>
        /// <param name="fechaInicio">Fecha en la cual se quiere buscar</param>
        /// <returns></returns>
        [ResponseType(typeof(List<Demanda48DTO>))]
        public IHttpActionResult GetDemandaDiariaReal(DateTime fechaInicio)
        {
            List<Medicion48DTO> lista = this.service.ObtenerDemandaDiariaReal(fechaInicio);
            try
            {
                return Ok(from campos in lista
                          select new
                          {
                              PTOMEDICODI = campos.PTOMEDICODI,
                              PTOMEDIELENOMB = campos.PTOMEDIELENOMB,
                              MEDIFECHA = campos.MEDIFECHA,
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
                log.Error("GetDemandaDiariaReal", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// Obtener demanda por áreas
        /// </summary>
        /// <param name="InicioFecha">Fecha en la cual se quiere buscar</param>
        /// <returns></returns>
        [ResponseType(typeof(List<Demanda48_DTO>))]
        public IHttpActionResult GetDemandaAreas(DateTime InicioFecha)
        {
            List<Medicion48DTO> lista = this.service.LeerDemandaAreas(InicioFecha);
            try
            {
                return Ok(   

              from campos in lista
              select new
              {
                  Ptomedicodi = campos.PTOMEDICODI,
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
                log.Error("GetDemandaAreas", ex);
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