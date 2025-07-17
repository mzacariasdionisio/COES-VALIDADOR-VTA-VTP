using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla RE_INTERRUPCION_SUMINISTRO_DET
    /// </summary>
    public class ReInterrupcionSuministroDetRepository: RepositoryBase, IReInterrupcionSuministroDetRepository
    {
        public ReInterrupcionSuministroDetRepository(string strConn): base(strConn)
        {
        }

        ReInterrupcionSuministroDetHelper helper = new ReInterrupcionSuministroDetHelper();

        public int Save(ReInterrupcionSuministroDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Reintdcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Reintdevidenciaresp, DbType.String, entity.Reintdevidenciaresp);
            dbProvider.AddInParameter(command, helper.Reintdconformidadsumi, DbType.String, entity.Reintdconformidadsumi);
            dbProvider.AddInParameter(command, helper.Reintdcomentariosumi, DbType.String, entity.Reintdcomentariosumi);
            dbProvider.AddInParameter(command, helper.Reintdevidenciasumi, DbType.String, entity.Reintdevidenciasumi);
            dbProvider.AddInParameter(command, helper.Reintcodi, DbType.Int32, entity.Reintcodi);
            dbProvider.AddInParameter(command, helper.Reintdestado, DbType.String, entity.Reintdestado);
            dbProvider.AddInParameter(command, helper.Reintdorden, DbType.Int32, entity.Reintdorden);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Reintdorcentaje, DbType.Decimal, entity.Reintdorcentaje);
            dbProvider.AddInParameter(command, helper.Reintdconformidadresp, DbType.String, entity.Reintdconformidadresp);
            dbProvider.AddInParameter(command, helper.Reintdobservacionresp, DbType.String, entity.Reintdobservacionresp);
            dbProvider.AddInParameter(command, helper.Reintddetalleresp, DbType.String, entity.Reintddetalleresp);
            dbProvider.AddInParameter(command, helper.Reintdcomentarioresp, DbType.String, entity.Reintdcomentarioresp);
            dbProvider.AddInParameter(command, helper.Reintddisposicion, DbType.String, entity.Reintddisposicion);
            dbProvider.AddInParameter(command, helper.Reintdcompcero, DbType.String, entity.Reintdcompcero);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(ReInterrupcionSuministroDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Reintdevidenciaresp, DbType.String, entity.Reintdevidenciaresp);
            dbProvider.AddInParameter(command, helper.Reintdconformidadsumi, DbType.String, entity.Reintdconformidadsumi);
            dbProvider.AddInParameter(command, helper.Reintdcomentariosumi, DbType.String, entity.Reintdcomentariosumi);
            dbProvider.AddInParameter(command, helper.Reintdevidenciasumi, DbType.String, entity.Reintdevidenciasumi);
            dbProvider.AddInParameter(command, helper.Reintcodi, DbType.Int32, entity.Reintcodi);
            dbProvider.AddInParameter(command, helper.Reintdestado, DbType.String, entity.Reintdestado);
            dbProvider.AddInParameter(command, helper.Reintdorden, DbType.Int32, entity.Reintdorden);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Reintdorcentaje, DbType.Decimal, entity.Reintdorcentaje);
            dbProvider.AddInParameter(command, helper.Reintdconformidadresp, DbType.String, entity.Reintdconformidadresp);
            dbProvider.AddInParameter(command, helper.Reintdobservacionresp, DbType.String, entity.Reintdobservacionresp);
            dbProvider.AddInParameter(command, helper.Reintddetalleresp, DbType.String, entity.Reintddetalleresp);
            dbProvider.AddInParameter(command, helper.Reintdcomentarioresp, DbType.String, entity.Reintdcomentarioresp);
            dbProvider.AddInParameter(command, helper.Reintddisposicion, DbType.String, entity.Reintddisposicion);
            dbProvider.AddInParameter(command, helper.Reintdcompcero, DbType.String, entity.Reintdcompcero);
            dbProvider.AddInParameter(command, helper.Reintdcodi, DbType.Int32, entity.Reintdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int reintdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Reintdcodi, DbType.Int32, reintdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public ReInterrupcionSuministroDetDTO GetById(int reintdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Reintdcodi, DbType.Int32, reintdcodi);
            ReInterrupcionSuministroDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ReInterrupcionSuministroDetDTO> List()
        {
            List<ReInterrupcionSuministroDetDTO> entitys = new List<ReInterrupcionSuministroDetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<ReInterrupcionSuministroDetDTO> GetByCriteria()
        {
            List<ReInterrupcionSuministroDetDTO> entitys = new List<ReInterrupcionSuministroDetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<ReInterrupcionSuministroDetDTO> ObtenerPorEmpresaPeriodo(int idEmpresa, int idPeriodo)
        {
            List<ReInterrupcionSuministroDetDTO> entitys = new List<ReInterrupcionSuministroDetDTO>();
            string sql = string.Format(helper.SqlObtenerPorEmpresaPeriodo, idEmpresa, idPeriodo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<ReInterrupcionSuministroDetDTO> ObtenerInterrupcionesPorResponsable(int idEmpresa, int idPeriodo)
        {
            List<ReInterrupcionSuministroDetDTO> entitys = new List<ReInterrupcionSuministroDetDTO>();
            string sql = string.Format(helper.SqlObtenerInterrupcionesPorResponsable, idEmpresa, idPeriodo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public void ActualizarObservacion(ReInterrupcionSuministroDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarObservacion);

            dbProvider.AddInParameter(command, helper.Reintdconformidadresp, DbType.String, entity.Reintdconformidadresp);
            dbProvider.AddInParameter(command, helper.Reintdobservacionresp, DbType.String, entity.Reintdobservacionresp);
            dbProvider.AddInParameter(command, helper.Reintddetalleresp, DbType.String, entity.Reintddetalleresp);
            dbProvider.AddInParameter(command, helper.Reintdcomentarioresp, DbType.String, entity.Reintdcomentarioresp);
            dbProvider.AddInParameter(command, helper.Reintdevidenciaresp, DbType.String, entity.Reintdevidenciaresp);
            dbProvider.AddInParameter(command, helper.Reintdcodi, DbType.Int32, entity.Reintdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizarRespuesta(ReInterrupcionSuministroDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarRespuesta);

            dbProvider.AddInParameter(command, helper.Reintdconformidadsumi, DbType.String, entity.Reintdconformidadsumi);
            dbProvider.AddInParameter(command, helper.Reintdcomentariosumi, DbType.String, entity.Reintdcomentariosumi);
            dbProvider.AddInParameter(command, helper.Reintdevidenciasumi, DbType.String, entity.Reintdevidenciasumi);           
            dbProvider.AddInParameter(command, helper.Reintdcodi, DbType.Int32, entity.Reintdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizarDatosAdicionales(ReInterrupcionSuministroDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarDatosAdicionales);

            dbProvider.AddInParameter(command, helper.Reintddisposicion, DbType.String, entity.Reintddisposicion);
            dbProvider.AddInParameter(command, helper.Reintdcompcero, DbType.String, entity.Reintdcompcero);
            dbProvider.AddInParameter(command, helper.Reintcodi, DbType.Int32, entity.Reintcodi);
            dbProvider.AddInParameter(command, helper.Reintdorden, DbType.Int32, entity.Reintdorden);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public ReInterrupcionSuministroDetDTO ObtenerPorOrden(int idSuministro, int orden)
        {
            string sql = string.Format(helper.SqlObtenerPorOrden, idSuministro, orden);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
                       
            ReInterrupcionSuministroDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ReEmpresaDTO> ObtenerResponsablesFinalesPorPeriodo(int repercodi)
        {
            List<ReEmpresaDTO> entitys = new List<ReEmpresaDTO>();

            string sql = string.Format(helper.SqlGetResponsablesFinalPorPeriodo, repercodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReEmpresaDTO entity = new ReEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ReInterrupcionSuministroDetDTO> GetConformidadResponsableNO(int repercodi)
        {
            List<ReInterrupcionSuministroDetDTO> entitys = new List<ReInterrupcionSuministroDetDTO>();            
            string sql = string.Format(helper.SqlGetConformidadResponsableNO, repercodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {                

                while (dr.Read())
                {
                    ReInterrupcionSuministroDetDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iSumId = dr.GetOrdinal(helper.SumId);
                    if (!dr.IsDBNull(iSumId)) entity.SumId = Convert.ToInt32(dr.GetValue(iSumId));

                    int iSumNomb = dr.GetOrdinal(helper.SumNomb);
                    if (!dr.IsDBNull(iSumNomb)) entity.SumNomb = dr.GetString(iSumNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void ActualizarArchivoObservacion(int id, string extension) 
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarArchivoObservacion);

            dbProvider.AddInParameter(command, helper.Reintdevidenciaresp, DbType.String, extension);
            dbProvider.AddInParameter(command, helper.Reintcodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizarArchivoRespuesta(int id, string extension)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarArchivoRespuesta);

            dbProvider.AddInParameter(command, helper.Reintdevidenciasumi, DbType.String, extension);
            dbProvider.AddInParameter(command, helper.Reintcodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizarDesdeTrimestral(int idInterrupcionSemestral, int idInterrupcionTrimestral)
        {
            string sql = string.Format(helper.SqlActualizarDesdeTrimestral, idInterrupcionSemestral, idInterrupcionTrimestral);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<ReInterrupcionSuministroDetDTO> ObtenerRegistrosConSustento(int idInterrupcionSemestral, int idInterrupcionTrimestral)
        {
            List<ReInterrupcionSuministroDetDTO> entitys = new List<ReInterrupcionSuministroDetDTO>();
            string sql = string.Format(helper.SqlObtenerRegistrosConSustento, idInterrupcionSemestral, idInterrupcionTrimestral);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {          
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

    }
}
