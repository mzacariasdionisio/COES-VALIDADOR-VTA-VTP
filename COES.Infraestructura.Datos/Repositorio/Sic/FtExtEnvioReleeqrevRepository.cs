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
    /// Clase de acceso a datos de la tabla FT_EXT_ENVIO_RELEEQREV
    /// </summary>
    public class FtExtEnvioReleeqrevRepository: RepositoryBase, IFtExtEnvioReleeqrevRepository
    {
        public FtExtEnvioReleeqrevRepository(string strConn): base(strConn)
        {
        }

        FtExtEnvioReleeqrevHelper helper = new FtExtEnvioReleeqrevHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int SaveTransaccional(FtExtEnvioReleeqrevDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Freqrvcodi, DbType.Int32, entity.Freqrvcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteeqcodi, DbType.Int32, entity.Fteeqcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftrevcodi, DbType.Int32, entity.Ftrevcodi));

            command.ExecuteNonQuery();
            return entity.Freqrvcodi;
        }
        public int Save(FtExtEnvioReleeqrevDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Freqrvcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Fteeqcodi, DbType.Int32, entity.Fteeqcodi);
            dbProvider.AddInParameter(command, helper.Ftrevcodi, DbType.Int32, entity.Ftrevcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FtExtEnvioReleeqrevDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Freqrvcodi, DbType.Int32, entity.Freqrvcodi);
            dbProvider.AddInParameter(command, helper.Fteeqcodi, DbType.Int32, entity.Fteeqcodi);
            dbProvider.AddInParameter(command, helper.Ftrevcodi, DbType.Int32, entity.Ftrevcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int freqrvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Freqrvcodi, DbType.Int32, freqrvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtEnvioReleeqrevDTO GetById(int freqrvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Freqrvcodi, DbType.Int32, freqrvcodi);
            FtExtEnvioReleeqrevDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtEnvioReleeqrevDTO> List()
        {
            List<FtExtEnvioReleeqrevDTO> entitys = new List<FtExtEnvioReleeqrevDTO>();
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

        public List<FtExtEnvioReleeqrevDTO> GetByCriteria()
        {
            List<FtExtEnvioReleeqrevDTO> entitys = new List<FtExtEnvioReleeqrevDTO>();
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

        public List<FtExtEnvioReleeqrevDTO> GetByEquipos(string fteeqcodis)
        {
            List<FtExtEnvioReleeqrevDTO> entitys = new List<FtExtEnvioReleeqrevDTO>();
            string sql = string.Format(helper.SqlGetByEquipos, fteeqcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioReleeqrevDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
