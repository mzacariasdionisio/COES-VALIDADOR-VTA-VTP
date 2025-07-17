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
    /// Clase de acceso a datos de la tabla AUD_NOTIFICACION
    /// </summary>
    public class AudNotificacionRepository: RepositoryBase, IAudNotificacionRepository
    {
        public AudNotificacionRepository(string strConn): base(strConn)
        {
        }

        AudNotificacionHelper helper = new AudNotificacionHelper();

        public int Save(AudNotificacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Noticodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Progacodi, DbType.Int32, entity.Progacodi);
            dbProvider.AddInParameter(command, helper.Archcodiarchivoadjunto, DbType.Int32, entity.Archcodiarchivoadjunto);
            dbProvider.AddInParameter(command, helper.Tabcdcoditiponotificacion, DbType.Int32, entity.Tabcdcoditiponotificacion);
            dbProvider.AddInParameter(command, helper.Notimensaje, DbType.String, entity.Notimensaje);
            dbProvider.AddInParameter(command, helper.Notiactivo, DbType.String, entity.Notiactivo);
            dbProvider.AddInParameter(command, helper.Notihistorico, DbType.String, entity.Notihistorico);
            dbProvider.AddInParameter(command, helper.Notiusuregistro, DbType.String, entity.Notiusuregistro);
            dbProvider.AddInParameter(command, helper.Notifecregistro, DbType.DateTime, entity.Notifecregistro);
            dbProvider.AddInParameter(command, helper.Notiusumodificacion, DbType.String, entity.Notiusumodificacion);
            dbProvider.AddInParameter(command, helper.Notifecmodificacion, DbType.DateTime, entity.Notifecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AudNotificacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Progacodi, DbType.Int32, entity.Progacodi);
            dbProvider.AddInParameter(command, helper.Archcodiarchivoadjunto, DbType.Int32, entity.Archcodiarchivoadjunto);
            dbProvider.AddInParameter(command, helper.Tabcdcoditiponotificacion, DbType.Int32, entity.Tabcdcoditiponotificacion);
            dbProvider.AddInParameter(command, helper.Notimensaje, DbType.String, entity.Notimensaje);
            dbProvider.AddInParameter(command, helper.Notiactivo, DbType.String, entity.Notiactivo);
            dbProvider.AddInParameter(command, helper.Notihistorico, DbType.String, entity.Notihistorico);
            dbProvider.AddInParameter(command, helper.Notiusuregistro, DbType.String, entity.Notiusuregistro);
            dbProvider.AddInParameter(command, helper.Notifecregistro, DbType.DateTime, entity.Notifecregistro);
            dbProvider.AddInParameter(command, helper.Notiusumodificacion, DbType.String, entity.Notiusumodificacion);
            dbProvider.AddInParameter(command, helper.Notifecmodificacion, DbType.DateTime, entity.Notifecmodificacion);
            dbProvider.AddInParameter(command, helper.Noticodi, DbType.Int32, entity.Noticodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int noticodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Noticodi, DbType.Int32, noticodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public AudNotificacionDTO GetById(int noticodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Noticodi, DbType.Int32, noticodi);
            AudNotificacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<AudNotificacionDTO> List()
        {
            List<AudNotificacionDTO> entitys = new List<AudNotificacionDTO>();
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

        public List<AudNotificacionDTO> GetByCriteria()
        {
            List<AudNotificacionDTO> entitys = new List<AudNotificacionDTO>();
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
