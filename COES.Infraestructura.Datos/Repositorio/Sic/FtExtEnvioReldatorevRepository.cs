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
    /// Clase de acceso a datos de la tabla FT_EXT_ENVIO_RELDATOREV
    /// </summary>
    public class FtExtEnvioReldatorevRepository: RepositoryBase, IFtExtEnvioReldatorevRepository
    {
        public FtExtEnvioReldatorevRepository(string strConn): base(strConn)
        {
        }

        FtExtEnvioReldatorevHelper helper = new FtExtEnvioReldatorevHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int SaveTransaccional(FtExtEnvioReldatorevDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftedatcodi, DbType.Int32, entity.Ftedatcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftrevcodi, DbType.Int32, entity.Ftrevcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Frdrevcodi, DbType.Int32, entity.Frdrevcodi));

            command.ExecuteNonQuery();
            return entity.Frdrevcodi;
        }

        public int Save(FtExtEnvioReldatorevDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            
            dbProvider.AddInParameter(command, helper.Ftedatcodi, DbType.Int32, entity.Ftedatcodi);
            dbProvider.AddInParameter(command, helper.Ftrevcodi, DbType.Int32, entity.Ftrevcodi);
            dbProvider.AddInParameter(command, helper.Frdrevcodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FtExtEnvioReldatorevDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ftedatcodi, DbType.Int32, entity.Ftedatcodi);
            dbProvider.AddInParameter(command, helper.Ftrevcodi, DbType.Int32, entity.Ftrevcodi);
            dbProvider.AddInParameter(command, helper.Frdrevcodi, DbType.Int32, entity.Frdrevcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int frdrevcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Frdrevcodi, DbType.Int32, frdrevcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtEnvioReldatorevDTO GetById(int frdrevcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Frdrevcodi, DbType.Int32, frdrevcodi);
            FtExtEnvioReldatorevDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtEnvioReldatorevDTO> List()
        {
            List<FtExtEnvioReldatorevDTO> entitys = new List<FtExtEnvioReldatorevDTO>();
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

        public List<FtExtEnvioReldatorevDTO> GetByCriteria()
        {
            List<FtExtEnvioReldatorevDTO> entitys = new List<FtExtEnvioReldatorevDTO>();
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

        public List<FtExtEnvioReldatorevDTO> GetByDatos(string ftedatcodis)
        {
            List<FtExtEnvioReldatorevDTO> entitys = new List<FtExtEnvioReldatorevDTO>();
            string sql = string.Format(helper.SqlGetByDatos, ftedatcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioReldatorevDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        
    }
}
