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
    /// Clase de acceso a datos de la tabla PMO_PTOXESTACION
    /// </summary>
    public class PmoPtoxestacionRepository : RepositoryBase, IPmoPtoxestacionRepository
    {
        public PmoPtoxestacionRepository(string strConn) : base(strConn)
        {
        }

        PmoPtoxestacionHelper helper = new PmoPtoxestacionHelper();

        public int Save(PmoPtoxestacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pmpxehcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pmehcodi, DbType.Int32, entity.Pmehcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Pmpxehestado, DbType.String, entity.Pmpxehestado);
            dbProvider.AddInParameter(command, helper.Pmpxehfactor, DbType.Decimal, entity.Pmpxehfactor);
            dbProvider.AddInParameter(command, helper.Pmpxehusucreacion, DbType.String, entity.Pmpxehusucreacion);
            dbProvider.AddInParameter(command, helper.Pmpxehfeccreacion, DbType.DateTime, entity.Pmpxehfeccreacion);
            dbProvider.AddInParameter(command, helper.Pmpxehusumodificacion, DbType.String, entity.Pmpxehusumodificacion);
            dbProvider.AddInParameter(command, helper.Pmpxehfecmodificacion, DbType.DateTime, entity.Pmpxehfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PmoPtoxestacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pmehcodi, DbType.Int32, entity.Pmehcodi);
            dbProvider.AddInParameter(command, helper.Pmpxehcodi, DbType.Int32, entity.Pmpxehcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Pmpxehestado, DbType.String, entity.Pmpxehestado);
            dbProvider.AddInParameter(command, helper.Pmpxehfactor, DbType.Decimal, entity.Pmpxehfactor);
            dbProvider.AddInParameter(command, helper.Pmpxehusucreacion, DbType.String, entity.Pmpxehusucreacion);
            dbProvider.AddInParameter(command, helper.Pmpxehfeccreacion, DbType.DateTime, entity.Pmpxehfeccreacion);
            dbProvider.AddInParameter(command, helper.Pmpxehusumodificacion, DbType.String, entity.Pmpxehusumodificacion);
            dbProvider.AddInParameter(command, helper.Pmpxehfecmodificacion, DbType.DateTime, entity.Pmpxehfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pmpxehcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pmpxehcodi, DbType.Int32, pmpxehcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PmoPtoxestacionDTO GetById(int pmpxehcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pmpxehcodi, DbType.Int32, pmpxehcodi);
            PmoPtoxestacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PmoPtoxestacionDTO> List()
        {
            List<PmoPtoxestacionDTO> entitys = new List<PmoPtoxestacionDTO>();
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

        public List<PmoPtoxestacionDTO> GetByCriteria(int pmehcodi)
        {
            List<PmoPtoxestacionDTO> entitys = new List<PmoPtoxestacionDTO>();
            string query = string.Format(helper.SqlGetByCriteria, pmehcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            PmoPtoxestacionDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    //entitys.Add(helper.Create(dr));
                    entity = helper.Create(dr);

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int Save(PmoPtoxestacionDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmehcodi, DbType.Int32, entity.Pmehcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmpxehcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmpxehestado, DbType.String, entity.Pmpxehestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmpxehfactor, DbType.Decimal, entity.Pmpxehfactor));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmpxehusucreacion, DbType.String, entity.Pmpxehusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmpxehfeccreacion, DbType.DateTime, entity.Pmpxehfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmpxehusumodificacion, DbType.String, entity.Pmpxehusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmpxehfecmodificacion, DbType.DateTime, entity.Pmpxehfecmodificacion));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        /// <summary>
        /// Actualizar estado de Ptoxestacion
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public void UpdateEstadoPtoxestacion(PmoPtoxestacionDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                var query = string.Format(helper.SqlUpdateEstado, entity.Pmpxehestado, entity.Pmpxehcodi);
                dbCommand.CommandText = query;

                dbCommand.ExecuteNonQuery();
            }
        }
    }
}
