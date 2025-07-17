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
    /// Clase de acceso a datos de la tabla RER_EVALUACION_SOLICITUDEDI
    /// </summary>
    public class RerEvaluacionSolicitudEdiRepository : RepositoryBase, IRerEvaluacionSolicitudEdiRepository
    {
        private string strConexion;
        RerEvaluacionSolicitudEdiHelper helper = new RerEvaluacionSolicitudEdiHelper();

        public RerEvaluacionSolicitudEdiRepository(string strConn) : base(strConn)
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

        public void Save(RerEvaluacionSolicitudEdiDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Reresecodi, DbType.Int32, entity.Reresecodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerevacodi, DbType.Int32, entity.Rerevacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rersedcodi, DbType.Int32, entity.Rersedcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercencodi, DbType.Int32, entity.Rercencodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ipericodi, DbType.Int32, entity.Ipericodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Reroricodi, DbType.Int32, entity.Reroricodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Reresefechahorainicio, DbType.DateTime, entity.Reresefechahorainicio));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Reresefechahorafin, DbType.DateTime, entity.Reresefechahorafin));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Reresedesc, DbType.String, entity.Reresedesc));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Reresetotenergia, DbType.Decimal, entity.Reresetotenergia));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Reresesustento, DbType.String, entity.Reresesustento));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereseestadodeenvio, DbType.String, entity.Rereseestadodeenvio));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereseeliminado, DbType.String, entity.Rereseeliminado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereseusucreacionext, DbType.String, entity.Rereseusucreacionext));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Reresefeccreacionext, DbType.DateTime, entity.Reresefeccreacionext));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereseusumodificacionext, DbType.String, entity.Rereseusumodificacionext));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Reresefecmodificacionext, DbType.DateTime, entity.Reresefecmodificacionext));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereseusucreacion, DbType.String, entity.Rereseusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Reresefeccreacion, DbType.DateTime, entity.Reresefeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereseusumodificacion, DbType.String, entity.Rereseusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Reresefecmodificacion, DbType.DateTime, entity.Reresefecmodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Reresetotenergiaestimada, DbType.Decimal, entity.Reresetotenergiaestimada));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereseediaprobada, DbType.Decimal, entity.Rereseediaprobada));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereserfpmc, DbType.Decimal, entity.Rereserfpmc));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereseresdesc, DbType.String, entity.Rereseresdesc));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rereseresestado, DbType.String, entity.Rereseresestado));
            command.ExecuteNonQuery();
        }

        public void Update(RerEvaluacionSolicitudEdiDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rerevacodi, DbType.Int32, entity.Rerevacodi);
            dbProvider.AddInParameter(command, helper.Rersedcodi, DbType.Int32, entity.Rersedcodi);
            dbProvider.AddInParameter(command, helper.Rercencodi, DbType.Int32, entity.Rercencodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Ipericodi, DbType.Int32, entity.Ipericodi);
            dbProvider.AddInParameter(command, helper.Reroricodi, DbType.Int32, entity.Reroricodi);
            dbProvider.AddInParameter(command, helper.Reresefechahorainicio, DbType.DateTime, entity.Reresefechahorainicio);
            dbProvider.AddInParameter(command, helper.Reresefechahorafin, DbType.DateTime, entity.Reresefechahorafin);
            dbProvider.AddInParameter(command, helper.Reresedesc, DbType.String, entity.Reresedesc);
            dbProvider.AddInParameter(command, helper.Reresetotenergia, DbType.Decimal, entity.Reresetotenergia);
            dbProvider.AddInParameter(command, helper.Reresesustento, DbType.String, entity.Reresesustento);
            dbProvider.AddInParameter(command, helper.Rereseestadodeenvio, DbType.String, entity.Rereseestadodeenvio);
            dbProvider.AddInParameter(command, helper.Rereseeliminado, DbType.String, entity.Rereseeliminado);
            dbProvider.AddInParameter(command, helper.Rereseusucreacionext, DbType.String, entity.Rereseusucreacionext);
            dbProvider.AddInParameter(command, helper.Reresefeccreacionext, DbType.DateTime, entity.Reresefeccreacionext);
            dbProvider.AddInParameter(command, helper.Rereseusumodificacionext, DbType.String, entity.Rereseusumodificacionext);
            dbProvider.AddInParameter(command, helper.Reresefecmodificacionext, DbType.DateTime, entity.Reresefecmodificacionext);
            dbProvider.AddInParameter(command, helper.Rereseusucreacion, DbType.String, entity.Rereseusucreacion);
            dbProvider.AddInParameter(command, helper.Reresefeccreacion, DbType.DateTime, entity.Reresefeccreacion);
            dbProvider.AddInParameter(command, helper.Rereseusumodificacion, DbType.String, entity.Rereseusumodificacion);
            dbProvider.AddInParameter(command, helper.Reresefecmodificacion, DbType.DateTime, entity.Reresefecmodificacion);
            dbProvider.AddInParameter(command, helper.Reresetotenergiaestimada, DbType.Decimal, entity.Reresetotenergiaestimada);
            dbProvider.AddInParameter(command, helper.Rereseediaprobada, DbType.Decimal, entity.Rereseediaprobada);
            dbProvider.AddInParameter(command, helper.Rereserfpmc, DbType.Decimal, entity.Rereserfpmc);
            dbProvider.AddInParameter(command, helper.Rereseresdesc, DbType.String, entity.Rereseresdesc);
            dbProvider.AddInParameter(command, helper.Rereseresestado, DbType.String, entity.Rereseresestado);
            dbProvider.AddInParameter(command, helper.Reresecodi, DbType.Int32, entity.Reresecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateFields(RerEvaluacionSolicitudEdiDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand) conn.CreateCommand();
            command.CommandText = string.Format(helper.SqlUpdateFields);
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            dbProvider.AddInParameter(command, helper.Reresefechahorainicio, DbType.DateTime, entity.Reresefechahorainicio);
            dbProvider.AddInParameter(command, helper.Reresefechahorafin, DbType.DateTime, entity.Reresefechahorafin);
            dbProvider.AddInParameter(command, helper.Reresedesc, DbType.String, entity.Reresedesc);
            dbProvider.AddInParameter(command, helper.Rereseediaprobada, DbType.Decimal, entity.Rereseediaprobada);
            dbProvider.AddInParameter(command, helper.Rereseresdesc, DbType.String, entity.Rereseresdesc);
            dbProvider.AddInParameter(command, helper.Rereseresestado, DbType.String, entity.Rereseresestado);
            dbProvider.AddInParameter(command, helper.Rereseusumodificacion, DbType.String, entity.Rereseusumodificacion);
            dbProvider.AddInParameter(command, helper.Reresefecmodificacion, DbType.DateTime, entity.Reresefecmodificacion);
            dbProvider.AddInParameter(command, helper.Reresecodi, DbType.Int32, entity.Reresecodi);
            dbProvider.AddInParameter(command, helper.Rerevacodi, DbType.Int32, entity.Rerevacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateFieldsForResults(RerEvaluacionSolicitudEdiDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = string.Format(helper.SqlUpdateFieldsForResults);
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            dbProvider.AddInParameter(command, helper.Reresefechahorainicio, DbType.DateTime, entity.Reresefechahorainicio);
            dbProvider.AddInParameter(command, helper.Reresefechahorafin, DbType.DateTime, entity.Reresefechahorafin);
            dbProvider.AddInParameter(command, helper.Reresedesc, DbType.String, entity.Reresedesc);
            dbProvider.AddInParameter(command, helper.Rereseediaprobada, DbType.Decimal, entity.Rereseediaprobada);
            dbProvider.AddInParameter(command, helper.Rereseresdesc, DbType.String, entity.Rereseresdesc);
            dbProvider.AddInParameter(command, helper.Rereseusumodificacion, DbType.String, entity.Rereseusumodificacion);
            dbProvider.AddInParameter(command, helper.Reresefecmodificacion, DbType.DateTime, entity.Reresefecmodificacion);
            dbProvider.AddInParameter(command, helper.Reresecodi, DbType.Int32, entity.Reresecodi);
            dbProvider.AddInParameter(command, helper.Rerevacodi, DbType.Int32, entity.Rerevacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateEnergias(RerEvaluacionSolicitudEdiDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand) conn.CreateCommand();
            command.CommandText = string.Format(helper.SqlUpdateEnergiaEstimada);
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            dbProvider.AddInParameter(command, helper.Reresetotenergiaestimada, DbType.Decimal, entity.Reresetotenergiaestimada);
            dbProvider.AddInParameter(command, helper.Rereseediaprobada, DbType.Decimal, entity.Rereseediaprobada);
            dbProvider.AddInParameter(command, helper.Rereseusumodificacion, DbType.String, entity.Rereseusumodificacion);
            dbProvider.AddInParameter(command, helper.Reresefecmodificacion, DbType.DateTime, entity.Reresefecmodificacion);
            dbProvider.AddInParameter(command, helper.Reresecodi, DbType.Int32, entity.Reresecodi);
            dbProvider.AddInParameter(command, helper.Rerevacodi, DbType.Int32, entity.Rerevacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int reresecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Reresecodi, DbType.Int32, reresecodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public RerEvaluacionSolicitudEdiDTO GetById(int reresecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Reresecodi, DbType.Int32, reresecodi);
            RerEvaluacionSolicitudEdiDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.CreateById(dr);
                }
            }

            return entity;
        }

        public List<RerEvaluacionSolicitudEdiDTO> List()
        {
            List<RerEvaluacionSolicitudEdiDTO> entitys = new List<RerEvaluacionSolicitudEdiDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateByList(dr));
                }
            }

            return entitys;
        }

        public List<RerEvaluacionSolicitudEdiDTO> GetByCriteria(int rerevacodi, int rersedcodi)
        {
            string query = string.Format(helper.SqlGetByCriteria, rerevacodi, rersedcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerEvaluacionSolicitudEdiDTO> entitys = new List<RerEvaluacionSolicitudEdiDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateByCriteria(dr));
                }
            }
            return entitys;
        }
        
        public List<RerEvaluacionSolicitudEdiDTO> GetByEvaluacionByEliminadoByEstado(string rerevacodi, string rereseeliminado, string rereseresestado)
        {
            string query = string.Format(helper.SqlGetByEvaluacionByEliminadoByEstado, rerevacodi, rereseeliminado, rereseresestado);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerEvaluacionSolicitudEdiDTO> entitys = new List<RerEvaluacionSolicitudEdiDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateByEvaluacionByEliminadoByEstado(dr));
                }
            }
            return entitys;
        }

    }
}
