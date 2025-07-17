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
    /// Clase de acceso a datos de la tabla FT_EXT_ENVIO_ARCHIVO
    /// </summary>
    public class FtExtEnvioArchivoRepository : RepositoryBase, IFtExtEnvioArchivoRepository
    {
        public FtExtEnvioArchivoRepository(string strConn) : base(strConn)
        {
        }

        FtExtEnvioArchivoHelper helper = new FtExtEnvioArchivoHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(FtExtEnvioArchivoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftearccodi, DbType.Int32, entity.Ftearccodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftearcnombreoriginal, DbType.String, entity.Ftearcnombreoriginal));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftearcnombrefisico, DbType.String, entity.Ftearcnombrefisico));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftearcorden, DbType.Int32, entity.Ftearcorden));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftearcestado, DbType.Int32, entity.Ftearcestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftearcflagsustentoconf, DbType.String, entity.Ftearcflagsustentoconf));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftearctipo, DbType.Int32, entity.Ftearctipo));

            command.ExecuteNonQuery();
            return entity.Ftearccodi;
        }

        public void Update(FtExtEnvioArchivoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlUpdate;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftearcestado, DbType.Int32, entity.Ftearcestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftearcflagsustentoconf, DbType.String, entity.Ftearcflagsustentoconf));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftearccodi, DbType.Int32, entity.Ftearccodi));

            command.ExecuteNonQuery();
        }

        public void Delete(int ftearccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ftearccodi, DbType.Int32, ftearccodi);

            dbProvider.ExecuteNonQuery(command);
        }
        
        public void DeletePorIds(string ftearccodis, IDbConnection connection, DbTransaction transaction)
        {
            string sql = string.Format(helper.SqlDeletePorIds, ftearccodis);            
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = sql;

                dbCommand.ExecuteNonQuery();
            }
            
        }

        public List<FtExtEnvioArchivoDTO> ListarRelacionesPorVersionAreaYEquipo(int ftevercodi, int faremcodi, int fteeqcodi)
        {
            List<FtExtEnvioArchivoDTO> entitys = new List<FtExtEnvioArchivoDTO>();

            string sql = string.Format(helper.SqlListarRelacionesPorVersionAreaYEquipo, ftevercodi, faremcodi, fteeqcodi);
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

        public List<FtExtEnvioArchivoDTO> ListarRelacionesContenidoPorVersionArea(int ftevercodi, int faremcodi)
        {
            List<FtExtEnvioArchivoDTO> entitys = new List<FtExtEnvioArchivoDTO>();

            string sql = string.Format(helper.SqlListarRelacionesContenidoPorVersionArea, ftevercodi, faremcodi);
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
        

        public FtExtEnvioArchivoDTO GetById(int ftearccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ftearccodi, DbType.Int32, ftearccodi);
            FtExtEnvioArchivoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtEnvioArchivoDTO> List()
        {
            List<FtExtEnvioArchivoDTO> entitys = new List<FtExtEnvioArchivoDTO>();
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

        public List<FtExtEnvioArchivoDTO> ListByVersionYReq(int ftevercodi)
        {
            List<FtExtEnvioArchivoDTO> entitys = new List<FtExtEnvioArchivoDTO>();

            string sql = string.Format(helper.SqlListByVersionYReq, ftevercodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iFtereqcodi = dr.GetOrdinal(helper.Ftereqcodi);
                    if (!dr.IsDBNull(iFtereqcodi)) entity.Ftereqcodi = Convert.ToInt32(dr.GetValue(iFtereqcodi));

                    int iFevrqcodi = dr.GetOrdinal(helper.Fevrqcodi);
                    if (!dr.IsDBNull(iFevrqcodi)) entity.Fevrqcodi = Convert.ToInt32(dr.GetValue(iFevrqcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtEnvioArchivoDTO> ListByVersionYEq(int ftevercodi)
        {
            List<FtExtEnvioArchivoDTO> entitys = new List<FtExtEnvioArchivoDTO>();

            string sql = string.Format(helper.SqlListByVersionYEq, ftevercodi);
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

        public List<FtExtEnvioArchivoDTO> ListByVersionYDato(int ftevercodi)
        {
            List<FtExtEnvioArchivoDTO> entitys = new List<FtExtEnvioArchivoDTO>();

            string sql = string.Format(helper.SqlListByVersionYDato, ftevercodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iFterdacodi = dr.GetOrdinal(helper.Fterdacodi);
                    if (!dr.IsDBNull(iFterdacodi)) entity.Fterdacodi = Convert.ToInt32(dr.GetValue(iFterdacodi));

                    int iFtedatcodi = dr.GetOrdinal(helper.Ftedatcodi);
                    if (!dr.IsDBNull(iFtedatcodi)) entity.Ftedatcodi = Convert.ToInt32(dr.GetValue(iFtedatcodi));

                    int iFteeqcodi = dr.GetOrdinal(helper.Fteeqcodi);
                    if (!dr.IsDBNull(iFteeqcodi)) entity.Fteeqcodi = Convert.ToInt32(dr.GetValue(iFteeqcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtEnvioArchivoDTO> GetByCriteria()
        {
            List<FtExtEnvioArchivoDTO> entitys = new List<FtExtEnvioArchivoDTO>();
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

        public List<FtExtEnvioArchivoDTO> ListarPorIds(string ftearccodis)
        {
            List<FtExtEnvioArchivoDTO> entitys = new List<FtExtEnvioArchivoDTO>();

            string sql = string.Format(helper.SqlListarPorIds, ftearccodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);                    

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtEnvioArchivoDTO> ListByRevision(string ftrevcodis)
        {
            List<FtExtEnvioArchivoDTO> entitys = new List<FtExtEnvioArchivoDTO>();

            string sql = string.Format(helper.SqlListByRevision, ftrevcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iFtrevcodi = dr.GetOrdinal(helper.Ftrevcodi);
                    if (!dr.IsDBNull(iFtrevcodi)) entity.Ftrevcodi = Convert.ToInt32(dr.GetValue(iFtrevcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtEnvioArchivoDTO> ListByRevisionAreas(string revacodis)
        {
            List<FtExtEnvioArchivoDTO> entitys = new List<FtExtEnvioArchivoDTO>();

            string sql = string.Format(helper.SqlListByRevisionAreas, revacodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iRevacodi = dr.GetOrdinal(helper.Revacodi);
                    if (!dr.IsDBNull(iRevacodi)) entity.Revacodi = Convert.ToInt32(dr.GetValue(iRevacodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtEnvioArchivoDTO> ListByVersionAreas(string ftvercodis)
        {
            List<FtExtEnvioArchivoDTO> entitys = new List<FtExtEnvioArchivoDTO>();

            string sql = string.Format(helper.SqlListByVersionAreas, ftvercodis);
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
