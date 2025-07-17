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
    /// Clase de acceso a datos de la tabla AUD_NOTIFICACIONDEST
    /// </summary>
    public class AudNotificaciondestRepository: RepositoryBase, IAudNotificaciondestRepository
    {
        public AudNotificaciondestRepository(string strConn): base(strConn)
        {
        }

        AudNotificaciondestHelper helper = new AudNotificaciondestHelper();

        public int Save(AudNotificaciondestDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Notdcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Noticodi, DbType.Int32, entity.Noticodi);
            dbProvider.AddInParameter(command, helper.Percodidestinatario, DbType.Int32, entity.Percodidestinatario);
            dbProvider.AddInParameter(command, helper.Tabcdcoditipodestinatario, DbType.Int32, entity.Tabcdcoditipodestinatario);
            dbProvider.AddInParameter(command, helper.Notdactivo, DbType.String, entity.Notdactivo);
            dbProvider.AddInParameter(command, helper.Notdhistorico, DbType.String, entity.Notdhistorico);
            dbProvider.AddInParameter(command, helper.Notdusucreacion, DbType.String, entity.Notdusucreacion);
            dbProvider.AddInParameter(command, helper.Notdfeccreacion, DbType.DateTime, entity.Notdfeccreacion);
            dbProvider.AddInParameter(command, helper.Notdusumodificacion, DbType.String, entity.Notdusumodificacion);
            dbProvider.AddInParameter(command, helper.Notdfecmodificacion, DbType.DateTime, entity.Notdfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AudNotificaciondestDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Noticodi, DbType.Int32, entity.Noticodi);
            dbProvider.AddInParameter(command, helper.Percodidestinatario, DbType.Int32, entity.Percodidestinatario);
            dbProvider.AddInParameter(command, helper.Tabcdcoditipodestinatario, DbType.Int32, entity.Tabcdcoditipodestinatario);
            dbProvider.AddInParameter(command, helper.Notdactivo, DbType.String, entity.Notdactivo);
            dbProvider.AddInParameter(command, helper.Notdhistorico, DbType.String, entity.Notdhistorico);
            dbProvider.AddInParameter(command, helper.Notdusucreacion, DbType.String, entity.Notdusucreacion);
            dbProvider.AddInParameter(command, helper.Notdfeccreacion, DbType.DateTime, entity.Notdfeccreacion);
            dbProvider.AddInParameter(command, helper.Notdusumodificacion, DbType.String, entity.Notdusumodificacion);
            dbProvider.AddInParameter(command, helper.Notdfecmodificacion, DbType.DateTime, entity.Notdfecmodificacion);
            dbProvider.AddInParameter(command, helper.Notdcodi, DbType.Int32, entity.Notdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int notdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Notdcodi, DbType.Int32, notdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public AudNotificaciondestDTO GetById(int notdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Notdcodi, DbType.Int32, notdcodi);
            AudNotificaciondestDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<AudNotificaciondestDTO> List()
        {
            List<AudNotificaciondestDTO> entitys = new List<AudNotificaciondestDTO>();
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

        public List<AudNotificaciondestDTO> GetByCriteria()
        {
            List<AudNotificaciondestDTO> entitys = new List<AudNotificaciondestDTO>();
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
