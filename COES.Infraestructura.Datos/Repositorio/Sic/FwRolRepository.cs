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
    /// Clase de acceso a datos de la tabla FW_ROL
    /// </summary>
    public class FwRolRepository: RepositoryBase, IFwRolRepository
    {
        public FwRolRepository(string strConn): base(strConn)
        {
        }

        FwRolHelper helper = new FwRolHelper();

        public int Save(FwRolDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rolcode, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rolname, DbType.String, entity.Rolname);
            dbProvider.AddInParameter(command, helper.Rolvalidate, DbType.Int32, entity.Rolvalidate);
            dbProvider.AddInParameter(command, helper.Rolcheck, DbType.Int32, entity.Rolcheck);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FwRolDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rolcode, DbType.Int32, entity.Rolcode);
            dbProvider.AddInParameter(command, helper.Rolname, DbType.String, entity.Rolname);
            dbProvider.AddInParameter(command, helper.Rolvalidate, DbType.Int32, entity.Rolvalidate);
            dbProvider.AddInParameter(command, helper.Rolcheck, DbType.Int32, entity.Rolcheck);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rolcode)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rolcode, DbType.Int32, rolcode);

            dbProvider.ExecuteNonQuery(command);
        }

        public FwRolDTO GetById(int rolcode)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rolcode, DbType.Int32, rolcode);
            FwRolDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FwRolDTO> List()
        {
            List<FwRolDTO> entitys = new List<FwRolDTO>();
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

        public List<FwRolDTO> GetByCriteria()
        {
            List<FwRolDTO> entitys = new List<FwRolDTO>();
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
