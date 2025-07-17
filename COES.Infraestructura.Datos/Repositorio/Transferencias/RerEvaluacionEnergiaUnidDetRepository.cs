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
    public class RerEvaluacionEnergiaUnidDetRepository : RepositoryBase, IRerEvaluacionEnergiaUnidDetRepository
    {
        private string strConexion;
        public RerEvaluacionEnergiaUnidDetRepository(string strConn) : base(strConn)
        {
            strConexion = strConn;
        }

        RerEvaluacionEnergiaUnidDetHelper helper = new RerEvaluacionEnergiaUnidDetHelper();

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

        public RerEvaluacionEnergiaUnidDetDTO GetById(int rereedcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rereedcodi, DbType.Int32, rereedcodi);
            RerEvaluacionEnergiaUnidDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public void Save(RerEvaluacionEnergiaUnidDetDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereedcodi, DbType.Int32, entity.Rereedcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereeucodi, DbType.Int32, entity.Rereeucodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereedenergiaunidad, DbType.String, entity.Rereedenergiaunidad));
            command.ExecuteNonQuery();
        }

        public int Save(RerEvaluacionEnergiaUnidDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rereedcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rereeucodi, DbType.Int32, entity.Rereeucodi);
            dbProvider.AddInParameter(command, helper.Rereedenergiaunidad, DbType.String, entity.Rereedenergiaunidad);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RerEvaluacionEnergiaUnidDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rereeucodi, DbType.Int32, entity.Rereeucodi);
            dbProvider.AddInParameter(command, helper.Rereedenergiaunidad, DbType.String, entity.Rereedenergiaunidad);
            dbProvider.AddInParameter(command, helper.Rereedcodi, DbType.Int32, entity.Rereedcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rereedcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rereedcodi, DbType.Int32, rereedcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<RerEvaluacionEnergiaUnidDetDTO> List()
        {
            List<RerEvaluacionEnergiaUnidDetDTO> entities = new List<RerEvaluacionEnergiaUnidDetDTO>();
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

        public List<RerEvaluacionEnergiaUnidDetDTO> GetByCriteria(string rereeucodi)
        {
            string query = string.Format(helper.SqlGetByCriteria, rereeucodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerEvaluacionEnergiaUnidDetDTO> entitys = new List<RerEvaluacionEnergiaUnidDetDTO>();

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


