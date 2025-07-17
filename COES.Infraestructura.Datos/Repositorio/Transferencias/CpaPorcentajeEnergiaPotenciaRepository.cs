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
    /// Clase de acceso a datos de la tabla CPA_PORCENTAJE_ENERGIAPOTENCIA
    /// </summary>
    public class CpaPorcentajeEnergiaPotenciaRepository : RepositoryBase, ICpaPorcentajeEnergiaPotenciaRepository
    {
        private readonly string strConexion;
        readonly CpaPorcentajeEnergiaPotenciaHelper helper = new CpaPorcentajeEnergiaPotenciaHelper();

        public CpaPorcentajeEnergiaPotenciaRepository(string strConn) : base(strConn)
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

        public int Save(CpaPorcentajeEnergiaPotenciaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpapepcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cpapcodi, DbType.Int32, entity.Cpapcodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Cpapepenemes01, DbType.Decimal, entity.Cpapepenemes01 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepenemes02, DbType.Decimal, entity.Cpapepenemes02 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepenemes03, DbType.Decimal, entity.Cpapepenemes03 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepenemes04, DbType.Decimal, entity.Cpapepenemes04 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepenemes05, DbType.Decimal, entity.Cpapepenemes05 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepenemes06, DbType.Decimal, entity.Cpapepenemes06 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepenemes07, DbType.Decimal, entity.Cpapepenemes07 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepenemes08, DbType.Decimal, entity.Cpapepenemes08 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepenemes09, DbType.Decimal, entity.Cpapepenemes09 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepenemes10, DbType.Decimal, entity.Cpapepenemes10 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepenemes11, DbType.Decimal, entity.Cpapepenemes11 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepenemes12, DbType.Decimal, entity.Cpapepenemes12 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepenetotal, DbType.Decimal, entity.Cpapepenetotal ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppotmes01, DbType.Decimal, entity.Cpapeppotmes01 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppotmes02, DbType.Decimal, entity.Cpapeppotmes02 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppotmes03, DbType.Decimal, entity.Cpapeppotmes03 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppotmes04, DbType.Decimal, entity.Cpapeppotmes04 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppotmes05, DbType.Decimal, entity.Cpapeppotmes05 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppotmes06, DbType.Decimal, entity.Cpapeppotmes06 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppotmes07, DbType.Decimal, entity.Cpapeppotmes07 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppotmes08, DbType.Decimal, entity.Cpapeppotmes08 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppotmes09, DbType.Decimal, entity.Cpapeppotmes09 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppotmes10, DbType.Decimal, entity.Cpapeppotmes10 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppotmes11, DbType.Decimal, entity.Cpapeppotmes11 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppotmes12, DbType.Decimal, entity.Cpapeppotmes12 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppottotal, DbType.Decimal, entity.Cpapeppottotal ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepusucreacion, DbType.String, entity.Cpapepusucreacion ?? string.Empty);
            dbProvider.AddInParameter(command, helper.Cpapepfeccreacion, DbType.DateTime, entity.Cpapepfeccreacion);

            return dbProvider.ExecuteNonQuery(command);
        }

        public void Save(CpaPorcentajeEnergiaPotenciaDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapepcodi, DbType.Int32, entity.Cpapepcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapcodi, DbType.Int32, entity.Cpapcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapepenemes01, DbType.Decimal, entity.Cpapepenemes01 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapepenemes02, DbType.Decimal, entity.Cpapepenemes02 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapepenemes03, DbType.Decimal, entity.Cpapepenemes03 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapepenemes04, DbType.Decimal, entity.Cpapepenemes04 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapepenemes05, DbType.Decimal, entity.Cpapepenemes05 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapepenemes06, DbType.Decimal, entity.Cpapepenemes06 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapepenemes07, DbType.Decimal, entity.Cpapepenemes07 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapepenemes08, DbType.Decimal, entity.Cpapepenemes08 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapepenemes09, DbType.Decimal, entity.Cpapepenemes09 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapepenemes10, DbType.Decimal, entity.Cpapepenemes10 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapepenemes11, DbType.Decimal, entity.Cpapepenemes11 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapepenemes12, DbType.Decimal, entity.Cpapepenemes12 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapepenetotal, DbType.Decimal, entity.Cpapepenetotal ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapeppotmes01, DbType.Decimal, entity.Cpapeppotmes01 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapeppotmes02, DbType.Decimal, entity.Cpapeppotmes02 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapeppotmes03, DbType.Decimal, entity.Cpapeppotmes03 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapeppotmes04, DbType.Decimal, entity.Cpapeppotmes04 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapeppotmes05, DbType.Decimal, entity.Cpapeppotmes05 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapeppotmes06, DbType.Decimal, entity.Cpapeppotmes06 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapeppotmes07, DbType.Decimal, entity.Cpapeppotmes07 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapeppotmes08, DbType.Decimal, entity.Cpapeppotmes08 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapeppotmes09, DbType.Decimal, entity.Cpapeppotmes09 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapeppotmes10, DbType.Decimal, entity.Cpapeppotmes10 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapeppotmes11, DbType.Decimal, entity.Cpapeppotmes11 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapeppotmes12, DbType.Decimal, entity.Cpapeppotmes12 ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapeppottotal, DbType.Decimal, entity.Cpapeppottotal ?? (object)DBNull.Value));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapepusucreacion, DbType.String, entity.Cpapepusucreacion ?? string.Empty));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapepfeccreacion, DbType.DateTime, entity.Cpapepfeccreacion));

            command.ExecuteNonQuery();
        }

        public void Update(CpaPorcentajeEnergiaPotenciaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cpapepcodi, DbType.Int32, entity.Cpapepcodi);
            dbProvider.AddInParameter(command, helper.Cpapcodi, DbType.Int32, entity.Cpapcodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Cpapepenemes01, DbType.Decimal, entity.Cpapepenemes01 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepenemes02, DbType.Decimal, entity.Cpapepenemes02 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepenemes03, DbType.Decimal, entity.Cpapepenemes03 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepenemes04, DbType.Decimal, entity.Cpapepenemes04 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepenemes05, DbType.Decimal, entity.Cpapepenemes05 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepenemes06, DbType.Decimal, entity.Cpapepenemes06 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepenemes07, DbType.Decimal, entity.Cpapepenemes07 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepenemes08, DbType.Decimal, entity.Cpapepenemes08 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepenemes09, DbType.Decimal, entity.Cpapepenemes09 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepenemes10, DbType.Decimal, entity.Cpapepenemes10 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepenemes11, DbType.Decimal, entity.Cpapepenemes11 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepenemes12, DbType.Decimal, entity.Cpapepenemes12 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepenetotal, DbType.Decimal, entity.Cpapepenetotal ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppotmes01, DbType.Decimal, entity.Cpapeppotmes01 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppotmes02, DbType.Decimal, entity.Cpapeppotmes02 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppotmes03, DbType.Decimal, entity.Cpapeppotmes03 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppotmes04, DbType.Decimal, entity.Cpapeppotmes04 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppotmes05, DbType.Decimal, entity.Cpapeppotmes05 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppotmes06, DbType.Decimal, entity.Cpapeppotmes06 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppotmes07, DbType.Decimal, entity.Cpapeppotmes07 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppotmes08, DbType.Decimal, entity.Cpapeppotmes08 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppotmes09, DbType.Decimal, entity.Cpapeppotmes09 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppotmes10, DbType.Decimal, entity.Cpapeppotmes10 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppotmes11, DbType.Decimal, entity.Cpapeppotmes11 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppotmes12, DbType.Decimal, entity.Cpapeppotmes12 ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapeppottotal, DbType.Decimal, entity.Cpapeppottotal ?? (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpapepusucreacion, DbType.String, entity.Cpapepusucreacion ?? string.Empty);
            dbProvider.AddInParameter(command, helper.Cpapepfeccreacion, DbType.DateTime, entity.Cpapepfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cpapepcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Cpapcodi, DbType.Int32, cpapepcodi);
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

        public List<CpaPorcentajeEnergiaPotenciaDTO> List()
        {
            List<CpaPorcentajeEnergiaPotenciaDTO> entities = new List<CpaPorcentajeEnergiaPotenciaDTO>();
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

        public List<CpaPorcentajeEnergiaPotenciaDTO> ListByRevision(int cparcodi)
        {
            CpaPorcentajeEnergiaPotenciaDTO entity;
            string query = string.Format(helper.SqlListByRevision, cparcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<CpaPorcentajeEnergiaPotenciaDTO> entities = new List<CpaPorcentajeEnergiaPotenciaDTO>();

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

        public CpaPorcentajeEnergiaPotenciaDTO GetById(int cpapepcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpapepcodi, DbType.Int32, cpapepcodi);
            CpaPorcentajeEnergiaPotenciaDTO entity = null;

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
