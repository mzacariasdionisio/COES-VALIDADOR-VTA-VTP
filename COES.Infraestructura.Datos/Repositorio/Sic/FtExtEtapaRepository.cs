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
    /// Clase de acceso a datos de la tabla FT_EXT_ETAPA
    /// </summary>
    public class FtExtEtapaRepository : RepositoryBase, IFtExtEtapaRepository
    {
        public FtExtEtapaRepository(string strConn) : base(strConn)
        {
        }

        FtExtEtapaHelper helper = new FtExtEtapaHelper();

        public int Save(FtExtEtapaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ftetcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ftetnombre, DbType.String, entity.Ftetnombre);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FtExtEtapaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ftetnombre, DbType.String, entity.Ftetnombre);
            dbProvider.AddInParameter(command, helper.Ftetcodi, DbType.Int32, entity.Ftetcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ftetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ftetcodi, DbType.Int32, ftetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtEtapaDTO GetById(int ftetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ftetcodi, DbType.Int32, ftetcodi);
            FtExtEtapaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtEtapaDTO> List()
        {
            List<FtExtEtapaDTO> entitys = new List<FtExtEtapaDTO>();
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

        public List<FtExtEtapaDTO> GetByCriteria()
        {
            List<FtExtEtapaDTO> entitys = new List<FtExtEtapaDTO>();
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
