using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla FT_EXT_ENVIO_RELREQREVAREA
    /// </summary>
    public class FtExtEnvioRelreqrevareaRepository: RepositoryBase, IFtExtEnvioRelreqrevareaRepository
    {
        public FtExtEnvioRelreqrevareaRepository(string strConn): base(strConn)
        {
        }

        FtExtEnvioRelreqrevareaHelper helper = new FtExtEnvioRelreqrevareaHelper();


        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(FtExtEnvioRelreqrevareaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Revarqcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Revacodi, DbType.Int32, entity.Revacodi);
            dbProvider.AddInParameter(command, helper.Ftereqcodi, DbType.Int32, entity.Ftereqcodi);
            dbProvider.AddInParameter(command, helper.Envarcodi, DbType.Int32, entity.Envarcodi);
            dbProvider.AddInParameter(command, helper.Ftevercodi, DbType.Int32, entity.Ftevercodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int Save(FtExtEnvioRelreqrevareaDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Revarqcodi, DbType.Int32, entity.Revarqcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Revacodi, DbType.Int32, entity.Revacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftereqcodi, DbType.Int32, entity.Ftereqcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Envarcodi, DbType.Int32, entity.Envarcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftevercodi, DbType.Int32, entity.Ftevercodi));

            command.ExecuteNonQuery();
            return entity.Revarqcodi;
        }

        public void Update(FtExtEnvioRelreqrevareaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Revarqcodi, DbType.Int32, entity.Revarqcodi);
            dbProvider.AddInParameter(command, helper.Revacodi, DbType.Int32, entity.Revacodi);
            dbProvider.AddInParameter(command, helper.Ftereqcodi, DbType.Int32, entity.Ftereqcodi);
            dbProvider.AddInParameter(command, helper.Envarcodi, DbType.Int32, entity.Envarcodi);
            dbProvider.AddInParameter(command, helper.Ftevercodi, DbType.Int32, entity.Ftevercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int revarqcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Revarqcodi, DbType.Int32, revarqcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtEnvioRelreqrevareaDTO GetById(int revarqcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Revarqcodi, DbType.Int32, revarqcodi);
            FtExtEnvioRelreqrevareaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtEnvioRelreqrevareaDTO> List()
        {
            List<FtExtEnvioRelreqrevareaDTO> entitys = new List<FtExtEnvioRelreqrevareaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<FtExtEnvioRelreqrevareaDTO> GetByCriteria()
        {
            List<FtExtEnvioRelreqrevareaDTO> entitys = new List<FtExtEnvioRelreqrevareaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public void DeletePorIds(string revarqcodis, IDbConnection connection, DbTransaction transaction)
        {
            string sql = string.Format(helper.SqlDeletePorIds, revarqcodis);
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = sql;

                dbCommand.ExecuteNonQuery();
            }
        }

        public List<FtExtEnvioRelreqrevareaDTO> ListarRelacionesPorVersionArea(int ftevercodi, int faremcodi)
        {
            List<FtExtEnvioRelreqrevareaDTO> entitys = new List<FtExtEnvioRelreqrevareaDTO>();

            string sql = string.Format(helper.SqlListarRelacionesPorVersionArea, ftevercodi, faremcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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
