using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CPA_PORCENTAJE_MONTO
    /// </summary>
    public class CpaPorcentajeMontoRepository : RepositoryBase, ICpaPorcentajeMontoRepository
    {
        private readonly string strConexion;
        readonly CpaPorcentajeMontoHelper helper = new CpaPorcentajeMontoHelper();

        public CpaPorcentajeMontoRepository(string strConn) : base(strConn)
        {
            strConexion = strConn;
        }

        public IDbConnection BeginConnection()
        {
            Database db = DatabaseFactory.CreateDatabase(strConexion);
            IDbConnection conn = db.CreateConnection();
            conn.Open();
            return conn;
        }

        public DbTransaction StartTransaction(IDbConnection conn)
        {
            return (DbTransaction)conn.BeginTransaction();
        }

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(CpaPorcentajeMontoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpapmtcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cpapcodi, DbType.Int32, entity.Cpapcodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.AddInParameter(command, helper.Cpapmtenemes01, DbType.Decimal, entity.Cpapmtenemes01 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtenemes02, DbType.Decimal, entity.Cpapmtenemes02 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtenemes03, DbType.Decimal, entity.Cpapmtenemes03 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtenemes04, DbType.Decimal, entity.Cpapmtenemes04 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtenemes05, DbType.Decimal, entity.Cpapmtenemes05 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtenemes06, DbType.Decimal, entity.Cpapmtenemes06 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtenemes07, DbType.Decimal, entity.Cpapmtenemes07 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtenemes08, DbType.Decimal, entity.Cpapmtenemes08 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtenemes09, DbType.Decimal, entity.Cpapmtenemes09 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtenemes10, DbType.Decimal, entity.Cpapmtenemes10 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtenemes11, DbType.Decimal, entity.Cpapmtenemes11 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtenemes12, DbType.Decimal, entity.Cpapmtenemes12 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtenetotal, DbType.Decimal, entity.Cpapmtenetotal ?? (object)DBNull.Value);

            dbProvider.AddInParameter(command, helper.Cpapmtpotmes01, DbType.Decimal, entity.Cpapmtpotmes01 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpotmes02, DbType.Decimal, entity.Cpapmtpotmes02 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpotmes03, DbType.Decimal, entity.Cpapmtpotmes03 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpotmes04, DbType.Decimal, entity.Cpapmtpotmes04 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpotmes05, DbType.Decimal, entity.Cpapmtpotmes05 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpotmes06, DbType.Decimal, entity.Cpapmtpotmes06 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpotmes07, DbType.Decimal, entity.Cpapmtpotmes07 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpotmes08, DbType.Decimal, entity.Cpapmtpotmes08 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpotmes09, DbType.Decimal, entity.Cpapmtpotmes09 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpotmes10, DbType.Decimal, entity.Cpapmtpotmes10 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpotmes11, DbType.Decimal, entity.Cpapmtpotmes11 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpotmes12, DbType.Decimal, entity.Cpapmtpotmes12 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpottotal, DbType.Decimal, entity.Cpapmtpottotal ?? (object)DBNull.Value);

            dbProvider.AddInParameter(command, helper.Cpapmttrames01, DbType.Decimal, entity.Cpapmttrames01 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmttrames02, DbType.Decimal, entity.Cpapmttrames02 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmttrames03, DbType.Decimal, entity.Cpapmttrames03 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmttrames04, DbType.Decimal, entity.Cpapmttrames04 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmttrames05, DbType.Decimal, entity.Cpapmttrames05 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmttrames06, DbType.Decimal, entity.Cpapmttrames06 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmttrames07, DbType.Decimal, entity.Cpapmttrames07 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmttrames08, DbType.Decimal, entity.Cpapmttrames08 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmttrames09, DbType.Decimal, entity.Cpapmttrames09 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmttrames10, DbType.Decimal, entity.Cpapmttrames10 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmttrames11, DbType.Decimal, entity.Cpapmttrames11 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmttrames12, DbType.Decimal, entity.Cpapmttrames12 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmttratotal, DbType.Decimal, entity.Cpapmttratotal ?? (object)DBNull.Value);

            dbProvider.AddInParameter(command, helper.Cpapmtusucreacion, DbType.String, entity.Cpapmtusucreacion ?? string.Empty);
            dbProvider.AddInParameter(command, helper.Cpapmtfeccreacion, DbType.DateTime, entity.Cpapmtfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return entity.Cpapmtcodi;
        }

        public void Save(CpaPorcentajeMontoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtcodi, DbType.Int32, entity.Cpapmtcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapcodi, DbType.Int32, entity.Cpapcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtenemes01, DbType.Decimal, entity.Cpapmtenemes01 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtenemes02, DbType.Decimal, entity.Cpapmtenemes02 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtenemes03, DbType.Decimal, entity.Cpapmtenemes03 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtenemes04, DbType.Decimal, entity.Cpapmtenemes04 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtenemes05, DbType.Decimal, entity.Cpapmtenemes05 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtenemes06, DbType.Decimal, entity.Cpapmtenemes06 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtenemes07, DbType.Decimal, entity.Cpapmtenemes07 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtenemes08, DbType.Decimal, entity.Cpapmtenemes08 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtenemes09, DbType.Decimal, entity.Cpapmtenemes09 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtenemes10, DbType.Decimal, entity.Cpapmtenemes10 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtenemes11, DbType.Decimal, entity.Cpapmtenemes11 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtenemes12, DbType.Decimal, entity.Cpapmtenemes12 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtenetotal, DbType.Decimal, entity.Cpapmtenetotal ?? (object)DBNull.Value));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtpotmes01, DbType.Decimal, entity.Cpapmtpotmes01 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtpotmes02, DbType.Decimal, entity.Cpapmtpotmes02 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtpotmes03, DbType.Decimal, entity.Cpapmtpotmes03 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtpotmes04, DbType.Decimal, entity.Cpapmtpotmes04 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtpotmes05, DbType.Decimal, entity.Cpapmtpotmes05 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtpotmes06, DbType.Decimal, entity.Cpapmtpotmes06 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtpotmes07, DbType.Decimal, entity.Cpapmtpotmes07 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtpotmes08, DbType.Decimal, entity.Cpapmtpotmes08 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtpotmes09, DbType.Decimal, entity.Cpapmtpotmes09 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtpotmes10, DbType.Decimal, entity.Cpapmtpotmes10 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtpotmes11, DbType.Decimal, entity.Cpapmtpotmes11 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtpotmes12, DbType.Decimal, entity.Cpapmtpotmes12 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtpottotal, DbType.Decimal, entity.Cpapmtpottotal ?? (object)DBNull.Value));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmttrames01, DbType.Decimal, entity.Cpapmttrames01 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmttrames02, DbType.Decimal, entity.Cpapmttrames02 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmttrames03, DbType.Decimal, entity.Cpapmttrames03 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmttrames04, DbType.Decimal, entity.Cpapmttrames04 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmttrames05, DbType.Decimal, entity.Cpapmttrames05 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmttrames06, DbType.Decimal, entity.Cpapmttrames06 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmttrames07, DbType.Decimal, entity.Cpapmttrames07 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmttrames08, DbType.Decimal, entity.Cpapmttrames08 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmttrames09, DbType.Decimal, entity.Cpapmttrames09 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmttrames10, DbType.Decimal, entity.Cpapmttrames10 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmttrames11, DbType.Decimal, entity.Cpapmttrames11 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmttrames12, DbType.Decimal, entity.Cpapmttrames12 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmttratotal, DbType.Decimal, entity.Cpapmttratotal ?? (object)DBNull.Value));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtusucreacion, DbType.String, entity.Cpapmtusucreacion ?? string.Empty));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapmtfeccreacion, DbType.DateTime, entity.Cpapmtfeccreacion));

            command.ExecuteNonQuery();
        }

        public void Update(CpaPorcentajeMontoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cpapmtcodi, DbType.Int32, entity.Cpapmtcodi);
            dbProvider.AddInParameter(command, helper.Cpapcodi, DbType.Int32, entity.Cpapcodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.AddInParameter(command, helper.Cpapmtenemes01, DbType.Decimal, entity.Cpapmtenemes01 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtenemes02, DbType.Decimal, entity.Cpapmtenemes02 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtenemes03, DbType.Decimal, entity.Cpapmtenemes03 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtenemes04, DbType.Decimal, entity.Cpapmtenemes04 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtenemes05, DbType.Decimal, entity.Cpapmtenemes05 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtenemes06, DbType.Decimal, entity.Cpapmtenemes06 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtenemes07, DbType.Decimal, entity.Cpapmtenemes07 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtenemes08, DbType.Decimal, entity.Cpapmtenemes08 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtenemes09, DbType.Decimal, entity.Cpapmtenemes09 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtenemes10, DbType.Decimal, entity.Cpapmtenemes10 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtenemes11, DbType.Decimal, entity.Cpapmtenemes11 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtenemes12, DbType.Decimal, entity.Cpapmtenemes12 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtenetotal, DbType.Decimal, entity.Cpapmtenetotal ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpotmes01, DbType.Decimal, entity.Cpapmtpotmes01 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpotmes02, DbType.Decimal, entity.Cpapmtpotmes02 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpotmes03, DbType.Decimal, entity.Cpapmtpotmes03 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpotmes04, DbType.Decimal, entity.Cpapmtpotmes04 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpotmes05, DbType.Decimal, entity.Cpapmtpotmes05 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpotmes06, DbType.Decimal, entity.Cpapmtpotmes06 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpotmes07, DbType.Decimal, entity.Cpapmtpotmes07 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpotmes08, DbType.Decimal, entity.Cpapmtpotmes08 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpotmes09, DbType.Decimal, entity.Cpapmtpotmes09 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpotmes10, DbType.Decimal, entity.Cpapmtpotmes10 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpotmes11, DbType.Decimal, entity.Cpapmtpotmes11 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpotmes12, DbType.Decimal, entity.Cpapmtpotmes12 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtpottotal, DbType.Decimal, entity.Cpapmtpottotal ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmttrames01, DbType.Decimal, entity.Cpapmttrames01 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmttrames02, DbType.Decimal, entity.Cpapmttrames02 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmttrames03, DbType.Decimal, entity.Cpapmttrames03 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmttrames04, DbType.Decimal, entity.Cpapmttrames04 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmttrames05, DbType.Decimal, entity.Cpapmttrames05 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmttrames06, DbType.Decimal, entity.Cpapmttrames06 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmttrames07, DbType.Decimal, entity.Cpapmttrames07 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmttrames08, DbType.Decimal, entity.Cpapmttrames08 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmttrames09, DbType.Decimal, entity.Cpapmttrames09 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmttrames10, DbType.Decimal, entity.Cpapmttrames10 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmttrames11, DbType.Decimal, entity.Cpapmttrames11 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmttrames12, DbType.Decimal, entity.Cpapmttrames12 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapmtusucreacion, DbType.String, entity.Cpapmtusucreacion ?? string.Empty);
            dbProvider.AddInParameter(command, helper.Cpapmtfeccreacion, DbType.DateTime, entity.Cpapmtfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cpapmtcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Cpapmtcodi, DbType.Int32, cpapmtcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByRevision(int cparcodi, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlDeleteByRevision;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparcodi, DbType.Int32, cparcodi));
            command.ExecuteNonQuery();
        }

        public List<CpaPorcentajeMontoDTO> List()
        {
            List<CpaPorcentajeMontoDTO> entities = new List<CpaPorcentajeMontoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }

        public List<CpaPorcentajeMontoDTO> ListByRevision(int cparcodi)
        {
            CpaPorcentajeMontoDTO entity;
            string query = string.Format(helper.SqlListByRevision, cparcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<CpaPorcentajeMontoDTO> entities = new List<CpaPorcentajeMontoDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = dr.GetInt32(iTipoemprcodi);

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprruc = dr.GetOrdinal(helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    entities.Add(entity);
                }
            }

            return entities;
        }

        public CpaPorcentajeMontoDTO GetById(int cpapmtcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpapmtcodi, DbType.Int32, cpapmtcodi);
            CpaPorcentajeMontoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

    }

}
