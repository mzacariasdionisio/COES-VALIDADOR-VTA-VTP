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
    /// Clase de acceso a datos de la tabla FT_EXT_ENVIO_RELREVAREAARCHIVO
    /// </summary>
    public class FtExtEnvioRelrevareaarchivoRepository: RepositoryBase, IFtExtEnvioRelrevareaarchivoRepository
    {
        public FtExtEnvioRelrevareaarchivoRepository(string strConn): base(strConn)
        {
        }

        FtExtEnvioRelrevareaarchivoHelper helper = new FtExtEnvioRelrevareaarchivoHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(FtExtEnvioRelrevareaarchivoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Revaacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Revacodi, DbType.Int32, entity.Revacodi);
            dbProvider.AddInParameter(command, helper.Ftearccodi, DbType.Int32, entity.Ftearccodi);
            dbProvider.AddInParameter(command, helper.Ftevercodi, DbType.Int32, entity.Ftevercodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int Save(FtExtEnvioRelrevareaarchivoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Revaacodi, DbType.Int32, entity.Revaacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Revacodi, DbType.Int32, entity.Revacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftearccodi, DbType.Int32, entity.Ftearccodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftevercodi, DbType.Int32, entity.Ftevercodi));

            command.ExecuteNonQuery();
            return entity.Revaacodi;
        }

        public void Update(FtExtEnvioRelrevareaarchivoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Revaacodi, DbType.Int32, entity.Revaacodi);
            dbProvider.AddInParameter(command, helper.Revacodi, DbType.Int32, entity.Revacodi);
            dbProvider.AddInParameter(command, helper.Ftearccodi, DbType.Int32, entity.Ftearccodi);
            dbProvider.AddInParameter(command, helper.Ftevercodi, DbType.Int32, entity.Ftevercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int revaacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Revaacodi, DbType.Int32, revaacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeletePorGrupo(string revaacodis)
        {
            string sql = string.Format(helper.SqlDeletePorGrupo, revaacodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command)) { }
        }

        

        public void DeletePorIds(string revaacodis, IDbConnection connection, DbTransaction transaction)
        {
            string sql = string.Format(helper.SqlDeletePorIds, revaacodis);
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = sql;

                dbCommand.ExecuteNonQuery();
            }
        }

        public List<FtExtEnvioRelrevareaarchivoDTO> ListarRelacionesPorVersionAreaYEquipo(int ftevercodi, int faremcodi, int fteeqcodi)
        {
            List<FtExtEnvioRelrevareaarchivoDTO> entitys = new List<FtExtEnvioRelrevareaarchivoDTO>();

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

        public List<FtExtEnvioRelrevareaarchivoDTO> ListarRelacionesContenidoPorVersionArea(int ftevercodi, int faremcodi)
        {
            List<FtExtEnvioRelrevareaarchivoDTO> entitys = new List<FtExtEnvioRelrevareaarchivoDTO>();

            string sql = string.Format(helper.SqlListarRelacionesContenidoPorVersionArea, ftevercodi, faremcodi);
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

        

        public FtExtEnvioRelrevareaarchivoDTO GetById(int revaacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Revaacodi, DbType.Int32, revaacodi);
            FtExtEnvioRelrevareaarchivoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtEnvioRelrevareaarchivoDTO> List()
        {
            List<FtExtEnvioRelrevareaarchivoDTO> entitys = new List<FtExtEnvioRelrevareaarchivoDTO>();
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

        public List<FtExtEnvioRelrevareaarchivoDTO> GetByCriteria()
        {
            List<FtExtEnvioRelrevareaarchivoDTO> entitys = new List<FtExtEnvioRelrevareaarchivoDTO>();
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

        public List<FtExtEnvioRelrevareaarchivoDTO> ListarPorVersiones(string ftevercodis)
        {
            List<FtExtEnvioRelrevareaarchivoDTO> entitys = new List<FtExtEnvioRelrevareaarchivoDTO>();

            string sql = string.Format(helper.SqlListarPorVersiones, ftevercodis);
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
