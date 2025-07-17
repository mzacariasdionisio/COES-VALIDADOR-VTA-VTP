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
    /// Clase de acceso a datos de la tabla EVE_CAUSAEVENTO
    /// </summary>
    public class EveCausaeventoRepository: RepositoryBase, IEveCausaeventoRepository
    {
        public EveCausaeventoRepository(string strConn): base(strConn)
        {
        }

        EveCausaeventoHelper helper = new EveCausaeventoHelper();

        public int Save(EveCausaeventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Causaevencodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Causaevendesc, DbType.String, entity.Causaevendesc);
            dbProvider.AddInParameter(command, helper.Causaevenabrev, DbType.String, entity.Causaevenabrev);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }
              
        public void Update(EveCausaeventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Causaevencodi, DbType.Int32, entity.Causaevencodi);
            dbProvider.AddInParameter(command, helper.Causaevendesc, DbType.String, entity.Causaevendesc);
            dbProvider.AddInParameter(command, helper.Causaevenabrev, DbType.String, entity.Causaevenabrev);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int causaevencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Causaevencodi, DbType.Int32, causaevencodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveCausaeventoDTO GetById(int causaevencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Causaevencodi, DbType.Int32, causaevencodi);
            EveCausaeventoDTO entity = null;
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EveCausaeventoDTO> List()
        {
            List<EveCausaeventoDTO> entitys = new List<EveCausaeventoDTO>();
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

        public List<EveCausaeventoDTO> GetByCriteria()
        {
            List<EveCausaeventoDTO> entitys = new List<EveCausaeventoDTO>();
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
