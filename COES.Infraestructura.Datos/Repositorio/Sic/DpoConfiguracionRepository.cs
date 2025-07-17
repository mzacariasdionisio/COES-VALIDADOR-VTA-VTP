using System;
using System.Collections.Generic;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class DpoConfiguracionRepository : RepositoryBase, IDpoConfiguracionRepository
    {
        public DpoConfiguracionRepository(string strConn) : base(strConn)
        {
        }

        DpoConfiguracionHelper helper = new DpoConfiguracionHelper();

        public void Save(DpoConfiguracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Dpocngcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vergrpcodi, DbType.Int32, entity.Vergrpcodi);
            dbProvider.AddInParameter(command, helper.Dpocngdias, DbType.Int32, entity.Dpocngdias);
            dbProvider.AddInParameter(command, helper.Dpocngpromedio, DbType.Int32, entity.Dpocngpromedio);
            dbProvider.AddInParameter(command, helper.Dpocngtendencia, DbType.Decimal, entity.Dpocngtendencia);
            dbProvider.AddInParameter(command, helper.Dpocnggaussiano, DbType.Decimal, entity.Dpocnggaussiano);
            dbProvider.AddInParameter(command, helper.Dpocngumbral, DbType.Decimal, entity.Dpocngumbral);
            dbProvider.AddInParameter(command, helper.Dpocngvmg, DbType.Int32, entity.Dpocngvmg);
            dbProvider.AddInParameter(command, helper.Dpocngstd, DbType.Decimal, entity.Dpocngstd);
            dbProvider.AddInParameter(command, helper.Dpocngfechora, DbType.String, entity.Dpocngfechora);
            dbProvider.AddInParameter(command, helper.Dpocngusucreacion, DbType.String, entity.Dpocngusucreacion);
            dbProvider.AddInParameter(command, helper.Dpocngfeccreacion, DbType.DateTime, entity.Dpocngfeccreacion);
            dbProvider.AddInParameter(command, helper.Dpocngusumodificacion, DbType.String, entity.Dpocngusumodificacion);
            dbProvider.AddInParameter(command, helper.Dpocngfecmodificacion, DbType.DateTime, entity.Dpocngfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(DpoConfiguracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Dpocngdias, DbType.Int32, entity.Dpocngdias);
            dbProvider.AddInParameter(command, helper.Dpocngpromedio, DbType.Int32, entity.Dpocngpromedio);
            dbProvider.AddInParameter(command, helper.Dpocngtendencia, DbType.Decimal, entity.Dpocngtendencia);
            dbProvider.AddInParameter(command, helper.Dpocnggaussiano, DbType.Decimal, entity.Dpocnggaussiano);
            dbProvider.AddInParameter(command, helper.Dpocngumbral, DbType.Decimal, entity.Dpocngumbral);
            dbProvider.AddInParameter(command, helper.Dpocngvmg, DbType.Int32, entity.Dpocngvmg);
            dbProvider.AddInParameter(command, helper.Dpocngstd, DbType.Decimal, entity.Dpocngstd);
            dbProvider.AddInParameter(command, helper.Dpocngfechora, DbType.String, entity.Dpocngfechora);
            dbProvider.AddInParameter(command, helper.Dpocngusumodificacion, DbType.String, entity.Dpocngusumodificacion);
            dbProvider.AddInParameter(command, helper.Dpocngfecmodificacion, DbType.DateTime, entity.Dpocngfecmodificacion);

            dbProvider.AddInParameter(command, helper.Dpocngcodi, DbType.Int32, entity.Dpocngcodi);

            dbProvider.ExecuteNonQuery(command);
        }
        public void Delete(int dpocngcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Dpocngdias, DbType.Int32, dpocngcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public DpoConfiguracionDTO GetById(int dpocngcodi)
        {
            DpoConfiguracionDTO entity = new DpoConfiguracionDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Dpocngcodi, DbType.Int32, dpocngcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<DpoConfiguracionDTO> List()
        {
            List<DpoConfiguracionDTO> entitys = new List<DpoConfiguracionDTO>();
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

        public DpoConfiguracionDTO GetByVersion(int vergrpcodi)
        {
            DpoConfiguracionDTO entity = new DpoConfiguracionDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByVersion);
            dbProvider.AddInParameter(command, helper.Vergrpcodi, DbType.Int32, vergrpcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
    }
}
