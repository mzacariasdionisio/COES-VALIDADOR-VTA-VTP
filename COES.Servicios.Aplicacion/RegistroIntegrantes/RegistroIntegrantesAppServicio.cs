using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System.Linq;
using System.Data;
using System.Data.Common;

namespace COES.Servicios.Aplicacion.RegistroIntegrantes
{
    /// <summary>
    /// Clases con métodos del módulo RegistroIntegrantes
    /// </summary>
    public class RegistroIntegrantesAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(RegistroIntegrantesAppServicio));

        #region Métodos Tabla RI_DETALLE_REVISION

        /// <summary>
        /// Inserta un registro de la tabla RI_DETALLE_REVISION
        /// </summary>
        public int SaveRiDetalleRevision(RiDetalleRevisionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            //try
            //{
            return FactorySic.GetRiDetalleRevisionRepository().Save(entity, conn, tran);
            //}
            //catch (Exception ex)
            //{
            //    Logger.Error(ConstantesAppServicio.LogError, ex);
            //    return 0;
            //    throw new Exception(ex.Message, ex);
            //}
        }


        /// <summary>
        /// Actualiza un registro de la tabla RI_DETALLE_REVISION
        /// </summary>
        public void UpdateRiDetalleRevision(RiDetalleRevisionDTO entity)
        {
            try
            {
                FactorySic.GetRiDetalleRevisionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla RI_DETALLE_REVISION
        /// </summary>
        public void DeleteRiDetalleRevision(int dervcodi)
        {
            try
            {
                FactorySic.GetRiDetalleRevisionRepository().Delete(dervcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RI_DETALLE_REVISION
        /// </summary>
        public RiDetalleRevisionDTO GetByIdRiDetalleRevision(int dervcodi)
        {
            return FactorySic.GetRiDetalleRevisionRepository().GetById(dervcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RI_DETALLE_REVISION
        /// </summary>
        public List<RiDetalleRevisionDTO> ListRiDetalleRevisions()
        {
            return FactorySic.GetRiDetalleRevisionRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RI_DETALLE_REVISION según Revicodi
        /// </summary>
        public List<RiDetalleRevisionDTO> ListRiDetalleRevisionByRevicodi(int revicodi)
        {
            return FactorySic.GetRiDetalleRevisionRepository().ListByRevicodi(revicodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RiDetalleRevision
        /// </summary>
        public List<RiDetalleRevisionDTO> GetByCriteriaRiDetalleRevisions()
        {
            return FactorySic.GetRiDetalleRevisionRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla RI_ETAPAREVISION

        /// <summary>
        /// Inserta un registro de la tabla RI_ETAPAREVISION
        /// </summary>
        public void SaveRiEtaparevision(RiEtaparevisionDTO entity)
        {
            try
            {
                FactorySic.GetRiEtaparevisionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla RI_ETAPAREVISION
        /// </summary>
        public void UpdateRiEtaparevision(RiEtaparevisionDTO entity)
        {
            try
            {
                FactorySic.GetRiEtaparevisionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla RI_ETAPAREVISION
        /// </summary>
        public void DeleteRiEtaparevision(int etrvcodi)
        {
            try
            {
                FactorySic.GetRiEtaparevisionRepository().Delete(etrvcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RI_ETAPAREVISION
        /// </summary>
        public RiEtaparevisionDTO GetByIdRiEtaparevision(int etrvcodi)
        {
            return FactorySic.GetRiEtaparevisionRepository().GetById(etrvcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RI_ETAPAREVISION
        /// </summary>
        public List<RiEtaparevisionDTO> ListRiEtaparevisions()
        {
            return FactorySic.GetRiEtaparevisionRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RiEtaparevision
        /// </summary>
        public List<RiEtaparevisionDTO> GetByCriteriaRiEtaparevisions()
        {
            return FactorySic.GetRiEtaparevisionRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla RI_REVISION

        /// <summary>
        /// Inserta un registro de la tabla RI_REVISION
        /// </summary>
        public int SaveRiRevision(RiRevisionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            //try
            //{
            return FactorySic.GetRiRevisionRepository().Save(entity, conn, tran);
            //}
            //catch (Exception ex)
            //{
            //    Logger.Error(ConstantesAppServicio.LogError, ex);
            //    return 0;
            //    throw new Exception(ex.Message, ex);
            //}
        }
        public bool SaveRevisionDJR(RiRevisionDTO entity)
        {
            bool resul = false;
            IDbConnection conn = null;
            DbTransaction tran = null;

            try
            {
                conn = FactorySic.GetSiEmpresaRIRepository().BeginConnection();
                tran = FactorySic.GetSiEmpresaRIRepository().StartTransaction(conn);

                this.UpdateRiRevision(entity, conn, tran);

                RiDetalleRevisionDTO objDR = new RiDetalleRevisionDTO();
                objDR.Dervfecmodificacion = DateTime.Now;
                objDR.Dervusumoficicacion = entity.Reviusucreacion;
                objDR.Revicodi = entity.Revicodi;
                objDR.Dervestado = "I";

                this.UpdateEstadoRiDetalleRevision(objDR, conn, tran);

                int Dervcodi = 0;
                foreach (var item in entity.Detalle)
                {
                    item.Revicodi = entity.Revicodi;
                    item.Dervcodi = Dervcodi;
                    Dervcodi = this.SaveRiDetalleRevision(item, conn, tran);
                }
                tran.Commit();
                resul = true;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                if (tran != null)
                    tran.Rollback();
            }
            finally
            {
                if (conn != null)
                    if (conn.State == ConnectionState.Open) conn.Close();
            }
            return resul;
        }

        public bool SaveRevisionSGI(RiRevisionDTO entity)
        {
            bool resul = false;
            IDbConnection conn = null;
            DbTransaction tran = null;

            try
            {
                conn = FactorySic.GetSiEmpresaRIRepository().BeginConnection();
                tran = FactorySic.GetSiEmpresaRIRepository().StartTransaction(conn);

                this.UpdateRiRevision(entity, conn, tran);

                RiDetalleRevisionDTO objDR = new RiDetalleRevisionDTO();
                objDR.Dervfecmodificacion = DateTime.Now;
                objDR.Dervusumoficicacion = entity.Reviusucreacion;
                objDR.Revicodi = entity.Revicodi;
                objDR.Dervestado = "I";

                this.UpdateEstadoRiDetalleRevision(objDR, conn, tran);

                int Dervcodi = 0;
                foreach (var item in entity.Detalle)
                {
                    item.Revicodi = entity.Revicodi;
                    item.Dervcodi = Dervcodi;
                    Dervcodi = this.SaveRiDetalleRevision(item, conn, tran);
                }
                tran.Commit();
                resul = true;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                if (tran != null)
                    tran.Rollback();
            }
            finally
            {
                if (conn != null)
                    if (conn.State == ConnectionState.Open) conn.Close();
            }
            return resul;
        }
        /// <summary>
        /// Actualiza un registro de la tabla RI_REVISION
        /// </summary>
        public void UpdateRiRevision(RiRevisionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            //try
            //{
            FactorySic.GetRiRevisionRepository().Update(entity, conn, tran);
            //}
            //catch (Exception ex)
            //{
            //    Logger.Error(ConstantesAppServicio.LogError, ex);
            //    throw new Exception(ex.Message, ex);
            //}
        }
        /// <summary>
        /// Actualiza un registro de la tabla RI_REVISION
        /// </summary>
        public void UpdateRiRevisionEstadoRegistroInactivo(RiRevisionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            //try
            //{
            FactorySic.GetRiRevisionRepository().UpdateEstadoRegistroInactivo(entity, conn, tran);
            //}
            //catch (Exception ex)
            //{
            //    Logger.Error(ConstantesAppServicio.LogError, ex);
            //    throw new Exception(ex.Message, ex);
            //}
        }

        public void UpdateEstadoRiDetalleRevision(RiDetalleRevisionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            //try
            //{
            FactorySic.GetRiDetalleRevisionRepository().UpdateEstado(entity, conn, tran);
            //}
            //catch (Exception ex)
            //{
            //    Logger.Error(ConstantesAppServicio.LogError, ex);
            //    throw new Exception(ex.Message, ex);
            //}
        }

        /// <summary>
        /// Elimina un registro de la tabla RI_REVISION
        /// </summary>
        public void DeleteRiRevision(int revicodi)
        {
            try
            {
                FactorySic.GetRiRevisionRepository().Delete(revicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite dar conformidad a una revisión (Cambio estado empresa a ACEPTADO FISICAMENTE)
        /// </summary>
        /// <param name="revicodi"></param>
        /// <param name="empresa">datos de empresa</param>
        /// <returns></returns>
        public int DarConformidad(int revicodi, SiEmpresaDTO empresa)
        {
            FactorySic.GetSiEmpresaRIRepository().ActualizarEstadoRegistro(empresa);
            FactorySic.GetSiEmpresaRIRepository().ActualizarFechaIngreso(empresa);
            return FactorySic.GetRiRevisionRepository().DarConformidad(revicodi);
        }


        /// <summary>
        /// Permite notificar el resultado de la revisión (Cambio estado empresa a ACEPTADO DIGITALMENTE)
        /// </summary>
        /// <param name="revicodi"></param>
        /// <param name="empresa">datos de empresa</param>
        /// <returns></returns>
        public int DarNotificar(int revicodi, SiEmpresaDTO empresa)
        {
            FactorySic.GetSiEmpresaRIRepository().ActualizarEstadoRegistro(empresa);
            return FactorySic.GetRiRevisionRepository().DarNotificar(revicodi);

        }

        /// <summary>
        /// Permite notificar el resultado de la revisión
        /// </summary>
        /// <param name="revicodi"></param>
        /// <returns></returns>
        public int DarNotificar(int revicodi)
        {
            return FactorySic.GetRiRevisionRepository().DarNotificar(revicodi);

        }

        /// <summary>
        /// Permite dar conformidad a una revisión interna
        /// </summary>
        /// <param name="revicodi"></param>
        /// <returns></returns>
        public int DarTerminar(int revicodi)
        {
            return FactorySic.GetRiRevisionRepository().DarTerminar(revicodi);

        }


        /// <summary>
        /// Permite devolver a pendiente una revisión (Cambio estado a PENDIENTE)
        /// </summary>
        /// <param name="revicodi"></param>
        /// <returns></returns>
        public int RevAsistente(int revicodi)
        {
            return FactorySic.GetRiRevisionRepository().RevAsistente(revicodi);

        }


        /// <summary>
        /// Permite obtener un registro de la tabla RI_REVISION
        /// </summary>
        public RiRevisionDTO GetByIdRiRevision(int revicodi)
        {
            return FactorySic.GetRiRevisionRepository().GetById(revicodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RI_REVISION
        /// </summary>
        public List<RiRevisionDTO> ListRiRevisions()
        {
            return FactorySic.GetRiRevisionRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RiRevision
        /// </summary>
        public List<RiRevisionDTO> GetByCriteriaRiRevisions()
        {
            return FactorySic.GetRiRevisionRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite listar los detalles de una revisión
        /// </summary>
        /// <param name="revicodi"></param>
        /// <returns></returns>
        public List<RiDetalleRevisionDTO> ListByRevicodi(int revicodi)
        {
            return FactorySic.GetRiDetalleRevisionRepository().ListByRevicodi(revicodi);
        }

        /// <summary>
        /// Permite listar revisiones según estado y tipo empresa
        /// </summary>
        /// <param name="estado"></param>
        /// <param name="tipemprcodi"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListByEstadoAndTipEmpr(string estado, int tipemprcodi, string nombre, int Page, int PageSize)
        {
            return FactorySic.GetRiRevisionRepository().ListByEstadoAndTipEmp(estado, tipemprcodi, nombre, Page, PageSize);
        }

        /// <summary>
        /// Permite obtener el total de registros del listado
        /// </summary>
        /// <param name="estado"></param>
        /// <param name="tipemprcodi"></param>
        /// <returns></returns>
        public int ObtenerTotalListByEstadoAndTipEmp(string estado, int tipemprcodi, string nombre)
        {
            return FactorySic.GetRiRevisionRepository().ObtenerTotalListByEstadoAndTipEmp(estado, tipemprcodi, nombre);
        }

        #endregion

        #region Métodos Tabla RI_SOLICITUD

        /// <summary>
        /// Inserta un registro de la tabla RI_SOLICITUD
        /// </summary>
        public void SaveRiSolicitud(RiSolicitudDTO entity)
        {
            try
            {
                FactorySic.GetRiSolicitudRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla RI_SOLICITUD
        /// </summary>
        public void UpdateRiSolicitud(RiSolicitudDTO entity)
        {
            try
            {
                FactorySic.GetRiSolicitudRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla RI_SOLICITUD
        /// </summary>
        public void DeleteRiSolicitud(int solicodi)
        {
            try
            {
                FactorySic.GetRiSolicitudRepository().Delete(solicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RI_SOLICITUD
        /// </summary>
        public RiSolicitudDTO GetByIdRiSolicitud(int solicodi)
        {
            return FactorySic.GetRiSolicitudRepository().GetById(solicodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RI_SOLICITUD
        /// </summary>
        public List<RiSolicitudDTO> ListRiSolicituds()
        {
            return FactorySic.GetRiSolicitudRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RiSolicitud
        /// </summary>
        public List<RiSolicitudDTO> GetByCriteriaRiSolicituds()
        {
            return FactorySic.GetRiSolicitudRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla RI_SOLICITUDDETALLE

        /// <summary>
        /// Inserta un registro de la tabla RI_SOLICITUDDETALLE
        /// </summary>
        public void SaveRiSolicituddetalle(RiSolicituddetalleDTO entity)
        {
            try
            {
                FactorySic.GetRiSolicituddetalleRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla RI_SOLICITUDDETALLE
        /// </summary>
        public void UpdateRiSolicituddetalle(RiSolicituddetalleDTO entity)
        {
            try
            {
                FactorySic.GetRiSolicituddetalleRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla RI_SOLICITUDDETALLE
        /// </summary>
        public void DeleteRiSolicituddetalle(int sdetcodi)
        {
            try
            {
                FactorySic.GetRiSolicituddetalleRepository().Delete(sdetcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RI_SOLICITUDDETALLE
        /// </summary>
        public RiSolicituddetalleDTO GetByIdRiSolicituddetalle(int sdetcodi)
        {
            return FactorySic.GetRiSolicituddetalleRepository().GetById(sdetcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RI_SOLICITUDDETALLE
        /// </summary>
        public List<RiSolicituddetalleDTO> ListRiSolicituddetalles()
        {
            return FactorySic.GetRiSolicituddetalleRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RiSolicituddetalle
        /// </summary>
        public List<RiSolicituddetalleDTO> GetByCriteriaRiSolicituddetalles()
        {
            return FactorySic.GetRiSolicituddetalleRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla RI_TIPOSOLICITUD

        /// <summary>
        /// Inserta un registro de la tabla RI_TIPOSOLICITUD
        /// </summary>
        public void SaveRiTiposolicitud(RiTiposolicitudDTO entity)
        {
            try
            {
                FactorySic.GetRiTiposolicitudRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla RI_TIPOSOLICITUD
        /// </summary>
        public void UpdateRiTiposolicitud(RiTiposolicitudDTO entity)
        {
            try
            {
                FactorySic.GetRiTiposolicitudRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla RI_TIPOSOLICITUD
        /// </summary>
        public void DeleteRiTiposolicitud(int tisocodi)
        {
            try
            {
                FactorySic.GetRiTiposolicitudRepository().Delete(tisocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RI_TIPOSOLICITUD
        /// </summary>
        public RiTiposolicitudDTO GetByIdRiTiposolicitud(int tisocodi)
        {
            return FactorySic.GetRiTiposolicitudRepository().GetById(tisocodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RI_TIPOSOLICITUD
        /// </summary>
        public List<RiTiposolicitudDTO> ListRiTiposolicituds()
        {
            return FactorySic.GetRiTiposolicitudRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RiTiposolicitud
        /// </summary>
        public List<RiTiposolicitudDTO> GetByCriteriaRiTiposolicituds()
        {
            return FactorySic.GetRiTiposolicitudRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla SI_REPRESENTANTE

        /// <summary>
        /// Inserta un registro de la tabla SI_REPRESENTANTE
        /// </summary>
        public int SaveSiRepresentante(SiRepresentanteDTO entity, IDbConnection conn, DbTransaction tran)
        {
            int id = 0;
            //try
            //{
            id = FactorySic.GetSiRepresentanteRepository().Save(entity, conn, tran);
            //}
            //catch (Exception ex)
            //{
            //    Logger.Error(ConstantesAppServicio.LogError, ex);
            //    throw new Exception(ex.Message, ex);
            //}
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_REPRESENTANTE
        /// </summary>
        public void UpdateSiRepresentante(SiRepresentanteDTO entity)
        {
            try
            {
                FactorySic.GetSiRepresentanteRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_REPRESENTANTE
        /// </summary>
        public void DeleteSiRepresentante(int rptecodi)
        {
            try
            {
                FactorySic.GetSiRepresentanteRepository().Delete(rptecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_REPRESENTANTE
        /// </summary>
        public SiRepresentanteDTO GetByIdSiRepresentante(int rptecodi)
        {
            return FactorySic.GetSiRepresentanteRepository().GetById(rptecodi);
        }

        /// <summary>
        /// Permite obtener registros de la tabla SI_REPRESENTANTE según su EMPRCODI
        /// </summary>
        public List<SiRepresentanteDTO> GetRepresentantesByEmprcodi(int emprcodi)
        {
            return FactorySic.GetSiRepresentanteRepository().GetByEmprcodi(emprcodi);
        }


        /// <summary>
        /// Permite listar todos los registros de la tabla SI_REPRESENTANTE
        /// </summary>
        public List<SiRepresentanteDTO> ListSiRepresentantes()
        {
            return FactorySic.GetSiRepresentanteRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiRepresentante
        /// </summary>
        public List<SiRepresentanteDTO> GetByCriteriaSiRepresentantes()
        {
            return FactorySic.GetSiRepresentanteRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla SI_TIPO_COMPORTAMIENTO


        /// <summary>
        /// Inserta un registro de la tabla SI_TIPO_COMPORTAMIENTO
        /// </summary>
        /// 
        public void SaveSiTipoComportamiento(SiTipoComportamientoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            //try
            //{
            FactorySic.GetSiTipoComportamientoRepository().Save(entity, conn, tran);
            //}
            //catch (Exception ex)
            //{
            //    Logger.Error(ConstantesAppServicio.LogError, ex);
            //    throw new Exception(ex.Message, ex);
            //}
        }




        /// <summary>
        /// Actualiza un registro de la tabla SI_TIPO_COMPORTAMIENTO
        /// </summary>
        public void UpdateSiTipoComportamiento(SiTipoComportamientoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetSiTipoComportamientoRepository().Update(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza el campo Estado de Registro
        /// </summary>
        public void UpdateSiRepresentanteEstadoRegistro(SiRepresentanteDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetSiRepresentanteRepository().UpdateEstadoRegistro(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_TIPO_COMPORTAMIENTO
        /// </summary>
        public void DeleteSiTipoComportamiento(int tipocodi)
        {
            try
            {
                FactorySic.GetSiTipoComportamientoRepository().Delete(tipocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_TIPO_COMPORTAMIENTO
        /// </summary>
        public SiTipoComportamientoDTO GetByIdSiTipoComportamiento(int tipocodi)
        {
            return FactorySic.GetSiTipoComportamientoRepository().GetById(tipocodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_TIPO_COMPORTAMIENTO
        /// </summary>
        public List<SiTipoComportamientoDTO> ListSiTipoComportamientos()
        {
            return FactorySic.GetSiTipoComportamientoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiTipoComportamiento
        /// </summary>
        public List<SiTipoComportamientoDTO> GetByCriteriaSiTipoComportamientos()
        {
            return FactorySic.GetSiTipoComportamientoRepository().GetByCriteria();
        }

        public List<SiTipoComportamientoDTO> ListSiTipoComportamientoByEmprcodi(int emprcodi)
        {
            return FactorySic.GetSiTipoComportamientoRepository().ListByEmprcodi(emprcodi);
        }

        #endregion

        #region Métodos Tabla SI_TIPO_EMPRESA

        public List<SiTipoempresaDTO> ListTipoEmpresa()
        {
            return FactorySic.GetSiTipoempresaRepository().List().Where(x => x.Tipoemprcodi != 5).ToList();
        }

        public int UpdateSiEmpresaCorrelativoRI(int emprcodi)
        {
            int nroRegistro = FactorySic.GetSiEmpresaRIRepository().ObtenerNroRegistro();
            FactorySic.GetSiEmpresaRIRepository().ActualizarEmpresaNroRegistro(emprcodi, nroRegistro);

            return nroRegistro;
        }
        public string SaveSIEmpresa(SiEmpresaDTO entity)
        {
            IDbConnection conn = null;
            DbTransaction tran = null;
            int id = 0;
            string para = "";
            try
            {
                conn = FactorySic.GetSiEmpresaRIRepository().BeginConnection();
                tran = FactorySic.GetSiEmpresaRIRepository().StartTransaction(conn);

                int nroConstancia = FactorySic.GetSiEmpresaRIRepository().ObtenerNroConstancia();
                entity.Emprnroconstancia = nroConstancia;

                bool flagRuc = FactorySic.GetSiEmpresaRepository().VerificarExistenciaPorRuc(entity.Emprruc);

                if (flagRuc == true)
                {
                    SiEmpresaDTO entityEmpresa = FactorySic.GetSiEmpresaRepository().ObtenerEmpresaPorRuc(entity.Emprruc);
                    entity.Emprcodi = entityEmpresa.Emprcodi;
                    FactorySic.GetSiEmpresaRepository().UpdateRI(entity);
                    id = entity.Emprcodi;
                }
                else
                {
                    id = FactorySic.GetSiEmpresaRepository().Save(entity);
                }


                para = id + "," + nroConstancia;

                if (id > 0)
                {
                    entity.TipoComportamiento.Emprcodi = id;
                    this.SaveSiTipoComportamiento(entity.TipoComportamiento, conn, tran);
                    int Rptecodi = 0;
                    foreach (var item in entity.Representante)
                    {
                        item.Emprcodi = id;
                        item.Rptecodi = Rptecodi;
                        item.Rpteindnotic = ConstantesAppServicio.NO;
                        Rptecodi = this.SaveSiRepresentante(item, conn, tran);
                    }

                    // Revision
                    RiRevisionDTO entityRVSGI = new RiRevisionDTO();
                    entityRVSGI.Etrvcodi = ConstantesRegistroIntegrantes.EtrvSGI;
                    entityRVSGI.Reviestadoregistro = ConstantesRegistroIntegrantes.RevisionEstadoRegistroActivo;
                    entityRVSGI.Emprcodi = id;
                    entityRVSGI.Reviiteracion = 1;
                    entityRVSGI.Reviestado = ConstantesRegistroIntegrantes.RevisionEstadoPendiente;

                    entityRVSGI.Reviusucreacion = ConstantesRegistroIntegrantes.RevisionUsuarioCreacionWeb;
                    entityRVSGI.Revifeccreacion = DateTime.Now;

                    entityRVSGI.Reviusumodificacion = null;
                    entityRVSGI.Revifecmodificacion = null;

                    entityRVSGI.Revifinalizado = ConstantesRegistroIntegrantes.RevisionFinalizadoNo;
                    entityRVSGI.Revifecfinalizado = null;

                    entityRVSGI.Revinotificado = ConstantesRegistroIntegrantes.RevisionNotificadoNo;
                    entityRVSGI.Revifecnotificado = null;

                    entityRVSGI.Reviterminado = ConstantesRegistroIntegrantes.RevisionTerminadoNo;
                    entityRVSGI.Revifecterminado = null;

                    entityRVSGI.Revienviado = ConstantesRegistroIntegrantes.RevisionEnviadoNo;
                    entityRVSGI.Revifecenviado = null;


                    int IdRVSGI = this.SaveRiRevision(entityRVSGI, conn, tran);
                    entityRVSGI.Revicodi = IdRVSGI;

                    RiRevisionDTO entityRVDJR = new RiRevisionDTO();
                    entityRVDJR.Etrvcodi = ConstantesRegistroIntegrantes.EtrvDJR;
                    entityRVDJR.Reviestadoregistro = ConstantesRegistroIntegrantes.RevisionEstadoRegistroActivo;
                    entityRVDJR.Emprcodi = id;
                    entityRVDJR.Reviiteracion = 1;
                    entityRVDJR.Reviestado = ConstantesRegistroIntegrantes.RevisionEstadoPendiente;

                    entityRVDJR.Reviusucreacion = ConstantesRegistroIntegrantes.RevisionUsuarioCreacionWeb;
                    entityRVDJR.Revifeccreacion = DateTime.Now;

                    entityRVDJR.Reviusumodificacion = null;
                    entityRVDJR.Revifecmodificacion = null;

                    entityRVDJR.Revifinalizado = ConstantesRegistroIntegrantes.RevisionFinalizadoNo;
                    entityRVDJR.Revifecfinalizado = null;

                    entityRVDJR.Revinotificado = ConstantesRegistroIntegrantes.RevisionNotificadoNo;
                    entityRVDJR.Revifecnotificado = null;

                    entityRVDJR.Reviterminado = ConstantesRegistroIntegrantes.RevisionTerminadoNo;
                    entityRVDJR.Revifecterminado = null;

                    entityRVDJR.Revienviado = ConstantesRegistroIntegrantes.RevisionEnviadoNo;
                    entityRVDJR.Revifecenviado = null;


                    entityRVDJR.Revicodi = IdRVSGI;
                    int IdRVDJR = this.SaveRiRevision(entityRVDJR, conn, tran);
                    entityRVDJR.Revicodi = IdRVDJR;

                    //- EmpresaDat
                    //SiEmpresadatDTO empresaDat = new SiEmpresadatDTO();
                    //FactorySic.GetSiEmpresadatRepository().Save(empresaDat);

                    tran.Commit();
                }
                else
                {
                    id = 0;
                    tran.Rollback();
                }
            }
            catch (Exception ex)
            {

                id = 0;
                if (tran != null)
                    tran.Rollback();
                para = "";
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
            finally
            {
                if (conn != null)
                    if (conn.State == ConnectionState.Open) conn.Close();
            }

            return para;

        }

        public int SaveSIEmpresaRe(SiEmpresaDTO entity)
        {

            IDbConnection conn = null;
            DbTransaction tran = null;
            int id = entity.Emprcodi;

            try
            {
                conn = FactorySic.GetSiEmpresaRIRepository().BeginConnection();
                tran = FactorySic.GetSiEmpresaRIRepository().StartTransaction(conn);

                if (entity.Emprcodi > 0)
                {
                    FactorySic.GetSiEmpresaRIRepository().ActualizarCondicion(entity);

                    entity.TipoComportamiento.Tipofecmodificacion = DateTime.Now;
                    entity.TipoComportamiento.Emprcodi = entity.Emprcodi;
                    this.UpdateSiTipoComportamiento(entity.TipoComportamiento, conn, tran);

                    //this.UpdateSiRepresentanteEstadoRegistro(entity.Emprcodi, conn, tran);

                    int Rptecodi = 0;
                    foreach (var item in entity.Representante)
                    {
                        if (item.RpteObservacion != "")
                        {
                            this.UpdateSiRepresentanteEstadoRegistro(item, conn, tran);
                            item.Emprcodi = id;
                            item.Rptecodi = Rptecodi;
                            Rptecodi = this.SaveSiRepresentante(item, conn, tran);
                        }
                    }


                    var objEmpresaRevi = this.GetEmpresaByIdConRevision(id);

                    // Revision
                    int IdRVSGI = 0;
                    if (entity.ReviiteracionSGI > 0 && objEmpresaRevi.ReviEstadoSGI == ConstantesRegistroIntegrantes.RevisionEstadoObservado)
                    {

                        RiRevisionDTO entityRVSGI = new RiRevisionDTO();
                        entityRVSGI.Etrvcodi = ConstantesRegistroIntegrantes.EtrvSGI;
                        entityRVSGI.Reviestadoregistro = ConstantesRegistroIntegrantes.RevisionEstadoRegistroActivo;
                        entityRVSGI.Emprcodi = id;
                        entityRVSGI.Reviiteracion = entity.ReviiteracionSGI + 1;
                        entityRVSGI.Reviestado = ConstantesRegistroIntegrantes.RevisionEstadoPendiente;

                        entityRVSGI.Reviusucreacion = ConstantesRegistroIntegrantes.RevisionUsuarioCreacionWeb;
                        entityRVSGI.Revifeccreacion = DateTime.Now;

                        entityRVSGI.Reviusumodificacion = null;
                        entityRVSGI.Revifecmodificacion = null;

                        entityRVSGI.Revifinalizado = ConstantesRegistroIntegrantes.RevisionFinalizadoNo;
                        entityRVSGI.Revifecfinalizado = null;

                        entityRVSGI.Revinotificado = ConstantesRegistroIntegrantes.RevisionNotificadoNo;
                        entityRVSGI.Revifecnotificado = null;

                        entityRVSGI.Reviterminado = ConstantesRegistroIntegrantes.RevisionTerminadoNo;
                        entityRVSGI.Revifecterminado = null;

                        entityRVSGI.Revienviado = ConstantesRegistroIntegrantes.RevisionEnviadoNo;
                        entityRVSGI.Revifecenviado = null;

                        this.UpdateRiRevisionEstadoRegistroInactivo(entityRVSGI, conn, tran);

                        IdRVSGI = this.SaveRiRevision(entityRVSGI, conn, tran);
                        entityRVSGI.Revicodi = IdRVSGI;

                    }

                    if (entity.ReviiteracionDRJ > 0 && objEmpresaRevi.ReviEstadoDJR == ConstantesRegistroIntegrantes.RevisionEstadoObservado)
                    {
                        RiRevisionDTO entityRVDJR = new RiRevisionDTO();
                        entityRVDJR.Etrvcodi = ConstantesRegistroIntegrantes.EtrvDJR;
                        entityRVDJR.Reviestadoregistro = ConstantesRegistroIntegrantes.RevisionEstadoRegistroActivo;
                        entityRVDJR.Emprcodi = id;
                        entityRVDJR.Reviiteracion = entity.ReviiteracionDRJ + 1;
                        entityRVDJR.Reviestado = ConstantesRegistroIntegrantes.RevisionEstadoPendiente;

                        entityRVDJR.Reviusucreacion = ConstantesRegistroIntegrantes.RevisionUsuarioCreacionWeb;
                        entityRVDJR.Revifeccreacion = DateTime.Now;

                        entityRVDJR.Reviusumodificacion = null;
                        entityRVDJR.Revifecmodificacion = null;

                        entityRVDJR.Revifinalizado = ConstantesRegistroIntegrantes.RevisionFinalizadoNo;
                        entityRVDJR.Revifecfinalizado = null;

                        entityRVDJR.Revinotificado = ConstantesRegistroIntegrantes.RevisionNotificadoNo;
                        entityRVDJR.Revifecnotificado = null;

                        entityRVDJR.Reviterminado = ConstantesRegistroIntegrantes.RevisionTerminadoNo;
                        entityRVDJR.Revifecterminado = null;

                        entityRVDJR.Revienviado = ConstantesRegistroIntegrantes.RevisionEnviadoNo;
                        entityRVDJR.Revifecenviado = null;


                        entityRVDJR.Revicodi = IdRVSGI;
                        this.UpdateRiRevisionEstadoRegistroInactivo(entityRVDJR, conn, tran);
                        int IdRVDJR = this.SaveRiRevision(entityRVDJR, conn, tran);
                        entityRVDJR.Revicodi = IdRVDJR;
                    }



                    tran.Commit();
                }
                else
                {
                    id = 0;
                    tran.Rollback();
                }
            }
            catch (Exception ex)
            {

                id = 0;
                if (tran != null)
                    tran.Rollback();

                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
            finally
            {
                if (conn != null)
                    if (conn.State == ConnectionState.Open) conn.Close();
            }

            return id;

        }

        #endregion

        #region Métodos Tabla SI_EMPRESA

        /// <summary>
        /// Obtiene los datos de empresa por su codigo
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public SiEmpresaDTO GetEmpresaById(int emprcodi)
        {
            return FactorySic.GetSiEmpresaRepository().GetById(emprcodi);
        }

        public SiEmpresaDTO GetEmpresaByIdConRevision(int emprcodi)
        {
            return FactorySic.GetSiEmpresaRIRepository().GetByIdConRevision(emprcodi);
        }

        /// <summary>
        /// Permite obtener a los generadores integrantes
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaMMEDTO> ObtenerAgentesParticipantes(int tipo)
        {
            return FactorySic.GetSiEmpresaRIRepository().ObtenerAgentesParticipantes(tipo);
        }

        #endregion
    }
}
