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
    /// Clase de acceso a datos de la tabla AUD_AUDPLANIFICADA_PROCESO
    /// </summary>
    public class AudAudplanificadaProcesoRepository: RepositoryBase, IAudAudplanificadaProcesoRepository
    {
        public AudAudplanificadaProcesoRepository(string strConn): base(strConn)
        {
        }

        AudAudplanificadaProcesoHelper helper = new AudAudplanificadaProcesoHelper();

        public int Save(AudAudplanificadaprocesoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Audppcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Audpcodi, DbType.Int32, entity.Audpcodi);
            dbProvider.AddInParameter(command, helper.Proccodi, DbType.Int32, entity.Proccodi);
            dbProvider.AddInParameter(command, helper.Audppactivo, DbType.String, entity.Audppactivo);
            dbProvider.AddInParameter(command, helper.Audpphistorico, DbType.String, entity.Audpphistorico);
            dbProvider.AddInParameter(command, helper.Audppusucreacion, DbType.String, entity.Audppusucreacion);
            dbProvider.AddInParameter(command, helper.Audppfeccreacion, DbType.DateTime, entity.Audppfeccreacion);
            dbProvider.AddInParameter(command, helper.Audppusumodificacion, DbType.String, entity.Audppusumodificacion);
            dbProvider.AddInParameter(command, helper.Audppfecmodificacion, DbType.DateTime, entity.Audppfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AudAudplanificadaprocesoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Audpcodi, DbType.Int32, entity.Audpcodi);
            dbProvider.AddInParameter(command, helper.Proccodi, DbType.Int32, entity.Proccodi);
            dbProvider.AddInParameter(command, helper.Audppactivo, DbType.String, entity.Audppactivo);
            dbProvider.AddInParameter(command, helper.Audpphistorico, DbType.String, entity.Audpphistorico);
            dbProvider.AddInParameter(command, helper.Audppusumodificacion, DbType.String, entity.Audppusumodificacion);
            dbProvider.AddInParameter(command, helper.Audppfecmodificacion, DbType.DateTime, entity.Audppfecmodificacion);
            dbProvider.AddInParameter(command, helper.Audppcodi, DbType.Int32, entity.Audppcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(AudAudplanificadaprocesoDTO audPlanificadaProceso)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Audppusumodificacion, DbType.String, audPlanificadaProceso.Audppusumodificacion);
            dbProvider.AddInParameter(command, helper.Audppfecmodificacion, DbType.DateTime, audPlanificadaProceso.Audppfecmodificacion);
            dbProvider.AddInParameter(command, helper.Audppcodi, DbType.Int32, audPlanificadaProceso.Audppcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByAudPlanificada(AudAudplanificadaprocesoDTO audPlanificadaProceso)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByAudPlanificada);

            dbProvider.AddInParameter(command, helper.Audppusumodificacion, DbType.String, audPlanificadaProceso.Audppusumodificacion);
            dbProvider.AddInParameter(command, helper.Audppfecmodificacion, DbType.DateTime, audPlanificadaProceso.Audppfecmodificacion);
            dbProvider.AddInParameter(command, helper.Audpcodi, DbType.Int32, audPlanificadaProceso.Audpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public AudAudplanificadaprocesoDTO GetById(int audpcodi, int proccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Audpcodi, DbType.Int32, audpcodi);
            dbProvider.AddInParameter(command, helper.Proccodi, DbType.Int32, proccodi);

            AudAudplanificadaprocesoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<AudAudplanificadaprocesoDTO> List(int audpcodi)
        {
            string sql = string.Format(helper.SqlList, audpcodi);

            List<AudAudplanificadaprocesoDTO> entitys = new List<AudAudplanificadaprocesoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudAudplanificadaprocesoDTO entity = helper.Create(dr);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<AudAudplanificadaprocesoDTO> GetByCriteria(int audpcodi, string areacodi)
        {
            List<AudAudplanificadaprocesoDTO> entitys = new List<AudAudplanificadaprocesoDTO>();

            string query = string.Format(helper.SqlGetByCriteria, audpcodi > 0 ? "pp.audpcodi = " + audpcodi + " and " : "", areacodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudAudplanificadaprocesoDTO entity = helper.Create(dr);

                    int iProcdescripcion = dr.GetOrdinal(helper.Procdescripcion);
                    if (!dr.IsDBNull(iProcdescripcion)) entity.Procdescripcion = dr.GetString(iProcdescripcion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
