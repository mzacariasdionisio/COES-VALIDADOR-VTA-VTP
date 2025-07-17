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
    /// Clase de acceso a datos de la tabla WB_REGISTROIMEI
    /// </summary>
    public class WbRegistroimeiRepository: RepositoryBase, IWbRegistroimeiRepository
    {
        public WbRegistroimeiRepository(string strConn): base(strConn)
        {
        }

        WbRegistroimeiHelper helper = new WbRegistroimeiHelper();

        public int Save(WbRegistroimeiDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Regimecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Regimeusuario, DbType.String, entity.Regimeusuario);
            dbProvider.AddInParameter(command, helper.Regimecodigoimei, DbType.String, entity.Regimecodigoimei);
            dbProvider.AddInParameter(command, helper.Regimeestado, DbType.String, entity.Regimeestado);
            dbProvider.AddInParameter(command, helper.Regimeusucreacion, DbType.String, entity.Regimeusucreacion);
            dbProvider.AddInParameter(command, helper.Regimefeccreacion, DbType.DateTime, entity.Regimefeccreacion);
            dbProvider.AddInParameter(command, helper.Regimeusumodificacion, DbType.String, entity.Regimeusumodificacion);
            dbProvider.AddInParameter(command, helper.Regimefecmodificacion, DbType.DateTime, entity.Regimefecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbRegistroimeiDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Regimecodi, DbType.Int32, entity.Regimecodi);
            dbProvider.AddInParameter(command, helper.Regimeusuario, DbType.String, entity.Regimeusuario);
            dbProvider.AddInParameter(command, helper.Regimecodigoimei, DbType.String, entity.Regimecodigoimei);
            dbProvider.AddInParameter(command, helper.Regimeestado, DbType.String, entity.Regimeestado);
            dbProvider.AddInParameter(command, helper.Regimeusucreacion, DbType.String, entity.Regimeusucreacion);
            dbProvider.AddInParameter(command, helper.Regimefeccreacion, DbType.DateTime, entity.Regimefeccreacion);
            dbProvider.AddInParameter(command, helper.Regimeusumodificacion, DbType.String, entity.Regimeusumodificacion);
            dbProvider.AddInParameter(command, helper.Regimefecmodificacion, DbType.DateTime, entity.Regimefecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int regimecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Regimecodi, DbType.Int32, regimecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbRegistroimeiDTO GetById(int regimecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Regimecodi, DbType.Int32, regimecodi);
            WbRegistroimeiDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbRegistroimeiDTO> List()
        {
            List<WbRegistroimeiDTO> entitys = new List<WbRegistroimeiDTO>();
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

        public List<WbRegistroimeiDTO> GetByCriteria()
        {
            List<WbRegistroimeiDTO> entitys = new List<WbRegistroimeiDTO>();
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
