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
    /// Clase de acceso a datos de la tabla FT_EXT_ENVIO_RELREVARCHIVO
    /// </summary>
    public class FtExtEnvioRelrevarchivoRepository: RepositoryBase, IFtExtEnvioRelrevarchivoRepository
    {
        public FtExtEnvioRelrevarchivoRepository(string strConn): base(strConn)
        {
        }

        FtExtEnvioRelrevarchivoHelper helper = new FtExtEnvioRelrevarchivoHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(FtExtEnvioRelrevarchivoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftrrvacodi, DbType.Int32, entity.Ftrrvacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftearccodi, DbType.Int32, entity.Ftearccodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftrevcodi, DbType.Int32, entity.Ftrevcodi));

            command.ExecuteNonQuery();

            return entity.Ftrrvacodi;
        }

        public int Save(FtExtEnvioRelrevarchivoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ftrrvacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ftearccodi, DbType.Int32, entity.Ftearccodi);
            dbProvider.AddInParameter(command, helper.Ftrevcodi, DbType.Int32, entity.Ftrevcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FtExtEnvioRelrevarchivoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ftearccodi, DbType.Int32, entity.Ftearccodi);
            dbProvider.AddInParameter(command, helper.Ftrevcodi, DbType.Int32, entity.Ftrevcodi);
            dbProvider.AddInParameter(command, helper.Ftrrvacodi, DbType.Int32, entity.Ftrrvacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ftrrvacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ftrrvacodi, DbType.Int32, ftrrvacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtEnvioRelrevarchivoDTO GetById(int ftrrvacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ftrrvacodi, DbType.Int32, ftrrvacodi);
            FtExtEnvioRelrevarchivoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtEnvioRelrevarchivoDTO> List()
        {
            List<FtExtEnvioRelrevarchivoDTO> entitys = new List<FtExtEnvioRelrevarchivoDTO>();
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

        public List<FtExtEnvioRelrevarchivoDTO> GetByCriteria()
        {
            List<FtExtEnvioRelrevarchivoDTO> entitys = new List<FtExtEnvioRelrevarchivoDTO>();
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

        public List<FtExtEnvioRelrevarchivoDTO> GetByRevision(string strFtrevcodis)
        {
            List<FtExtEnvioRelrevarchivoDTO> entitys = new List<FtExtEnvioRelrevarchivoDTO>();
            string sql = string.Format(helper.SqlGetByRevision, strFtrevcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioRelrevarchivoDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        
    }
}
