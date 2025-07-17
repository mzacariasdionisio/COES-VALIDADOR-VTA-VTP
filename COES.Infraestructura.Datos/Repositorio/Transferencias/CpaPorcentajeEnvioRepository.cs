using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CPA_EMPRESA
    /// </summary>
    public class CpaPorcentajeEnvioRepository : RepositoryBase, ICpaPorcentajeEnvioRepository
    {
        private readonly string strConexion;
        readonly CpaPorcentajeEnvioHelper helper = new CpaPorcentajeEnvioHelper();

        public CpaPorcentajeEnvioRepository(string strConn) : base(strConn)
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

        public int Save(CpaPorcentajeEnvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            int id = GetMaxId(); // Obtenemos el id máximo para usarlo

            dbProvider.AddInParameter(command, helper.Cpapecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cpapcodi, DbType.Int32, entity.Cpapcodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Cpapetipo, DbType.String, entity.Cpapetipo);
            dbProvider.AddInParameter(command, helper.Cpapemes, DbType.Int32, entity.Cpapemes);
            dbProvider.AddInParameter(command, helper.Cpapenumenvio, DbType.Int32, entity.Cpapenumenvio);
            dbProvider.AddInParameter(command, helper.Cpapeusucreacion, DbType.String, entity.Cpapeusucreacion);
            dbProvider.AddInParameter(command, helper.Cpapefeccreacion, DbType.DateTime, entity.Cpapefeccreacion);

            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public void Save(CpaPorcentajeEnvioDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapecodi, DbType.Int32, entity.Cpapecodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapcodi, DbType.Int32, entity.Cpapcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapetipo, DbType.String, entity.Cpapetipo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapemes, DbType.Int32, entity.Cpapemes));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapenumenvio, DbType.Int32, entity.Cpapenumenvio));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapeusucreacion, DbType.String, entity.Cpapeusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapefeccreacion, DbType.DateTime, entity.Cpapefeccreacion));
            command.ExecuteNonQuery();
        }

        public void Update(CpaPorcentajeEnvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cpapecodi, DbType.Int32, entity.Cpapecodi);
            dbProvider.AddInParameter(command, helper.Cpapcodi, DbType.Int32, entity.Cpapcodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Cpapetipo, DbType.String, entity.Cpapetipo);
            dbProvider.AddInParameter(command, helper.Cpapemes, DbType.Int32, entity.Cpapemes);
            dbProvider.AddInParameter(command, helper.Cpapenumenvio, DbType.Int32, entity.Cpapenumenvio);
            dbProvider.AddInParameter(command, helper.Cpapeusucreacion, DbType.String, entity.Cpapeusucreacion);
            dbProvider.AddInParameter(command, helper.Cpapefeccreacion, DbType.DateTime, entity.Cpapefeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cpapecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Cpapecodi, DbType.Int32, cpapecodi);
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

        public List<CpaPorcentajeEnvioDTO> List()
        {
            List<CpaPorcentajeEnvioDTO> entities = new List<CpaPorcentajeEnvioDTO>();
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

        public List<CpaPorcentajeEnvioDTO> ListByRevision(int cparcodi)
        {
            string query = string.Format(helper.SqlListByRevision, cparcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<CpaPorcentajeEnvioDTO> entities = new List<CpaPorcentajeEnvioDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }

        public CpaPorcentajeEnvioDTO GetById(int cpapecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Cpapecodi, DbType.Int32, cpapecodi);

            CpaPorcentajeEnvioDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpaPorcentajeEnvioDTO> GetByCparcodi(int cparcodi)
        {
            string query = string.Format(helper.SqlGetByCriteria, cparcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            List<CpaPorcentajeEnvioDTO> entities = new List<CpaPorcentajeEnvioDTO>();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }
    }

}
