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
    /// Clase de acceso a datos de la tabla CO_URS_ESPECIALBASE
    /// </summary>
    public class CoUrsEspecialbaseRepository: RepositoryBase, ICoUrsEspecialbaseRepository
    {
        public CoUrsEspecialbaseRepository(string strConn): base(strConn)
        {
        }

        CoUrsEspecialbaseHelper helper = new CoUrsEspecialbaseHelper();

        public int Save(CoUrsEspecialbaseDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Couebacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Couebausucreacion, DbType.String, entity.Couebausucreacion);
            dbProvider.AddInParameter(command, helper.Couebafeccreacion, DbType.DateTime, entity.Couebafeccreacion);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CoUrsEspecialbaseDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Couebausucreacion, DbType.String, entity.Couebausucreacion);
            dbProvider.AddInParameter(command, helper.Couebafeccreacion, DbType.DateTime, entity.Couebafeccreacion);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Couebacodi, DbType.Int32, entity.Couebacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int couebacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Couebacodi, DbType.Int32, couebacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CoUrsEspecialbaseDTO GetById(int couebacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Couebacodi, DbType.Int32, couebacodi);
            CoUrsEspecialbaseDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CoUrsEspecialbaseDTO> List()
        {
            List<CoUrsEspecialbaseDTO> entitys = new List<CoUrsEspecialbaseDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoUrsEspecialbaseDTO entity = helper.Create(dr);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CoUrsEspecialbaseDTO> GetByCriteria()
        {
            List<CoUrsEspecialbaseDTO> entitys = new List<CoUrsEspecialbaseDTO>();
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
