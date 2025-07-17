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
    /// Clase de acceso a datos de la tabla CO_URS_ESPECIAL
    /// </summary>
    public class CoUrsEspecialRepository: RepositoryBase, ICoUrsEspecialRepository
    {
        public CoUrsEspecialRepository(string strConn): base(strConn)
        {
        }

        CoUrsEspecialHelper helper = new CoUrsEspecialHelper();

        public int Save(CoUrsEspecialDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Courescodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, entity.Copercodi);
            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, entity.Covercodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Couebacodi, DbType.Int32, entity.Couebacodi);
            dbProvider.AddInParameter(command, helper.Couresusucreacion, DbType.String, entity.Couresusucreacion);
            dbProvider.AddInParameter(command, helper.Couresfeccreacion, DbType.DateTime, entity.Couresfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CoUrsEspecialDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, entity.Copercodi);
            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, entity.Covercodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Couebacodi, DbType.Int32, entity.Couebacodi);
            dbProvider.AddInParameter(command, helper.Couresusucreacion, DbType.String, entity.Couresusucreacion);
            dbProvider.AddInParameter(command, helper.Couresfeccreacion, DbType.DateTime, entity.Couresfeccreacion);
            dbProvider.AddInParameter(command, helper.Courescodi, DbType.Int32, entity.Courescodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int courescodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, courescodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CoUrsEspecialDTO GetById(int courescodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Courescodi, DbType.Int32, courescodi);
            CoUrsEspecialDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CoUrsEspecialDTO> List()
        {
            List<CoUrsEspecialDTO> entitys = new List<CoUrsEspecialDTO>();
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

        public List<CoUrsEspecialDTO> GetByCriteria(int idVersion)
        {
            List<CoUrsEspecialDTO> entitys = new List<CoUrsEspecialDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, idVersion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoUrsEspecialDTO entity = helper.Create(dr);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
