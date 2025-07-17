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
    /// Clase de acceso a datos de la tabla ME_ENVCORREO_CONF
    /// </summary>
    public class MeEnvcorreoConfRepository: RepositoryBase, IMeEnvcorreoConfRepository
    {
        public MeEnvcorreoConfRepository(string strConn): base(strConn)
        {
        }

        MeEnvcorreoConfHelper helper = new MeEnvcorreoConfHelper();

        public int Save(MeEnvcorreoConfDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ecconfcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ecconfnombre, DbType.String, entity.Ecconfnombre);
            dbProvider.AddInParameter(command, helper.Ecconfcargo, DbType.String, entity.Ecconfcargo);
            dbProvider.AddInParameter(command, helper.Ecconfanexo, DbType.String, entity.Ecconfanexo);
            dbProvider.AddInParameter(command, helper.Ecconfestadonot, DbType.String, entity.Ecconfestadonot);
            dbProvider.AddInParameter(command, helper.Ecconfhoraenvio, DbType.String, entity.Ecconfhoraenvio);
            dbProvider.AddInParameter(command, helper.Ecconfusucreacion, DbType.String, entity.Ecconfusucreacion);
            dbProvider.AddInParameter(command, helper.Ecconffeccreacion, DbType.DateTime, entity.Ecconffeccreacion);
            dbProvider.AddInParameter(command, helper.Ecconfusumodificacion, DbType.String, entity.Ecconfusumodificacion);
            dbProvider.AddInParameter(command, helper.Ecconffecmodificacion, DbType.DateTime, entity.Ecconffecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MeEnvcorreoConfDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ecconfnombre, DbType.String, entity.Ecconfnombre);
            dbProvider.AddInParameter(command, helper.Ecconfcargo, DbType.String, entity.Ecconfcargo);
            dbProvider.AddInParameter(command, helper.Ecconfanexo, DbType.String, entity.Ecconfanexo);
            dbProvider.AddInParameter(command, helper.Ecconfestadonot, DbType.String, entity.Ecconfestadonot);
            dbProvider.AddInParameter(command, helper.Ecconfhoraenvio, DbType.String, entity.Ecconfhoraenvio);
            dbProvider.AddInParameter(command, helper.Ecconfusucreacion, DbType.String, entity.Ecconfusucreacion);
            dbProvider.AddInParameter(command, helper.Ecconffeccreacion, DbType.DateTime, entity.Ecconffeccreacion);
            dbProvider.AddInParameter(command, helper.Ecconfusumodificacion, DbType.String, entity.Ecconfusumodificacion);
            dbProvider.AddInParameter(command, helper.Ecconffecmodificacion, DbType.DateTime, entity.Ecconffecmodificacion);
            dbProvider.AddInParameter(command, helper.Ecconfcodi, DbType.Int32, entity.Ecconfcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ecconfcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ecconfcodi, DbType.Int32, ecconfcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeEnvcorreoConfDTO GetById(int ecconfcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ecconfcodi, DbType.Int32, ecconfcodi);
            MeEnvcorreoConfDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeEnvcorreoConfDTO> List()
        {
            List<MeEnvcorreoConfDTO> entitys = new List<MeEnvcorreoConfDTO>();
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

        public List<MeEnvcorreoConfDTO> GetByCriteria()
        {
            List<MeEnvcorreoConfDTO> entitys = new List<MeEnvcorreoConfDTO>();
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
