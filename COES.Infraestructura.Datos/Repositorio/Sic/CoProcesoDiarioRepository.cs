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
    /// Clase de acceso a datos de la tabla CO_PROCESO_DIARIO
    /// </summary>
    public class CoProcesoDiarioRepository: RepositoryBase, ICoProcesoDiarioRepository
    {
        public CoProcesoDiarioRepository(string strConn): base(strConn)
        {
        }

        CoProcesoDiarioHelper helper = new CoProcesoDiarioHelper();

        public int Save(CoProcesoDiarioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Prodiacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Prodiafecha, DbType.DateTime, entity.Prodiafecha);
            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, entity.Copercodi);
            dbProvider.AddInParameter(command, helper.Perprgcodi, DbType.Int32, entity.Perprgcodi);
            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, entity.Covercodi);
            dbProvider.AddInParameter(command, helper.Prodiaindreproceso, DbType.String, entity.Prodiaindreproceso);
            dbProvider.AddInParameter(command, helper.Prodiatipo, DbType.String, entity.Prodiatipo);
            dbProvider.AddInParameter(command, helper.Prodiaestado, DbType.String, entity.Prodiaestado);
            dbProvider.AddInParameter(command, helper.Prodiausucreacion, DbType.String, entity.Prodiausucreacion);
            dbProvider.AddInParameter(command, helper.Prodiafeccreacion, DbType.DateTime, entity.Prodiafeccreacion);
            dbProvider.AddInParameter(command, helper.Prodiausumodificacion, DbType.String, entity.Prodiausumodificacion);
            dbProvider.AddInParameter(command, helper.Prodiafecmodificacion, DbType.DateTime, entity.Prodiafecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int Save(CoProcesoDiarioDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prodiacodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prodiafecha, DbType.DateTime, entity.Prodiafecha));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Copercodi, DbType.Int32, entity.Copercodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Perprgcodi, DbType.Int32, entity.Perprgcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Covercodi, DbType.Int32, entity.Covercodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prodiaindreproceso, DbType.String, entity.Prodiaindreproceso));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prodiatipo, DbType.String, entity.Prodiatipo));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prodiaestado, DbType.String, entity.Prodiaestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prodiausucreacion, DbType.String, entity.Prodiausucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prodiafeccreacion, DbType.DateTime, entity.Prodiafeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prodiausumodificacion, DbType.String, entity.Prodiausumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prodiafecmodificacion, DbType.DateTime, entity.Prodiafecmodificacion));
               

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void Update(CoProcesoDiarioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
                        
            dbProvider.AddInParameter(command, helper.Prodiafecha, DbType.DateTime, entity.Prodiafecha);
            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, entity.Copercodi);
            dbProvider.AddInParameter(command, helper.Perprgcodi, DbType.Int32, entity.Perprgcodi);
            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, entity.Covercodi);
            dbProvider.AddInParameter(command, helper.Prodiaindreproceso, DbType.String, entity.Prodiaindreproceso);
            dbProvider.AddInParameter(command, helper.Prodiatipo, DbType.String, entity.Prodiatipo);
            dbProvider.AddInParameter(command, helper.Prodiaestado, DbType.String, entity.Prodiaestado);
            dbProvider.AddInParameter(command, helper.Prodiausucreacion, DbType.String, entity.Prodiausucreacion);
            dbProvider.AddInParameter(command, helper.Prodiafeccreacion, DbType.DateTime, entity.Prodiafeccreacion);
            dbProvider.AddInParameter(command, helper.Prodiausumodificacion, DbType.String, entity.Prodiausumodificacion);
            dbProvider.AddInParameter(command, helper.Prodiafecmodificacion, DbType.DateTime, entity.Prodiafecmodificacion);
            dbProvider.AddInParameter(command, helper.Prodiacodi, DbType.Int32, entity.Prodiacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int prodiacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Prodiacodi, DbType.Int32, prodiacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CoProcesoDiarioDTO GetById(int prodiacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Prodiacodi, DbType.Int32, prodiacodi);
            CoProcesoDiarioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CoProcesoDiarioDTO> List()
        {
            List<CoProcesoDiarioDTO> entitys = new List<CoProcesoDiarioDTO>();
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

        public List<CoProcesoDiarioDTO> GetByCriteria(string tipo, int copercodi, int covercodi, DateTime fechaInicio, DateTime fechaFin)
        {
            List<CoProcesoDiarioDTO> entitys = new List<CoProcesoDiarioDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, tipo, copercodi, covercodi,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
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

        public List<CoProcesoDiarioDTO> Listar(int periodo, int version)
        {
            List<CoProcesoDiarioDTO> entitys = new List<CoProcesoDiarioDTO>();
            string query = string.Format(helper.SqlListarByPeriodoVersion, periodo, version);
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

        public List<CoProcesoDiarioDTO> ListarByPeriodo(int periodo)
        {
            List<CoProcesoDiarioDTO> entitys = new List<CoProcesoDiarioDTO>();
            string query = string.Format(helper.SqlListarByPeriodo, periodo);
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

        public List<CoProcesoDiarioDTO> ListarPorRangoFechas(string fechaIni, string fechaFin, string prodiatipo)
        {
            List<CoProcesoDiarioDTO> entitys = new List<CoProcesoDiarioDTO>();
            string query = string.Format(helper.SqlListarByRango, fechaIni, fechaFin, prodiatipo);
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
        public void EliminarProcesosDiarios(string listaProdiacodis)
        {
            string query = string.Format(helper.SqlEliminarProcesosDiarios, listaProdiacodis);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);

        }

        public CoProcesoDiarioDTO ObtenerProcesoDiario(DateTime fecha)
        {
            string sql = string.Format(helper.SqlObtenerProcesoDiario, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
          
            CoProcesoDiarioDTO entity = null;

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
