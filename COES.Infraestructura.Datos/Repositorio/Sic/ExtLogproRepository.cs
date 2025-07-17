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
    /// Clase de acceso a datos de la tabla EXT_LOGPRO
    /// </summary>
    public class ExtLogproRepository: RepositoryBase, IExtLogproRepository
    {
        public ExtLogproRepository(string strConn): base(strConn)
        {
        }

        ExtLogproHelper helper = new ExtLogproHelper();

        public void Save(ExtLogproDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Mencodi, DbType.Int32, entity.Mencodi);
            dbProvider.AddInParameter(command, helper.Logpdetmen, DbType.String, entity.Logpdetmen);
            dbProvider.AddInParameter(command, helper.Logpfechor, DbType.DateTime, entity.Logpfechor);
            dbProvider.AddInParameter(command, helper.Logpsecuen, DbType.Int32, entity.Logpsecuen);
            dbProvider.AddInParameter(command, helper.Earcodi, DbType.Int32, entity.Earcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(ExtLogproDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mencodi, DbType.Int32, entity.Mencodi);
            dbProvider.AddInParameter(command, helper.Logpdetmen, DbType.String, entity.Logpdetmen);
            dbProvider.AddInParameter(command, helper.Logpfechor, DbType.DateTime, entity.Logpfechor);
            dbProvider.AddInParameter(command, helper.Logpsecuen, DbType.Int32, entity.Logpsecuen);
            dbProvider.AddInParameter(command, helper.Earcodi, DbType.Int32, entity.Earcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);


            dbProvider.ExecuteNonQuery(command);
        }

        public ExtLogproDTO GetById()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            ExtLogproDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ExtLogproDTO> List()
        {
            List<ExtLogproDTO> entitys = new List<ExtLogproDTO>();
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

        public List<ExtLogproDTO> GetByCriteria()
        {
            List<ExtLogproDTO> entitys = new List<ExtLogproDTO>();
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
