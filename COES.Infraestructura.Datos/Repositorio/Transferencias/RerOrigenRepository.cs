using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla RER_ORIGEN
    /// </summary>
    public class RerOrigenRepository : RepositoryBase, IRerOrigenRepository
    {
        public RerOrigenRepository(string strConn)
            : base(strConn)
        {
        }

        RerOrigenHelper helper = new RerOrigenHelper();

        public int Save(RerOrigenDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Reroridesc, DbType.String, entity.Reroridesc);
            dbProvider.AddInParameter(command, helper.Reroriusucreacion, DbType.String, entity.Reroriusucreacion);
            dbProvider.AddInParameter(command, helper.Rerorifeccreacion, DbType.DateTime, entity.Rerorifeccreacion);
            dbProvider.AddInParameter(command, helper.Reroriusumodificacion, DbType.String, entity.Reroriusumodificacion);
            dbProvider.AddInParameter(command, helper.Rerorifecmodificacion, DbType.DateTime, entity.Rerorifecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RerOrigenDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Reroridesc, DbType.String, entity.Reroridesc);
            dbProvider.AddInParameter(command, helper.Reroriusucreacion, DbType.String, entity.Reroriusucreacion);
            dbProvider.AddInParameter(command, helper.Rerorifeccreacion, DbType.DateTime, entity.Rerorifeccreacion);
            dbProvider.AddInParameter(command, helper.Reroriusumodificacion, DbType.String, entity.Reroriusumodificacion);
            dbProvider.AddInParameter(command, helper.Rerorifecmodificacion, DbType.DateTime, entity.Rerorifecmodificacion);
            dbProvider.AddInParameter(command, helper.Reroricodi, DbType.Int32, entity.Reroricodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Reroricodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Reroricodi, DbType.Int32, Reroricodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RerOrigenDTO GetById(int Reroricodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Reroricodi, DbType.Int32, Reroricodi);
            RerOrigenDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RerOrigenDTO> List()
        {
            List<RerOrigenDTO> entities = new List<RerOrigenDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }
    }
}
