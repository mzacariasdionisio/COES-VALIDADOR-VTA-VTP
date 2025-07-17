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
    /// Clase de acceso a datos de la tabla SPO_REPORTE
    /// </summary>
    public class SpoReporteRepository: RepositoryBase, ISpoReporteRepository
    {
        public SpoReporteRepository(string strConn): base(strConn)
        {
        }

        SpoReporteHelper helper = new SpoReporteHelper();

        public int Save(SpoReporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Repcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Repdiaapertura, DbType.Int32, entity.Repdiaapertura);
            dbProvider.AddInParameter(command, helper.Repdiaclausura, DbType.Int32, entity.Repdiaclausura);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SpoReporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Repcodi, DbType.Int32, entity.Repcodi);
            dbProvider.AddInParameter(command, helper.Repdiaapertura, DbType.Int32, entity.Repdiaapertura);
            dbProvider.AddInParameter(command, helper.Repdiaclausura, DbType.Int32, entity.Repdiaclausura);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int repcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Repcodi, DbType.Int32, repcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SpoReporteDTO GetById(int repcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Repcodi, DbType.Int32, repcodi);
            SpoReporteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SpoReporteDTO> List()
        {
            List<SpoReporteDTO> entitys = new List<SpoReporteDTO>();
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

        public List<SpoReporteDTO> GetByCriteria()
        {
            List<SpoReporteDTO> entitys = new List<SpoReporteDTO>();
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
