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
    /// Clase de acceso a datos de la tabla AUD_AUDITORIAELEMENTO
    /// </summary>
    public class AudAuditoriaprocesoRepository: RepositoryBase, IAudAuditoriaprocesoRepository
    {
        public AudAuditoriaprocesoRepository(string strConn): base(strConn)
        {
        }

        AudAuditoriaprocesoHelper helper = new AudAuditoriaprocesoHelper();

        public int Save(AudAuditoriaprocesoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Audipcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Audipsplanificado, DbType.String, entity.Audipsplanificado);
            dbProvider.AddInParameter(command, helper.Audipactivo, DbType.String, entity.Audipactivo);
            dbProvider.AddInParameter(command, helper.Audiphistorico, DbType.String, entity.Audiphistorico);
            dbProvider.AddInParameter(command, helper.Audipusucreacion, DbType.String, entity.Audipusucreacion);
            dbProvider.AddInParameter(command, helper.Audipfeccreacion, DbType.DateTime, entity.Audipfeccreacion);
            dbProvider.AddInParameter(command, helper.Audipusumodificacion, DbType.String, entity.Audipusumodificacion);
            dbProvider.AddInParameter(command, helper.Audipfecmodificacion, DbType.DateTime, entity.Audipfecmodificacion);
            dbProvider.AddInParameter(command, helper.Audicodi, DbType.Int32, entity.Audicodi);
            dbProvider.AddInParameter(command, helper.Audppcodi, DbType.Int32, entity.Audppcodi);
            dbProvider.AddInParameter(command, helper.Proccodi, DbType.Int32, entity.Proccodi);
            
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AudAuditoriaprocesoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Audipsplanificado, DbType.String, entity.Audipsplanificado);
            dbProvider.AddInParameter(command, helper.Audipactivo, DbType.String, entity.Audipactivo);
            dbProvider.AddInParameter(command, helper.Audiphistorico, DbType.String, entity.Audiphistorico);
            dbProvider.AddInParameter(command, helper.Audipusucreacion, DbType.String, entity.Audipusucreacion);
            dbProvider.AddInParameter(command, helper.Audipfeccreacion, DbType.DateTime, entity.Audipfeccreacion);
            dbProvider.AddInParameter(command, helper.Audipusumodificacion, DbType.String, entity.Audipusumodificacion);
            dbProvider.AddInParameter(command, helper.Audipfecmodificacion, DbType.DateTime, entity.Audipfecmodificacion);
            dbProvider.AddInParameter(command, helper.Audicodi, DbType.Int32, entity.Audicodi);
            dbProvider.AddInParameter(command, helper.Audppcodi, DbType.Int32, entity.Audppcodi);
            dbProvider.AddInParameter(command, helper.Proccodi, DbType.Int32, entity.Proccodi);
            dbProvider.AddInParameter(command, helper.Audipcodi, DbType.Int32, entity.Audipcodi);
           
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(AudAuditoriaprocesoDTO auditoriaproceso)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Audipusumodificacion, DbType.String, auditoriaproceso.Audipusumodificacion);
            dbProvider.AddInParameter(command, helper.Audipfecmodificacion, DbType.DateTime, auditoriaproceso.Audipfecmodificacion);
            dbProvider.AddInParameter(command, helper.Audipcodi, DbType.Int32, auditoriaproceso.Audipcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteAllAudAuditoriaproceso(AudAuditoriaprocesoDTO auditoriaproceso)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteAllAudAuditoriaelemento);

            dbProvider.AddInParameter(command, helper.Audipusumodificacion, DbType.String, auditoriaproceso.Audipusumodificacion);
            dbProvider.AddInParameter(command, helper.Audipfecmodificacion, DbType.DateTime, auditoriaproceso.Audipfecmodificacion);
            dbProvider.AddInParameter(command, helper.Audicodi, DbType.Int32, auditoriaproceso.Audicodi);

            dbProvider.ExecuteNonQuery(command);
        }
        
        public AudAuditoriaprocesoDTO GetById(int Audipcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Audipcodi, DbType.Int32, Audipcodi);
            AudAuditoriaprocesoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public AudAuditoriaprocesoDTO GetByAudppcodi(int audicodi, int Audppcodi, int Proccodi)
        {
            string condicion = "";

            if (Audppcodi > 0) {
                condicion = " audppcodi = " + Audppcodi;
            }

            if (Proccodi > 0)
            {
                condicion = " proccodi = " + Proccodi;
            }

            condicion += " and audicodi = " + audicodi;

            string sql = string.Format(helper.SqlGetByAudppcodi, condicion);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            AudAuditoriaprocesoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<AudAuditoriaprocesoDTO> List(int audpcodi)
        {
            List<AudAuditoriaprocesoDTO> entitys = new List<AudAuditoriaprocesoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Audppcodi, DbType.Int32, audpcodi);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudAuditoriaprocesoDTO entity = helper.Create(dr);

                    int iProccodi = dr.GetOrdinal(helper.Proccodi);
                    if (!dr.IsDBNull(iProccodi)) entity.Proccodi = Convert.ToInt32(dr.GetValue(iProccodi));

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<AudAuditoriaprocesoDTO> GetByCriteria(int audicodi)
        {
            List<AudAuditoriaprocesoDTO> entitys = new List<AudAuditoriaprocesoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Audicodi, DbType.Int32, audicodi);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudAuditoriaprocesoDTO entity = new AudAuditoriaprocesoDTO();

                    int iAudicodi = dr.GetOrdinal(helper.Audicodi);
                    if (!dr.IsDBNull(iAudicodi)) entity.Audicodi = Convert.ToInt32(dr.GetValue(iAudicodi));

                    int iProccodi = dr.GetOrdinal(helper.Proccodi);
                    if (!dr.IsDBNull(iProccodi)) entity.Proccodi = Convert.ToInt32(dr.GetValue(iProccodi));

                    int iProcdescripcion = dr.GetOrdinal(helper.Procdescripcion);
                    if (!dr.IsDBNull(iProcdescripcion)) entity.Procdescripcion = dr.GetString(iProcdescripcion);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<AudAuditoriaprocesoDTO> GetByAuditoriaElementoPorTipo(int audicodi, int tabcdcoditipoelemento)
        {
            List<AudAuditoriaprocesoDTO> entitys = new List<AudAuditoriaprocesoDTO>();

            string sql = string.Format(helper.SqlGetByAuditoriaElementoPorTipo, audicodi, tabcdcoditipoelemento);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudAuditoriaprocesoDTO entity = helper.Create(dr);

                    int iElemdescripcion = dr.GetOrdinal(helper.Elemdescripcion);
                    if (!dr.IsDBNull(iElemdescripcion)) entity.Elemdescripcion = dr.GetString(iElemdescripcion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        
    }
}
