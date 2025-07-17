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
    /// Clase de acceso a datos de la tabla FT_EXT_ENVIO_RELREQREV
    /// </summary>
    public class FtExtEnvioRelreqrevRepository: RepositoryBase, IFtExtEnvioRelreqrevRepository
    {
        public FtExtEnvioRelreqrevRepository(string strConn): base(strConn)
        {
        }

        FtExtEnvioRelreqrevHelper helper = new FtExtEnvioRelreqrevHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int SaveTransaccional(FtExtEnvioRelreqrevDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;


            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Frrrevcodi, DbType.Int32, entity.Frrrevcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftereqcodi, DbType.Int32, entity.Ftereqcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftrevcodi, DbType.Int32, entity.Ftrevcodi));
            

            command.ExecuteNonQuery();
            return entity.Frrrevcodi;
        }

        public int Save(FtExtEnvioRelreqrevDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Frrrevcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ftereqcodi, DbType.Int32, entity.Ftereqcodi);
            dbProvider.AddInParameter(command, helper.Ftrevcodi, DbType.Int32, entity.Ftrevcodi);
            

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FtExtEnvioRelreqrevDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ftrevcodi, DbType.Int32, entity.Ftrevcodi);
            dbProvider.AddInParameter(command, helper.Frrrevcodi, DbType.Int32, entity.Frrrevcodi);
            dbProvider.AddInParameter(command, helper.Ftereqcodi, DbType.Int32, entity.Ftereqcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int frrrevcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Frrrevcodi, DbType.Int32, frrrevcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtEnvioRelreqrevDTO GetById(int frrrevcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Frrrevcodi, DbType.Int32, frrrevcodi);
            FtExtEnvioRelreqrevDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtEnvioRelreqrevDTO> List()
        {
            List<FtExtEnvioRelreqrevDTO> entitys = new List<FtExtEnvioRelreqrevDTO>();
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

        public List<FtExtEnvioRelreqrevDTO> GetByCriteria()
        {
            List<FtExtEnvioRelreqrevDTO> entitys = new List<FtExtEnvioRelreqrevDTO>();
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

        public List<FtExtEnvioRelreqrevDTO> GetByRequisitos(string ftereqcodis)
        {
            List<FtExtEnvioRelreqrevDTO> entitys = new List<FtExtEnvioRelreqrevDTO>();
            string sql = string.Format(helper.SqlGetByRequisitos, ftereqcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioRelreqrevDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
