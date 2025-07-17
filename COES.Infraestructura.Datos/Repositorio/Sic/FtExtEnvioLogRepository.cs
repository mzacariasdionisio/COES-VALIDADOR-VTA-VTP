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
    /// Clase de acceso a datos de la tabla FT_EXT_ENVIO_LOG
    /// </summary>
    public class FtExtEnvioLogRepository : RepositoryBase, IFtExtEnvioLogRepository
    {
        public FtExtEnvioLogRepository(string strConn) : base(strConn)
        {
        }

        FtExtEnvioLogHelper helper = new FtExtEnvioLogHelper();

        public int Save(FtExtEnvioLogDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ftelogcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ftelogusucreacion, DbType.String, entity.Ftelogusucreacion);
            dbProvider.AddInParameter(command, helper.Ftelogfeccreacion, DbType.DateTime, entity.Ftelogfeccreacion);
            dbProvider.AddInParameter(command, helper.Ftelogobs, DbType.String, entity.Ftelogobs);
            dbProvider.AddInParameter(command, helper.Ftelogcondicion, DbType.String, entity.Ftelogcondicion);
            dbProvider.AddInParameter(command, helper.Ftelogfecampliacion, DbType.DateTime, entity.Ftelogfecampliacion);
            
            dbProvider.AddInParameter(command, helper.Envarcodi, DbType.Int32, entity.Envarcodi);
           
            dbProvider.AddInParameter(command, helper.Ftenvcodi, DbType.Int32, entity.Ftenvcodi);
            dbProvider.AddInParameter(command, helper.Estenvcodi, DbType.Int32, entity.Estenvcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        //public int Save(FtExtEnvioLogDTO entity, IDbConnection conn, DbTransaction tran)
        public int Save(FtExtEnvioLogDTO entity, IDbConnection connection, DbTransaction transaction)
        {            
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftelogcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftelogusucreacion, DbType.String, entity.Ftelogusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftelogfeccreacion, DbType.DateTime, entity.Ftelogfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftelogobs, DbType.String, entity.Ftelogobs));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftelogcondicion, DbType.String, entity.Ftelogcondicion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftelogfecampliacion, DbType.DateTime, entity.Ftelogfecampliacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Envarcodi, DbType.Int32, entity.Envarcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftenvcodi, DbType.Int32, entity.Ftenvcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Estenvcodi, DbType.Int32, entity.Estenvcodi));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }
        

        public void Update(FtExtEnvioLogDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ftelogcodi, DbType.Int32, entity.Ftelogcodi);
            dbProvider.AddInParameter(command, helper.Ftelogusucreacion, DbType.String, entity.Ftelogusucreacion);
            dbProvider.AddInParameter(command, helper.Ftelogfeccreacion, DbType.DateTime, entity.Ftelogfeccreacion);
            dbProvider.AddInParameter(command, helper.Ftelogobs, DbType.String, entity.Ftelogobs);
            dbProvider.AddInParameter(command, helper.Ftelogcondicion, DbType.String, entity.Ftelogcondicion);
            dbProvider.AddInParameter(command, helper.Ftelogfecampliacion, DbType.DateTime, entity.Ftelogfecampliacion);
            dbProvider.AddInParameter(command, helper.Envarcodi, DbType.Int32, entity.Envarcodi);
            dbProvider.AddInParameter(command, helper.Ftenvcodi, DbType.Int32, entity.Ftenvcodi);
            dbProvider.AddInParameter(command, helper.Estenvcodi, DbType.Int32, entity.Estenvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ftelogcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ftelogcodi, DbType.Int32, ftelogcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtEnvioLogDTO GetById(int ftelogcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ftelogcodi, DbType.Int32, ftelogcodi);
            FtExtEnvioLogDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtEnvioLogDTO> List()
        {
            List<FtExtEnvioLogDTO> entitys = new List<FtExtEnvioLogDTO>();
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

        public List<FtExtEnvioLogDTO> GetByCriteria(int ftenvcodi)
        {
            List<FtExtEnvioLogDTO> entitys = new List<FtExtEnvioLogDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, ftenvcodi);
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

        public List<FtExtEnvioLogDTO> GetByIdsEnvio(string ftenvcodistring)
        {
            List<FtExtEnvioLogDTO> entitys = new List<FtExtEnvioLogDTO>();

            string sql = string.Format(helper.SqlGetByIdsEnvio, ftenvcodistring);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEstenvnomb = dr.GetOrdinal(helper.Estenvnomb);
                    if (!dr.IsDBNull(iEstenvnomb)) entity.Estenvnomb = dr.GetString(iEstenvnomb);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtEnvioLogDTO> GetByIdsEnvioRevisionAreas(string ftenvcodistring)
        {
            List<FtExtEnvioLogDTO> entitys = new List<FtExtEnvioLogDTO>();

            string sql = string.Format(helper.SqlGetByIdsEnvioRevisionAreas, ftenvcodistring);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEstenvnomb = dr.GetOrdinal(helper.Estenvnomb);
                    if (!dr.IsDBNull(iEstenvnomb)) entity.Estenvnomb = dr.GetString(iEstenvnomb);

                    int iFaremcodi = dr.GetOrdinal(helper.Faremcodi);
                    if (!dr.IsDBNull(iFaremcodi)) entity.Faremcodi = Convert.ToInt32(dr.GetValue(iFaremcodi));

                    int iFaremnombre = dr.GetOrdinal(helper.Faremnombre);
                    if (!dr.IsDBNull(iFaremnombre)) entity.Faremnombre = dr.GetString(iFaremnombre);

                    int iEnvarfecmaxrpta = dr.GetOrdinal(helper.Envarfecmaxrpta);
                    if (!dr.IsDBNull(iEnvarfecmaxrpta)) entity.Envarfecmaxrpta = dr.GetDateTime(iEnvarfecmaxrpta);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        

        public List<FtExtEnvioLogDTO> ListarPorEnviosYEstados(string ftenvcodis, string estenvcodis)
        {
            List<FtExtEnvioLogDTO> entitys = new List<FtExtEnvioLogDTO>();
            string sql = string.Format(helper.SqlGetByEnviosYEstados, ftenvcodis, estenvcodis);
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


        public List<FtExtEnvioLogDTO> ListarLogsEnviosAmpliados(string ftenvcodis)
        {
            List<FtExtEnvioLogDTO> entitys = new List<FtExtEnvioLogDTO>();

            string sql = string.Format(helper.SqlListarLogsEnviosAmpliados, ftenvcodis);
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
