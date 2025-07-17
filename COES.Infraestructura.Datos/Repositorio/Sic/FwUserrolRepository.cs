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
    /// Clase de acceso a datos de la tabla FW_USERROL
    /// </summary>
    public class FwUserrolRepository: RepositoryBase, IFwUserrolRepository
    {
        public FwUserrolRepository(string strConn): base(strConn)
        {
        }

        FwUserrolHelper helper = new FwUserrolHelper();

        public void Save(FwUserrolDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            dbProvider.AddInParameter(command, helper.Rolcode, DbType.Int32, entity.Rolcode);
            dbProvider.AddInParameter(command, helper.Userrolvalidate, DbType.Int32, entity.Userrolvalidate);
            dbProvider.AddInParameter(command, helper.Userrolcheck, DbType.Int32, entity.Userrolcheck);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(FwUserrolDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            dbProvider.AddInParameter(command, helper.Rolcode, DbType.Int32, entity.Rolcode);
            dbProvider.AddInParameter(command, helper.Userrolvalidate, DbType.Int32, entity.Userrolvalidate);
            dbProvider.AddInParameter(command, helper.Userrolcheck, DbType.Int32, entity.Userrolcheck);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int usercode, int rolcode)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, usercode);
            dbProvider.AddInParameter(command, helper.Rolcode, DbType.Int32, rolcode);

            dbProvider.ExecuteNonQuery(command);
        }

        public FwUserrolDTO GetById(int usercode, int rolcode)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, usercode);
            dbProvider.AddInParameter(command, helper.Rolcode, DbType.Int32, rolcode);
            FwUserrolDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FwUserrolDTO> List()
        {
            List<FwUserrolDTO> entitys = new List<FwUserrolDTO>();
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

        public List<FwUserrolDTO> GetByCriteria()
        {
            List<FwUserrolDTO> entitys = new List<FwUserrolDTO>();
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

        public List<FwUserrolDTO> GetByRol(string rolcodes)
        {
            List<FwUserrolDTO> entitys = new List<FwUserrolDTO>();
            string query = string.Format(helper.SqlGetByRol, rolcodes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

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
