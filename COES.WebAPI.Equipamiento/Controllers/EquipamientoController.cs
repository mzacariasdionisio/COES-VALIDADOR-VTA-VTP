using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Equipamiento;
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
    public class EquipamientoController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(EquipamientoController));
        EquipamientoAppServicio service = new EquipamientoAppServicio();
        

        /// <summary>
        /// Guarda un Equipo
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IHttpActionResult POSTSaveEqEquipo(EqEquipoDTO entity)
        {
            try
            {
                return Ok(
               this.service.SaveEqEquipo(entity)
            );
            }
            catch (Exception ex)
            {
                log.Error("POSTSaveEqEquipo", ex);                                    
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }

        }
        
        /// <summary>
        /// Obtener Equipo por area Empresa Todos
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="Areaid"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerEquipoPorAreaEmpresaTodos(int idEmpresa, int Areaid)
        {
            try
            {
            return Ok(
          this.service.ObtenerEquipoPorAreaEmpresaTodos(idEmpresa, Areaid)
            );
            }
        catch (Exception ex)
        {
            log.Error("GetObtenerEquipoPorAreaEmpresaTodos", ex);
            var response = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                StatusCode = HttpStatusCode.InternalServerError
            };
            throw new HttpResponseException(response);
            }           
        }
        /// <summary>
        ///  Obtener Equipo Por Area Empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idArea"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerEquipoPorAreaEmpresa(int idEmpresa, int idArea)
        {
            try
            {
                return Ok(
               this.service.ObtenerEquipoPorAreaEmpresa(idEmpresa, idArea)
            );
            }
            catch (Exception ex)
            {
                log.Error("GetObtenerEquipoPorAreaEmpresa", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }
            
        }

        /// <summary>
        /// Obtener Equipo por Id
        /// </summary>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        public IHttpActionResult GetByIdEqEquipo(int equicodi)
        {
            try
            {
                return Ok(
                  this.service.GetByIdEqEquipo(equicodi)
               );

            }
            catch (Exception ex)
            {
                log.Error("GetByIdEqEquipo", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response); 
            }
            
        }
        /// <summary>
        /// Obtener Lista de Equipos
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetListEqEquipos()
        {
            try
            {
                return Ok(
              this.service.ListEqEquipos()
           );
            }
            catch (Exception ex)
            {
                log.Error("GetListEqEquipos", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }

        }

        /// <summary>
        /// Listar Equipos por filtro
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="sEstado"></param>
        /// <param name="idTipoEquipo"></param>
        /// <param name="idTipoEmpresa"></param>
        /// <param name="sNombreEquipo"></param>
        /// <param name="idPadre"></param>
        /// <returns></returns>
        public IHttpActionResult GetListarEquiposxFiltro(int idEmpresa, string sEstado, int idTipoEquipo, int idTipoEmpresa,
            string sNombreEquipo, int idPadre)
        {
            try
            {
                return Ok(
              this.service.ListarEquiposxFiltro(idEmpresa, sEstado, idTipoEquipo, idTipoEmpresa, sNombreEquipo, idPadre)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetListarEquiposxFiltro", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response); 
            }
           
        }

        /// <summary>
        /// Listar equipos por filtro paginado
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="sEstado"></param>
        /// <param name="idTipoEquipo"></param>
        /// <param name="idTipoEmpresa"></param>
        /// <param name="sNombreEquipo"></param>
        /// <param name="idPadre"></param>
        /// <param name="nroPagina"></param>
        /// <param name="nroFilas"></param>
        /// <param name="totalPaginas"></param>
        /// <param name="totalRegistros"></param>
        /// <returns></returns>
        public IHttpActionResult GetListarEquiposxFiltroPaginado(int idEmpresa, string sEstado, int idTipoEquipo,
            int idTipoEmpresa, string sNombreEquipo, int idPadre, int nroPagina, int nroFilas,int totalPaginas,
            int totalRegistros)
        {
            try
            {
                return Ok(
              this.service.ListarEquiposxFiltroPaginado(idEmpresa, sEstado, idTipoEquipo, idTipoEmpresa, sNombreEquipo, idPadre,
               nroPagina, nroFilas, ref totalPaginas, ref totalRegistros)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetListarEquiposxFiltroPaginado", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response); 
            }
           
        }
        /// <summary>
        /// Obtener Detalle del Equipo
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerDetalleEquipo(int idEquipo)
        {
            try
            {
                return Ok(
               this.service.ObtenerDetalleEquipo(idEquipo)
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
        /// Buscar Equipos por Nombre
        /// </summary>
        /// <param name="coincidencia"></param>
        /// <param name="nroPagina"></param>
        /// <param name="nroFilas"></param>
        /// <returns></returns>
        public IHttpActionResult GetBuscarEquipoxNombre(string coincidencia, int nroPagina, int nroFilas)
        {
            try
            {
                return Ok(
               this.service.BuscarEquipoxNombre(coincidencia, nroPagina, nroFilas)
            );
            }
            catch (Exception ex)
            {
                log.Error("GetBuscarEquipoxNombre", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }
            
        }

        /// <summary>
        /// Listar Eequipos por familias
        /// </summary>
        /// <param name="iCodFamilias"></param>
        /// <returns></returns>
        public IHttpActionResult GetListarEquipoxFamilias([FromUri] params int[] iCodFamilias)
        {
            try
            {
                return Ok(
               this.service.ListarEquipoxFamilias(iCodFamilias)
            );
            }
            catch (Exception ex)
            {
                log.Error("GetListarEquipoxFamilias", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }
            
        }
        /// <summary>
        /// Listar equipos por familias por empresas
        /// </summary>
        /// <param name="iCodFamilias"></param>
        /// <param name="iEmpresas"></param>
        /// <returns></returns>
        public IHttpActionResult GetListarEquipoxFamiliasxEmpresas([FromUri]int[] iCodFamilias, [FromUri] int[] iEmpresas)
        {
            try
            {
                return Ok(
               this.service.ListarEquipoxFamiliasxEmpresas(iCodFamilias, iEmpresas)
            );
            }
            catch (Exception ex)
            {
                log.Error("GetListarEquipoxFamiliasxEmpresas", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }
            
        }
        /// <summary>
        /// Lista de equipamiento paginado
        /// </summary>
        /// <param name="iEmpresa"></param>
        /// <param name="iFamilia"></param>
        /// <param name="iTipoEmpresa"></param>
        /// <param name="iEquipo"></param>
        /// <param name="sEstado"></param>
        /// <param name="nombre"></param>
        /// <param name="nroPagina"></param>
        /// <param name="nroFilas"></param>
        /// <returns></returns>
        public IHttpActionResult GetListaEquipamientoPaginado(int iEmpresa, int iFamilia, int iTipoEmpresa, int iEquipo,
            string sEstado, string nombre, int nroPagina, int nroFilas)
        {
            try
            {
                return Ok(
              this.service.ListaEquipamientoPaginado(iEmpresa, iFamilia, iTipoEmpresa, iEquipo, sEstado, nombre, nroPagina, nroFilas)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetListaEquipamientoPaginado", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }
           
        }
        /// <summary>
        /// Obtener total Equipamiento
        /// </summary>
        /// <param name="iEmpresa"></param>
        /// <param name="iFamilia"></param>
        /// <param name="iTipoEmpresa"></param>
        /// <param name="iEquipo"></param>
        /// <param name="sEstado"></param>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public IHttpActionResult GetTotalEquipamiento(int iEmpresa, int iFamilia, int iTipoEmpresa, int iEquipo, string sEstado, string nombre)
        {
            try
            {
                return Ok(
                  this.service.TotalEquipamiento(iEmpresa, iFamilia, iTipoEmpresa, iEquipo, sEstado, nombre)
               );
            }
            catch (Exception ex)
            {
                log.Error("GetTotalEquipamiento", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }
           
        }
        /// <summary>
        /// Obtener equipos proc maniobras
        /// </summary>
        /// <param name="famCodi"></param>
        /// <param name="propCodi"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerEquiposProcManiobras(int famCodi, int propCodi)
        {
            try
            {
                return Ok(
               this.service.ObtenerEquiposProcManiobras(famCodi, propCodi)
            );
            }
            catch (Exception ex)
            {
                log.Error("GetObtenerEquiposProcManiobras", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }
            
        }

        /// <summary>
        /// Obtener equipo por area empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idArea"></param>
        /// <returns></returns>
        public IHttpActionResult ObtenerEquipoPorAreaEmpresa(int idEmpresa, int idArea)
        {
            try
            {
                return Ok(
              this.service.ObtenerEquipoPorAreaEmpresa(idEmpresa, idArea)
           );
            }
            catch (Exception ex)
            {
                log.Error("ObtenerEquipoPorAreaEmpresa", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }
           
        }
        /// <summary>
        /// Lista de equipo por empres y familia
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="codifam"></param>
        /// <returns></returns>
        public IHttpActionResult GetListaEquipoXEmpresaYFamilia(int emprcodi, int codifam)
        {
            try
            {
                return Ok(
                this.service.GetListaEquipoXEmpresaYFamilia(emprcodi, codifam)
             );
            }
            catch (Exception ex)
            {
                log.Error("GetListaEquipoXEmpresaYFamilia", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }

            
        }

        /// <summary>
        /// Obtener criterio de Equipo
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public IHttpActionResult GetByCriteriaEqequipo(int emprcodi, int famcodi)
        {
            try
            {
                return Ok(
               this.service.GetByCriteriaEqequipo(emprcodi, famcodi)
            );
            }
            catch (Exception ex)
            {
                log.Error("GetByCriteriaEqequipo", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }
            
        }

        /// <summary>
        /// Listar equipos propiedades AGC
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public IHttpActionResult GetListarEquiposPropiedadesAGC(string fecha)
        {
            try
            {
                return Ok(
                 this.service.ListarEquiposPropiedadesAGC(fecha)
              );

            }
            catch (Exception ex)
            {
                log.Error("GetListarEquiposPropiedadesAGC", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }
           
        }
        /// <summary>
        /// Obtener Valor Propiedad Equipo fecha
        /// </summary>
        /// <param name="propcodi"></param>
        /// <param name="equicodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public IHttpActionResult GetObtenerValorPropiedadEquipoFecha(int propcodi, int equicodi, string fecha)
        {
            try
            {
                return Ok(
              this.service.ObtenerValorPropiedadEquipoFecha(propcodi, equicodi, fecha)
           );
            }
            catch (Exception ex)
            {
                log.Error("GetObtenerValorPropiedadEquipoFecha", ex);
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Error en el lado del servidor", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                throw new HttpResponseException(response);
            }
           
        }       

        /// <summary>
        /// Listado Equipos por padre
        /// </summary>
        /// <param name="equipadre"></param>
        /// <returns></returns>
        public IHttpActionResult GetListarEquiposPorPadre(int equipadre)
        {
            try
            {
                return Ok(
              this.service.ListarEquiposPorPadre(equipadre)
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
        /// Listar equipos propiedades
        /// </summary>
        /// <param name="propcodi"></param>
        /// <param name="fecha"></param>
        /// <param name="emprCodi"></param>
        /// <param name="areacodi"></param>
        /// <param name="famCodi"></param>
        /// <param name="nroPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IHttpActionResult GetListarEquiposPropiedades(int propcodi, DateTime fecha, int emprCodi, int areacodi, int famCodi, int nroPage, int pageSize)
        {
            try
            {
                return Ok(
               this.service.ListarEquiposPropiedades(propcodi, fecha, emprCodi, areacodi, famCodi, nroPage, pageSize)
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