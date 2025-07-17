using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla RER_CALCULO_MENSUAL
    /// </summary>
    public class RerCalculoMensualRepository : RepositoryBase, IRerCalculoMensualRepository
    {
        private string strConexion;
        RerCalculoMensualHelper helper = new RerCalculoMensualHelper();

        public RerCalculoMensualRepository(string strConn) : base(strConn)
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
            return (DbTransaction) conn.BeginTransaction();
        }

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public void Save(RerCalculoMensualDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercmcodi, DbType.Int32, entity.Rercmcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerpprcodi, DbType.Int32, entity.Rerpprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercmfatipintervalo, DbType.String, entity.Rercmfatipintervalo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercmfafecintervalo, DbType.DateTime, entity.Rercmfafecintervalo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercmfavalintervalo, DbType.Decimal, entity.Rercmfavalintervalo));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercmtaradj, DbType.Decimal, entity.Rercmtaradj));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercmsummulinfa, DbType.Decimal, entity.Rercmsummulinfa));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercminggarantizado, DbType.Decimal, entity.Rercminggarantizado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercminsingpotencia, DbType.Decimal, entity.Rercminsingpotencia));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercmsumfadivn, DbType.Decimal, entity.Rercmsumfadivn));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercmingpotencia, DbType.Decimal, entity.Rercmingpotencia));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercmingprimarer, DbType.Decimal, entity.Rercmingprimarer));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercmingenergia, DbType.Decimal, entity.Rercmingenergia));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercmsaldovtea, DbType.Decimal, entity.Rercmsaldovtea));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercmsaldovtp, DbType.Decimal, entity.Rercmsaldovtp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercmtipocambio, DbType.Decimal, entity.Rercmtipocambio));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercmimcp, DbType.Decimal, entity.Rercmimcp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercmsalmencompensar, DbType.Decimal, entity.Rercmsalmencompensar));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercmusucreacion, DbType.String, entity.Rercmusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercmfeccreacion, DbType.DateTime, entity.Rercmfeccreacion));

            command.ExecuteNonQuery();
        }

        public void Update(RerCalculoMensualDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, entity.Rerpprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rercmfatipintervalo, DbType.String, entity.Rercmfatipintervalo);
            dbProvider.AddInParameter(command, helper.Rercmfafecintervalo, DbType.DateTime, entity.Rercmfafecintervalo);
            dbProvider.AddInParameter(command, helper.Rercmfavalintervalo, DbType.Decimal, entity.Rercmfavalintervalo);

            dbProvider.AddInParameter(command, helper.Rercmtaradj, DbType.Decimal, entity.Rercmtaradj);
            dbProvider.AddInParameter(command, helper.Rercmsummulinfa, DbType.Decimal, entity.Rercmsummulinfa);
            dbProvider.AddInParameter(command, helper.Rercminggarantizado, DbType.Decimal, entity.Rercminggarantizado);
            dbProvider.AddInParameter(command, helper.Rercminsingpotencia, DbType.Decimal, entity.Rercminsingpotencia);
            dbProvider.AddInParameter(command, helper.Rercmsumfadivn, DbType.Decimal, entity.Rercmsumfadivn);
            dbProvider.AddInParameter(command, helper.Rercmingpotencia, DbType.Decimal, entity.Rercmingpotencia);
            dbProvider.AddInParameter(command, helper.Rercmingprimarer, DbType.Decimal, entity.Rercmingprimarer);
            dbProvider.AddInParameter(command, helper.Rercmingenergia, DbType.Decimal, entity.Rercmingenergia);
            dbProvider.AddInParameter(command, helper.Rercmsaldovtea, DbType.Decimal, entity.Rercmsaldovtea);
            dbProvider.AddInParameter(command, helper.Rercmsaldovtp, DbType.Decimal, entity.Rercmsaldovtp);
            dbProvider.AddInParameter(command, helper.Rercmtipocambio, DbType.Decimal, entity.Rercmtipocambio);
            dbProvider.AddInParameter(command, helper.Rercmimcp, DbType.Decimal, entity.Rercmimcp);
            dbProvider.AddInParameter(command, helper.Rercmsalmencompensar, DbType.Decimal, entity.Rercmsalmencompensar);

            dbProvider.AddInParameter(command, helper.Rercmusucreacion, DbType.String, entity.Rercmusucreacion);
            dbProvider.AddInParameter(command, helper.Rercmfeccreacion, DbType.DateTime, entity.Rercmfeccreacion);
            dbProvider.AddInParameter(command, helper.Rercmcodi, DbType.Int32, entity.Rercmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rercmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Rercmcodi, DbType.Int32, rercmcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByAnioVersion(int reravcodi, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlDeleteByAnioVersion;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Reravcodi, DbType.Int32, reravcodi));
            command.ExecuteNonQuery();
        }

        public RerCalculoMensualDTO GetById(int rercmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Rercmcodi, DbType.Int32, rercmcodi);
            RerCalculoMensualDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RerCalculoMensualDTO> List()
        {
            List<RerCalculoMensualDTO> entities = new List<RerCalculoMensualDTO>();
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

        public List<RerCalculoMensualDTO> GetByAnioTarifario(int reravcodi)
        {
            string query = string.Format(helper.SqlGetByAnioTarifario, reravcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerCalculoMensualDTO> entities = new List<RerCalculoMensualDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.CreateByAnioTarifario(dr));
                }
            }

            return entities;
        }
        
    }
}

