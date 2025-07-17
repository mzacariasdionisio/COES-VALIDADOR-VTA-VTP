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
    /// Clase de acceso a datos de la tabla RER_CALCULO_ANUAL
    /// </summary>
    public class RerCalculoAnualRepository : RepositoryBase, IRerCalculoAnualRepository
    {
        private string strConexion;
        RerCalculoAnualHelper helper = new RerCalculoAnualHelper();

        public RerCalculoAnualRepository(string strConn) : base(strConn)
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

        public void Save(RerCalculoAnualDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercacodi, DbType.Int32, entity.Rercacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Reravcodi, DbType.Int32, entity.Reravcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercaippi, DbType.Decimal, entity.Rercaippi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercaippo, DbType.Decimal, entity.Rercaippo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercataradjbase, DbType.Decimal, entity.Rercataradjbase));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercafaccorreccion, DbType.Decimal, entity.Rercafaccorreccion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercafacactanterior, DbType.Decimal, entity.Rercafacactanterior));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercafacactualizacion, DbType.Decimal, entity.Rercafacactualizacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercataradj, DbType.Decimal, entity.Rercataradj));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercacomment, DbType.String, entity.Rercacomment));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercausucreacion, DbType.String, entity.Rercausucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercafeccreacion, DbType.DateTime, entity.Rercafeccreacion));
            command.ExecuteNonQuery();
        }

        public void Update(RerCalculoAnualDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Reravcodi, DbType.Int32, entity.Reravcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rercaippi, DbType.Decimal, entity.Rercaippi);
            dbProvider.AddInParameter(command, helper.Rercaippo, DbType.Decimal, entity.Rercaippo);
            dbProvider.AddInParameter(command, helper.Rercataradjbase, DbType.Decimal, entity.Rercataradjbase);
            dbProvider.AddInParameter(command, helper.Rercafaccorreccion, DbType.Decimal, entity.Rercafaccorreccion);
            dbProvider.AddInParameter(command, helper.Rercafacactanterior, DbType.Decimal, entity.Rercafacactanterior);
            dbProvider.AddInParameter(command, helper.Rercafacactualizacion, DbType.Decimal, entity.Rercafacactualizacion);
            dbProvider.AddInParameter(command, helper.Rercataradj, DbType.Decimal, entity.Rercataradj);
            dbProvider.AddInParameter(command, helper.Rercacomment, DbType.String, entity.Rercacomment);
            dbProvider.AddInParameter(command, helper.Rercausucreacion, DbType.String, entity.Rercausucreacion);
            dbProvider.AddInParameter(command, helper.Rercafeccreacion, DbType.DateTime, entity.Rercafeccreacion);
            dbProvider.AddInParameter(command, helper.Rercacodi, DbType.Int32, entity.Rercacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rercacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rercacodi, DbType.Int32, rercacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByAnioVersion(int reravcodi, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand) conn.CreateCommand();
            command.CommandText = helper.SqlDeleteByAnioVersion;
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Reravcodi, DbType.Int32, reravcodi));
            command.ExecuteNonQuery();
        }

        public RerCalculoAnualDTO GetById(int rercacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rercacodi, DbType.Int32, rercacodi);
            RerCalculoAnualDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RerCalculoAnualDTO> GetByAnnioAndVersion(int reravaniotarif, string reravversion)
        {
            string query = string.Format(helper.SqlGetByAnioAndVersion, reravversion, reravaniotarif);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerCalculoAnualDTO> entities = new List<RerCalculoAnualDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.CreateByAnnioAndVersion(dr));
                }

            }

            return entities;
        }

        public List<RerCalculoAnualDTO> List()
        {
            List<RerCalculoAnualDTO> entities = new List<RerCalculoAnualDTO>();
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
    }
}

