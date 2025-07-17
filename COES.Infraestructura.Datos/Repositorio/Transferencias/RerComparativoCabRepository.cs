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
    /// Clase de acceso a datos de la tabla RER_COMPARATIVO_CAB
    /// </summary>
    public class RerComparativoCabRepository : RepositoryBase, IRerComparativoCabRepository
    {
        private string strConexion;
        RerComparativoCabHelper helper = new RerComparativoCabHelper();

        public RerComparativoCabRepository(string strConn) : base(strConn)
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

        public RerComparativoCabDTO GetById(int rerccbcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rerccbcodi, DbType.Int32, rerccbcodi);
            RerComparativoCabDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public void Save(RerComparativoCabDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand) conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerccbcodi, DbType.Int32, entity.Rerccbcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerevacodi, DbType.Int32, entity.Rerevacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Reresecodi, DbType.Int32, entity.Reresecodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereeucodi, DbType.Int32, entity.Rereeucodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerccboridatos, DbType.String, entity.Rerccboridatos));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerccbtotenesolicitada, DbType.Decimal, entity.Rerccbtotenesolicitada));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerccbtoteneestimada, DbType.Decimal, entity.Rerccbtoteneestimada));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerccbusucreacion, DbType.String, entity.Rerccbusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerccbfeccreacion, DbType.DateTime, entity.Rerccbfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerccbusumodificacion, DbType.String, entity.Rerccbusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerccbfecmodificacion, DbType.DateTime, entity.Rerccbfecmodificacion));
            command.ExecuteNonQuery();
        }

        public void Update(RerComparativoCabDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand) conn.CreateCommand();
            command.CommandText = helper.SqlUpdate;
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerevacodi, DbType.Int32, entity.Rerevacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Reresecodi, DbType.Int32, entity.Reresecodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereeucodi, DbType.Int32, entity.Rereeucodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerccboridatos, DbType.String, entity.Rerccboridatos));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerccbtotenesolicitada, DbType.Decimal, entity.Rerccbtotenesolicitada));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerccbtoteneestimada, DbType.Decimal, entity.Rerccbtoteneestimada));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerccbusumodificacion, DbType.String, entity.Rerccbusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerccbfecmodificacion, DbType.DateTime, entity.Rerccbfecmodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerccbcodi, DbType.Int32, entity.Rerccbcodi));
            command.ExecuteNonQuery();
        }

        public void Delete(int rerccbcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rerccbcodi, DbType.Int32, rerccbcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<RerComparativoCabDTO> List()
        {
            List<RerComparativoCabDTO> entities = new List<RerComparativoCabDTO>();
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

        public List<RerComparativoCabDTO> GetByCriteria(int rerevacodi)
        {
            string query = string.Format(helper.SqlGetByCriteria, rerevacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerComparativoCabDTO> entitys = new List<RerComparativoCabDTO>();

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
