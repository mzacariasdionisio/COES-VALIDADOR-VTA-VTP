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
    /// Clase de acceso a datos de la tabla CPA_REVISION
    /// </summary>
    public class CpaRevisionRepository : RepositoryBase, ICpaRevisionRepository
    {
        private readonly string strConexion;
        readonly CpaRevisionHelper helper = new CpaRevisionHelper();

        public CpaRevisionRepository(string strConn) : base(strConn)
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

        public int Save(CpaRevisionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cpaapcodi, DbType.Int32, entity.Cpaapcodi);
            dbProvider.AddInParameter(command, helper.Cparrevision, DbType.String, entity.Cparrevision);
            dbProvider.AddInParameter(command, helper.Cparcorrelativo, DbType.Int32, entity.Cparcorrelativo);
            dbProvider.AddInParameter(command, helper.Cparestado, DbType.String, entity.Cparestado);
            dbProvider.AddInParameter(command, helper.Cparultimo, DbType.String, entity.Cparultimo);
            dbProvider.AddInParameter(command, helper.Cparcmpmpo, DbType.Int32, entity.Cparcmpmpo);
            dbProvider.AddInParameter(command, helper.Cparusucreacion, DbType.String, entity.Cparusucreacion);
            dbProvider.AddInParameter(command, helper.Cparfeccreacion, DbType.DateTime, entity.Cparfeccreacion);
            dbProvider.AddInParameter(command, helper.Cparusumodificacion, DbType.String, entity.Cparusumodificacion);
            dbProvider.AddInParameter(command, helper.Cparfecmodificacion, DbType.DateTime, entity.Cparfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Save(CpaRevisionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpaapcodi, DbType.Int32, entity.Cpaapcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparrevision, DbType.String, entity.Cparrevision));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparcorrelativo, DbType.Int32, entity.Cparcorrelativo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparestado, DbType.String, entity.Cparestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparultimo, DbType.String, entity.Cparultimo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparcmpmpo, DbType.Int32, entity.Cparcmpmpo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparusucreacion, DbType.String, entity.Cparusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparfeccreacion, DbType.DateTime, entity.Cparfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparusumodificacion, DbType.String, entity.Cparusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparfecmodificacion, DbType.DateTime, entity.Cparfecmodificacion));
            command.ExecuteNonQuery();
        }

        public void Update(CpaRevisionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cpaapcodi, DbType.Int32, entity.Cpaapcodi);
            dbProvider.AddInParameter(command, helper.Cparrevision, DbType.String, entity.Cparrevision);
            dbProvider.AddInParameter(command, helper.Cparcorrelativo, DbType.Int32, entity.Cparcorrelativo);
            dbProvider.AddInParameter(command, helper.Cparestado, DbType.String, entity.Cparestado);
            dbProvider.AddInParameter(command, helper.Cparultimo, DbType.String, entity.Cparultimo);
            dbProvider.AddInParameter(command, helper.Cparcmpmpo, DbType.Int32, entity.Cparcmpmpo);
            dbProvider.AddInParameter(command, helper.Cparusucreacion, DbType.String, entity.Cparusucreacion);
            dbProvider.AddInParameter(command, helper.Cparfeccreacion, DbType.DateTime, entity.Cparfeccreacion);
            dbProvider.AddInParameter(command, helper.Cparusumodificacion, DbType.String, entity.Cparusumodificacion);
            dbProvider.AddInParameter(command, helper.Cparfecmodificacion, DbType.DateTime, entity.Cparfecmodificacion);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateUltimoByAnioByAjuste(string cparultimo, int cpaapanio, string cpaapajuste, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlUpdateUltimoByAnioByAjuste;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparultimo, DbType.String, cparultimo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpaapanio, DbType.Int32, cpaapanio));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpaapajuste, DbType.String, cpaapajuste));
            command.ExecuteNonQuery();
        }

        public void UpdateUltimoByCodi(string cparultimo, int cparcodi, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlUpdateUltimoByCodi;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparultimo, DbType.String, cparultimo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparcodi, DbType.Int32, cparcodi));
            command.ExecuteNonQuery();
        }

        public void UpdateEstado(int cparcodi, string cparestado, string cparusumodificacion, DateTime cparfecmodificacion, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlUpdateEstado;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparestado, DbType.String, cparestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparusumodificacion, DbType.String, cparusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparfecmodificacion, DbType.DateTime, cparfecmodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparcodi, DbType.Int32, cparcodi));
            command.ExecuteNonQuery();
        }

        public void UpdateEstadoYCMgPMPO(int cparcodi, string cparestado, int cparcmpmpo, string cparusumodificacion, DateTime cparfecmodificacion, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlUpdateEstadoYCMgPMPO;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparestado, DbType.String, cparestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparcmpmpo, DbType.Int32, cparcmpmpo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparusumodificacion, DbType.String, cparusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparfecmodificacion, DbType.DateTime, cparfecmodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparcodi, DbType.Int32, cparcodi));
            command.ExecuteNonQuery();
        }

        public void Delete(int cparcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, cparcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<CpaRevisionDTO> List()
        {
            List<CpaRevisionDTO> entities = new List<CpaRevisionDTO>();
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

        public CpaRevisionDTO GetById(int cparcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, cparcodi);
            CpaRevisionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.CreateById(dr);
                }
            }

            return entity;
        }

        public List<CpaRevisionDTO> GetByCriteria(int cpaapaniofrom, int cpaapaniountil, string cparajuste, string cparestados)
        {
            string query = string.Format(helper.SqlGetByCriteria, cpaapaniofrom, cpaapaniountil, cparajuste, cparestados);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<CpaRevisionDTO> entitys = new List<CpaRevisionDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpaRevisionDTO entity = helper.CreateByCriteria(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
