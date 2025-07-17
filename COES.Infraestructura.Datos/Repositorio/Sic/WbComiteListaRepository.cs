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
    /// Clase de acceso a datos de la tabla WB_COMITE_LISTA
    /// </summary>
    public class WbComiteListaRepository : RepositoryBase, IWbComiteListaRepository
    {
        public WbComiteListaRepository(string strConn) : base(strConn)
        {
        }

        WbComiteListaHelper helper = new WbComiteListaHelper();

        public int Save(WbComiteListaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Comitecodi, DbType.Int32, entity.Comitecodi);
            dbProvider.AddInParameter(command, helper.Comitelistacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Comitelistaname, DbType.String, entity.Comitelistaname);
            dbProvider.AddInParameter(command, helper.Comitelistaestado, DbType.String, entity.Comitelistaestado);
            dbProvider.AddInParameter(command, helper.Comitelistausucreacion, DbType.String, entity.Comitelistausucreacion);
            dbProvider.AddInParameter(command, helper.Comitelistafeccreacion, DbType.DateTime, entity.Comitelistafeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbComiteListaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Comitelistaestado, DbType.String, entity.Comitelistaestado);
            dbProvider.AddInParameter(command, helper.Comitecodi, DbType.Int32, entity.Comitelistacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int comitecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Comitecodi, DbType.Int32, comitecodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<WbComiteListaDTO> ListByComite(int comitecodi)
        {
            List<WbComiteListaDTO> entitys = new List<WbComiteListaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetListaByComite);
            dbProvider.AddInParameter(command, helper.Comitecodi, DbType.Int32, comitecodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<WbComiteListaDTO> GetByCriteria()
        {
            List<WbComiteListaDTO> entitys = new List<WbComiteListaDTO>();
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
