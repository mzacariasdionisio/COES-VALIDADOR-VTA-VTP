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
    /// Clase de acceso a datos de la tabla FT_EXT_ENVIO_RELREQARCHIVO
    /// </summary>
    public class FtExtEnvioRelreqarchivoRepository : RepositoryBase, IFtExtEnvioRelreqarchivoRepository
    {
        public FtExtEnvioRelreqarchivoRepository(string strConn) : base(strConn)
        {
        }

        FtExtEnvioRelreqarchivoHelper helper = new FtExtEnvioRelreqarchivoHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(FtExtEnvioRelreqarchivoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fterracodi, DbType.Int32, entity.Fterracodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftereqcodi, DbType.Int32, entity.Ftereqcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftearccodi, DbType.Int32, entity.Ftearccodi));

            command.ExecuteNonQuery();
            return entity.Fterracodi;
        }

        public void Update(FtExtEnvioRelreqarchivoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Fterracodi, DbType.Int32, entity.Fterracodi);
            dbProvider.AddInParameter(command, helper.Ftereqcodi, DbType.Int32, entity.Ftereqcodi);
            dbProvider.AddInParameter(command, helper.Ftearccodi, DbType.Int32, entity.Ftearccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int fterracodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Fterracodi, DbType.Int32, fterracodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtEnvioRelreqarchivoDTO GetById(int fterracodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Fterracodi, DbType.Int32, fterracodi);
            FtExtEnvioRelreqarchivoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtEnvioRelreqarchivoDTO> List()
        {
            List<FtExtEnvioRelreqarchivoDTO> entitys = new List<FtExtEnvioRelreqarchivoDTO>();
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

        public List<FtExtEnvioRelreqarchivoDTO> GetByCriteria()
        {
            List<FtExtEnvioRelreqarchivoDTO> entitys = new List<FtExtEnvioRelreqarchivoDTO>();
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

        public List<FtExtEnvioRelreqarchivoDTO> GetByRequisitos(string ftereqcodis)
        {
            List<FtExtEnvioRelreqarchivoDTO> entitys = new List<FtExtEnvioRelreqarchivoDTO>();
            string sql = string.Format(helper.SqlGetByRequisitos, ftereqcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioRelreqarchivoDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        
    }
}
