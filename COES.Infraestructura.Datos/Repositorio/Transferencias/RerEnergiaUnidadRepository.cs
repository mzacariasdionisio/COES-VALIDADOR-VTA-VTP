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
    /// Clase de acceso a datos de la tabla RER_ENERGIAUNIDAD
    /// </summary>
    public class RerEnergiaUnidadRepository : RepositoryBase, IRerEnergiaUnidadRepository
    {
        private string strConexion;
        RerEnergiaUnidadHelper helper = new RerEnergiaUnidadHelper();
        public RerEnergiaUnidadRepository(string strConn) : base(strConn)
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

        public void Save(RerEnergiaUnidadDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereucodi, DbType.Int32, entity.Rereucodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rersedcodi, DbType.Int32, entity.Rersedcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereuenergiaunidad, DbType.String, entity.Rereuenergiaunidad));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereutotenergia, DbType.Decimal, entity.Rereutotenergia));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereuusucreacion, DbType.String, entity.Rereuusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereufeccreacion, DbType.DateTime, entity.Rereufeccreacion));
            command.ExecuteNonQuery();
        }

        public void Update(RerEnergiaUnidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rersedcodi, DbType.Int32, entity.Rersedcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rereuenergiaunidad, DbType.String, entity.Rereuenergiaunidad);
            dbProvider.AddInParameter(command, helper.Rereutotenergia, DbType.Decimal, entity.Rereutotenergia);
            dbProvider.AddInParameter(command, helper.Rereuusucreacion, DbType.String, entity.Rereuusucreacion);
            dbProvider.AddInParameter(command, helper.Rereufeccreacion, DbType.DateTime, entity.Rereufeccreacion);
            dbProvider.AddInParameter(command, helper.Rereucodi, DbType.Int32, entity.Rereucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rersedcodi, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlDelete;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rersedcodi, DbType.Int32, rersedcodi));
            command.ExecuteNonQuery();
        }

        public RerEnergiaUnidadDTO GetById(int rereucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rereucodi, DbType.Int32, rereucodi);
            RerEnergiaUnidadDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RerEnergiaUnidadDTO> List(int rersedcodi)
        {
            List<RerEnergiaUnidadDTO> entities = new List<RerEnergiaUnidadDTO>();
            string query = string.Format(helper.SqlList, rersedcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }

        public List<RerEnergiaUnidadDTO> ListByPeriodo(int ipericodi)
        {
            string query = string.Format(helper.SqlListByPeriodo, ipericodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerEnergiaUnidadDTO> entities = new List<RerEnergiaUnidadDTO>();

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
