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
    /// Clase de acceso a datos de la tabla IEE_RECENERGETICO_TIPO
    /// </summary>
    public class IeeRecenergeticoTipoRepository : RepositoryBase, IIeeRecenergeticoTipoRepository
    {
        public IeeRecenergeticoTipoRepository(string strConn) : base(strConn)
        {
        }

        IeeRecenergeticoTipoHelper helper = new IeeRecenergeticoTipoHelper();

        public int Save(IeeRecenergeticoTipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Renertipcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Renerabrev, DbType.String, entity.Renerabrev);
            dbProvider.AddInParameter(command, helper.Renertipnomb, DbType.String, entity.Renertipnomb);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(IeeRecenergeticoTipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Renertipcodi, DbType.Int32, entity.Renertipcodi);
            dbProvider.AddInParameter(command, helper.Renerabrev, DbType.String, entity.Renerabrev);
            dbProvider.AddInParameter(command, helper.Renertipnomb, DbType.String, entity.Renertipnomb);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int renertipcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Renertipcodi, DbType.Int32, renertipcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public IeeRecenergeticoTipoDTO GetById(int renertipcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Renertipcodi, DbType.Int32, renertipcodi);
            IeeRecenergeticoTipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<IeeRecenergeticoTipoDTO> List()
        {
            List<IeeRecenergeticoTipoDTO> entitys = new List<IeeRecenergeticoTipoDTO>();
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

        public List<IeeRecenergeticoTipoDTO> GetByCriteria()
        {
            List<IeeRecenergeticoTipoDTO> entitys = new List<IeeRecenergeticoTipoDTO>();
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
