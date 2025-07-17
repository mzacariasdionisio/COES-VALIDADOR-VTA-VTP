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
    /// Clase de acceso a datos de la tabla PR_COMBUSTIBLE
    /// </summary>
    public class PrCombustibleRepository : RepositoryBase, IPrCombustibleRepository
    {
        public PrCombustibleRepository(string strConn)
            : base(strConn)
        {
        }

        PrCombustibleHelper helper = new PrCombustibleHelper();

        public int Save(CombustibleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Combcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Combabrev, DbType.String, entity.Combabrev);
            dbProvider.AddInParameter(command, helper.Combnomb, DbType.String, entity.Combnomb);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CombustibleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Combcodi, DbType.Int32, entity.Combcodi);
            dbProvider.AddInParameter(command, helper.Combabrev, DbType.String, entity.Combabrev);
            dbProvider.AddInParameter(command, helper.Combnomb, DbType.String, entity.Combnomb);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int combcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Combcodi, DbType.Int32, combcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CombustibleDTO GetById(int combcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Combcodi, DbType.Int32, combcodi);
            CombustibleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CombustibleDTO> List()
        {
            List<CombustibleDTO> entitys = new List<CombustibleDTO>();
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

        public List<CombustibleDTO> GetByCriteria()
        {
            List<CombustibleDTO> entitys = new List<CombustibleDTO>();
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

