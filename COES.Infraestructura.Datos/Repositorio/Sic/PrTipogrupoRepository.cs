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
    /// Clase de acceso a datos de la tabla PR_TIPOGRUPO
    /// </summary>
    public class PrTipogrupoRepository: RepositoryBase, IPrTipogrupoRepository
    {
        public PrTipogrupoRepository(string strConn): base(strConn)
        {
        }

        PrTipogrupoHelper helper = new PrTipogrupoHelper();

        public int Save(PrTipogrupoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tipogrupocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tipogruponomb, DbType.String, entity.Tipogruponomb);
            dbProvider.AddInParameter(command, helper.Tipogrupoabrev, DbType.String, entity.Tipogrupoabrev);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PrTipogrupoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tipogrupocodi, DbType.Int32, entity.Tipogrupocodi);
            dbProvider.AddInParameter(command, helper.Tipogruponomb, DbType.String, entity.Tipogruponomb);
            dbProvider.AddInParameter(command, helper.Tipogrupoabrev, DbType.String, entity.Tipogrupoabrev);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int tipogrupocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Tipogrupocodi, DbType.Int32, tipogrupocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrTipogrupoDTO GetById(int tipogrupocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tipogrupocodi, DbType.Int32, tipogrupocodi);
            PrTipogrupoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrTipogrupoDTO> List()
        {
            List<PrTipogrupoDTO> entitys = new List<PrTipogrupoDTO>();
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

        public List<PrTipogrupoDTO> GetByCriteria()
        {
            List<PrTipogrupoDTO> entitys = new List<PrTipogrupoDTO>();
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
