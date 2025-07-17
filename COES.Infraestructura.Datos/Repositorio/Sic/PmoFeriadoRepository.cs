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
    /// Clase de acceso a datos de la tabla PMO_FERIADO
    /// </summary>
    public class PmoFeriadoRepository : RepositoryBase, IPmoFeriadoRepository
    {
        public PmoFeriadoRepository(string strConn) : base(strConn)
        {
        }

        PmoFeriadoHelper helper = new PmoFeriadoHelper();

        public int Save(PmoFeriadoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pmfrdocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pmanopcodi, DbType.Int32, entity.Pmanopcodi);
            dbProvider.AddInParameter(command, helper.Pmfrdofecha, DbType.DateTime, entity.Pmfrdofecha);
            dbProvider.AddInParameter(command, helper.Pmfrdodescripcion, DbType.String, entity.Pmfrdodescripcion);
            dbProvider.AddInParameter(command, helper.Pmfrdoestado, DbType.Int32, entity.Pmfrdoestado);
            dbProvider.AddInParameter(command, helper.Pmfrdousucreacion, DbType.String, entity.Pmfrdousucreacion);
            dbProvider.AddInParameter(command, helper.Pmfrdofeccreacion, DbType.DateTime, entity.Pmfrdofeccreacion);
            dbProvider.AddInParameter(command, helper.Pmfrdousumodificacion, DbType.String, entity.Pmfrdousumodificacion);
            dbProvider.AddInParameter(command, helper.Pmfrdofecmodificacion, DbType.DateTime, entity.Pmfrdofecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PmoFeriadoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pmfrdocodi, DbType.Int32, entity.Pmfrdocodi);
            dbProvider.AddInParameter(command, helper.Pmanopcodi, DbType.Int32, entity.Pmanopcodi);
            dbProvider.AddInParameter(command, helper.Pmfrdofecha, DbType.DateTime, entity.Pmfrdofecha);
            dbProvider.AddInParameter(command, helper.Pmfrdodescripcion, DbType.String, entity.Pmfrdodescripcion);
            dbProvider.AddInParameter(command, helper.Pmfrdoestado, DbType.Int32, entity.Pmfrdoestado);
            dbProvider.AddInParameter(command, helper.Pmfrdousucreacion, DbType.String, entity.Pmfrdousucreacion);
            dbProvider.AddInParameter(command, helper.Pmfrdofeccreacion, DbType.DateTime, entity.Pmfrdofeccreacion);
            dbProvider.AddInParameter(command, helper.Pmfrdousumodificacion, DbType.String, entity.Pmfrdousumodificacion);
            dbProvider.AddInParameter(command, helper.Pmfrdofecmodificacion, DbType.DateTime, entity.Pmfrdofecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pmfrdocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pmfrdocodi, DbType.Int32, pmfrdocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PmoFeriadoDTO GetById(int pmfrdocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pmfrdocodi, DbType.Int32, pmfrdocodi);
            PmoFeriadoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PmoFeriadoDTO> List()
        {
            List<PmoFeriadoDTO> entitys = new List<PmoFeriadoDTO>();
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

        public List<PmoFeriadoDTO> GetByCriteria(int pmanopcodi)
        {
            List<PmoFeriadoDTO> entitys = new List<PmoFeriadoDTO>();

            string query = string.Format(helper.SqlGetByCriteria, pmanopcodi);
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

        public int Save(PmoFeriadoDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmfrdocodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmanopcodi, DbType.Int32, entity.Pmanopcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmfrdofecha, DbType.DateTime, entity.Pmfrdofecha));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmfrdodescripcion, DbType.String, entity.Pmfrdodescripcion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmfrdoestado, DbType.Int32, entity.Pmfrdoestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmfrdousucreacion, DbType.String, entity.Pmfrdousucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmfrdofeccreacion, DbType.DateTime, entity.Pmfrdofeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmfrdousumodificacion, DbType.String, entity.Pmfrdousumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmfrdofecmodificacion, DbType.DateTime, entity.Pmfrdofecmodificacion));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void UpdateBajaFeriados(PmoFeriadoDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                var query = string.Format(helper.SqlUpdateEstadoBaja, entity.Pmfrdocodi);
                dbCommand.CommandText = query;

                dbCommand.ExecuteNonQuery();
            }
        }

        public void UpdateAprobar(PmoFeriadoDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlUpdateAprobar;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmfrdoestado, DbType.Int32, entity.Pmfrdoestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmfrdousumodificacion, DbType.String, entity.Pmfrdousumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmfrdofecmodificacion, DbType.DateTime, entity.Pmfrdofecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmfrdocodi, DbType.Int32, entity.Pmfrdocodi));

                dbCommand.ExecuteNonQuery();

            }
        }
    }
}
