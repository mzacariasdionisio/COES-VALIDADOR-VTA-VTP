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
    /// Clase de acceso a datos de la tabla RI_TIPOSOLICITUD
    /// </summary>
    public class RiTiposolicitudRepository: RepositoryBase, IRiTiposolicitudRepository
    {
        public RiTiposolicitudRepository(string strConn): base(strConn)
        {
        }

        RiTiposolicitudHelper helper = new RiTiposolicitudHelper();

        public int Save(RiTiposolicitudDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tisocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tisonombre, DbType.String, entity.Tisonombre);
            dbProvider.AddInParameter(command, helper.Tisoestado, DbType.String, entity.Tisoestado);
            dbProvider.AddInParameter(command, helper.Tisousucreacion, DbType.String, entity.Tisousucreacion);
            dbProvider.AddInParameter(command, helper.Tisofeccreacion, DbType.DateTime, entity.Tisofeccreacion);
            dbProvider.AddInParameter(command, helper.Tisousumodificacion, DbType.String, entity.Tisousumodificacion);
            dbProvider.AddInParameter(command, helper.Tisofecmodificacion, DbType.DateTime, entity.Tisofecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RiTiposolicitudDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tisonombre, DbType.String, entity.Tisonombre);
            dbProvider.AddInParameter(command, helper.Tisoestado, DbType.String, entity.Tisoestado);
            dbProvider.AddInParameter(command, helper.Tisousucreacion, DbType.String, entity.Tisousucreacion);
            dbProvider.AddInParameter(command, helper.Tisofeccreacion, DbType.DateTime, entity.Tisofeccreacion);
            dbProvider.AddInParameter(command, helper.Tisousumodificacion, DbType.String, entity.Tisousumodificacion);
            dbProvider.AddInParameter(command, helper.Tisofecmodificacion, DbType.DateTime, entity.Tisofecmodificacion);
            dbProvider.AddInParameter(command, helper.Tisocodi, DbType.Int32, entity.Tisocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int tisocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Tisocodi, DbType.Int32, tisocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RiTiposolicitudDTO GetById(int tisocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tisocodi, DbType.Int32, tisocodi);
            RiTiposolicitudDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RiTiposolicitudDTO> List()
        {
            List<RiTiposolicitudDTO> entitys = new List<RiTiposolicitudDTO>();
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

        public List<RiTiposolicitudDTO> GetByCriteria()
        {
            List<RiTiposolicitudDTO> entitys = new List<RiTiposolicitudDTO>();
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
