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
    /// Clase de acceso a datos de la tabla HT_CENTRAL_CFG
    /// </summary>
    public class HtCentralCfgRepository: RepositoryBase, IHtCentralCfgRepository
    {
        public HtCentralCfgRepository(string strConn): base(strConn)
        {
        }

        HtCentralCfgHelper helper = new HtCentralCfgHelper();

        public int Save(HtCentralCfgDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Htcentcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Htcentfuente, DbType.String, entity.Htcentfuente);
            dbProvider.AddInParameter(command, helper.Htcentfecregistro, DbType.DateTime, entity.Htcentfecregistro);
            dbProvider.AddInParameter(command, helper.Htcentusuregistro, DbType.String, entity.Htcentusuregistro);
            dbProvider.AddInParameter(command, helper.Htcentfecmodificacion, DbType.DateTime, entity.Htcentfecmodificacion);
            dbProvider.AddInParameter(command, helper.Htcentusumodificacion, DbType.String, entity.Htcentusumodificacion);
            dbProvider.AddInParameter(command, helper.Htcentactivo, DbType.Int32, entity.Htcentactivo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(HtCentralCfgDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Htcentfuente, DbType.String, entity.Htcentfuente);
            dbProvider.AddInParameter(command, helper.Htcentfecregistro, DbType.DateTime, entity.Htcentfecregistro);
            dbProvider.AddInParameter(command, helper.Htcentusuregistro, DbType.String, entity.Htcentusuregistro);
            dbProvider.AddInParameter(command, helper.Htcentfecmodificacion, DbType.DateTime, entity.Htcentfecmodificacion);
            dbProvider.AddInParameter(command, helper.Htcentusumodificacion, DbType.String, entity.Htcentusumodificacion);
            dbProvider.AddInParameter(command, helper.Htcentactivo, DbType.Int32, entity.Htcentactivo);
            dbProvider.AddInParameter(command, helper.Htcentcodi, DbType.Int32, entity.Htcentcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int htcentcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Htcentcodi, DbType.Int32, htcentcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public HtCentralCfgDTO GetById(int htcentcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Htcentcodi, DbType.Int32, htcentcodi);
            HtCentralCfgDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<HtCentralCfgDTO> List()
        {
            List<HtCentralCfgDTO> entitys = new List<HtCentralCfgDTO>();
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

        public List<HtCentralCfgDTO> GetByCriteria()
        {
            List<HtCentralCfgDTO> entitys = new List<HtCentralCfgDTO>();
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
