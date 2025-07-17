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
    /// Clase de acceso a datos de la tabla PMO_QN_CAMBIOENVIO
    /// </summary>
    public class PmoQnCambioenvioRepository : RepositoryBase, IPmoQnCambioenvioRepository
    {
        public PmoQnCambioenvioRepository(string strConn) : base(strConn)
        {
        }

        PmoQnCambioenvioHelper helper = new PmoQnCambioenvioHelper();

        public int Save(PmoQnCambioenvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Qncmbecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Qnbenvcodi, DbType.Int32, entity.Qnbenvcodi);
            dbProvider.AddInParameter(command, helper.Sddpcodi, DbType.Int32, entity.Sddpcodi);
            dbProvider.AddInParameter(command, helper.Qncmbefecha, DbType.DateTime, entity.Qncmbefecha);
            dbProvider.AddInParameter(command, helper.Qncmbedatos, DbType.String, entity.Qncmbedatos);
            dbProvider.AddInParameter(command, helper.Qncmbecolvar, DbType.String, entity.Qncmbecolvar);
            dbProvider.AddInParameter(command, helper.Qncmbeusucreacion, DbType.String, entity.Qncmbeusucreacion);
            dbProvider.AddInParameter(command, helper.Qncmbefeccreacion, DbType.DateTime, entity.Qncmbefeccreacion);
            dbProvider.AddInParameter(command, helper.Qncmbeorigen, DbType.String, entity.Qncmbeorigen);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PmoQnCambioenvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Qnbenvcodi, DbType.Int32, entity.Qnbenvcodi);
            dbProvider.AddInParameter(command, helper.Sddpcodi, DbType.Int32, entity.Sddpcodi);
            dbProvider.AddInParameter(command, helper.Qncmbefecha, DbType.DateTime, entity.Qncmbefecha);
            dbProvider.AddInParameter(command, helper.Qncmbedatos, DbType.String, entity.Qncmbedatos);
            dbProvider.AddInParameter(command, helper.Qncmbecolvar, DbType.String, entity.Qncmbecolvar);
            dbProvider.AddInParameter(command, helper.Qncmbeusucreacion, DbType.String, entity.Qncmbeusucreacion);
            dbProvider.AddInParameter(command, helper.Qncmbefeccreacion, DbType.DateTime, entity.Qncmbefeccreacion);
            dbProvider.AddInParameter(command, helper.Qncmbeorigen, DbType.String, entity.Qncmbeorigen);
            dbProvider.AddInParameter(command, helper.Qncmbecodi, DbType.Int32, entity.Qncmbecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int qncmbecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Qncmbecodi, DbType.Int32, qncmbecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PmoQnCambioenvioDTO GetById(int qncmbecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Qncmbecodi, DbType.Int32, qncmbecodi);
            PmoQnCambioenvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PmoQnCambioenvioDTO> List()
        {
            List<PmoQnCambioenvioDTO> entitys = new List<PmoQnCambioenvioDTO>();
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

        public List<PmoQnCambioenvioDTO> GetByCriteria()
        {
            List<PmoQnCambioenvioDTO> entitys = new List<PmoQnCambioenvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<PmoQnCambioenvioDTO> ListByEnvio(string enviocodis)
        {
            string sqlQuery = string.Format(helper.SqlListByEnvio, enviocodis);
            List<PmoQnCambioenvioDTO> entitys = new List<PmoQnCambioenvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public int Save(PmoQnCambioenvioDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qncmbecodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnbenvcodi, DbType.Int32, entity.Qnbenvcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Sddpcodi, DbType.Int32, entity.Sddpcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qncmbefecha, DbType.DateTime, entity.Qncmbefecha));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qncmbedatos, DbType.String, entity.Qncmbedatos));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qncmbecolvar, DbType.String, entity.Qncmbecolvar));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qncmbeusucreacion, DbType.String, entity.Qncmbeusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qncmbefeccreacion, DbType.DateTime, entity.Qncmbefeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qncmbeorigen, DbType.String, entity.Qncmbeorigen));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }
    }
}
