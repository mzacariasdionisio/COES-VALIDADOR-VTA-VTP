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
    /// Clase de acceso a datos de la tabla FT_EXT_ENVIO_RELDATOARCHIVO
    /// </summary>
    public class FtExtEnvioReldatoarchivoRepository : RepositoryBase, IFtExtEnvioReldatoarchivoRepository
    {
        public FtExtEnvioReldatoarchivoRepository(string strConn) : base(strConn)
        {
        }

        FtExtEnvioReldatoarchivoHelper helper = new FtExtEnvioReldatoarchivoHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(FtExtEnvioReldatoarchivoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftedatcodi, DbType.Int32, entity.Ftedatcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftearccodi, DbType.Int32, entity.Ftearccodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fterdacodi, DbType.Int32, entity.Fterdacodi));

            command.ExecuteNonQuery();
            return entity.Fterdacodi;
        }

        public void Update(FtExtEnvioReldatoarchivoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ftedatcodi, DbType.Int32, entity.Ftedatcodi);
            dbProvider.AddInParameter(command, helper.Ftearccodi, DbType.Int32, entity.Ftearccodi);
            dbProvider.AddInParameter(command, helper.Fterdacodi, DbType.Int32, entity.Fterdacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int fterdacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Fterdacodi, DbType.Int32, fterdacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtEnvioReldatoarchivoDTO GetById(int fterdacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Fterdacodi, DbType.Int32, fterdacodi);
            FtExtEnvioReldatoarchivoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtEnvioReldatoarchivoDTO> List()
        {
            List<FtExtEnvioReldatoarchivoDTO> entitys = new List<FtExtEnvioReldatoarchivoDTO>();
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

        public List<FtExtEnvioReldatoarchivoDTO> GetByCriteria()
        {
            List<FtExtEnvioReldatoarchivoDTO> entitys = new List<FtExtEnvioReldatoarchivoDTO>();
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
