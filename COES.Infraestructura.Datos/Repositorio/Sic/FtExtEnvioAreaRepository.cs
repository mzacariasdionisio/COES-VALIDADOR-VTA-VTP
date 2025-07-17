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
    /// Clase de acceso a datos de la tabla FT_EXT_ENVIO_AREA
    /// </summary>
    public class FtExtEnvioAreaRepository: RepositoryBase, IFtExtEnvioAreaRepository
    {
        public FtExtEnvioAreaRepository(string strConn): base(strConn)
        {
        }

        FtExtEnvioAreaHelper helper = new FtExtEnvioAreaHelper();

        public int Save(FtExtEnvioAreaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Envarcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ftevercodi, DbType.Int32, entity.Ftevercodi);
            dbProvider.AddInParameter(command, helper.Faremcodi, DbType.Int32, entity.Faremcodi);
            dbProvider.AddInParameter(command, helper.Envarfecmaxrpta, DbType.DateTime, entity.Envarfecmaxrpta);
            dbProvider.AddInParameter(command, helper.Envarestado, DbType.String, entity.Envarestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int Save(FtExtEnvioAreaDTO entity, IDbConnection connection, DbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Envarcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftevercodi, DbType.Int32, entity.Ftevercodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremcodi, DbType.Int32, entity.Faremcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Envarfecmaxrpta, DbType.DateTime, entity.Envarfecmaxrpta));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Envarestado, DbType.String, entity.Envarestado));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }


        public void Update(FtExtEnvioAreaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ftevercodi, DbType.Int32, entity.Ftevercodi);
            dbProvider.AddInParameter(command, helper.Faremcodi, DbType.Int32, entity.Faremcodi);            
            dbProvider.AddInParameter(command, helper.Envarfecmaxrpta, DbType.DateTime, entity.Envarfecmaxrpta);
            dbProvider.AddInParameter(command, helper.Envarestado, DbType.String, entity.Envarestado);
            dbProvider.AddInParameter(command, helper.Envarcodi, DbType.Int32, entity.Envarcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int envarcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Envarcodi, DbType.Int32, envarcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtEnvioAreaDTO GetById(int envarcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Envarcodi, DbType.Int32, envarcodi);
            FtExtEnvioAreaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public FtExtEnvioAreaDTO GetByVersionYArea(int ftevercodi, int faremcodi)
        {
            FtExtEnvioAreaDTO entity = new FtExtEnvioAreaDTO();
            string query = string.Format(helper.SqlGetByVersionYArea, ftevercodi, faremcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
        

        public List<FtExtEnvioAreaDTO> List()
        {
            List<FtExtEnvioAreaDTO> entitys = new List<FtExtEnvioAreaDTO>();
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

        public List<FtExtEnvioAreaDTO> GetByCriteria()
        {
            List<FtExtEnvioAreaDTO> entitys = new List<FtExtEnvioAreaDTO>();
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

        public List<FtExtEnvioAreaDTO> ListarPorVersiones(string strVersiones)
        {
            List<FtExtEnvioAreaDTO> entitys = new List<FtExtEnvioAreaDTO>();
            string query = string.Format(helper.SqlListarPorVersiones, strVersiones);
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

        public List<FtExtEnvioAreaDTO> ListarPorEnvioCarpetaYEstado(int estenvcodi, string strFtenvcodis, string envarestado)
        {
            List<FtExtEnvioAreaDTO> entitys = new List<FtExtEnvioAreaDTO>();
            string query = string.Format(helper.SqlListarPorEnvioCarpetaYEstado, estenvcodi, strFtenvcodis, envarestado);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioAreaDTO entity = helper.Create(dr);

                    int iFtenvcodi = dr.GetOrdinal(helper.Ftenvcodi);
                    if (!dr.IsDBNull(iFtenvcodi)) entity.Ftenvcodi = Convert.ToInt32(dr.GetValue(iFtenvcodi));

                    int iEstenvcodi = dr.GetOrdinal(helper.Estenvcodi);
                    if (!dr.IsDBNull(iEstenvcodi)) entity.Estenvcodi = Convert.ToInt32(dr.GetValue(iEstenvcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        


    }
}
