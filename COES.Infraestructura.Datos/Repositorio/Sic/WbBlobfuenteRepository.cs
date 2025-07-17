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
    /// Clase de acceso a datos de la tabla WB_BLOBFUENTE
    /// </summary>
    public class WbBlobfuenteRepository : RepositoryBase, IWbBlobfuenteRepository
    {
        public WbBlobfuenteRepository(string strConn) : base(strConn)
        {
        }

        WbBlobfuenteHelper helper = new WbBlobfuenteHelper();

        public int Save(WbBlobfuenteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Blofuecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Blofuenomb, DbType.String, entity.Blofuenomb);
            dbProvider.AddInParameter(command, helper.Blofueestado, DbType.String, entity.Blofueestado);
            dbProvider.AddInParameter(command, helper.Blofueusucreacion, DbType.String, entity.Blofueusucreacion);
            dbProvider.AddInParameter(command, helper.Blofuefeccreacion, DbType.DateTime, entity.Blofuefeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbBlobfuenteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Blofuecodi, DbType.Int32, entity.Blofuecodi);
            dbProvider.AddInParameter(command, helper.Blofuenomb, DbType.String, entity.Blofuenomb);
            dbProvider.AddInParameter(command, helper.Blofueestado, DbType.String, entity.Blofueestado);
            dbProvider.AddInParameter(command, helper.Blofueusucreacion, DbType.String, entity.Blofueusucreacion);
            dbProvider.AddInParameter(command, helper.Blofuefeccreacion, DbType.DateTime, entity.Blofuefeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int blofuecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Blofuecodi, DbType.Int32, blofuecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbBlobfuenteDTO GetById(int blofuecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Blofuecodi, DbType.Int32, blofuecodi);
            WbBlobfuenteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbBlobfuenteDTO> List()
        {
            List<WbBlobfuenteDTO> entitys = new List<WbBlobfuenteDTO>();
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

        public List<WbBlobfuenteDTO> GetByCriteria()
        {
            List<WbBlobfuenteDTO> entitys = new List<WbBlobfuenteDTO>();
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
