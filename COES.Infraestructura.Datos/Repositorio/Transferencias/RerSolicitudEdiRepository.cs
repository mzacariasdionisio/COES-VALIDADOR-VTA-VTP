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
    /// Clase de acceso a datos de la tabla RER_SOLICITUDEDI
    /// </summary>
    public class RerSolicitudEdiRepository : RepositoryBase, IRerSolicitudEdiRepository
    {
        private string strConexion;
        RerSolicitudEdiHelper helper = new RerSolicitudEdiHelper();

        public RerSolicitudEdiRepository(string strConn) : base(strConn)
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

        public void Save(RerSolicitudEdiDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rersedcodi, DbType.Int32, entity.Rersedcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercencodi, DbType.Int32, entity.Rercencodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ipericodi, DbType.Int32, entity.Ipericodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Reroricodi, DbType.Int32, entity.Reroricodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rersedfechahorainicio, DbType.DateTime, entity.Rersedfechahorainicio));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rersedfechahorafin, DbType.DateTime, entity.Rersedfechahorafin));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerseddesc, DbType.String, entity.Rerseddesc));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rersedtotenergia, DbType.Decimal, entity.Rersedtotenergia));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rersedsustento, DbType.String, entity.Rersedsustento));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rersedestadodeenvio, DbType.String, entity.Rersedestadodeenvio));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rersedeliminado, DbType.String, "NO"));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rersedusucreacion, DbType.String, entity.Rersedusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rersedfeccreacion, DbType.DateTime, entity.Rersedfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rersedusumodificacion, DbType.String, entity.Rersedusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rersedfecmodificacion, DbType.DateTime, entity.Rersedfecmodificacion));
            command.ExecuteNonQuery();
        }

        public void Update(RerSolicitudEdiDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlUpdate;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rercencodi, DbType.Int32, entity.Rercencodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ipericodi, DbType.Int32, entity.Ipericodi));
            //command.Parameters.Add(dbProvider.CreateParameter(command, helper.Reroricodi, DbType.Int32, entity.Reroricodi));
            //command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rersedfechahorainicio, DbType.DateTime, entity.Rersedfechahorainicio));
            //command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rersedfechahorafin, DbType.DateTime, entity.Rersedfechahorafin));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rerseddesc, DbType.String, entity.Rerseddesc));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rersedtotenergia, DbType.Decimal, entity.Rersedtotenergia));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rersedsustento, DbType.String, entity.Rersedsustento));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rersedestadodeenvio, DbType.String, entity.Rersedestadodeenvio));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rersedusumodificacion, DbType.String, entity.Rersedusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rersedfecmodificacion, DbType.DateTime, entity.Rersedfecmodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Rersedcodi, DbType.Int32, entity.Rersedcodi));
            command.ExecuteNonQuery();
        }
        public void LogicalDelete(int rersedcodi, string rersedusumodificacion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlLogicalDelete);

            dbProvider.AddInParameter(command, helper.Rersedusumodificacion, DbType.String, rersedusumodificacion);
            dbProvider.AddInParameter(command, helper.Rersedfecmodificacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Rersedcodi, DbType.Int32, rersedcodi);

            dbProvider.ExecuteNonQuery(command);
        }
        public RerSolicitudEdiDTO GetById(int rersedcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rersedcodi, DbType.Int32, rersedcodi);
            RerSolicitudEdiDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public RerSolicitudEdiDTO GetByIdView(int rersedcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdView);
            dbProvider.AddInParameter(command, helper.Rersedcodi, DbType.Int32, rersedcodi);

            RerSolicitudEdiDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iReroridesc = dr.GetOrdinal(helper.Reroridesc);
                    if (!dr.IsDBNull(iReroridesc)) entity.Reroridesc = dr.GetString(iReroridesc);
                }
            }

            return entity;
        }

        public List<RerSolicitudEdiDTO> List()
        {
            List<RerSolicitudEdiDTO> entities = new List<RerSolicitudEdiDTO>();
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
        public List<RerSolicitudEdiDTO> GetByCriteria(int emprcodi, int ipericodi)
        {
            string query = string.Format(helper.SqlGetByCriteria, emprcodi, ipericodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            List<RerSolicitudEdiDTO> entitys = new List<RerSolicitudEdiDTO>();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerSolicitudEdiDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iReroridesc = dr.GetOrdinal(helper.Reroridesc);
                    if (!dr.IsDBNull(iReroridesc)) entity.Reroridesc = dr.GetString(iReroridesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RerSolicitudEdiDTO> ListByPeriodo(int ipericodi)
        {
            string query = string.Format(helper.SqlListByPeriodo, ipericodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerSolicitudEdiDTO> entities = new List<RerSolicitudEdiDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerSolicitudEdiDTO entity = helper.Create(dr);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    entities.Add(entity);
                }
            }

            return entities;
        }

    }
}
