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
    /// Clase de acceso a datos de la tabla SEG_REGION
    /// </summary>
    public class SegRegionRepository: RepositoryBase, ISegRegionRepository
    {
        public SegRegionRepository(string strConn): base(strConn)
        {
        }

        SegRegionHelper helper = new SegRegionHelper();

        public int Save(SegRegionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Regnombre, DbType.String, entity.Regnombre);
            dbProvider.AddInParameter(command, helper.Regusucreacion, DbType.String, entity.Regusucreacion);
            dbProvider.AddInParameter(command, helper.Regusumodificacion, DbType.String, entity.Regusumodificacion);
            dbProvider.AddInParameter(command, helper.Regfecmodificacion, DbType.DateTime, entity.Regfecmodificacion);
            dbProvider.AddInParameter(command, helper.Regfeccreacion, DbType.DateTime, entity.Regfeccreacion);
            dbProvider.AddInParameter(command, helper.Regestado, DbType.String, entity.Regestado);
            dbProvider.AddInParameter(command, helper.Regcodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SegRegionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Regnombre, DbType.String, entity.Regnombre);
            dbProvider.AddInParameter(command, helper.Regusumodificacion, DbType.String, entity.Regusumodificacion);
            dbProvider.AddInParameter(command, helper.Regfecmodificacion, DbType.DateTime, entity.Regfecmodificacion);
            dbProvider.AddInParameter(command, helper.Regestado, DbType.String, entity.Regestado);
            dbProvider.AddInParameter(command, helper.Regcodi, DbType.Int32, entity.Regcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int regcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);


            dbProvider.ExecuteNonQuery(command);
        }

        public SegRegionDTO GetById(int regcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Regcodi, DbType.Int32, regcodi);
            SegRegionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SegRegionDTO> List()
        {
            List<SegRegionDTO> entitys = new List<SegRegionDTO>();
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

        public List<SegRegionDTO> GetByCriteria()
        {
            List<SegRegionDTO> entitys = new List<SegRegionDTO>();
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


        public void ActualizarCongestion(int regcodi, string usuario, string estado)
        {
            string strSql = string.Format(helper.SqlActualizarCongestion, estado, usuario, regcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(strSql);
            dbProvider.ExecuteNonQuery(command);
        }

    }
}
