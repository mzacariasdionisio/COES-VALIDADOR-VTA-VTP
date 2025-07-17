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
    /// Clase de acceso a datos de la tabla RE_TIPOCORREO
    /// </summary>
    public class ReTipocorreoRepository: RepositoryBase, IReTipocorreoRepository
    {
        public ReTipocorreoRepository(string strConn): base(strConn)
        {
        }

        ReTipocorreoHelper helper = new ReTipocorreoHelper();

        public int Save(ReTipocorreoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Retcorcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Retcornombre, DbType.String, entity.Retcornombre);
            dbProvider.AddInParameter(command, helper.Retcorusucreacion, DbType.String, entity.Retcorusucreacion);
            dbProvider.AddInParameter(command, helper.Retcorfeccreacion, DbType.DateTime, entity.Retcorfeccreacion);
            dbProvider.AddInParameter(command, helper.Retcorusumodificacion, DbType.String, entity.Retcorusumodificacion);
            dbProvider.AddInParameter(command, helper.Retcorfecmodificacion, DbType.DateTime, entity.Retcorfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(ReTipocorreoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Retcornombre, DbType.String, entity.Retcornombre);
            dbProvider.AddInParameter(command, helper.Retcorusucreacion, DbType.String, entity.Retcorusucreacion);
            dbProvider.AddInParameter(command, helper.Retcorfeccreacion, DbType.DateTime, entity.Retcorfeccreacion);
            dbProvider.AddInParameter(command, helper.Retcorusumodificacion, DbType.String, entity.Retcorusumodificacion);
            dbProvider.AddInParameter(command, helper.Retcorfecmodificacion, DbType.DateTime, entity.Retcorfecmodificacion);
            dbProvider.AddInParameter(command, helper.Retcorcodi, DbType.Int32, entity.Retcorcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int retcorcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Retcorcodi, DbType.Int32, retcorcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public ReTipocorreoDTO GetById(int retcorcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Retcorcodi, DbType.Int32, retcorcodi);
            ReTipocorreoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ReTipocorreoDTO> List()
        {
            List<ReTipocorreoDTO> entitys = new List<ReTipocorreoDTO>();
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

        public List<ReTipocorreoDTO> GetByCriteria()
        {
            List<ReTipocorreoDTO> entitys = new List<ReTipocorreoDTO>();
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
