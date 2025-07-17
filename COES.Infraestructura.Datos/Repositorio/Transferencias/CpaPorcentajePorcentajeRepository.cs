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
    /// Clase de acceso a datos de la tabla CPA_PORCENTAJE_PORCENTAJE
    /// </summary>
    public class CpaPorcentajePorcentajeRepository : RepositoryBase, ICpaPorcentajePorcentajeRepository
    {
        private readonly string strConexion;
        readonly CpaPorcentajePorcentajeHelper helper = new CpaPorcentajePorcentajeHelper();

        public CpaPorcentajePorcentajeRepository(string strConn) : base(strConn)
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

        public int Save(CpaPorcentajePorcentajeDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpappcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cpapcodi, DbType.Int32, entity.Cpapcodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Cpappgentotene, DbType.Decimal, entity.Cpappgentotene);
            dbProvider.AddInParameter(command, helper.Cpappgentotpot, DbType.Decimal, entity.Cpappgentotpot);
            dbProvider.AddInParameter(command, helper.Cpappdistotene, DbType.Decimal, entity.Cpappdistotene);
            dbProvider.AddInParameter(command, helper.Cpappdistotpot, DbType.Decimal, entity.Cpappdistotpot);
            dbProvider.AddInParameter(command, helper.Cpappultotene, DbType.Decimal, entity.Cpappultotene);
            dbProvider.AddInParameter(command, helper.Cpappultotpot, DbType.Decimal, entity.Cpappultotpot);
            dbProvider.AddInParameter(command, helper.Cpapptratot, DbType.Decimal, entity.Cpapptratot);
            dbProvider.AddInParameter(command, helper.Cpapptotal, DbType.Decimal, entity.Cpapptotal);
            dbProvider.AddInParameter(command, helper.Cpappporcentaje, DbType.Decimal, entity.Cpappporcentaje);
            dbProvider.AddInParameter(command, helper.Cpappusucreacion, DbType.String, entity.Cpappusucreacion);
            dbProvider.AddInParameter(command, helper.Cpappfeccreacion, DbType.DateTime, entity.Cpappfeccreacion);

            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public void Save(CpaPorcentajePorcentajeDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpappcodi, DbType.Int32, entity.Cpappcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapcodi, DbType.Int32, entity.Cpapcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpappgentotene, DbType.Decimal, entity.Cpappgentotene));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpappgentotpot, DbType.Decimal, entity.Cpappgentotpot));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpappdistotene, DbType.Decimal, entity.Cpappdistotene));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpappdistotpot, DbType.Decimal, entity.Cpappdistotpot));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpappultotene, DbType.Decimal, entity.Cpappultotene));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpappultotpot, DbType.Decimal, entity.Cpappultotpot));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapptratot, DbType.Decimal, entity.Cpapptratot));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpapptotal, DbType.Decimal, entity.Cpapptotal));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpappporcentaje, DbType.Decimal, entity.Cpappporcentaje));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpappusucreacion, DbType.String, entity.Cpappusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpappfeccreacion, DbType.DateTime, entity.Cpappfeccreacion));

            command.ExecuteNonQuery();
        }

        public void Update(CpaPorcentajePorcentajeDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cpapcodi, DbType.Int32, entity.Cpapcodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Cpappgentotene, DbType.Decimal, entity.Cpappgentotene);
            dbProvider.AddInParameter(command, helper.Cpappgentotpot, DbType.Decimal, entity.Cpappgentotpot);
            dbProvider.AddInParameter(command, helper.Cpappdistotene, DbType.Decimal, entity.Cpappdistotene);
            dbProvider.AddInParameter(command, helper.Cpappdistotpot, DbType.Decimal, entity.Cpappdistotpot);
            dbProvider.AddInParameter(command, helper.Cpappultotene, DbType.Decimal, entity.Cpappultotene);
            dbProvider.AddInParameter(command, helper.Cpappultotpot, DbType.Decimal, entity.Cpappultotpot);
            dbProvider.AddInParameter(command, helper.Cpapptratot, DbType.Decimal, entity.Cpapptratot);
            dbProvider.AddInParameter(command, helper.Cpapptotal, DbType.Decimal, entity.Cpapptotal);
            dbProvider.AddInParameter(command, helper.Cpappporcentaje, DbType.Decimal, entity.Cpappporcentaje);
            dbProvider.AddInParameter(command, helper.Cpappusucreacion, DbType.String, entity.Cpappusucreacion);
            dbProvider.AddInParameter(command, helper.Cpappfeccreacion, DbType.DateTime, entity.Cpappfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cpappcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Cpappcodi, DbType.Int32, cpappcodi);
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

        public List<CpaPorcentajePorcentajeDTO> List()
        {
            List<CpaPorcentajePorcentajeDTO> entities = new List<CpaPorcentajePorcentajeDTO>();
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

        public List<CpaPorcentajePorcentajeDTO> ListByRevision(int cparcodi)
        {
            CpaPorcentajePorcentajeDTO entity;
            string query = string.Format(helper.SqlListByRevision, cparcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<CpaPorcentajePorcentajeDTO> entities = new List<CpaPorcentajePorcentajeDTO>();

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

        public CpaPorcentajePorcentajeDTO GetById(int cpappcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpappcodi, DbType.Int32, cpappcodi);
            CpaPorcentajePorcentajeDTO entity = null;

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
