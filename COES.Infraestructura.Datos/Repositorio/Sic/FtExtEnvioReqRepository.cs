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
    /// Clase de acceso a datos de la tabla FT_EXT_ENVIO_REQ
    /// </summary>
    public class FtExtEnvioReqRepository : RepositoryBase, IFtExtEnvioReqRepository
    {
        public FtExtEnvioReqRepository(string strConn) : base(strConn)
        {
        }

        FtExtEnvioReqHelper helper = new FtExtEnvioReqHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(FtExtEnvioReqDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftereqcodi, DbType.Int32, entity.Ftereqcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fevrqcodi, DbType.Int32, entity.Fevrqcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftevercodi, DbType.Int32, entity.Ftevercodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftereqflageditable, DbType.String, entity.Ftereqflageditable));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftereqflagrevisable, DbType.String, entity.Ftereqflagrevisable));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftereqflagarchivo, DbType.Int32, entity.Ftereqflagarchivo));

            command.ExecuteNonQuery();
            return entity.Ftereqcodi;
        }

        public void Update(FtExtEnvioReqDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ftereqcodi, DbType.Int32, entity.Ftereqcodi);
            dbProvider.AddInParameter(command, helper.Fevrqcodi, DbType.Int32, entity.Fevrqcodi);
            dbProvider.AddInParameter(command, helper.Ftevercodi, DbType.Int32, entity.Ftevercodi);
            dbProvider.AddInParameter(command, helper.Ftereqflageditable, DbType.String, entity.Ftereqflageditable);
            dbProvider.AddInParameter(command, helper.Ftereqflagrevisable, DbType.String, entity.Ftereqflagrevisable);
            dbProvider.AddInParameter(command, helper.Ftereqflagarchivo, DbType.Int32, entity.Ftereqflagarchivo);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ftereqcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ftereqcodi, DbType.Int32, ftereqcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtEnvioReqDTO GetById(int ftereqcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ftereqcodi, DbType.Int32, ftereqcodi);
            FtExtEnvioReqDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtEnvioReqDTO> List()
        {
            List<FtExtEnvioReqDTO> entitys = new List<FtExtEnvioReqDTO>();
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

        public List<FtExtEnvioReqDTO> GetByCriteria()
        {
            List<FtExtEnvioReqDTO> entitys = new List<FtExtEnvioReqDTO>();
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

        public List<FtExtEnvioReqDTO> GetListByVersiones(string ftevercodis)
        {
            List<FtExtEnvioReqDTO> entitys = new List<FtExtEnvioReqDTO>();
            string sql = string.Format(helper.SqlGetListByVersiones, ftevercodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioReqDTO entity = helper.Create(dr);

                    int iFevrqliteral = dr.GetOrdinal(helper.Fevrqliteral);
                    if (!dr.IsDBNull(iFevrqliteral)) entity.Fevrqliteral = dr.GetString(iFevrqliteral);
                    

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        
    }
}
