using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla SIO_CABECERADET
    /// </summary>
    public class SioCabeceradetRepository : RepositoryBase, ISioCabeceradetRepository
    {
        private readonly string strConexion;
        public SioCabeceradetRepository(string strConn)
            : base(strConn)
        {
            strConexion = strConn;
        }

        SioCabeceradetHelper helper = new SioCabeceradetHelper();

        public IDbConnection BeginConnection()
        {
            Database db = DatabaseFactory.CreateDatabase(strConexion);
            IDbConnection conn = db.CreateConnection();
            conn.Open();
            return conn;
        }

        public IDbTransaction StartTransaction(IDbConnection conn)
        {
            return conn.BeginTransaction();
        }

        public int Save(SioCabeceradetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = result == null ? 1 : Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlObtenerUltNroVersion, entity.Cabpriperiodo.ToString(ConstantesBase.FormatoFecha), entity.Tpriecodi));
            object result2 = dbProvider.ExecuteScalar(command);
            int version = 0;
            if (result2 != null)
                try
                {
                    version = Convert.ToInt32(result2);
                }
                catch (Exception ex)
                {

                }
            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Cabpricodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tpriecodi, DbType.Int32, entity.Tpriecodi);
            dbProvider.AddInParameter(command, helper.Cabpriperiodo, DbType.DateTime, entity.Cabpriperiodo);
            dbProvider.AddInParameter(command, helper.Cabpriusucreacion, DbType.String, entity.Cabpriusucreacion);
            dbProvider.AddInParameter(command, helper.Cabprifeccreacion, DbType.DateTime, entity.Cabprifeccreacion);
            dbProvider.AddInParameter(command, helper.Cabpriversion, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.Cabpritieneregistros, DbType.Int32, entity.Cabpritieneregistros);

            dbProvider.ExecuteNonQuery(command);
            return id;

        }

        public void Update(SioCabeceradetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cabpricodi, DbType.Int32, entity.Cabpricodi);
            dbProvider.AddInParameter(command, helper.Tpriecodi, DbType.Int32, entity.Tpriecodi);
            dbProvider.AddInParameter(command, helper.Cabpriperiodo, DbType.DateTime, entity.Cabpriperiodo);
            dbProvider.AddInParameter(command, helper.Cabpriusucreacion, DbType.String, entity.Cabpriusucreacion);
            dbProvider.AddInParameter(command, helper.Cabprifeccreacion, DbType.DateTime, entity.Cabprifeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cabpricodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cabpricodi, DbType.Int32, cabpricodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SioCabeceradetDTO GetById(int cabpricodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cabpricodi, DbType.Int32, cabpricodi);
            SioCabeceradetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SioCabeceradetDTO> List()
        {
            List<SioCabeceradetDTO> entitys = new List<SioCabeceradetDTO>();
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

        public List<SioCabeceradetDTO> GetByCriteria(DateTime fechaProceso, int tpriecodi)
        {
            List<SioCabeceradetDTO> entitys = new List<SioCabeceradetDTO>();
            string query = string.Format(helper.SqlGetByCriteria, fechaProceso.ToString(ConstantesBase.FormatoFecha), tpriecodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SioCabeceradetDTO> GetByCriteriaPeriodo(DateTime fechaProceso)
        {
            List<SioCabeceradetDTO> entitys = new List<SioCabeceradetDTO>();
            string query = string.Format(helper.SqlGetByCriteriaPeriodo, fechaProceso.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public int Save(SioCabeceradetDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                var lastVersion = this.ObtenerUltNroVersion(entity.Cabpriperiodo, entity.Tpriecodi);
                int version = (lastVersion ?? 0) + 1;

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cabpricodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Tpriecodi, DbType.Int32, entity.Tpriecodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cabpriperiodo, DbType.DateTime, entity.Cabpriperiodo));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cabpriusucreacion, DbType.String, entity.Cabpriusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cabprifeccreacion, DbType.DateTime, entity.Cabprifeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cabpriversion, DbType.Int32, version));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cabpritieneregistros, DbType.Int32, entity.Cabpritieneregistros));

                dbCommand.ExecuteNonQuery();
                return id;

            }
        }

        public int? ObtenerUltNroVersion(DateTime periodo, int tpriecodi)
        {
            int? lastVersion = null;
            var query = string.Format(helper.SqlObtenerUltNroVersion, periodo.ToString(ConstantesBase.FormatoFecha), tpriecodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object result = dbProvider.ExecuteScalar(command);
            if (result != null && result != DBNull.Value)
            {
                lastVersion = Convert.ToInt32(result);
            }

            return lastVersion;
        }

        public SioCabeceradetDTO ObtenerUltVersion(DateTime periodo, int tpriecodi)
        {
            var query = string.Format(helper.SqlObtenerUltVersion, periodo.ToString(ConstantesBase.FormatoFecha), tpriecodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            SioCabeceradetDTO entity = null;

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
