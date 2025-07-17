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
    /// Clase de acceso a datos de la tabla SMA_ACTIVACION_MOTIVO
    /// </summary>
    public class SmaActivacionMotivoRepository: RepositoryBase, ISmaActivacionMotivoRepository
    {
        public SmaActivacionMotivoRepository(string strConn): base(strConn)
        {
        }

        SmaActivacionMotivoHelper helper = new SmaActivacionMotivoHelper();

        public int Save(SmaActivacionMotivoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Smaacmcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Smapaccodi, DbType.Int32, entity.Smapaccodi);
            dbProvider.AddInParameter(command, helper.Smammcodi, DbType.Int32, entity.Smammcodi);
            dbProvider.AddInParameter(command, helper.Smaacmtiporeserva, DbType.String, entity.Smaacmtiporeserva);
            dbProvider.AddInParameter(command, helper.Smaacmusucreacion, DbType.String, entity.Smaacmusucreacion);
            dbProvider.AddInParameter(command, helper.Smaacmfeccreacion, DbType.DateTime, entity.Smaacmfeccreacion);
            dbProvider.AddInParameter(command, helper.Smaacmusumodificacion, DbType.String, entity.Smaacmusumodificacion);
            dbProvider.AddInParameter(command, helper.Smaacmfecmodificacion, DbType.DateTime, entity.Smaacmfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int SaveTransaccional(SmaActivacionMotivoDTO entity, IDbConnection connection, DbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Smaacmcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Smapaccodi, DbType.Int32, entity.Smapaccodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Smammcodi, DbType.Int32, entity.Smammcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Smaacmtiporeserva, DbType.String, entity.Smaacmtiporeserva));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Smaacmusucreacion, DbType.String, entity.Smaacmusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Smaacmfeccreacion, DbType.DateTime, entity.Smaacmfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Smaacmusumodificacion, DbType.String, entity.Smaacmusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Smaacmfecmodificacion, DbType.DateTime, entity.Smaacmfecmodificacion));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void Update(SmaActivacionMotivoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Smaacmcodi, DbType.Int32, entity.Smaacmcodi);
            dbProvider.AddInParameter(command, helper.Smapaccodi, DbType.Int32, entity.Smapaccodi);
            dbProvider.AddInParameter(command, helper.Smammcodi, DbType.Int32, entity.Smammcodi);
            dbProvider.AddInParameter(command, helper.Smaacmtiporeserva, DbType.String, entity.Smaacmtiporeserva);
            dbProvider.AddInParameter(command, helper.Smaacmusucreacion, DbType.String, entity.Smaacmusucreacion);
            dbProvider.AddInParameter(command, helper.Smaacmfeccreacion, DbType.DateTime, entity.Smaacmfeccreacion);
            dbProvider.AddInParameter(command, helper.Smaacmusumodificacion, DbType.String, entity.Smaacmusumodificacion);
            dbProvider.AddInParameter(command, helper.Smaacmfecmodificacion, DbType.DateTime, entity.Smaacmfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int smaacmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Smaacmcodi, DbType.Int32, smaacmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SmaActivacionMotivoDTO GetById(int smaacmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Smaacmcodi, DbType.Int32, smaacmcodi);
            SmaActivacionMotivoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SmaActivacionMotivoDTO> List()
        {
            List<SmaActivacionMotivoDTO> entitys = new List<SmaActivacionMotivoDTO>();
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

        public List<SmaActivacionMotivoDTO> GetByCriteria()
        {
            List<SmaActivacionMotivoDTO> entitys = new List<SmaActivacionMotivoDTO>();
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

        public List<SmaActivacionMotivoDTO> ObtenerPorActivacionesOferta(string smapaccodis)
        {
            List<SmaActivacionMotivoDTO> entitys = new List<SmaActivacionMotivoDTO>();

            string query = string.Format(helper.SqlObtenerPorActivacionesOferta, smapaccodis);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SmaActivacionMotivoDTO entity = helper.Create(dr);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
