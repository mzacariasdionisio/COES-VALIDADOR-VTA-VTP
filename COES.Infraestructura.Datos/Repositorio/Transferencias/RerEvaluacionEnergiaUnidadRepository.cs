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
    /// Clase de acceso a datos de la tabla RER_EVALUACION_ENERGIAUNIDAD
    /// </summary>
    public class RerEvaluacionEnergiaUnidadRepository : RepositoryBase, IRerEvaluacionEnergiaUnidadRepository
    {
        private string strConexion;
        RerEvaluacionEnergiaUnidadHelper helper = new RerEvaluacionEnergiaUnidadHelper();

        public RerEvaluacionEnergiaUnidadRepository(string strConn) : base(strConn)
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

        public void Save(RerEvaluacionEnergiaUnidadDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereeucodi, DbType.Int32, entity.Rereeucodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Reresecodi, DbType.Int32, entity.Reresecodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerevacodi, DbType.Int32, entity.Rerevacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereucodi, DbType.Int32, entity.Rereucodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rersedcodi, DbType.Int32, entity.Rersedcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereeuenergiaunidad, DbType.String, entity.Rereeuenergiaunidad));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereeutotenergia, DbType.Decimal, entity.Rereeutotenergia));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereeuusucreacionext, DbType.String, entity.Rereeuusucreacionext));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereeufeccreacionext, DbType.DateTime, entity.Rereeufeccreacionext));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereeuusucreacion, DbType.String, entity.Rereeuusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereeufeccreacion, DbType.DateTime, entity.Rereeufeccreacion));
            command.ExecuteNonQuery();
        }

        public void Update(RerEvaluacionEnergiaUnidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Reresecodi, DbType.Int32, entity.Reresecodi);
            dbProvider.AddInParameter(command, helper.Rerevacodi, DbType.Int32, entity.Rerevacodi);
            dbProvider.AddInParameter(command, helper.Rereucodi, DbType.Int32, entity.Rereucodi);
            dbProvider.AddInParameter(command, helper.Rersedcodi, DbType.Int32, entity.Rersedcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rereeuenergiaunidad, DbType.String, entity.Rereeuenergiaunidad);
            dbProvider.AddInParameter(command, helper.Rereeuusucreacionext, DbType.String, entity.Rereeuusucreacionext);
            dbProvider.AddInParameter(command, helper.Rereeufeccreacionext, DbType.DateTime, entity.Rereeufeccreacionext);
            dbProvider.AddInParameter(command, helper.Rereeuusucreacion, DbType.String, entity.Rereeuusucreacion);
            dbProvider.AddInParameter(command, helper.Rereeufeccreacion, DbType.DateTime, entity.Rereeufeccreacion);
            dbProvider.AddInParameter(command, helper.Rereeucodi, DbType.Int32, entity.Rereeucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rereeucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rereeucodi, DbType.Int32, rereeucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RerEvaluacionEnergiaUnidadDTO GetById(int rereeucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rereeucodi, DbType.Int32, rereeucodi);
            RerEvaluacionEnergiaUnidadDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.CreateById(dr);
                }
            }

            return entity;
        }

        public List<RerEvaluacionEnergiaUnidadDTO> List()
        {
            List<RerEvaluacionEnergiaUnidadDTO> entities = new List<RerEvaluacionEnergiaUnidadDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.CreateByList(dr));
                }
            }

            return entities;
        }

        public List<RerEvaluacionEnergiaUnidadDTO> GetByCriteria(int reresecodi, int rerevacodi)
        {
            string query = string.Format(helper.SqlGetByCriteria, reresecodi, rerevacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerEvaluacionEnergiaUnidadDTO> entitys = new List<RerEvaluacionEnergiaUnidadDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateByCriteria(dr));
                }
            }
            return entitys;
        }
    }
}
