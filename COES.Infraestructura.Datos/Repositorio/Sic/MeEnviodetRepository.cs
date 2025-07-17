using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla ME_ENVIODET
    /// </summary>
    public class MeEnviodetRepository : RepositoryBase, IMeEnviodetRepository
    {
        private string strConexion;
        public MeEnviodetRepository(string strConn) : base(strConn)
        {
            strConexion = strConn;
        }
        
        MeEnviodetHelper helper = new MeEnviodetHelper();
        MeEnviodetmensajeHelper helperEnvioMensajeDet = new MeEnviodetmensajeHelper();

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
        
        public int Save(MeEnviodetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Envdetcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, entity.Enviocodi);
            dbProvider.AddInParameter(command, helper.Envdetfpkcodi, DbType.Int32, entity.Envdetfpkcodi);
            dbProvider.AddInParameter(command, helper.Envdetusucreacion, DbType.String, entity.Envdetusucreacion);
            dbProvider.AddInParameter(command, helper.Envdetfeccreacion, DbType.DateTime, entity.Envdetfeccreacion);
            dbProvider.AddInParameter(command, helper.Envdetusumodificacion, DbType.String, entity.Envdetusumodificacion);
            dbProvider.AddInParameter(command, helper.Envdetfecmodificacion, DbType.DateTime, entity.Envdetfecmodificacion);

            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public void Update(MeEnviodetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Envdetcodi, DbType.Int32, entity.Envdetcodi);
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, entity.Enviocodi);
            dbProvider.AddInParameter(command, helper.Envdetfpkcodi, DbType.Int32, entity.Envdetfpkcodi);
            dbProvider.AddInParameter(command, helper.Envdetusucreacion, DbType.String, entity.Envdetusucreacion);
            dbProvider.AddInParameter(command, helper.Envdetfeccreacion, DbType.DateTime, entity.Envdetfeccreacion);
            dbProvider.AddInParameter(command, helper.Envdetusumodificacion, DbType.String, entity.Envdetusumodificacion);
            dbProvider.AddInParameter(command, helper.Envdetfecmodificacion, DbType.DateTime, entity.Envdetfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int enviocodi, int fdatpkcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, enviocodi);
            dbProvider.AddInParameter(command, helper.Envdetfpkcodi, DbType.Int32, fdatpkcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeEnviodetDTO GetById(int enviocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, enviocodi);
            MeEnviodetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeEnviodetDTO> List()
        {
            List<MeEnviodetDTO> entitys = new List<MeEnviodetDTO>();
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

        public List<MeEnviodetDTO> GetByCriteria(int enviocodi)
        {
            List<MeEnviodetDTO> entitys = new List<MeEnviodetDTO>();
            string query = string.Format(helper.SqlGetByCriteria, enviocodi);
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

        #region INTERVENCIONES

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public void Save(MeEnviodetDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand commandTbEnvioDet = (DbCommand)conn.CreateCommand();
            commandTbEnvioDet.CommandText = helper.SqlInsertarDetalleEnvio;
            commandTbEnvioDet.Transaction = tran;
            commandTbEnvioDet.Connection = (DbConnection)conn;

            commandTbEnvioDet.Parameters.Add(dbProvider.CreateParameter(commandTbEnvioDet, helper.Enviocodi, DbType.Int32, entity.Enviocodi));
            commandTbEnvioDet.Parameters.Add(dbProvider.CreateParameter(commandTbEnvioDet, helper.Envdetfpkcodi, DbType.Int32, entity.Envdetfpkcodi));
            commandTbEnvioDet.Parameters.Add(dbProvider.CreateParameter(commandTbEnvioDet, helper.Envdetusucreacion, DbType.String, entity.Envdetusucreacion));
            commandTbEnvioDet.Parameters.Add(dbProvider.CreateParameter(commandTbEnvioDet, helper.Envdetfeccreacion, DbType.DateTime, entity.Envdetfeccreacion));
            commandTbEnvioDet.Parameters.Add(dbProvider.CreateParameter(commandTbEnvioDet, helper.Envdetusumodificacion, DbType.String, null));
            commandTbEnvioDet.Parameters.Add(dbProvider.CreateParameter(commandTbEnvioDet, helper.Envdetfecmodificacion, DbType.DateTime, null));
            commandTbEnvioDet.Parameters.Add(dbProvider.CreateParameter(commandTbEnvioDet, helper.Envdetcodi, DbType.Int32, entity.Envdetcodi));
            commandTbEnvioDet.ExecuteNonQuery();
        }

        public MeEnviodetDTO GetByInterCodi(int intercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerDetalleEnvioPorIntervencionId);

            dbProvider.AddInParameter(command, helper.Envdetfpkcodi, DbType.Int32, intercodi);
            MeEnviodetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }
            return entity;
        }

        public void EliminarFisicoPorIntervencionId(int fdatpkcodi, IDbConnection conn, DbTransaction tran)
        {
            DbCommand commandEliminarEnvioDetalle = (DbCommand)conn.CreateCommand();
            commandEliminarEnvioDetalle.CommandText = helper.SqlEliminarDetalleEnvioFisicoPorIntervencionId;
            commandEliminarEnvioDetalle.Transaction = tran;
            commandEliminarEnvioDetalle.Connection = (DbConnection)conn;
            commandEliminarEnvioDetalle.Parameters.Add(dbProvider.CreateParameter(commandEliminarEnvioDetalle, helper.Envdetfpkcodi, DbType.Int32, fdatpkcodi));
            commandEliminarEnvioDetalle.ExecuteNonQuery();
        }

        public int ObtenerEnvDetCodi(int IdIntervencion)
        {
            int iEnvdetcodi = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEnvDetCodi);

            dbProvider.AddInParameter(command, helper.Envdetfpkcodi, DbType.Int32, IdIntervencion);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    iEnvdetcodi = Convert.ToInt32(dr["EnvDetCodi"].ToString().Trim());
                }
            }

            return iEnvdetcodi;
        }

        public string ObtenerMsgCodi(int id)
        {
            string MsgCodi = "";

            DbCommand command = dbProvider.GetSqlStringCommand(helperEnvioMensajeDet.SqlObtenerMsgCodi);
            dbProvider.AddInParameter(command, helperEnvioMensajeDet.Envdetcodi, DbType.Int32, ObtenerEnvDetCodi(id));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MsgCodi += (Convert.ToInt32(dr["Msgcodi"].ToString().Trim())).ToString() + ",";
                }
            }

            return MsgCodi.TrimEnd(',');
        }

        #endregion
    }
}
