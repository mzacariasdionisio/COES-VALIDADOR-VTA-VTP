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
    /// Clase de acceso a datos de la tabla PMO_LOG
    /// </summary>
    public class PmoLogRepository : RepositoryBase, IPmoLogRepository
    {
        public PmoLogRepository(string strConn) : base(strConn)
        {
        }

        PmoLogHelper helper = new PmoLogHelper();

        public int Save(PmoLogDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pmologcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, entity.Enviocodi);
            dbProvider.AddInParameter(command, helper.Logcodi, DbType.Int32, entity.Logcodi);
            dbProvider.AddInParameter(command, helper.Pmologtipo, DbType.Int32, entity.Pmologtipo);
            dbProvider.AddInParameter(command, helper.Pmftabcodi, DbType.Int32, entity.Pmftabcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PmoLogDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pmologcodi, DbType.Int32, entity.Pmologcodi);
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, entity.Enviocodi);
            dbProvider.AddInParameter(command, helper.Logcodi, DbType.Int32, entity.Logcodi);
            dbProvider.AddInParameter(command, helper.Pmologtipo, DbType.Int32, entity.Pmologtipo);
            dbProvider.AddInParameter(command, helper.Pmftabcodi, DbType.Int32, entity.Pmftabcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pmologcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pmologcodi, DbType.Int32, pmologcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PmoLogDTO GetById(int pmologcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pmologcodi, DbType.Int32, pmologcodi);
            PmoLogDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PmoLogDTO> List()
        {
            List<PmoLogDTO> entitys = new List<PmoLogDTO>();
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

        public List<PmoLogDTO> GetByCriteria(int enviocodi)
        {
            List<PmoLogDTO> entitys = new List<PmoLogDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, enviocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iLogFecha = dr.GetOrdinal(helper.LogFecha);
                    if (!dr.IsDBNull(iLogFecha)) entity.LogFecha = dr.GetDateTime(iLogFecha);

                    int iLogUser = dr.GetOrdinal(helper.LogUser);
                    if (!dr.IsDBNull(iLogUser)) entity.LogUser = dr.GetString(iLogUser);

                    int iLogDesc = dr.GetOrdinal(helper.LogDesc);
                    if (!dr.IsDBNull(iLogDesc)) entity.LogDesc = dr.GetString(iLogDesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
