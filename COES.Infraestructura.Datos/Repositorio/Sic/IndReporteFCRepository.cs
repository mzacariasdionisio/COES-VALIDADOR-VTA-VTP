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
    /// Clase de acceso a datos de la tabla IND_REPORTE_FC
    /// </summary>
    public class IndReporteFCRepository : RepositoryBase, IIndReporteFCRepository
    {
        public IndReporteFCRepository(string strConn) : base(strConn)
        {
        }

        IndReporteFCHelper helper = new IndReporteFCHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public void Save(IndReporteFCDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand) conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptfccodi, DbType.Int32, entity.Irptfccodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotcodi, DbType.Int32, entity.Itotcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptfctipcombustible, DbType.String, entity.Irptfctipcombustible));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptfcnomcombustible, DbType.String, entity.Irptfcnomcombustible));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptfcmw, DbType.Decimal, entity.Irptfcmw));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptfcm3h, DbType.Decimal, entity.Irptfcm3h));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptfc1000m3h, DbType.Decimal, entity.Irptfc1000m3h));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptfckpch, DbType.Decimal, entity.Irptfckpch));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptfcmmpch, DbType.Decimal, entity.Irptfcmmpch));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptfclh, DbType.Decimal, entity.Irptfclh));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptfcgalh, DbType.Decimal, entity.Irptfcgalh));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptfccmtr, DbType.Decimal, entity.Irptfccmtr));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptfcnumdias, DbType.Int32, entity.Irptfcnumdias));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptfcrngdias, DbType.String, entity.Irptfcrngdias));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptfcsec, DbType.Int32, entity.Irptfcsec));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptfcusucreacion, DbType.String, entity.Irptfcusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptfcfeccreacion, DbType.DateTime, entity.Irptfcfeccreacion));

            command.ExecuteNonQuery();
        }

        public void Delete(int irptfccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Irptfccodi, DbType.Int32, irptfccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public IndReporteFCDTO GetById(int irptfccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Irptfccodi, DbType.Int32, irptfccodi);
            IndReporteFCDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<IndReporteFCDTO> GetByCriteria(string itotcodi)
        {
            List<IndReporteFCDTO> entitys = new List<IndReporteFCDTO>();

            string query = string.Format(helper.SqlGetByCriteria, itotcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndReporteFCDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IndReporteFCDTO> List()
        {
            List<IndReporteFCDTO> entitys = new List<IndReporteFCDTO>();
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

    }
}
