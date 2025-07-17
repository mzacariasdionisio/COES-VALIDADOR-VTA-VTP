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
    /// Clase de acceso a datos de la tabla NR_SUBMODULO
    /// </summary>
    public class NrSubmoduloRepository: RepositoryBase, INrSubmoduloRepository
    {
        public NrSubmoduloRepository(string strConn): base(strConn)
        {
        }

        NrSubmoduloHelper helper = new NrSubmoduloHelper();

        public int Save(NrSubmoduloDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Nrsmodcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Nrsmodnombre, DbType.String, entity.Nrsmodnombre);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(NrSubmoduloDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Nrsmodnombre, DbType.String, entity.Nrsmodnombre);
            dbProvider.AddInParameter(command, helper.Nrsmodcodi, DbType.Int32, entity.Nrsmodcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int nrsmodcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Nrsmodcodi, DbType.Int32, nrsmodcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public NrSubmoduloDTO GetById(int nrsmodcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Nrsmodcodi, DbType.Int32, nrsmodcodi);
            NrSubmoduloDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<NrSubmoduloDTO> List()
        {
            List<NrSubmoduloDTO> entitys = new List<NrSubmoduloDTO>();
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

        public List<NrSubmoduloDTO> GetByCriteria()
        {
            List<NrSubmoduloDTO> entitys = new List<NrSubmoduloDTO>();
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
