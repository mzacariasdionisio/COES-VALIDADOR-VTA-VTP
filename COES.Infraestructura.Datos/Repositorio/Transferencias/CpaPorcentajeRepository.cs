using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CPA_PORCENTAJE
    /// </summary>
    public class CpaPorcentajeRepository : RepositoryBase, ICpaPorcentajeRepository
    {
        private readonly string strConexion;
        readonly CpaPorcentajeHelper helper = new CpaPorcentajeHelper();

        public CpaPorcentajeRepository(string strConn) : base(strConn)
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

        public int Save(CpaPorcentajeDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpapcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Cpaplog, DbType.String, entity.Cpaplog);
            dbProvider.AddInParameter(command, helper.Cpapestpub, DbType.String, entity.Cpapestpub.ToString());
            dbProvider.AddInParameter(command, helper.Cpapusucreacion, DbType.String, entity.Cpapusucreacion);
            dbProvider.AddInParameter(command, helper.Cpapfeccreacion, DbType.DateTime, entity.Cpapfeccreacion);
            dbProvider.AddInParameter(command, helper.Cpapusumodificacion, DbType.String, entity.Cpapusumodificacion);
            dbProvider.AddInParameter(command, helper.Cpapfecmodificacion, DbType.DateTime, entity.Cpapfecmodificacion);

            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public void Save(CpaPorcentajeDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapcodi, DbType.Int32, entity.Cpapcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpaplog, DbType.String, entity.Cpaplog));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapestpub, DbType.String, entity.Cpapestpub.ToString()));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapusucreacion, DbType.String, entity.Cpapusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapfeccreacion, DbType.DateTime, entity.Cpapfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapusumodificacion, DbType.String, entity.Cpapusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapfecmodificacion, DbType.DateTime, entity.Cpapfecmodificacion));
            command.ExecuteNonQuery();
        }

        public void Update(CpaPorcentajeDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cpapcodi, DbType.Int32, entity.Cpapcodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Cpaplog, DbType.String, entity.Cpaplog);
            dbProvider.AddInParameter(command, helper.Cpapestpub, DbType.String, entity.Cpapestpub.ToString());
            dbProvider.AddInParameter(command, helper.Cpapusucreacion, DbType.String, entity.Cpapusucreacion);
            dbProvider.AddInParameter(command, helper.Cpapfeccreacion, DbType.DateTime, entity.Cpapfeccreacion);
            dbProvider.AddInParameter(command, helper.Cpapusumodificacion, DbType.String, entity.Cpapusumodificacion);
            dbProvider.AddInParameter(command, helper.Cpapfecmodificacion, DbType.DateTime, entity.Cpapfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateEstadoPublicacion(int cparcodi, string cpapestpub, string cpapusumodificacion, DateTime cpapfecmodificacion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateEstadoPublicacion);
            dbProvider.AddInParameter(command, helper.Cpapestpub, DbType.String, cpapestpub);
            dbProvider.AddInParameter(command, helper.Cpapusumodificacion, DbType.String, cpapusumodificacion);
            dbProvider.AddInParameter(command, helper.Cpapfecmodificacion, DbType.DateTime, cpapfecmodificacion);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, cparcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cpapcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Cpapcodi, DbType.Int32, cpapcodi);
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

        public List<CpaPorcentajeDTO> List()
        {
            List<CpaPorcentajeDTO> entities = new List<CpaPorcentajeDTO>();
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

        public CpaPorcentajeDTO GetById(int cpapcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpapcodi, DbType.Int32, cpapcodi);
            CpaPorcentajeDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public CpaPorcentajeDTO GetByCriteria(int cparcodi)
        {
            string query = string.Format(helper.SqlGetByCriteria, cparcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            CpaPorcentajeDTO entity = null;

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
