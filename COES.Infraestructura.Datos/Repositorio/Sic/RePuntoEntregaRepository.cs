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
    /// Clase de acceso a datos de la tabla RE_PUNTO_ENTREGA
    /// </summary>
    public class RePuntoEntregaRepository: RepositoryBase, IRePuntoEntregaRepository
    {
        public RePuntoEntregaRepository(string strConn): base(strConn)
        {
        }

        RePuntoEntregaHelper helper = new RePuntoEntregaHelper();

        public int Save(RePuntoEntregaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Repentcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Repentnombre, DbType.String, entity.Repentnombre);
            dbProvider.AddInParameter(command, helper.Rentcodi, DbType.Int32, entity.Rentcodi);
            dbProvider.AddInParameter(command, helper.Repentestado, DbType.String, entity.Repentestado);
            dbProvider.AddInParameter(command, helper.Repentusucreacion, DbType.String, entity.Repentusucreacion);
            dbProvider.AddInParameter(command, helper.Repentfeccreacion, DbType.DateTime, entity.Repentfeccreacion);
            dbProvider.AddInParameter(command, helper.Repentusumodificacion, DbType.String, entity.Repentusumodificacion);
            dbProvider.AddInParameter(command, helper.Repentfecmodificacion, DbType.DateTime, entity.Repentfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RePuntoEntregaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Repentnombre, DbType.String, entity.Repentnombre);
            dbProvider.AddInParameter(command, helper.Rentcodi, DbType.Int32, entity.Rentcodi);
            dbProvider.AddInParameter(command, helper.Repentestado, DbType.String, entity.Repentestado);
            dbProvider.AddInParameter(command, helper.Repentusucreacion, DbType.String, entity.Repentusucreacion);
            dbProvider.AddInParameter(command, helper.Repentfeccreacion, DbType.DateTime, entity.Repentfeccreacion);
            dbProvider.AddInParameter(command, helper.Repentusumodificacion, DbType.String, entity.Repentusumodificacion);
            dbProvider.AddInParameter(command, helper.Repentfecmodificacion, DbType.DateTime, entity.Repentfecmodificacion);
            dbProvider.AddInParameter(command, helper.Repentcodi, DbType.Int32, entity.Repentcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int repentcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Repentcodi, DbType.Int32, repentcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RePuntoEntregaDTO GetById(int repentcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Repentcodi, DbType.Int32, repentcodi);
            RePuntoEntregaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RePuntoEntregaDTO> List()
        {
            List<RePuntoEntregaDTO> entitys = new List<RePuntoEntregaDTO>();
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

        public List<RePuntoEntregaDTO> GetByCriteria()
        {
            List<RePuntoEntregaDTO> entitys = new List<RePuntoEntregaDTO>();
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
