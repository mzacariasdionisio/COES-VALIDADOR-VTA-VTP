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
    /// Clase de acceso a datos de la tabla FT_FICTECPROP
    /// </summary>
    public class FtFictecPropRepository : RepositoryBase, IFtFictecPropRepository
    {
        public FtFictecPropRepository(string strConn)
            : base(strConn)
        {
        }

        FtFictecPropHelper helper = new FtFictecPropHelper();

        public int Save(FtFictecPropDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ftpropcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ftpropnomb, DbType.String, entity.Ftpropnomb);
            dbProvider.AddInParameter(command, helper.Ftpropestado, DbType.Int32, entity.Ftpropestado);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi);
            dbProvider.AddInParameter(command, helper.Catecodi, DbType.Int32, entity.Catecodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FtFictecPropDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ftpropcodi, DbType.Int32, entity.Ftpropcodi);
            dbProvider.AddInParameter(command, helper.Ftpropnomb, DbType.String, entity.Ftpropnomb);
            dbProvider.AddInParameter(command, helper.Ftpropestado, DbType.Int32, entity.Ftpropestado);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi);
            dbProvider.AddInParameter(command, helper.Catecodi, DbType.Int32, entity.Catecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ftpropcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ftpropcodi, DbType.Int32, ftpropcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtFictecPropDTO GetById(int ftpropcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ftpropcodi, DbType.Int32, ftpropcodi);
            FtFictecPropDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtFictecPropDTO> List()
        {
            List<FtFictecPropDTO> entitys = new List<FtFictecPropDTO>();
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

        public List<FtFictecPropDTO> GetByCriteria()
        {
            List<FtFictecPropDTO> entitys = new List<FtFictecPropDTO>();
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
