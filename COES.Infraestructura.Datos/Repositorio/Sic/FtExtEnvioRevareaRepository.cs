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
    /// Clase de acceso a datos de la tabla FT_EXT_ENVIO_REVAREA
    /// </summary>
    public class FtExtEnvioRevareaRepository: RepositoryBase, IFtExtEnvioRevareaRepository
    {
        public FtExtEnvioRevareaRepository(string strConn): base(strConn)
        {
        }

        FtExtEnvioRevareaHelper helper = new FtExtEnvioRevareaHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }
        public int Save(FtExtEnvioRevareaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Revacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Revaestadoronda1, DbType.String, entity.Revaestadoronda1);
            dbProvider.AddInParameter(command, helper.Revahtmlronda1, DbType.String, entity.Revahtmlronda1);
            dbProvider.AddInParameter(command, helper.Revaestadoronda2, DbType.String, entity.Revaestadoronda2);
            dbProvider.AddInParameter(command, helper.Revahtmlronda2, DbType.String, entity.Revahtmlronda2);
            dbProvider.AddInParameter(command, helper.Ftevercodi, DbType.Int32, entity.Ftevercodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int Save(FtExtEnvioRevareaDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Revacodi, DbType.Int32, entity.Revacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Revaestadoronda1, DbType.String, entity.Revaestadoronda1));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Revahtmlronda1, DbType.String, entity.Revahtmlronda1));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Revaestadoronda2, DbType.String, entity.Revaestadoronda2));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Revahtmlronda2, DbType.String, entity.Revahtmlronda2));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftevercodi, DbType.Int32, entity.Ftevercodi));

            command.ExecuteNonQuery();
            return entity.Revacodi;
        }
        public void Update(FtExtEnvioRevareaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Revacodi, DbType.Int32, entity.Revacodi);
            dbProvider.AddInParameter(command, helper.Revaestadoronda1, DbType.String, entity.Revaestadoronda1);
            dbProvider.AddInParameter(command, helper.Revahtmlronda1, DbType.String, entity.Revahtmlronda1);
            dbProvider.AddInParameter(command, helper.Revaestadoronda2, DbType.String, entity.Revaestadoronda2);
            dbProvider.AddInParameter(command, helper.Revahtmlronda2, DbType.String, entity.Revahtmlronda2);
            dbProvider.AddInParameter(command, helper.Ftevercodi, DbType.Int32, entity.Ftevercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int revacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Revacodi, DbType.Int32, revacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeletePorGrupo(string revacodis)
        {
            string sql = string.Format(helper.SqlDeletePorGrupo, revacodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command)) { }
        }       

        public void DeletePorIds(string revacodis, IDbConnection connection, DbTransaction transaction)
        {
            string sql = string.Format(helper.SqlDeletePorIds, revacodis);
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = sql;

                dbCommand.ExecuteNonQuery();
            }
        }

        public List<FtExtEnvioRevareaDTO> ListarRelacionesPorVersionAreaYEquipo(int ftevercodi, int faremcodi, int fteeqcodi)
        {
            List<FtExtEnvioRevareaDTO> entitys = new List<FtExtEnvioRevareaDTO>();

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

        public List<FtExtEnvioRevareaDTO> ListarRelacionesContenidoPorVersionArea(int ftevercodi, int faremcodi)
        {
            List<FtExtEnvioRevareaDTO> entitys = new List<FtExtEnvioRevareaDTO>();

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
        

        public FtExtEnvioRevareaDTO GetById(int revacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Revacodi, DbType.Int32, revacodi);
            FtExtEnvioRevareaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtEnvioRevareaDTO> List()
        {
            List<FtExtEnvioRevareaDTO> entitys = new List<FtExtEnvioRevareaDTO>();
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

        public List<FtExtEnvioRevareaDTO> GetByCriteria()
        {
            List<FtExtEnvioRevareaDTO> entitys = new List<FtExtEnvioRevareaDTO>();
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

        public List<FtExtEnvioRevareaDTO> ListarRevisionPorAreaVersionYReq(string faremcodi, int ftevercodi, string ftereqcodis)
        {
            List<FtExtEnvioRevareaDTO> entitys = new List<FtExtEnvioRevareaDTO>();
            string sql = string.Format(helper.SqlListarRevisionPorAreaVersionYReq, faremcodi, ftevercodi, ftereqcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioRevareaDTO entity = helper.Create(dr);                   

                    int iFtereqcodi = dr.GetOrdinal(helper.Ftereqcodi);
                    if (!dr.IsDBNull(iFtereqcodi)) entity.Ftereqcodi = Convert.ToInt32(dr.GetValue(iFtereqcodi));

                    int iFaremcodi = dr.GetOrdinal(helper.Faremcodi);
                    if (!dr.IsDBNull(iFaremcodi)) entity.Faremcodi = Convert.ToInt32(dr.GetValue(iFaremcodi));

                    int iEnvarestado = dr.GetOrdinal(helper.Envarestado);
                    if (!dr.IsDBNull(iEnvarestado)) entity.Envarestado = dr.GetString(iEnvarestado);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        

        public List<FtExtEnvioRevareaDTO> ListarRevisionPorAreaVersionYDatos(string faremcodi, int ftevercodi, string ftedatcodis)
        {
            List<FtExtEnvioRevareaDTO> entitys = new List<FtExtEnvioRevareaDTO>();
            string sql = string.Format(helper.SqlListarRevisionPorAreaVersionYDatos, faremcodi, ftevercodi, ftedatcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioRevareaDTO entity = helper.Create(dr);

                    int iRevadcodi = dr.GetOrdinal(helper.Revadcodi);
                    if (!dr.IsDBNull(iRevadcodi)) entity.Revadcodi = Convert.ToInt32(dr.GetValue(iRevadcodi));

                    int iFtedatcodi = dr.GetOrdinal(helper.Ftedatcodi);
                    if (!dr.IsDBNull(iFtedatcodi)) entity.Ftedatcodi = Convert.ToInt32(dr.GetValue(iFtedatcodi));

                    int iFteeqcodi = dr.GetOrdinal(helper.Fteeqcodi);
                    if (!dr.IsDBNull(iFteeqcodi)) entity.Fteeqcodi = Convert.ToInt32(dr.GetValue(iFteeqcodi));

                    int iFtitcodi = dr.GetOrdinal(helper.Ftitcodi);
                    if (!dr.IsDBNull(iFtitcodi)) entity.Ftitcodi = Convert.ToInt32(dr.GetValue(iFtitcodi));

                    int iFaremcodi = dr.GetOrdinal(helper.Faremcodi);
                    if (!dr.IsDBNull(iFaremcodi)) entity.Faremcodi = Convert.ToInt32(dr.GetValue(iFaremcodi));

                    int iEnvarestado = dr.GetOrdinal(helper.Envarestado);
                    if (!dr.IsDBNull(iEnvarestado)) entity.Envarestado = dr.GetString(iEnvarestado);
                    
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        

        public List<FtExtEnvioRevareaDTO> ListByVersionYDato(int ftevercodi)
        {
            List<FtExtEnvioRevareaDTO> entitys = new List<FtExtEnvioRevareaDTO>();
            string sql = string.Format(helper.SqlListarPorDatos, ftevercodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioRevareaDTO entity = helper.Create(dr);

                    int iRevadcodi = dr.GetOrdinal(helper.Revadcodi);
                    if (!dr.IsDBNull(iRevadcodi)) entity.Revadcodi = Convert.ToInt32(dr.GetValue(iRevadcodi));

                    int iFtedatcodi = dr.GetOrdinal(helper.Ftedatcodi);
                    if (!dr.IsDBNull(iFtedatcodi)) entity.Ftedatcodi = Convert.ToInt32(dr.GetValue(iFtedatcodi));

                    int iFteeqcodi = dr.GetOrdinal(helper.Fteeqcodi);
                    if (!dr.IsDBNull(iFteeqcodi)) entity.Fteeqcodi = Convert.ToInt32(dr.GetValue(iFteeqcodi));

                    int iFtitcodi = dr.GetOrdinal(helper.Ftitcodi);
                    if (!dr.IsDBNull(iFtitcodi)) entity.Ftitcodi = Convert.ToInt32(dr.GetValue(iFtitcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //public List<FtExtEnvioRevisionDTO> ListByVersionYReq(int ftevercodi)
        //{
        //    List<FtExtEnvioRevisionDTO> entitys = new List<FtExtEnvioRevisionDTO>();
        //    string sql = string.Format(helper.SqlListarPorRequisitos, ftevercodi);
        //    DbCommand command = dbProvider.GetSqlStringCommand(sql);

        //    using (IDataReader dr = dbProvider.ExecuteReader(command))
        //    {
        //        while (dr.Read())
        //        {
        //            FtExtEnvioRevisionDTO entity = helper.Create(dr);

        //            int iFtereqcodi = dr.GetOrdinal(helper.Ftereqcodi);
        //            if (!dr.IsDBNull(iFtereqcodi)) entity.Ftereqcodi = Convert.ToInt32(dr.GetValue(iFtereqcodi));

        //            int iFevrqcodi = dr.GetOrdinal(helper.Fevrqcodi);
        //            if (!dr.IsDBNull(iFevrqcodi)) entity.Fevrqcodi = Convert.ToInt32(dr.GetValue(iFevrqcodi));

        //            entitys.Add(entity);
        //        }
        //    }

        //    return entitys;
        //}

        //public List<FtExtEnvioRevisionDTO> ListByVersionYEq(int ftevercodi)
        //{
        //    List<FtExtEnvioRevisionDTO> entitys = new List<FtExtEnvioRevisionDTO>();
        //    string sql = string.Format(helper.SqlListarPorModoOp, ftevercodi);
        //    DbCommand command = dbProvider.GetSqlStringCommand(sql);

        //    using (IDataReader dr = dbProvider.ExecuteReader(command))
        //    {
        //        while (dr.Read())
        //        {
        //            FtExtEnvioRevisionDTO entity = helper.Create(dr);

        //            int iFteeqcodi = dr.GetOrdinal(helper.Fteeqcodi);
        //            if (!dr.IsDBNull(iFteeqcodi)) entity.Fteeqcodi = Convert.ToInt32(dr.GetValue(iFteeqcodi));


        //            entitys.Add(entity);
        //        }
        //    }

        //    return entitys;
        //}
    }
}
