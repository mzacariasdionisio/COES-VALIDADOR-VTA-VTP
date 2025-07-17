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
    /// Clase de acceso a datos de la tabla PFR_REPORTE
    /// </summary>
    public class PfrReporteRepository: RepositoryBase, IPfrReporteRepository
    {
        public PfrReporteRepository(string strConn): base(strConn)
        {
        }

        PfrReporteHelper helper = new PfrReporteHelper();
        
        public int Save(PfrReporteDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrrptcodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrcuacodi, DbType.Int32, entity.Pfrcuacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrreccodi, DbType.Int32, entity.Pfrreccodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrrptesfinal, DbType.Int32, entity.Pfrrptesfinal));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrrptusucreacion, DbType.String, entity.Pfrrptusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrrptfeccreacion, DbType.DateTime, entity.Pfrrptfeccreacion));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrrptcr, DbType.Decimal, entity.Pfrrptcr));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrrptca, DbType.Decimal, entity.Pfrrptca));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrrptmr, DbType.Decimal, entity.Pfrrptmr));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrrptfecmd, DbType.DateTime, entity.Pfrrptfecmd));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrrptmd, DbType.Decimal, entity.Pfrrptmd));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrrptrevisionvtp, DbType.Int32, entity.Pfrrptrevisionvtp));
            

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfrReporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfrrptcodi, DbType.Int32, entity.Pfrrptcodi);
            dbProvider.AddInParameter(command, helper.Pfrcuacodi, DbType.Int32, entity.Pfrcuacodi);
            dbProvider.AddInParameter(command, helper.Pfrreccodi, DbType.Int32, entity.Pfrreccodi);
            dbProvider.AddInParameter(command, helper.Pfrrptesfinal, DbType.Int32, entity.Pfrrptesfinal);
            dbProvider.AddInParameter(command, helper.Pfrrptusucreacion, DbType.String, entity.Pfrrptusucreacion);
            dbProvider.AddInParameter(command, helper.Pfrrptfeccreacion, DbType.DateTime, entity.Pfrrptfeccreacion);
            dbProvider.AddInParameter(command, helper.Pfrrptcr, DbType.Decimal, entity.Pfrrptcr);
            dbProvider.AddInParameter(command, helper.Pfrrptca, DbType.Decimal, entity.Pfrrptca);
            dbProvider.AddInParameter(command, helper.Pfrrptmr, DbType.Decimal, entity.Pfrrptmr);
            dbProvider.AddInParameter(command, helper.Pfrrptfecmd, DbType.DateTime, entity.Pfrrptfecmd);
            dbProvider.AddInParameter(command, helper.Pfrrptmd, DbType.Decimal, entity.Pfrrptmd);
            dbProvider.AddInParameter(command, helper.Pfrrptrevisionvtp, DbType.Int32, entity.Pfrrptrevisionvtp);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfrrptcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfrrptcodi, DbType.Int32, pfrrptcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfrReporteDTO GetById(int pfrrptcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfrrptcodi, DbType.Int32, pfrrptcodi);
            PfrReporteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfrReporteDTO> List()
        {
            List<PfrReporteDTO> entitys = new List<PfrReporteDTO>();
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

        public List<PfrReporteDTO> GetByCriteria(int pfrreccodi, int pfrcuacodi)
        {
            List<PfrReporteDTO> entitys = new List<PfrReporteDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, pfrreccodi, pfrcuacodi);

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
