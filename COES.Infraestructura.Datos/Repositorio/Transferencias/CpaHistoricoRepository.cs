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
    /// Clase de acceso a datos de la tabla CPA_HISTORICO
    /// </summary>
    public class CpaHistoricoRepository : RepositoryBase, ICpaHistoricoRepository
    {
        private readonly string strConexion;
        readonly CpaHistoricoHelper helper = new CpaHistoricoHelper();

        public CpaHistoricoRepository(string strConn)
            : base(strConn)
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

        public int Save(CpaHistoricoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpahcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Cpahtipo, DbType.String, entity.Cpahtipo);
            dbProvider.AddInParameter(command, helper.Cpahusumodificacion, DbType.String, entity.Cpahusumodificacion);
            dbProvider.AddInParameter(command, helper.Cpahfecmodificacion, DbType.DateTime, entity.Cpahfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Save(CpaHistoricoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpahcodi, DbType.Int32, entity.Cpahcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpahtipo, DbType.String, entity.Cpahtipo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpahusumodificacion, DbType.String, entity.Cpahusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpahfecmodificacion, DbType.DateTime, entity.Cpahfecmodificacion));
            command.ExecuteNonQuery();
        }

        public void Update(CpaHistoricoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Cpahtipo, DbType.String, entity.Cpahtipo);
            dbProvider.AddInParameter(command, helper.Cpahusumodificacion, DbType.String, entity.Cpahusumodificacion);
            dbProvider.AddInParameter(command, helper.Cpahfecmodificacion, DbType.DateTime, entity.Cpahfecmodificacion);
            dbProvider.AddInParameter(command, helper.Cpahcodi, DbType.Int32, entity.Cpahcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cphcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cpahcodi, DbType.Int32, cphcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<CpaHistoricoDTO> List()
        {
            List<CpaHistoricoDTO> entities = new List<CpaHistoricoDTO>();
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

        public CpaHistoricoDTO GetById(int cphcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpahcodi, DbType.Int32, cphcodi);
            CpaHistoricoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpaHistoricoDTO> GetByCriteria(int cparcodi)
        {
            string query = string.Format(helper.SqlGetByCriteria, cparcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<CpaHistoricoDTO> entitys = new List<CpaHistoricoDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpaHistoricoDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
