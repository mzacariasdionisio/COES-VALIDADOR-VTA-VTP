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
    /// Clase de acceso a datos de la tabla WB_BLOBCONFIG
    /// </summary>
    public class WbBlobconfigRepository : RepositoryBase, IWbBlobconfigRepository
    {
        public WbBlobconfigRepository(string strConn) : base(strConn)
        {
        }

        WbBlobconfigHelper helper = new WbBlobconfigHelper();

        public int Save(WbBlobconfigDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Usercreate, DbType.String, entity.Usercreate);
            dbProvider.AddInParameter(command, helper.Datecreate, DbType.DateTime, entity.Datecreate);
            dbProvider.AddInParameter(command, helper.Userupdate, DbType.String, entity.Userupdate);
            dbProvider.AddInParameter(command, helper.Dateupdate, DbType.DateTime, entity.Dateupdate);
            dbProvider.AddInParameter(command, helper.Configname, DbType.String, entity.Configname);
            dbProvider.AddInParameter(command, helper.Configstate, DbType.String, entity.Configstate);
            dbProvider.AddInParameter(command, helper.Configdefault, DbType.String, entity.Configdefault);
            dbProvider.AddInParameter(command, helper.Configorder, DbType.String, entity.Configorder);
            dbProvider.AddInParameter(command, helper.Configespecial, DbType.String, entity.Configespecial);
            dbProvider.AddInParameter(command, helper.Columncodi, DbType.Int32, entity.Columncodi);
            dbProvider.AddInParameter(command, helper.Blofuecodi, DbType.Int32, entity.Blofuecodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbBlobconfigDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, entity.Configcodi);
            dbProvider.AddInParameter(command, helper.Usercreate, DbType.String, entity.Usercreate);
            dbProvider.AddInParameter(command, helper.Datecreate, DbType.DateTime, entity.Datecreate);
            dbProvider.AddInParameter(command, helper.Userupdate, DbType.String, entity.Userupdate);
            dbProvider.AddInParameter(command, helper.Dateupdate, DbType.DateTime, entity.Dateupdate);
            dbProvider.AddInParameter(command, helper.Configname, DbType.String, entity.Configname);
            dbProvider.AddInParameter(command, helper.Configstate, DbType.String, entity.Configstate);
            dbProvider.AddInParameter(command, helper.Configdefault, DbType.String, entity.Configdefault);
            dbProvider.AddInParameter(command, helper.Configorder, DbType.String, entity.Configorder);
            dbProvider.AddInParameter(command, helper.Configespecial, DbType.String, entity.Configespecial);
            dbProvider.AddInParameter(command, helper.Columncodi, DbType.Int32, entity.Columncodi);
            dbProvider.AddInParameter(command, helper.Blofuecodi, DbType.Int32, entity.Blofuecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int configcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, configcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbBlobconfigDTO GetById(int configcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, configcodi);
            WbBlobconfigDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbBlobconfigDTO> List()
        {
            List<WbBlobconfigDTO> entitys = new List<WbBlobconfigDTO>();
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

        public List<WbBlobconfigDTO> GetByCriteria()
        {
            List<WbBlobconfigDTO> entitys = new List<WbBlobconfigDTO>();
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
