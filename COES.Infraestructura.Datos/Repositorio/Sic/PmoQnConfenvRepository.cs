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
    /// Clase de acceso a datos de la tabla PMO_QN_CONFENV
    /// </summary>
    public class PmoQnConfenvRepository : RepositoryBase, IPmoQnConfenvRepository
    {
        public PmoQnConfenvRepository(string strConn) : base(strConn)
        {
        }

        PmoQnConfenvHelper helper = new PmoQnConfenvHelper();

        public int Save(PmoQnConfenvDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Qncfgecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Qnlectcodi, DbType.Int32, entity.Qnlectcodi);
            dbProvider.AddInParameter(command, helper.Qncfgesddps, DbType.String, entity.Qncfgesddps);
            dbProvider.AddInParameter(command, helper.Qncfgeusucreacion, DbType.String, entity.Qncfgeusucreacion);
            dbProvider.AddInParameter(command, helper.Qncfgefeccreacion, DbType.DateTime, entity.Qncfgefeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PmoQnConfenvDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Qnlectcodi, DbType.Int32, entity.Qnlectcodi);
            dbProvider.AddInParameter(command, helper.Qncfgesddps, DbType.String, entity.Qncfgesddps);
            dbProvider.AddInParameter(command, helper.Qncfgeusucreacion, DbType.String, entity.Qncfgeusucreacion);
            dbProvider.AddInParameter(command, helper.Qncfgefeccreacion, DbType.DateTime, entity.Qncfgefeccreacion);
            dbProvider.AddInParameter(command, helper.Qncfgecodi, DbType.Int32, entity.Qncfgecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int qncfgecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Qncfgecodi, DbType.Int32, qncfgecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PmoQnConfenvDTO GetById(int qncfgecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Qncfgecodi, DbType.Int32, qncfgecodi);
            PmoQnConfenvDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PmoQnConfenvDTO> List()
        {
            List<PmoQnConfenvDTO> entitys = new List<PmoQnConfenvDTO>();
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

        public List<PmoQnConfenvDTO> GetByCriteria()
        {
            List<PmoQnConfenvDTO> entitys = new List<PmoQnConfenvDTO>();
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

        public int Save(PmoQnConfenvDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qncfgecodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnlectcodi, DbType.Int32, entity.Qnlectcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qncfgesddps, DbType.String, entity.Qncfgesddps));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qncfgeusucreacion, DbType.String, entity.Qncfgeusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qncfgefeccreacion, DbType.DateTime, entity.Qncfgefeccreacion));


                dbCommand.ExecuteNonQuery();
                return id;
            }
        }
    }
}
