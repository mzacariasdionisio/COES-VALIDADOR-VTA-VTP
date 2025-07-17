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
    /// Clase de acceso a datos de la tabla PR_CATEGORIA
    /// </summary>
    public class PrCategoriaRepository: RepositoryBase, IPrCategoriaRepository
    {
        public PrCategoriaRepository(string strConn): base(strConn)
        {
        }

        PrCategoriaHelper helper = new PrCategoriaHelper();

        public int Save(PrCategoriaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Catecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cateabrev, DbType.String, entity.Cateabrev);
            dbProvider.AddInParameter(command, helper.Catenomb, DbType.String, entity.Catenomb);
            dbProvider.AddInParameter(command, helper.Catetipo, DbType.String, entity.Catetipo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PrCategoriaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cateabrev, DbType.String, entity.Cateabrev);
            dbProvider.AddInParameter(command, helper.Catenomb, DbType.String, entity.Catenomb);
            dbProvider.AddInParameter(command, helper.Catetipo, DbType.String, entity.Catetipo);
            dbProvider.AddInParameter(command, helper.Catecodi, DbType.Int32, entity.Catecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int catecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Catecodi, DbType.Int32, catecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrCategoriaDTO GetById(int catecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Catecodi, DbType.Int32, catecodi);
            PrCategoriaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrCategoriaDTO> List()
        {
            List<PrCategoriaDTO> entitys = new List<PrCategoriaDTO>();
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

        public List<PrCategoriaDTO> GetByCriteria()
        {
            List<PrCategoriaDTO> entitys = new List<PrCategoriaDTO>();
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

        public List<PrCategoriaDTO> ListByOriglectcodiYEmprcodi(int origlectcodi, int emprcodi)
        {
            List<PrCategoriaDTO> entitys = new List<PrCategoriaDTO>();
            string sql = string.Format(helper.SqlListByOriglectcodiYEmprcodi, origlectcodi, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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
