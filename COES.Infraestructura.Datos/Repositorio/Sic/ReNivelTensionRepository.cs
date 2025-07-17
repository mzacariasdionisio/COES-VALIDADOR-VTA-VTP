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
    /// Clase de acceso a datos de la tabla RE_NIVEL_TENSION
    /// </summary>
    public class ReNivelTensionRepository: RepositoryBase, IReNivelTensionRepository
    {
        public ReNivelTensionRepository(string strConn): base(strConn)
        {
        }

        ReNivelTensionHelper helper = new ReNivelTensionHelper();

        public int Save(ReNivelTensionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rentcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rentabrev, DbType.String, entity.Rentabrev);
            dbProvider.AddInParameter(command, helper.Rentnombre, DbType.String, entity.Rentnombre);
            dbProvider.AddInParameter(command, helper.Rentusucreacion, DbType.String, entity.Rentusucreacion);
            dbProvider.AddInParameter(command, helper.Rentfeccreacion, DbType.DateTime, entity.Rentfeccreacion);
            dbProvider.AddInParameter(command, helper.Rentusumodificacion, DbType.String, entity.Rentusumodificacion);
            dbProvider.AddInParameter(command, helper.Rentfecmodificacion, DbType.DateTime, entity.Rentfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(ReNivelTensionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rentabrev, DbType.String, entity.Rentabrev);
            dbProvider.AddInParameter(command, helper.Rentnombre, DbType.String, entity.Rentnombre);
            dbProvider.AddInParameter(command, helper.Rentusucreacion, DbType.String, entity.Rentusucreacion);
            dbProvider.AddInParameter(command, helper.Rentfeccreacion, DbType.DateTime, entity.Rentfeccreacion);
            dbProvider.AddInParameter(command, helper.Rentusumodificacion, DbType.String, entity.Rentusumodificacion);
            dbProvider.AddInParameter(command, helper.Rentfecmodificacion, DbType.DateTime, entity.Rentfecmodificacion);
            dbProvider.AddInParameter(command, helper.Rentcodi, DbType.Int32, entity.Rentcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rentcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rentcodi, DbType.Int32, rentcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public ReNivelTensionDTO GetById(int rentcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rentcodi, DbType.Int32, rentcodi);
            ReNivelTensionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ReNivelTensionDTO> List()
        {
            List<ReNivelTensionDTO> entitys = new List<ReNivelTensionDTO>();
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

        public List<ReNivelTensionDTO> GetByCriteria()
        {
            List<ReNivelTensionDTO> entitys = new List<ReNivelTensionDTO>();
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
