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
    /// Clase de acceso a datos de la tabla PMO_ESTACIONH
    /// </summary>
    public class PmoEstacionhRepository : RepositoryBase, IPmoEstacionhRepository
    {
        public PmoEstacionhRepository(string strConn) : base(strConn)
        {
        }

        PmoEstacionhHelper helper = new PmoEstacionhHelper();

        public int Save(PmoEstacionhDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pmehcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pmehdesc, DbType.String, entity.Pmehdesc);
            dbProvider.AddInParameter(command, helper.Sddpcodi, DbType.Int32, entity.Sddpcodi);
            dbProvider.AddInParameter(command, helper.Pmehreferencia, DbType.String, entity.Pmehreferencia);
            dbProvider.AddInParameter(command, helper.Pmehorden, DbType.Int32, entity.Pmehorden);
            dbProvider.AddInParameter(command, helper.Pmehusucreacion, DbType.String, entity.Pmehusucreacion);
            dbProvider.AddInParameter(command, helper.Pmehfeccreacion, DbType.DateTime, entity.Pmehfeccreacion);
            dbProvider.AddInParameter(command, helper.Pmehusumodificacion, DbType.String, entity.Pmehusumodificacion);
            dbProvider.AddInParameter(command, helper.Pmehfecmodificacion, DbType.DateTime, entity.Pmehfecmodificacion);
            dbProvider.AddInParameter(command, helper.Pmehestado, DbType.String, entity.Pmehestado);
            dbProvider.AddInParameter(command, helper.Pmehnumversion, DbType.Int32, entity.Pmehnumversion);
            dbProvider.AddInParameter(command, helper.Pmehintegrante, DbType.String, entity.Pmehintegrante);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PmoEstacionhDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pmehdesc, DbType.String, entity.Pmehdesc);
            dbProvider.AddInParameter(command, helper.Sddpcodi, DbType.Int32, entity.Sddpcodi);
            dbProvider.AddInParameter(command, helper.Pmehreferencia, DbType.String, entity.Pmehreferencia);
            dbProvider.AddInParameter(command, helper.Pmehorden, DbType.Int32, entity.Pmehorden);
            dbProvider.AddInParameter(command, helper.Pmehusucreacion, DbType.String, entity.Pmehusucreacion);
            dbProvider.AddInParameter(command, helper.Pmehfeccreacion, DbType.DateTime, entity.Pmehfeccreacion);
            dbProvider.AddInParameter(command, helper.Pmehusumodificacion, DbType.String, entity.Pmehusumodificacion);
            dbProvider.AddInParameter(command, helper.Pmehfecmodificacion, DbType.DateTime, entity.Pmehfecmodificacion);
            dbProvider.AddInParameter(command, helper.Pmehestado, DbType.String, entity.Pmehestado);
            dbProvider.AddInParameter(command, helper.Pmehnumversion, DbType.Int32, entity.Pmehnumversion);
            dbProvider.AddInParameter(command, helper.Pmehintegrante, DbType.String, entity.Pmehintegrante);
            dbProvider.AddInParameter(command, helper.Pmehcodi, DbType.Int32, entity.Pmehcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pmehcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pmehcodi, DbType.Int32, pmehcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PmoEstacionhDTO GetById(int pmehcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pmehcodi, DbType.Int32, pmehcodi);
            PmoEstacionhDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PmoEstacionhDTO> List()
        {
            List<PmoEstacionhDTO> entitys = new List<PmoEstacionhDTO>();
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

        public List<PmoEstacionhDTO> GetByCriteria(int ptomedicodi)
        {

            List<PmoEstacionhDTO> entitys = new List<PmoEstacionhDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, ptomedicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        /// <summary>
        /// Guardado transaccional
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int Save(PmoEstacionhDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmehcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmehdesc, DbType.String, entity.Pmehdesc));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Sddpcodi, DbType.Int32, entity.Sddpcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmehreferencia, DbType.String, entity.Pmehreferencia));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmehorden, DbType.Int32, entity.Pmehorden));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmehusucreacion, DbType.String, entity.Pmehusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmehfeccreacion, DbType.DateTime, entity.Pmehfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmehusumodificacion, DbType.String, entity.Pmehusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmehfecmodificacion, DbType.DateTime, entity.Pmehfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmehestado, DbType.String, entity.Pmehestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmehnumversion, DbType.Int32, entity.Pmehnumversion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmehintegrante, DbType.String, entity.Pmehintegrante));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        /// <summary>
        /// Actualizar el estado de la estación Hidrológica
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public void UpdateEstadoEstacionHidro(PmoEstacionhDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                var query = string.Format(helper.SqlUpdateEstado, entity.Pmehestado, entity.Pmehcodi);
                dbCommand.CommandText = query;

                dbCommand.ExecuteNonQuery();
            }
        }

        public void UpdateOrdenEstacionHidro(int orden, int sddpCodi)
        {
            string sql = string.Format(helper.SqlUpdateOrden, orden, sddpCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            dbProvider.ExecuteNonQuery(command);
            //using (var dbCommand = (DbCommand)connection.CreateCommand())
            //{
            //    dbCommand.Transaction = (DbTransaction)transaction;
            //    dbCommand.Connection = (DbConnection)connection;

            //    var query = string.Format(helper.SqlUpdateOrden, orden, sddpCodi);
            //    dbCommand.CommandText = query;

            //    dbCommand.ExecuteNonQuery();
            //}
        }
    }
}
