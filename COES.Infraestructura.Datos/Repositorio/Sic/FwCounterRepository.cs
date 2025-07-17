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
    /// Clase de acceso a datos de la tabla FW_COUNTER
    /// </summary>
    public class FwCounterRepository: RepositoryBase, IFwCounterRepository
    {
        public FwCounterRepository(string strConn): base(strConn)
        {
        }

        FwCounterHelper helper = new FwCounterHelper();

        

        public void Update(FwCounterDTO entity)
        {
            string query = string.Format(helper.SqlUpdate, entity.Maxcount, entity.Tablename);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(string tablename)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Tablename, DbType.String, tablename);

            dbProvider.ExecuteNonQuery(command);
        }

        public FwCounterDTO GetById(string tablename)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tablename, DbType.String, tablename);
            FwCounterDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FwCounterDTO> List()
        {
            List<FwCounterDTO> entitys = new List<FwCounterDTO>();
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

        public List<FwCounterDTO> GetByCriteria()
        {
            List<FwCounterDTO> entitys = new List<FwCounterDTO>();
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

        public void UpdateMaxCount(string tablename)
        {
            string query = string.Format(helper.SqlUpdateMaxCount, tablename);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}
