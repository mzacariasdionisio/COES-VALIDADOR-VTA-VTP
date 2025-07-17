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
    /// Clase de acceso a datos de la tabla RER_REVISION
    /// </summary>
    public class RerRevisionRepository : RepositoryBase, IRerRevisionRepository
    {
        private string strConexion;
        RerRevisionHelper helper = new RerRevisionHelper();

        public RerRevisionRepository(string strConn) : base(strConn)
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

        public RerRevisionDTO GetById(int rerrevcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rerrevcodi, DbType.Int32, rerrevcodi);
            RerRevisionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public int Save(RerRevisionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Rerrevcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ipericodi, DbType.Int32, entity.Ipericodi);
            dbProvider.AddInParameter(command, helper.Rerrevnombre, DbType.String, entity.Rerrevnombre);
            dbProvider.AddInParameter(command, helper.Rerrevtipo, DbType.String, entity.Rerrevtipo);
            dbProvider.AddInParameter(command, helper.Rerrevfecha, DbType.DateTime, entity.Rerrevfecha);
            dbProvider.AddInParameter(command, helper.Rerrevestado, DbType.String, entity.Rerrevestado);
            dbProvider.AddInParameter(command, helper.Rerrevusucreacion, DbType.String, entity.Rerrevusucreacion);
            dbProvider.AddInParameter(command, helper.Rerrevfeccreacion, DbType.DateTime, entity.Rerrevfeccreacion);
            dbProvider.AddInParameter(command, helper.Rerrevusumodificacion, DbType.String, entity.Rerrevusumodificacion);
            dbProvider.AddInParameter(command, helper.Rerrevfecmodificacion, DbType.DateTime, entity.Rerrevfecmodificacion);
            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public void Save(RerRevisionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerrevcodi, DbType.Int32, entity.Rerrevcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ipericodi, DbType.Int32, entity.Ipericodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerrevnombre, DbType.String, entity.Rerrevnombre));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerrevtipo, DbType.String, entity.Rerrevtipo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerrevfecha, DbType.DateTime, entity.Rerrevfecha));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerrevestado, DbType.String, entity.Rerrevestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerrevusucreacion, DbType.String, entity.Rerrevusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerrevfeccreacion, DbType.DateTime, entity.Rerrevfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerrevusumodificacion, DbType.String, entity.Rerrevusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerrevfecmodificacion, DbType.DateTime, entity.Rerrevfecmodificacion));
            command.ExecuteNonQuery();
        }

        public void Update(RerRevisionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand) conn.CreateCommand();
            command.CommandText = helper.SqlUpdate;
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ipericodi, DbType.Int32, entity.Ipericodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerrevnombre, DbType.String, entity.Rerrevnombre));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerrevtipo, DbType.String, entity.Rerrevtipo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerrevfecha, DbType.DateTime, entity.Rerrevfecha));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerrevestado, DbType.String, entity.Rerrevestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerrevusumodificacion, DbType.String, entity.Rerrevusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerrevfecmodificacion, DbType.DateTime, entity.Rerrevfecmodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerrevcodi, DbType.Int32, entity.Rerrevcodi));
            command.ExecuteNonQuery();
        }

        public void UpdateEstado(int rerrevcodi, string rerrevestado, string rerrevusumodificacion, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand) conn.CreateCommand();
            command.CommandText = helper.SqlUpdateEstado;
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerrevestado, DbType.String, rerrevestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerrevusumodificacion, DbType.String, rerrevusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerrevfecmodificacion, DbType.DateTime, DateTime.Now));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerrevcodi, DbType.Int32, rerrevcodi));
            command.ExecuteNonQuery();
        }

        public List<RerRevisionDTO> GetByCriteria(int ipericodi)
        {
            string query = string.Format(helper.SqlGetByCriteria, ipericodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerRevisionDTO> entitys = new List<RerRevisionDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerRevisionDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RerRevisionDTO> ListPeriodosConUltimaRevision(int idPlazoEntregaEdi)
        {
            string query = string.Format(helper.SqlListPeriodosConUltimaRevision, idPlazoEntregaEdi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerRevisionDTO> entitys = new List<RerRevisionDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerRevisionDTO entity = helper.CreateListPeriodosConUltimaRevision(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int GetCantidadRevisionesTipoRevision(int ipericodi)
        {
            string query = string.Format(helper.SqlGetCantidadRevisionesTipoRevision, ipericodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            int cantidad = 0;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) cantidad = Convert.ToInt32(result);

            return cantidad;
        }

    }
}
