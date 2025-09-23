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
    /// Clase de acceso a datos de la tabla PR_RESTRIC_TIPO
    /// </summary>
    public class PrRestricTipoRepository: RepositoryBase, IPrRestricTipoRepository
    {
        public PrRestricTipoRepository(string strConn): base(strConn)
        {
        }

        PrRestricTipoHelper helper = new PrRestricTipoHelper();

        public int Save(PrRestricTipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Restipcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Restipnombre, DbType.String, entity.Restipnombre);
            dbProvider.AddInParameter(command, helper.Restipusucreacion, DbType.String, entity.Restipusucreacion);
            dbProvider.AddInParameter(command, helper.Restipfeccreacion, DbType.DateTime, entity.Restipfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PrRestricTipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Restipcodi, DbType.Int32, entity.Restipcodi);
            dbProvider.AddInParameter(command, helper.Restipnombre, DbType.String, entity.Restipnombre);
            dbProvider.AddInParameter(command, helper.Restipusucreacion, DbType.String, entity.Restipusucreacion);
            dbProvider.AddInParameter(command, helper.Restipfeccreacion, DbType.DateTime, entity.Restipfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int restipcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Restipcodi, DbType.Int32, restipcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrRestricTipoDTO GetById(int restipcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Restipcodi, DbType.Int32, restipcodi);
            PrRestricTipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrRestricTipoDTO> List()
        {
            List<PrRestricTipoDTO> entitys = new List<PrRestricTipoDTO>();
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

        public List<PrRestricTipoDTO> GetByCriteria()
        {
            List<PrRestricTipoDTO> entitys = new List<PrRestricTipoDTO>();
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
