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
    /// Clase de acceso a datos de la tabla CO_PROCESO_ERROR
    /// </summary>
    public class CoProcesoErrorRepository: RepositoryBase, ICoProcesoErrorRepository
    {
        public CoProcesoErrorRepository(string strConn): base(strConn)
        {
        }

        CoProcesoErrorHelper helper = new CoProcesoErrorHelper();

        public int Save(CoProcesoErrorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Proerrcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Prodiacodi, DbType.Int32, entity.Prodiacodi);
            dbProvider.AddInParameter(command, helper.Proerrmsg, DbType.String, entity.Proerrmsg);
            dbProvider.AddInParameter(command, helper.Proerrtipo, DbType.String, entity.Proerrtipo);
            dbProvider.AddInParameter(command, helper.Proerrusucreacion, DbType.String, entity.Proerrusucreacion);
            dbProvider.AddInParameter(command, helper.Proerrfeccreacion, DbType.DateTime, entity.Proerrfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int Save(CoProcesoErrorDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Proerrcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prodiacodi, DbType.Int32, entity.Prodiacodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Proerrmsg, DbType.String, entity.Proerrmsg));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Proerrtipo, DbType.String, entity.Proerrtipo));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Proerrusucreacion, DbType.String, entity.Proerrusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Proerrfeccreacion, DbType.DateTime, entity.Proerrfeccreacion));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void GrabarDatosXBloques(List<CoProcesoErrorDTO> entitys)
        {
            dbProvider.AddColumnMapping(helper.Proerrcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Prodiacodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Proerrmsg, DbType.String);
            dbProvider.AddColumnMapping(helper.Proerrtipo, DbType.String);
            dbProvider.AddColumnMapping(helper.Proerrusucreacion, DbType.String);
            dbProvider.AddColumnMapping(helper.Proerrfeccreacion, DbType.DateTime);
            

            dbProvider.BulkInsertRSF<CoProcesoErrorDTO>(entitys, helper.TableName);
        }
        public void Update(CoProcesoErrorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Proerrcodi, DbType.Int32, entity.Proerrcodi);
            dbProvider.AddInParameter(command, helper.Prodiacodi, DbType.Int32, entity.Prodiacodi);
            dbProvider.AddInParameter(command, helper.Proerrmsg, DbType.String, entity.Proerrmsg);
            dbProvider.AddInParameter(command, helper.Proerrtipo, DbType.String, entity.Proerrtipo);
            dbProvider.AddInParameter(command, helper.Proerrusucreacion, DbType.String, entity.Proerrusucreacion);
            dbProvider.AddInParameter(command, helper.Proerrfeccreacion, DbType.DateTime, entity.Proerrfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int proerrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Proerrcodi, DbType.Int32, proerrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CoProcesoErrorDTO GetById(int proerrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Proerrcodi, DbType.Int32, proerrcodi);
            CoProcesoErrorDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CoProcesoErrorDTO> List()
        {
            List<CoProcesoErrorDTO> entitys = new List<CoProcesoErrorDTO>();
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

        public List<CoProcesoErrorDTO> GetByCriteria()
        {
            List<CoProcesoErrorDTO> entitys = new List<CoProcesoErrorDTO>();
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

        public List<CoProcesoErrorDTO> ListarTablas(string tablas)
        {
            List<CoProcesoErrorDTO> entitys = new List<CoProcesoErrorDTO>();
            String sql = String.Format(helper.SqlListarTablas, tablas);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {                    
                    CoProcesoErrorDTO entity = new CoProcesoErrorDTO();

                    int iTablanomb = dr.GetOrdinal(this.helper.Tablanomb);
                    if (!dr.IsDBNull(iTablanomb)) entity.Tablanomb = dr.GetString(iTablanomb);

                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public int GetMaximoID()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public void EliminarProcesosError(string listaProdiacodis)
        {
            string query = string.Format(helper.SqlEliminarProcesoError, listaProdiacodis);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);

        }

        public List<CoProcesoErrorDTO> ListarPorDia(int prodiacodi)
        {
            List<CoProcesoErrorDTO> entitys = new List<CoProcesoErrorDTO>();
            String sql = String.Format(helper.SqlListarPorDia, prodiacodi);
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
        
    }
}
