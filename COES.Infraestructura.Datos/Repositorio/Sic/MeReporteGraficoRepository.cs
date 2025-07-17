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
    /// Clase de acceso a datos de la tabla ME_REPORTE_GRAFICO
    /// </summary>
    public class MeReporteGraficoRepository: RepositoryBase, IMeReporteGraficoRepository
    {
        public MeReporteGraficoRepository(string strConn): base(strConn)
        {
        }

        MeReporteGraficoHelper helper = new MeReporteGraficoHelper();

        public int Save(MeReporteGraficoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Repgrcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Repgrname, DbType.String, entity.Repgrname);
            dbProvider.AddInParameter(command, helper.Repgrtype, DbType.String, entity.Repgrtype);
            dbProvider.AddInParameter(command, helper.Repgryaxis, DbType.Int32, entity.Repgryaxis);
            dbProvider.AddInParameter(command, helper.Repgrcolor, DbType.String, entity.Repgrcolor);
            dbProvider.AddInParameter(command, helper.Reporcodi, DbType.Int32, entity.Reporcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MeReporteGraficoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Repgrcodi, DbType.Int32, entity.Repgrcodi);
            dbProvider.AddInParameter(command, helper.Repgrname, DbType.String, entity.Repgrname);
            dbProvider.AddInParameter(command, helper.Repgrtype, DbType.String, entity.Repgrtype);
            dbProvider.AddInParameter(command, helper.Repgryaxis, DbType.Int32, entity.Repgryaxis);
            dbProvider.AddInParameter(command, helper.Repgrcolor, DbType.String, entity.Repgrcolor);
            dbProvider.AddInParameter(command, helper.Reporcodi, DbType.Int32, entity.Reporcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int repgrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Repgrcodi, DbType.Int32, repgrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeReporteGraficoDTO GetById(int repgrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Repgrcodi, DbType.Int32, repgrcodi);
            MeReporteGraficoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeReporteGraficoDTO> List()
        {
            List<MeReporteGraficoDTO> entitys = new List<MeReporteGraficoDTO>();
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

        public List<MeReporteGraficoDTO> GetByCriteria(int reporcodi)
        {
            List<MeReporteGraficoDTO> entitys = new List<MeReporteGraficoDTO>();
            string query = string.Format(helper.SqlGetByCriteria, reporcodi);
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
