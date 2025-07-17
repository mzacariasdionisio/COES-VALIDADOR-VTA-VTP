using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CPA_CALCULO
    /// </summary>
    public class CpaCalculoRepository : RepositoryBase, ICpaCalculoRepository
    {
        private readonly string strConexion;
        readonly CpaCalculoHelper helper = new CpaCalculoHelper();

        public CpaCalculoRepository(string strConn) : base(strConn)
        {
            strConexion = strConn;
        }

        public IDbConnection BeginConnection()
        {
            Database db = DatabaseFactory.CreateDatabase(strConexion);
            IDbConnection conn = db.CreateConnection();
            conn.Open();
            return conn;
        }

        public DbTransaction StartTransaction(IDbConnection conn)
        {
            return (DbTransaction)conn.BeginTransaction();
        }

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(CpaCalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpaccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Cpaclog, DbType.String, entity.Cpaclog);
            dbProvider.AddInParameter(command, helper.Cpacusucreacion, DbType.String, entity.Cpacusucreacion);
            dbProvider.AddInParameter(command, helper.Cpacfeccreacion, DbType.DateTime, entity.Cpacfeccreacion);

            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public void Save(CpaCalculoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpaccodi, DbType.Int32, entity.Cpaccodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpaclog, DbType.String, entity.Cpaclog));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpacusucreacion, DbType.String, entity.Cpacusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpacfeccreacion, DbType.DateTime, entity.Cpacfeccreacion));
            command.ExecuteNonQuery();
        }

        public void Update(CpaCalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Cpaclog, DbType.String, entity.Cpaclog);
            dbProvider.AddInParameter(command, helper.Cpacusucreacion, DbType.String, entity.Cpacusucreacion);
            dbProvider.AddInParameter(command, helper.Cpacfeccreacion, DbType.DateTime, entity.Cpacfeccreacion);
            dbProvider.AddInParameter(command, helper.Cpaccodi, DbType.Int32, entity.Cpaccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cpaccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Cpaccodi, DbType.Int32, cpaccodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByRevision(int cparcodi, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlDeleteByRevision;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparcodi, DbType.Int32, cparcodi));
            command.ExecuteNonQuery();
        }

        public List<CpaCalculoDTO> List()
        {
            List<CpaCalculoDTO> entities = new List<CpaCalculoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }
            
            return entities;
        }

        public CpaCalculoDTO GetById(int cpaccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpaccodi, DbType.Int32, cpaccodi);
            CpaCalculoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public CpaCalculoDTO GetByCriteria(int cparcodi)
        {
            string query = string.Format(helper.SqlGetByCriteria, cparcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            CpaCalculoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

    }
}
