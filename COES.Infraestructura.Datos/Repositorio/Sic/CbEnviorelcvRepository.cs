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
    /// Clase de acceso a datos de la tabla CB_ENVIORELCV
    /// </summary>
    public class CbEnviorelcvRepository: RepositoryBase, ICbEnviorelcvRepository
    {
        public CbEnviorelcvRepository(string strConn): base(strConn)
        {
        }

        CbEnviorelcvHelper helper = new CbEnviorelcvHelper();

        public int Save(CbEnviorelcvDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cbcvcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cbenvcodi, DbType.Int32, entity.Cbenvcodi);
            dbProvider.AddInParameter(command, helper.Repcodi, DbType.Int32, entity.Repcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int SaveTransaccional(CbEnviorelcvDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            DbCommand command2 = (DbCommand)connection.CreateCommand();

            command2.CommandText = helper.SqlSave;
            command2.Transaction = (DbTransaction)transaction;
            command2.Connection = (DbConnection)connection;

            command2.Parameters.Add(dbProvider.CreateParameter(command2, helper.Cbcvcodi, DbType.Int32, id));
            command2.Parameters.Add(dbProvider.CreateParameter(command2, helper.Cbenvcodi, DbType.Int32, entity.Cbenvcodi));
            command2.Parameters.Add(dbProvider.CreateParameter(command2, helper.Repcodi, DbType.Int32, entity.Repcodi));

            command2.ExecuteNonQuery();

            return id;
        }

        public void Update(CbEnviorelcvDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cbcvcodi, DbType.Int32, entity.Cbcvcodi);
            dbProvider.AddInParameter(command, helper.Cbenvcodi, DbType.Int32, entity.Cbenvcodi);
            dbProvider.AddInParameter(command, helper.Repcodi, DbType.Int32, entity.Repcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cbcvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cbcvcodi, DbType.Int32, cbcvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CbEnviorelcvDTO GetById(int cbcvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cbcvcodi, DbType.Int32, cbcvcodi);
            CbEnviorelcvDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CbEnviorelcvDTO> List()
        {
            List<CbEnviorelcvDTO> entitys = new List<CbEnviorelcvDTO>();
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

        public List<CbEnviorelcvDTO> GetByCriteria()
        {
            List<CbEnviorelcvDTO> entitys = new List<CbEnviorelcvDTO>();
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
