using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla PF_REPORTE
    /// </summary>
    public class PfReporteRepository : RepositoryBase, IPfReporteRepository
    {
        public PfReporteRepository(string strConn) : base(strConn)
        {
        }

        PfReporteHelper helper = new PfReporteHelper();

        public int Save(PfReporteDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrptcodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrptusucreacion, DbType.String, entity.Pfrptusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrptfeccreacion, DbType.DateTime, entity.Pfrptfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrptesfinal, DbType.Int32, entity.Pfrptesfinal));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrecacodi, DbType.Int32, entity.Pfrecacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfcuacodi, DbType.Int32, entity.Pfcuacodi));

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfReporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfrptcodi, DbType.Int32, entity.Pfrptcodi);
            dbProvider.AddInParameter(command, helper.Pfrptusucreacion, DbType.String, entity.Pfrptusucreacion);
            dbProvider.AddInParameter(command, helper.Pfrptfeccreacion, DbType.DateTime, entity.Pfrptfeccreacion);
            dbProvider.AddInParameter(command, helper.Pfrptesfinal, DbType.Int32, entity.Pfrptesfinal);
            dbProvider.AddInParameter(command, helper.Pfrecacodi, DbType.Int32, entity.Pfrecacodi);
            dbProvider.AddInParameter(command, helper.Pfcuacodi, DbType.Int32, entity.Pfcuacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfrptcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfrptcodi, DbType.Int32, pfrptcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfReporteDTO GetById(int pfrptcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfrptcodi, DbType.Int32, pfrptcodi);
            PfReporteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfReporteDTO> List()
        {
            List<PfReporteDTO> entitys = new List<PfReporteDTO>();
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

        public List<PfReporteDTO> GetByCriteria(int pfrecacodi, int pfcuacodi)
        {
            List<PfReporteDTO> entitys = new List<PfReporteDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, pfrecacodi, pfcuacodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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
