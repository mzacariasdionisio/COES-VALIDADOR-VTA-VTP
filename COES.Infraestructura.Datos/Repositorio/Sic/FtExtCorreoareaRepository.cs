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
    /// Clase de acceso a datos de la tabla FT_EXT_CORREOAREA
    /// </summary>
    public class FtExtCorreoareaRepository : RepositoryBase, IFtExtCorreoareaRepository
    {
        public FtExtCorreoareaRepository(string strConn) : base(strConn)
        {
        }

        FtExtCorreoareaHelper helper = new FtExtCorreoareaHelper();

        public int Save(FtExtCorreoareaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Faremcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Faremfeccreacion, DbType.DateTime, entity.Faremfeccreacion);
            dbProvider.AddInParameter(command, helper.Faremusucreacion, DbType.String, entity.Faremusucreacion);
            dbProvider.AddInParameter(command, helper.Faremfecmodificacion, DbType.DateTime, entity.Faremfecmodificacion);
            dbProvider.AddInParameter(command, helper.Faremusumodificacion, DbType.String, entity.Faremusumodificacion);
            dbProvider.AddInParameter(command, helper.Faremnombre, DbType.String, entity.Faremnombre);
            dbProvider.AddInParameter(command, helper.Faremestado, DbType.String, entity.Faremestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int Save(FtExtCorreoareaDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremfeccreacion, DbType.DateTime, entity.Faremfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremusucreacion, DbType.String, entity.Faremusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremfecmodificacion, DbType.DateTime, entity.Faremfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremusumodificacion, DbType.String, entity.Faremusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremnombre, DbType.String, entity.Faremnombre));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremestado, DbType.String, entity.Faremestado));


                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void Update(FtExtCorreoareaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Faremfeccreacion, DbType.DateTime, entity.Faremfeccreacion);
            dbProvider.AddInParameter(command, helper.Faremusucreacion, DbType.String, entity.Faremusucreacion);
            dbProvider.AddInParameter(command, helper.Faremfecmodificacion, DbType.DateTime, entity.Faremfecmodificacion);
            dbProvider.AddInParameter(command, helper.Faremusumodificacion, DbType.String, entity.Faremusumodificacion);
            dbProvider.AddInParameter(command, helper.Faremnombre, DbType.String, entity.Faremnombre);
            dbProvider.AddInParameter(command, helper.Faremestado, DbType.String, entity.Faremestado);
            dbProvider.AddInParameter(command, helper.Faremcodi, DbType.Int32, entity.Faremcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(FtExtCorreoareaDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlUpdate;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremfeccreacion, DbType.DateTime, entity.Faremfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremusucreacion, DbType.String, entity.Faremusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremfecmodificacion, DbType.DateTime, entity.Faremfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremusumodificacion, DbType.String, entity.Faremusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremnombre, DbType.String, entity.Faremnombre));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremestado, DbType.String, entity.Faremestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremcodi, DbType.Int32, entity.Faremcodi));


                dbCommand.ExecuteNonQuery();
            }
        }

        public void Delete(int faremcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Faremcodi, DbType.Int32, faremcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtCorreoareaDTO GetById(int faremcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Faremcodi, DbType.Int32, faremcodi);
            FtExtCorreoareaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtCorreoareaDTO> List()
        {
            List<FtExtCorreoareaDTO> entitys = new List<FtExtCorreoareaDTO>();
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

        public List<FtExtCorreoareaDTO> GetByCriteria()
        {
            List<FtExtCorreoareaDTO> entitys = new List<FtExtCorreoareaDTO>();
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

        public List<FtExtCorreoareaDTO> ListarPorParametros(string estadoRelacion, string strListaFtitcodis)
        {
            List<FtExtCorreoareaDTO> entitys = new List<FtExtCorreoareaDTO>();
            string sql = string.Format(helper.SqlListarPorParametros, estadoRelacion, strListaFtitcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtCorreoareaDTO entity = helper.Create(dr);

                    int iFtitcodi = dr.GetOrdinal(helper.Ftitcodi);
                    if (!dr.IsDBNull(iFtitcodi)) entity.Ftitcodi = Convert.ToInt32(dr.GetValue(iFtitcodi));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtCorreoareaDTO> ListarPorRequisitos(string estadoRelacion, string strListaFevrqcodis)
        {
            List<FtExtCorreoareaDTO> entitys = new List<FtExtCorreoareaDTO>();
            string sql = string.Format(helper.SqlListarPorRequisitos, estadoRelacion, strListaFevrqcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtCorreoareaDTO entity = helper.Create(dr);

                    int iFevrqcodi = dr.GetOrdinal(helper.Fevrqcodi);
                    if (!dr.IsDBNull(iFevrqcodi)) entity.Fevrqcodi = Convert.ToInt32(dr.GetValue(iFevrqcodi));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        
        public List<FtExtCorreoareaDTO> ListarPorIds(string strFaremcodis)
        {
            List<FtExtCorreoareaDTO> entitys = new List<FtExtCorreoareaDTO>();
            string sql = string.Format(helper.SqlListarPorIds, strFaremcodis);
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
