using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla PMO_ANIO_OPERATIVO
    /// </summary>
    public class PmoAnioOperativoRepository : RepositoryBase, IPmoAnioOperativoRepository
    {
        public PmoAnioOperativoRepository(string strConn) : base(strConn)
        {
        }

        PmoAnioOperativoHelper helper = new PmoAnioOperativoHelper();

        public int Save(PmoAnioOperativoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pmanopcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pmanopanio, DbType.Int32, entity.Pmanopanio);
            dbProvider.AddInParameter(command, helper.Pmanopfecini, DbType.DateTime, entity.Pmanopfecini);
            dbProvider.AddInParameter(command, helper.Pmanopfecfin, DbType.DateTime, entity.Pmanopfecfin);
            dbProvider.AddInParameter(command, helper.Pmanopestado, DbType.Int32, entity.Pmanopestado);
            dbProvider.AddInParameter(command, helper.Pmanopnumversion, DbType.Int32, entity.Pmanopnumversion);
            dbProvider.AddInParameter(command, helper.Pmanopusucreacion, DbType.String, entity.Pmanopusucreacion);
            dbProvider.AddInParameter(command, helper.Pmanopfeccreacion, DbType.DateTime, entity.Pmanopfeccreacion);
            dbProvider.AddInParameter(command, helper.Pmanopusumodificacion, DbType.String, entity.Pmanopusumodificacion);
            dbProvider.AddInParameter(command, helper.Pmanopfecmodificacion, DbType.DateTime, entity.Pmanopfecmodificacion);
            dbProvider.AddInParameter(command, helper.Pmanopdesc, DbType.String, entity.Pmanopdesc);
            dbProvider.AddInParameter(command, helper.Pmanopprocesado, DbType.Int32, entity.Pmanopprocesado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PmoAnioOperativoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pmanopanio, DbType.Int32, entity.Pmanopanio);
            dbProvider.AddInParameter(command, helper.Pmanopfecini, DbType.DateTime, entity.Pmanopfecini);
            dbProvider.AddInParameter(command, helper.Pmanopfecfin, DbType.DateTime, entity.Pmanopfecfin);
            dbProvider.AddInParameter(command, helper.Pmanopestado, DbType.Int32, entity.Pmanopestado);
            dbProvider.AddInParameter(command, helper.Pmanopnumversion, DbType.Int32, entity.Pmanopnumversion);
            dbProvider.AddInParameter(command, helper.Pmanopusucreacion, DbType.String, entity.Pmanopusucreacion);
            dbProvider.AddInParameter(command, helper.Pmanopfeccreacion, DbType.DateTime, entity.Pmanopfeccreacion);
            dbProvider.AddInParameter(command, helper.Pmanopusumodificacion, DbType.String, entity.Pmanopusumodificacion);
            dbProvider.AddInParameter(command, helper.Pmanopfecmodificacion, DbType.DateTime, entity.Pmanopfecmodificacion);
            dbProvider.AddInParameter(command, helper.Pmanopdesc, DbType.String, entity.Pmanopdesc);
            dbProvider.AddInParameter(command, helper.Pmanopprocesado, DbType.Int32, entity.Pmanopprocesado);
            dbProvider.AddInParameter(command, helper.Pmanopcodi, DbType.Int32, entity.Pmanopcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pmanopcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pmanopcodi, DbType.Int32, pmanopcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PmoAnioOperativoDTO GetById(int pmanopcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pmanopcodi, DbType.Int32, pmanopcodi);
            PmoAnioOperativoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PmoAnioOperativoDTO> List()
        {
            List<PmoAnioOperativoDTO> entitys = new List<PmoAnioOperativoDTO>();
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

        public List<PmoAnioOperativoDTO> GetByCriteria(string anio)
        {
            List<PmoAnioOperativoDTO> entitys = new List<PmoAnioOperativoDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, anio);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iNumVersiones = dr.GetOrdinal(helper.NumVersiones);
                    if (!dr.IsDBNull(iNumVersiones)) entity.NumVersiones = Convert.ToInt32(dr.GetValue(iNumVersiones));

                    int iPmanopNumFeriados = dr.GetOrdinal(helper.PmanopNumFeriados);
                    if (!dr.IsDBNull(iNumVersiones)) entity.PmanopNumFeriados = Convert.ToInt32(dr.GetValue(iPmanopNumFeriados)).ToString();

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int Save(PmoAnioOperativoDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmanopcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmanopanio, DbType.Int32, entity.Pmanopanio));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmanopfecini, DbType.DateTime, entity.Pmanopfecini));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmanopfecfin, DbType.DateTime, entity.Pmanopfecfin));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmanopestado, DbType.Int32, entity.Pmanopestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmanopnumversion, DbType.Int32, entity.Pmanopnumversion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmanopusucreacion, DbType.String, entity.Pmanopusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmanopfeccreacion, DbType.DateTime, entity.Pmanopfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmanopusumodificacion, DbType.String, entity.Pmanopusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmanopfecmodificacion, DbType.DateTime, entity.Pmanopfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmanopdesc, DbType.String, entity.Pmanopdesc));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmanopprocesado, DbType.Int32, entity.Pmanopprocesado));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void UpdateBajaAnioOperativo(PmoAnioOperativoDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                var query = string.Format(helper.SqlUpdateEstadoBaja, entity.Pmanopcodi);
                dbCommand.CommandText = query;

                dbCommand.ExecuteNonQuery();
            }
        }

        public void UpdateAprobar(PmoAnioOperativoDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlUpdateAprobar;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmanopestado, DbType.Int32, entity.Pmanopestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmanopusumodificacion, DbType.String, entity.Pmanopusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmanopfecmodificacion, DbType.DateTime, entity.Pmanopfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmanopcodi, DbType.Int32, entity.Pmanopcodi));

                dbCommand.ExecuteNonQuery();

            }
        }

        public void UpdateEstadoProcesado(int anio, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                var query = string.Format(helper.SqlUpdateEstadoProcesado, anio);
                dbCommand.CommandText = query;

                dbCommand.ExecuteNonQuery();
            }
        }
    }
}
