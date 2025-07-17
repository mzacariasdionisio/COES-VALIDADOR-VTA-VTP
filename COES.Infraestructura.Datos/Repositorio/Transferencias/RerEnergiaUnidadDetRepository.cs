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
    /// Clase de acceso a datos de la tabla RER_ENERGIAUNIDAD_DET
    /// </summary>
    public class RerEnergiaUnidadDetRepository : RepositoryBase, IRerEnergiaUnidadDetRepository
    {
        private string strConexion;
        RerEnergiaUnidadDetHelper helper = new RerEnergiaUnidadDetHelper();

        public RerEnergiaUnidadDetRepository(string strConn) : base(strConn)
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

        public RerEnergiaUnidadDetDTO GetById(int rereudcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rereudcodi, DbType.Int32, rereudcodi);
            RerEnergiaUnidadDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public void Save(RerEnergiaUnidadDetDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereudcodi, DbType.Int32, entity.Rereudcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereucodi, DbType.Int32, entity.Rereucodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereudenergiaunidad, DbType.String, entity.Rereudenergiaunidad));
            command.ExecuteNonQuery();
        }

        public int Save(RerEnergiaUnidadDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rereudcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rereucodi, DbType.Int32, entity.Rereucodi);
            dbProvider.AddInParameter(command, helper.Rereudenergiaunidad, DbType.String, entity.Rereudenergiaunidad);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RerEnergiaUnidadDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rereucodi, DbType.Int32, entity.Rereucodi);
            dbProvider.AddInParameter(command, helper.Rereudenergiaunidad, DbType.String, entity.Rereudenergiaunidad);
            dbProvider.AddInParameter(command, helper.Rereudcodi, DbType.Int32, entity.Rereudcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rereucodi, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlDelete;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereucodi, DbType.Int32, rereucodi));
            command.ExecuteNonQuery();
        }

        public List<RerEnergiaUnidadDetDTO> List()
        {
            List<RerEnergiaUnidadDetDTO> entities = new List<RerEnergiaUnidadDetDTO>();
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

        public List<RerEnergiaUnidadDetDTO> GetByCriteria(string rereucodi)
        {
            string query = string.Format(helper.SqlGetByCriteria, rereucodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerEnergiaUnidadDetDTO> entitys = new List<RerEnergiaUnidadDetDTO>();

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

