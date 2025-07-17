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
    /// Clase de acceso a datos de la tabla CM_PARAMETRO
    /// </summary>
    public class CmParametroRepository: RepositoryBase, ICmParametroRepository
    {
        public CmParametroRepository(string strConn): base(strConn)
        {
        }

        CmParametroHelper helper = new CmParametroHelper();

        public int Save(CmParametroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cmparcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cmparnombre, DbType.String, entity.Cmparnombre);
            dbProvider.AddInParameter(command, helper.Cmparvalor, DbType.String, entity.Cmparvalor);
            dbProvider.AddInParameter(command, helper.Cmparlastuser, DbType.String, entity.Cmparlastuser);
            dbProvider.AddInParameter(command, helper.Cmparlastdate, DbType.DateTime, entity.Cmparlastdate);
            dbProvider.AddInParameter(command, helper.Cmparinferior, DbType.Int32, entity.Cmparinferior);
            dbProvider.AddInParameter(command, helper.Cmparsuperior, DbType.Decimal, entity.Cmparsuperior);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmParametroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cmparnombre, DbType.String, entity.Cmparnombre);
            dbProvider.AddInParameter(command, helper.Cmparvalor, DbType.String, entity.Cmparvalor);
            dbProvider.AddInParameter(command, helper.Cmparlastuser, DbType.String, entity.Cmparlastuser);
            dbProvider.AddInParameter(command, helper.Cmparlastdate, DbType.DateTime, entity.Cmparlastdate);
            dbProvider.AddInParameter(command, helper.Cmparinferior, DbType.Int32, entity.Cmparinferior);
            dbProvider.AddInParameter(command, helper.Cmparsuperior, DbType.Decimal, entity.Cmparsuperior);
            dbProvider.AddInParameter(command, helper.Cmparcodi, DbType.Int32, entity.Cmparcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cmparcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cmparcodi, DbType.Int32, cmparcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmParametroDTO GetById(int cmparcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cmparcodi, DbType.Int32, cmparcodi);
            CmParametroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmParametroDTO> List()
        {
            List<CmParametroDTO> entitys = new List<CmParametroDTO>();
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

        public List<CmParametroDTO> GetByCriteria()
        {
            List<CmParametroDTO> entitys = new List<CmParametroDTO>();
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
