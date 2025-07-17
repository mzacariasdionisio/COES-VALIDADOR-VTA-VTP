using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using System.Configuration;

namespace COES.Servicios.Aplicacion.RegistroIntegrantes
{
    public class GestionSolicitudesAppServicio : AppServicioBase
    {
        /// <summary>
        /// Permite listar los tipos de solicitud
        /// </summary>
        /// <returns></returns>
        public List<RiTiposolicitudDTO> ListarTipoSolicitud()
        {
            return FactorySic.GetRiTiposolicitudRepository().List();
        }


        /// <summary>
        /// Permite listar las solicitudes
        /// </summary>
        /// <returns></returns>
        public List<RiSolicitudDTO> ListarSolicitud()
        {
            return FactorySic.GetRiSolicitudRepository().List();
        }

        /// <summary>
        /// Permite listar las solicitudes pendientes, según tipo de solicitud 
        /// </summary>
        /// <param name="soliestado">Estado de la solicitud</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<RiSolicitudDTO> ListarSolicitudesPendientes(string soliestado, int Page, int PageSize)
        {
            List<RiSolicitudDTO> lista = new List<RiSolicitudDTO>();

            lista = FactorySic.GetRiSolicitudRepository().ListPend(soliestado, Page, PageSize);

            return lista;
        }

        /// <summary>
        /// Permite listar las solicitudes pendientes, según tipo de solicitud y por empresa
        /// </summary>
        /// <param name="soliestado"></param>
        /// <param name="emprcodi"></param>
        /// <param name="Page"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public List<RiSolicitudDTO> ListarSolicitudesPendientesporEmpresa(string soliestado, int Page, int PageSize, int emprcodi)
        {
            List<RiSolicitudDTO> lista = new List<RiSolicitudDTO>();

            lista = FactorySic.GetRiSolicitudRepository().ListPendporEmpresa(soliestado, Page, PageSize, emprcodi);

            

            return lista;
        }

        /// <summary>
        /// Devuelve un listado de solicitudes segun el estado
        /// </summary>
        /// <param name="soliestado">Estado de la solicitud</param>
        /// <returns>Numero de registros</returns>
        public int ObtenerTotalRegListPend(string soliestado)
        {
            return FactorySic.GetRiSolicitudRepository().ObtenerTotalListPend(soliestado);
        }

        /// <summary>
        /// Obtiene el total de registros del listado, según tipo de solicitud y por empresa
        /// </summary>
        /// <param name="soliestado"></param>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public int ObtenerTotalRegListPendporEmpresa(string soliestado, int emprcodi)
        {
            return FactorySic.GetRiSolicitudRepository().ObtenerTotalListPendporEmpresa(soliestado, emprcodi);
        }
        /// <summary>
        /// Permite dar conformidad a una solicitud (Cambio estado a APROBADO_FISICAMENTE)
        /// </summary>
        /// <param name="solicodi"></param>
        /// <returns></returns>
        public int DarConformidad(int solicodi)
        {
            return FactorySic.GetRiSolicitudRepository().DarConformidad(solicodi);
        }

        /// <summary>
        /// Permite dar conformidad a una solicitud (Cambio estado a APROBADO_DIGITALMENTE)
        /// </summary>
        /// <param name="solicodi"></param>
        /// <returns></returns>
        public int DarNotificar(int solicodi)
        {
            return FactorySic.GetRiSolicitudRepository().DarNotificar(solicodi);
        }

        /// <summary>
        /// Obtiene una solicitud por su codigo
        /// </summary>
        /// <param name="solicodi"></param>
        /// <returns>Objeto Solicitud</returns>
        public RiSolicitudDTO GetById(int solicodi)
        {
            return FactorySic.GetRiSolicitudRepository().GetById(solicodi);
        }

        /// <summary>
        /// Obtiene el listado de detalles de solicitud por codigo
        /// </summary>
        /// <param name="solicodi"></param>
        /// <returns>Listado de objetos de Detalle de Solicitud</returns>
        public List<RiSolicituddetalleDTO> ListDetalleBySolicodi(int solicodi)
        {
            return FactorySic.GetRiSolicituddetalleRepository().ListBySolicodi(solicodi);
        }

        /// <summary>
        /// Obtiene los datos de empresa por su codigo
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public SiEmpresaDTO GetCabeceraSolicitudById(int emprcodi)
        {

            return FactorySic.GetSiEmpresaRIRepository().GetCabeceraSolicitudById(emprcodi);
        }

        /// <summary>
        /// Realiza la finalización de la solicitud
        /// </summary>
        /// <param name="solicodi"></param>
        /// <param name="estado"></param>
        /// <param name="observacion"></param>
        /// <returns></returns>
        public int FinalizarSolicitud(int solicodi, string estado, string observacion)
        {
            //Consultar observacion
            return FactorySic.GetRiSolicitudRepository().FinalizarSolicitud(solicodi, estado, observacion);

        }

        /// <summary>
        /// Realiza actualizacion de la fecha de proceso
        /// </summary>
        /// <param name="solicodi"></param>
        /// <param name="fecha"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int ActualizarFechaProceso(int solicodi, DateTime fecha, int usuario)
        {
            //Consultar observacion
            return FactorySic.GetRiSolicitudRepository().ActualizarFechaProceso(solicodi, fecha, usuario);

        }


        /// <summary>
        /// Permite guardar una solicitud
        /// </summary>
        /// <param name="objSolicitud"></param>
        /// <returns></returns>
        public int Save(RiSolicitudDTO objSolicitud)
        {
            return FactorySic.GetRiSolicitudRepository().Save(objSolicitud);
        }

        /// <summary>
        /// Permite guardar los detalles de una solicitud
        /// </summary>
        /// <param name="ListDetalles"></param>
        /// <returns></returns>
        public int SaveDetails(List<RiSolicituddetalleDTO> ListDetalles)
        {
            int retorno = 0;
            foreach (RiSolicituddetalleDTO item in ListDetalles)
            {
                retorno = FactorySic.GetRiSolicituddetalleRepository().Save(item);
            }
            return retorno;
        }


        public bool SolicitudEnCurso(int emprcodi, int codigoTipoSolicitud)
        {
            int retorno = FactorySic.GetRiSolicitudRepository().SolicitudEnCurso(emprcodi, codigoTipoSolicitud);
            if (retorno > 0)
                return true;
            else
                return false;
        }

        public void EnviarCorreoSolicitudAgente(int emprcodi, int tiposolicitud)
        {
            try
            {
                var appEmpresa = new EmpresaAppServicio();
                var model = appEmpresa.GetByIdSiEmpresa(emprcodi);                
                string empresa = (model.Emprnombrecomercial.Length > 3) ? model.Emprnombrecomercial : model.Emprrazsocial;
                RiTiposolicitudDTO entitySolicitud = FactorySic.GetRiTiposolicitudRepository().GetById(tiposolicitud);
                string solicitud = entitySolicitud.Tisonombre;
                string toEmail = ConfigurationManager.AppSettings["admRegistroIntegrantes"];
                string mensaje = RegistroIntegrantesHelper.Solicitud_BodyMailAgente(empresa, solicitud);

                COES.Base.Tools.Util.SendEmail(toEmail, string.Empty, "Notificacion de Solicitud: " + solicitud + " - Registro de Integrantes", mensaje);
            }
            catch 
            {
                
            }

        }
    }
}
