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
    /// Clase de acceso a datos de la tabla RER_COMPARATIVO_DET
    /// </summary>
    public class RerComparativoDetRepository : RepositoryBase, IRerComparativoDetRepository
    {
        private string strConexion;
        RerComparativoDetHelper helper = new RerComparativoDetHelper();
        public RerComparativoDetRepository(string strConn) : base(strConn)
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

        public void Save(RerComparativoDetDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand) conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercdtcodi, DbType.Int32, entity.Rercdtcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerccbcodi, DbType.Int32, entity.Rerccbcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerevacodi, DbType.Int32, entity.Rerevacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Reresecodi, DbType.Int32, entity.Reresecodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereeucodi, DbType.Int32, entity.Rereeucodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercdtfecha, DbType.DateTime, entity.Rercdtfecha));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercdthora, DbType.String, entity.Rercdthora));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercdtmedfpm, DbType.Decimal, entity.Rercdtmedfpm));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercdtenesolicitada, DbType.Decimal, entity.Rercdtenesolicitada));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercdteneestimada, DbType.Decimal, entity.Rercdteneestimada));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercdtpordesviacion, DbType.Decimal, entity.Rercdtpordesviacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercdtflag, DbType.String, entity.Rercdtflag));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercdtusucreacion, DbType.String, entity.Rercdtusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercdtfeccreacion, DbType.DateTime, entity.Rercdtfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercdtusumodificacion, DbType.String, entity.Rercdtusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercdtfecmodificacion, DbType.DateTime, entity.Rercdtfecmodificacion));
            command.ExecuteNonQuery();
        }

        public void Update(RerComparativoDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rerccbcodi, DbType.Int32, entity.Rerccbcodi);
            dbProvider.AddInParameter(command, helper.Reresecodi, DbType.Int32, entity.Reresecodi);
            dbProvider.AddInParameter(command, helper.Rerevacodi, DbType.Int32, entity.Rerevacodi);
            dbProvider.AddInParameter(command, helper.Rercdtfecha, DbType.DateTime, entity.Rercdtfecha);
            dbProvider.AddInParameter(command, helper.Rercdthora, DbType.String, entity.Rercdthora);
            dbProvider.AddInParameter(command, helper.Rercdtmedfpm, DbType.Decimal, entity.Rercdtmedfpm);
            dbProvider.AddInParameter(command, helper.Rercdtenesolicitada, DbType.Decimal, entity.Rercdtenesolicitada);
            dbProvider.AddInParameter(command, helper.Rercdteneestimada, DbType.Decimal, entity.Rercdteneestimada);
            dbProvider.AddInParameter(command, helper.Rercdtpordesviacion, DbType.Decimal, entity.Rercdtpordesviacion);
            dbProvider.AddInParameter(command, helper.Rercdtflag, DbType.String, entity.Rercdtflag);
            dbProvider.AddInParameter(command, helper.Rercdtusucreacion, DbType.String, entity.Rercdtusucreacion);
            dbProvider.AddInParameter(command, helper.Rercdtfeccreacion, DbType.DateTime, entity.Rercdtfeccreacion);
            dbProvider.AddInParameter(command, helper.Rercdtusumodificacion, DbType.String, entity.Rercdtusumodificacion);
            dbProvider.AddInParameter(command, helper.Rercdtfecmodificacion, DbType.DateTime, entity.Rercdtfecmodificacion);
            dbProvider.AddInParameter(command, helper.Rercdtcodi, DbType.Int32, entity.Rercdtcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rercdtcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Rercdtcodi, DbType.Int32, rercdtcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByRerccbcodi(int rerccbcodi, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand) conn.CreateCommand();
            command.CommandText = helper.SqlDeleteByRerccbcodi;
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerccbcodi, DbType.Int32, rerccbcodi));
            command.ExecuteNonQuery(); 
        }

        public RerComparativoDetDTO GetById(int rercdtcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rercdtcodi, DbType.Int32, rercdtcodi);
            RerComparativoDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RerComparativoDetDTO> List()
        {
            List<RerComparativoDetDTO> entities = new List<RerComparativoDetDTO>();
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

        public List<RerComparativoDetDTO> GetByCriteria(int rerevacodi, int reresecodi, int rereeucodi)
        {
            string query = string.Format(helper.SqlGetByCriteria, rerevacodi, reresecodi, rereeucodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerComparativoDetDTO> entitys = new List<RerComparativoDetDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public List<RerComparativoDetDTO> GetEEDRByCriteria(int rerevacodi, int reresecodi, int rereeucodi)
        {
            string query = string.Format(helper.SqlGetEEDRByCriteria, rerevacodi, reresecodi, rereeucodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerComparativoDetDTO> entitys = new List<RerComparativoDetDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public List<RerComparativoDetDTO> GetComparativoAprobadaValidadByMes(DateTime dia1Mes)
        {
            string sDia1MesActual = dia1Mes.ToString("dd/MM/yyyy");
            DateTime dDia2MesActual = dia1Mes.AddDays(1);
            DateTime dDiaFinMesActual = dia1Mes.AddMonths(1).AddDays(-1);
            DateTime dDia1MesSiguiente = dia1Mes.AddMonths(1);

            string sDiaFinMesActual = dDiaFinMesActual.ToString("dd/MM/yyyy");
            string sDia1MesSiguiente = dDia1MesSiguiente.ToString("dd/MM/yyyy");
            string sDia2MesActual = dDia2MesActual.ToString("dd/MM/yyyy");

            List<RerComparativoDetDTO> entitys = new List<RerComparativoDetDTO>();
            string query = string.Format(helper.SqlListComparativoAprobadaValidadaByMes, sDia1MesActual, sDia2MesActual, sDiaFinMesActual, sDia1MesSiguiente);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerComparativoDetDTO entity = helper.Create(dr);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


    }
}
