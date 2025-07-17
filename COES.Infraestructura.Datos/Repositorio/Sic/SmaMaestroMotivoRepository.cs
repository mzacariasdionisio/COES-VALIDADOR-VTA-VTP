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
    /// Clase de acceso a datos de la tabla SMA_MAESTRO_MOTIVO
    /// </summary>
    public class SmaMaestroMotivoRepository: RepositoryBase, ISmaMaestroMotivoRepository
    {
        public SmaMaestroMotivoRepository(string strConn): base(strConn)
        {
        }

        SmaMaestroMotivoHelper helper = new SmaMaestroMotivoHelper();

        public int Save(SmaMaestroMotivoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Smammcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Smammdescripcion, DbType.String, entity.Smammdescripcion);
            dbProvider.AddInParameter(command, helper.Smammestado, DbType.String, entity.Smammestado);
            dbProvider.AddInParameter(command, helper.Smammusucreacion, DbType.String, entity.Smammusucreacion);
            dbProvider.AddInParameter(command, helper.Smammfeccreacion, DbType.DateTime, entity.Smammfeccreacion);
            dbProvider.AddInParameter(command, helper.Smammusumodificacion, DbType.String, entity.Smammusumodificacion);
            dbProvider.AddInParameter(command, helper.Smammfecmodificacion, DbType.DateTime, entity.Smammfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SmaMaestroMotivoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Smammdescripcion, DbType.String, entity.Smammdescripcion);
            dbProvider.AddInParameter(command, helper.Smammestado, DbType.String, entity.Smammestado);
            dbProvider.AddInParameter(command, helper.Smammusucreacion, DbType.String, entity.Smammusucreacion);
            dbProvider.AddInParameter(command, helper.Smammfeccreacion, DbType.DateTime, entity.Smammfeccreacion);
            dbProvider.AddInParameter(command, helper.Smammusumodificacion, DbType.String, entity.Smammusumodificacion);
            dbProvider.AddInParameter(command, helper.Smammfecmodificacion, DbType.DateTime, entity.Smammfecmodificacion);

            dbProvider.AddInParameter(command, helper.Smammcodi, DbType.Int32, entity.Smammcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int smammcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Smammcodi, DbType.Int32, smammcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SmaMaestroMotivoDTO GetById(int smammcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Smammcodi, DbType.Int32, smammcodi);
            SmaMaestroMotivoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SmaMaestroMotivoDTO> List()
        {
            List<SmaMaestroMotivoDTO> entitys = new List<SmaMaestroMotivoDTO>();
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

        public List<SmaMaestroMotivoDTO> GetByCriteria(string smammcodis)
        {
            List<SmaMaestroMotivoDTO> entitys = new List<SmaMaestroMotivoDTO>();
            
            string sql = string.Format(helper.SqlGetByCriteria, smammcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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
