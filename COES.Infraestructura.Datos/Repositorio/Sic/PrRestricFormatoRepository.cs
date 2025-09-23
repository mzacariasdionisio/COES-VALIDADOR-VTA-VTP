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
    /// Clase de acceso a datos de la tabla PR_RESTRIC_FORMATO
    /// </summary>
    public class PrRestricFormatoRepository: RepositoryBase, IPrRestricFormatoRepository
    {
        public PrRestricFormatoRepository(string strConn): base(strConn)
        {
        }

        PrRestricFormatoHelper helper = new PrRestricFormatoHelper();

        public int Save(PrRestricFormatoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Resfmtcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Resfmtnombre, DbType.String, entity.Resfmtnombre);
            dbProvider.AddInParameter(command, helper.Resfmtusucreacion, DbType.String, entity.Resfmtusucreacion);
            dbProvider.AddInParameter(command, helper.Resfmtfeccreacion, DbType.DateTime, entity.Resfmtfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PrRestricFormatoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Resfmtcodi, DbType.Int32, entity.Resfmtcodi);
            dbProvider.AddInParameter(command, helper.Resfmtnombre, DbType.String, entity.Resfmtnombre);
            dbProvider.AddInParameter(command, helper.Resfmtusucreacion, DbType.String, entity.Resfmtusucreacion);
            dbProvider.AddInParameter(command, helper.Resfmtfeccreacion, DbType.DateTime, entity.Resfmtfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int resfmtcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Resfmtcodi, DbType.Int32, resfmtcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrRestricFormatoDTO GetById(int resfmtcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Resfmtcodi, DbType.Int32, resfmtcodi);
            PrRestricFormatoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrRestricFormatoDTO> List()
        {
            List<PrRestricFormatoDTO> entitys = new List<PrRestricFormatoDTO>();
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

        public List<PrRestricFormatoDTO> GetByCriteria()
        {
            List<PrRestricFormatoDTO> entitys = new List<PrRestricFormatoDTO>();
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
