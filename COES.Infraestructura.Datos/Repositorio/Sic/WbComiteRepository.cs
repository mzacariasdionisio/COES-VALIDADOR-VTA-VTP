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
    /// Clase de acceso a datos de la tabla WB_COMITE
    /// </summary>
    public class WbComiteRepository: RepositoryBase, IWbComiteRepository
    {
        public WbComiteRepository(string strConn): base(strConn)
        {
        }

        WbComiteHelper helper = new WbComiteHelper();

        public int Save(WbComiteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Comitecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Comitename, DbType.String, entity.Comitename);
            dbProvider.AddInParameter(command, helper.Comiteestado, DbType.String, entity.Comiteestado);
            dbProvider.AddInParameter(command, helper.Comiteusucreacion, DbType.String, entity.Comiteusucreacion);
            dbProvider.AddInParameter(command, helper.Comiteusumodificacion, DbType.String, entity.Comiteusumodificacion);
            dbProvider.AddInParameter(command, helper.Comitefeccreacion, DbType.DateTime, entity.Comitefeccreacion);
            dbProvider.AddInParameter(command, helper.Comitefecmodificacion, DbType.DateTime, entity.Comitefecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbComiteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            
            dbProvider.AddInParameter(command, helper.Comitename, DbType.String, entity.Comitename);
            dbProvider.AddInParameter(command, helper.Comiteestado, DbType.String, entity.Comiteestado);
            dbProvider.AddInParameter(command, helper.Comiteusumodificacion, DbType.String, entity.Comiteusumodificacion);
            dbProvider.AddInParameter(command, helper.Comitefecmodificacion, DbType.DateTime, entity.Comitefecmodificacion);
            dbProvider.AddInParameter(command, helper.Comitecodi, DbType.Int32, entity.Comitecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int comitecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Comitecodi, DbType.Int32, comitecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbComiteDTO GetById(int comitecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Comitecodi, DbType.Int32, comitecodi);
            WbComiteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbComiteDTO> List()
        {
            List<WbComiteDTO> entitys = new List<WbComiteDTO>();
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

        public List<WbComiteDTO> GetByCriteria()
        {
            List<WbComiteDTO> entitys = new List<WbComiteDTO>();
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
