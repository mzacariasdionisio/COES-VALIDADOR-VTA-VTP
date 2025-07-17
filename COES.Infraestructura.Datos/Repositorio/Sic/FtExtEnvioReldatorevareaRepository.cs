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
    /// Clase de acceso a datos de la tabla FT_EXT_ENVIO_RELDATOREVAREA
    /// </summary>
    public class FtExtEnvioReldatorevareaRepository: RepositoryBase, IFtExtEnvioReldatorevareaRepository
    {
        public FtExtEnvioReldatorevareaRepository(string strConn): base(strConn)
        {
        }

        FtExtEnvioReldatorevareaHelper helper = new FtExtEnvioReldatorevareaHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(FtExtEnvioReldatorevareaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Revadcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ftedatcodi, DbType.Int32, entity.Ftedatcodi);
            dbProvider.AddInParameter(command, helper.Revacodi, DbType.Int32, entity.Revacodi);
            dbProvider.AddInParameter(command, helper.Envarcodi, DbType.Int32, entity.Envarcodi);
            dbProvider.AddInParameter(command, helper.Ftevercodi, DbType.Int32, entity.Ftevercodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int Save(FtExtEnvioReldatorevareaDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Revadcodi, DbType.Int32, entity.Revadcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftedatcodi, DbType.Int32, entity.Ftedatcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Revacodi, DbType.Int32, entity.Revacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Envarcodi, DbType.Int32, entity.Envarcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftevercodi, DbType.Int32, entity.Ftevercodi));

            command.ExecuteNonQuery();
            return entity.Revadcodi;
        }

        public void Update(FtExtEnvioReldatorevareaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Revadcodi, DbType.Int32, entity.Revadcodi);
            dbProvider.AddInParameter(command, helper.Ftedatcodi, DbType.Int32, entity.Ftedatcodi);
            dbProvider.AddInParameter(command, helper.Revacodi, DbType.Int32, entity.Revacodi);
            dbProvider.AddInParameter(command, helper.Envarcodi, DbType.Int32, entity.Envarcodi);
            dbProvider.AddInParameter(command, helper.Ftevercodi, DbType.Int32, entity.Ftevercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int revadcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Revadcodi, DbType.Int32, revadcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeletePorGrupo(string revadcodis)
        {            
            string sql = string.Format(helper.SqlDeletePorGrupo, revadcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command)) { }            
        }

        public void DeletePorIds(string revadcodis, IDbConnection connection, DbTransaction transaction)
        {
            string sql = string.Format(helper.SqlDeletePorIds, revadcodis);
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = sql;

                dbCommand.ExecuteNonQuery();
            }
        }

        public List<FtExtEnvioReldatorevareaDTO> ListarRelacionesPorVersionAreaYEquipo(int ftevercodi, int faremcodi, int fteeqcodi)
        {            
            List<FtExtEnvioReldatorevareaDTO> entitys = new List<FtExtEnvioReldatorevareaDTO>();

            string sql = string.Format(helper.SqlListarRelacionesPorVersionAreaYEquipo, ftevercodi, faremcodi, fteeqcodi);
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


        public FtExtEnvioReldatorevareaDTO GetById(int revadcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Revadcodi, DbType.Int32, revadcodi);
            FtExtEnvioReldatorevareaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtEnvioReldatorevareaDTO> List()
        {
            List<FtExtEnvioReldatorevareaDTO> entitys = new List<FtExtEnvioReldatorevareaDTO>();
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

        public List<FtExtEnvioReldatorevareaDTO> GetByCriteria()
        {
            List<FtExtEnvioReldatorevareaDTO> entitys = new List<FtExtEnvioReldatorevareaDTO>();
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
    }
}
