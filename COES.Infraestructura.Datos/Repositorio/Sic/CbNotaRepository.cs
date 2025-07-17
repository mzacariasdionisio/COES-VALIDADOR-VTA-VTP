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
    /// Clase de acceso a datos de la tabla CB_NOTA
    /// </summary>
    public class CbNotaRepository: RepositoryBase, ICbNotaRepository
    {
        public CbNotaRepository(string strConn): base(strConn)
        {
        }

        CbNotaHelper helper = new CbNotaHelper();

        public int getIdDisponible()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public void Save(CbNotaDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cbnotacodi, DbType.Int32, entity.Cbnotacodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cbrepcodi, DbType.Int32, entity.Cbrepcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cbnotaitem, DbType.String, entity.Cbnotaitem));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cbnotadescripcion, DbType.String, entity.Cbnotadescripcion));

                dbCommand.ExecuteNonQuery();
            }
        }

        public int Save(CbNotaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cbnotacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cbrepcodi, DbType.Int32, entity.Cbrepcodi);
            dbProvider.AddInParameter(command, helper.Cbnotaitem, DbType.String, entity.Cbnotaitem);
            dbProvider.AddInParameter(command, helper.Cbnotadescripcion, DbType.String, entity.Cbnotadescripcion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CbNotaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            
            dbProvider.AddInParameter(command, helper.Cbrepcodi, DbType.Int32, entity.Cbrepcodi);
            dbProvider.AddInParameter(command, helper.Cbnotaitem, DbType.String, entity.Cbnotaitem);
            dbProvider.AddInParameter(command, helper.Cbnotadescripcion, DbType.String, entity.Cbnotadescripcion);
            dbProvider.AddInParameter(command, helper.Cbnotacodi, DbType.Int32, entity.Cbnotacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cbnotacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cbnotacodi, DbType.Int32, cbnotacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CbNotaDTO GetById(int cbnotacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cbnotacodi, DbType.Int32, cbnotacodi);
            CbNotaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CbNotaDTO> List()
        {
            List<CbNotaDTO> entitys = new List<CbNotaDTO>();
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

        public List<CbNotaDTO> GetByCriteria()
        {
            List<CbNotaDTO> entitys = new List<CbNotaDTO>();
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

        public List<CbNotaDTO> GetByReporte(int cbrepcodi)
        {
            List<CbNotaDTO> entitys = new List<CbNotaDTO>();
            var sqlQuery = string.Format(helper.SqlGetByReporte, cbrepcodi);
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

        public List<CbNotaDTO> GetByTipoReporte(int cbreptipo)
        {
            List<CbNotaDTO> entitys = new List<CbNotaDTO>();
            var sqlQuery = string.Format(helper.SqlGetByTipoReporte, cbreptipo);
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
    }
}
