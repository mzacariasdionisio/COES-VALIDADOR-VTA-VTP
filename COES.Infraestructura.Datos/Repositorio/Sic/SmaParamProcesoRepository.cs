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
    /// Clase de acceso a datos de la tabla SMA_PARAM_PROCESO
    /// </summary>
    public class SmaParamProcesoRepository: RepositoryBase, ISmaParamProcesoRepository
    {
        public SmaParamProcesoRepository(string strConn): base(strConn)
        {
        }

        SmaParamProcesoHelper helper = new SmaParamProcesoHelper();

        public int Save(SmaParamProcesoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Papocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Papohorainicio, DbType.String, entity.Papohorainicio);
            dbProvider.AddInParameter(command, helper.Papohorafin, DbType.String, entity.Papohorafin);
            dbProvider.AddInParameter(command, helper.Papousucreacion, DbType.String, entity.Papousucreacion);
            dbProvider.AddInParameter(command, helper.Papohoraenvioncp, DbType.String, entity.Papohoraenvioncp);
            // STS 19 marzo 2018
            dbProvider.AddInParameter(command, helper.Papomaxdiasoferta, DbType.Int32, entity.Papomaxdiasoferta);
            

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SmaParamProcesoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Papocodi, DbType.Int32, entity.Papocodi);
            dbProvider.AddInParameter(command, helper.Papohorainicio, DbType.String, entity.Papohorainicio);
            dbProvider.AddInParameter(command, helper.Papohorafin, DbType.String, entity.Papohorafin);
            dbProvider.AddInParameter(command, helper.Papousucreacion, DbType.String, entity.Papousucreacion);
            dbProvider.AddInParameter(command, helper.Papofeccreacion, DbType.DateTime, entity.Papofeccreacion);
            dbProvider.AddInParameter(command, helper.Papofecmodificacion, DbType.DateTime, entity.Papofecmodificacion);
            dbProvider.AddInParameter(command, helper.Papousumodificacion, DbType.String, entity.Papousumodificacion);
            dbProvider.AddInParameter(command, helper.Papohoraenvioncp, DbType.String, entity.Papohoraenvioncp);
            dbProvider.AddInParameter(command, helper.Papoestado, DbType.String, entity.Papoestado);
            // STS 19 marzo 2018
            dbProvider.AddInParameter(command, helper.Papomaxdiasoferta, DbType.Int32, entity.Papomaxdiasoferta);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateInactive(int papocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateInactive);

            dbProvider.AddInParameter(command, helper.Papocodi, DbType.Int32, papocodi);

            dbProvider.ExecuteNonQuery(command);
        }


        public void Delete(int papocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Papocodi, DbType.Int32, papocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SmaParamProcesoDTO GetById(int papocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Papocodi, DbType.Int32, papocodi);
            SmaParamProcesoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SmaParamProcesoDTO> List()
        {
            List<SmaParamProcesoDTO> entitys = new List<SmaParamProcesoDTO>();
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

        public SmaParamProcesoDTO GetValidRangeNCP()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetValidRangeNCP);
            SmaParamProcesoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SmaParamProcesoDTO> GetByCriteria()
        {
            List<SmaParamProcesoDTO> entitys = new List<SmaParamProcesoDTO>();
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
