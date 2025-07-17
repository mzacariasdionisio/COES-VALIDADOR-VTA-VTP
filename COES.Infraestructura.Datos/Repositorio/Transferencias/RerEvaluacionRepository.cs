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
    /// Clase de acceso a datos de la tabla RER_EVALUACION
    /// </summary>
    public class RerEvaluacionRepository : RepositoryBase, IRerEvaluacionRepository
    {
        private string strConexion;
        RerEvaluacionHelper helper = new RerEvaluacionHelper();

        public RerEvaluacionRepository(string strConn) : base(strConn)
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
            return (DbTransaction) conn.BeginTransaction();
        }

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public RerEvaluacionDTO GetById(int rerevacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rerevacodi, DbType.Int32, rerevacodi);
            RerEvaluacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.CreateById(dr);
                }
            }

            return entity;
        }

        public void Save(RerEvaluacionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand) conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerevacodi, DbType.Int32, entity.Rerevacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerrevcodi, DbType.Int32, entity.Rerrevcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerevanumversion, DbType.Int32, entity.Rerevanumversion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerevaestado, DbType.String, entity.Rerevaestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerevausucreacion, DbType.String, entity.Rerevausucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerevafeccreacion, DbType.DateTime, entity.Rerevafeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerevausumodificacion, DbType.String, entity.Rerevausumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerevafecmodificacion, DbType.DateTime, entity.Rerevafecmodificacion));
            command.ExecuteNonQuery();
        }

        public void Update(RerEvaluacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rerrevcodi, DbType.Int32, entity.Rerrevcodi);
            dbProvider.AddInParameter(command, helper.Rerevanumversion, DbType.Int32, entity.Rerevanumversion);
            dbProvider.AddInParameter(command, helper.Rerevaestado, DbType.String, entity.Rerevaestado);
            dbProvider.AddInParameter(command, helper.Rerevausucreacion, DbType.String, entity.Rerevausucreacion);
            dbProvider.AddInParameter(command, helper.Rerevafeccreacion, DbType.DateTime, entity.Rerevafeccreacion);
            dbProvider.AddInParameter(command, helper.Rerevausumodificacion, DbType.String, entity.Rerevausumodificacion);
            dbProvider.AddInParameter(command, helper.Rerevafecmodificacion, DbType.DateTime, entity.Rerevafecmodificacion);
            dbProvider.AddInParameter(command, helper.Rerevacodi, DbType.Int32, entity.Rerevacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateEstado(int rerevacodi, string rerevaestado, string rerevausumodificacion, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand) conn.CreateCommand();
            command.CommandText = helper.SqlUpdateEstado;
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerevaestado, DbType.String, rerevaestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerevausumodificacion, DbType.String, rerevausumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerevafecmodificacion, DbType.DateTime, DateTime.Now));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerevacodi, DbType.Int32, rerevacodi));
            command.ExecuteNonQuery();
        }

        public void UpdateEstadoAGenerado(int rerrevcodi, string rerevausumodificacion, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand) conn.CreateCommand();
            command.CommandText = helper.SqlUpdateEstadoAGenerado;
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerevausumodificacion, DbType.String, rerevausumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerevafecmodificacion, DbType.DateTime, DateTime.Now));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerrevcodi, DbType.Int32, rerrevcodi));
            command.ExecuteNonQuery();
        }

        public void Delete(int rerevacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rerevacodi, DbType.Int32, rerevacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<RerEvaluacionDTO> List()
        {
            List<RerEvaluacionDTO> entities = new List<RerEvaluacionDTO>();
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

        public List<RerEvaluacionDTO> GetByCriteria(int rerrevcodi)
        {
            string query = string.Format(helper.SqlGetByCriteria, rerrevcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerEvaluacionDTO> entitys = new List<RerEvaluacionDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerEvaluacionDTO entity = helper.CreateByCriteria(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int GetNextNumVersion(int rerrevcodi)
        {
            string query = string.Format(helper.SqlGetNextNumVersion, rerrevcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            int nextNumVersion = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) nextNumVersion = Convert.ToInt32(result);

            return nextNumVersion;
        }

        public RerEvaluacionDTO GetByRevisionAndLastNumVersion(int rerrevcodi)
        {
            string query = string.Format(helper.SqlGetByRevisionAndLastNumVersion, rerrevcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            RerEvaluacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RerEvaluacionDTO> GetUltimaByEstadoEvaluacionByAnioTarifario(string rerevaestado, int anio)
        {
            string query = string.Format(helper.SqlGetUltimaByEstadoEvaluacionByAnioTarifario, rerevaestado, anio, anio + 1);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerEvaluacionDTO> entitys = new List<RerEvaluacionDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerEvaluacionDTO entity = helper.CreateUltimaByEstadoEvaluacionByAnioTarifario(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int GetCantidadEvaluacionValidado(int rerrevcodi)
        {
            string query = string.Format(helper.SqlGetCantidadEvaluacionValidado, rerrevcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            int cantidad = 0;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) cantidad = Convert.ToInt32(result);

            return cantidad;
        }
    }
}
