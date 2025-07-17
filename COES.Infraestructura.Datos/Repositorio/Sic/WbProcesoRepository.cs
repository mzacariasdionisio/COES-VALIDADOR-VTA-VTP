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
    /// Clase de acceso a datos de la tabla WB_PROCESO
    /// </summary>
    public class WbProcesoRepository : RepositoryBase, IWbProcesoRepository
    {
        public WbProcesoRepository(string strConn) : base(strConn)
        {
        }

        WbProcesoHelper helper = new WbProcesoHelper();

        public int Save(WbProcesoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Procesocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Procesoname, DbType.String, entity.Procesoname);
            dbProvider.AddInParameter(command, helper.Procesoestado, DbType.String, entity.Procesoestado);
            dbProvider.AddInParameter(command, helper.Procesousucreacion, DbType.String, entity.Procesousucreacion);
            dbProvider.AddInParameter(command, helper.Procesousumodificacion, DbType.String, entity.Procesousumodificacion);
            dbProvider.AddInParameter(command, helper.Procesofeccreacion, DbType.DateTime, entity.Procesofeccreacion);
            dbProvider.AddInParameter(command, helper.Procesofecmodificacion, DbType.DateTime, entity.Procesofecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbProcesoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);


            dbProvider.AddInParameter(command, helper.Procesoname, DbType.String, entity.Procesoname);
            dbProvider.AddInParameter(command, helper.Procesoestado, DbType.String, entity.Procesoestado);
            dbProvider.AddInParameter(command, helper.Procesousumodificacion, DbType.String, entity.Procesousumodificacion);
            dbProvider.AddInParameter(command, helper.Procesofecmodificacion, DbType.DateTime, entity.Procesofecmodificacion);
            dbProvider.AddInParameter(command, helper.Procesocodi, DbType.Int32, entity.Procesocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int procesocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Procesocodi, DbType.Int32, procesocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbProcesoDTO GetById(int procesocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Procesocodi, DbType.Int32, procesocodi);
            WbProcesoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbProcesoDTO> List()
        {
            List<WbProcesoDTO> entitys = new List<WbProcesoDTO>();
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

        public List<WbProcesoDTO> GetByCriteria()
        {
            List<WbProcesoDTO> entitys = new List<WbProcesoDTO>();
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
