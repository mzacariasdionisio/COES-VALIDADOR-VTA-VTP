using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Security.Principal;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla PR_RESERVA
    /// </summary>
    public class PrReservaRepository: RepositoryBase, IPrReservaRepository
    {
        private string strConexion;
        public PrReservaRepository(string strConn): base(strConn)
        {
            strConexion = strConn;
        }

        PrReservaHelper helper = new PrReservaHelper();

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

        public int Save(PrReservaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Prsvcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Prsvdato, DbType.String, entity.Prsvdato);
            dbProvider.AddInParameter(command, helper.Prsvactivo, DbType.Int32, entity.Prsvactivo);
            dbProvider.AddInParameter(command, helper.Prsvfechavigencia, DbType.DateTime, entity.Prsvfechavigencia);
            dbProvider.AddInParameter(command, helper.Prsvfeccreacion, DbType.DateTime, entity.Prsvfeccreacion);
            dbProvider.AddInParameter(command, helper.Prsvusucreacion, DbType.String, entity.Prsvusucreacion);
            dbProvider.AddInParameter(command, helper.Prsvfecmodificacion, DbType.DateTime, entity.Prsvfecmodificacion);
            dbProvider.AddInParameter(command, helper.Prsvusumodificacion, DbType.String, entity.Prsvusumodificacion);
            dbProvider.AddInParameter(command, helper.Prsvtipo, DbType.Int32, entity.Prsvtipo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int SaveTransaccional(PrReservaDTO entity, IDbConnection connection, DbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prsvcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prsvdato, DbType.String, entity.Prsvdato));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prsvactivo, DbType.Int32, entity.Prsvactivo));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prsvfechavigencia, DbType.DateTime, entity.Prsvfechavigencia));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prsvfeccreacion, DbType.DateTime, entity.Prsvfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prsvusucreacion, DbType.String, entity.Prsvusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prsvfecmodificacion, DbType.DateTime, entity.Prsvfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prsvusumodificacion, DbType.String, entity.Prsvusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prsvtipo, DbType.Int32, entity.Prsvtipo));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void Update(PrReservaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Prsvdato, DbType.String, entity.Prsvdato);
            dbProvider.AddInParameter(command, helper.Prsvactivo, DbType.Int32, entity.Prsvactivo);
            dbProvider.AddInParameter(command, helper.Prsvfechavigencia, DbType.DateTime, entity.Prsvfechavigencia);
            dbProvider.AddInParameter(command, helper.Prsvfeccreacion, DbType.DateTime, entity.Prsvfeccreacion);
            dbProvider.AddInParameter(command, helper.Prsvusucreacion, DbType.String, entity.Prsvusucreacion);
            dbProvider.AddInParameter(command, helper.Prsvfecmodificacion, DbType.DateTime, entity.Prsvfecmodificacion);
            dbProvider.AddInParameter(command, helper.Prsvusumodificacion, DbType.String, entity.Prsvusumodificacion);
            dbProvider.AddInParameter(command, helper.Prsvtipo, DbType.Int32, entity.Prsvtipo);
            dbProvider.AddInParameter(command, helper.Prsvcodi, DbType.Int32, entity.Prsvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateTransaccional(PrReservaDTO entity, IDbConnection connection, DbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlUpdate;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prsvdato, DbType.String, entity.Prsvdato));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prsvactivo, DbType.Int32, entity.Prsvactivo));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prsvfechavigencia, DbType.DateTime, entity.Prsvfechavigencia));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prsvfeccreacion, DbType.DateTime, entity.Prsvfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prsvusucreacion, DbType.String, entity.Prsvusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prsvfecmodificacion, DbType.DateTime, entity.Prsvfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prsvusumodificacion, DbType.String, entity.Prsvusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prsvtipo, DbType.Int32, entity.Prsvtipo));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prsvcodi, DbType.Int32, entity.Prsvcodi));

                dbCommand.ExecuteNonQuery();
            }
        }

        public void Delete(int prsvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Prsvcodi, DbType.Int32, prsvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrReservaDTO GetById(int prsvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Prsvcodi, DbType.Int32, prsvcodi);
            PrReservaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrReservaDTO> List()
        {
            List<PrReservaDTO> entitys = new List<PrReservaDTO>();
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

        public List<PrReservaDTO> GetByCriteria(DateTime fecha, string tipo)
        {
            List<PrReservaDTO> entitys = new List<PrReservaDTO>();
            var sComando = string.Format(helper.SqlGetByCriteria, fecha.ToString(ConstantesBase.FormatoFechaExtendido), tipo);
            DbCommand command = dbProvider.GetSqlStringCommand(sComando);
            //DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public void ActualizarEstadoRegistro(PrReservaDTO entity)
        {
            var sComando = string.Format(helper.SqlActualizarEstadoRegistro, entity.Prsvactivo, entity.Prsvusumodificacion, entity.Prsvfecmodificacion.Value.ToString(ConstantesBase.FormatoFechaExtendido), entity.Prsvcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sComando);

            dbProvider.ExecuteNonQuery(command);
        }
    }
}
