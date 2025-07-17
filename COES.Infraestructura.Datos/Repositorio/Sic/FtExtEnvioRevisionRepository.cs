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
    /// Clase de acceso a datos de la tabla FT_EXT_ENVIO_REVISION
    /// </summary>
    public class FtExtEnvioRevisionRepository : RepositoryBase, IFtExtEnvioRevisionRepository
    {
        public FtExtEnvioRevisionRepository(string strConn) : base(strConn)
        {
        }

        FtExtEnvioRevisionHelper helper = new FtExtEnvioRevisionHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int SaveTransaccional(FtExtEnvioRevisionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftrevcodi, DbType.Int32, entity.Ftrevcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftrevhtmlobscoes, DbType.String, entity.Ftrevhtmlobscoes));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftrevhtmlrptaagente, DbType.String, entity.Ftrevhtmlrptaagente));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftrevhtmlrptacoes, DbType.String, entity.Ftrevhtmlrptacoes));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftrevestado, DbType.String, entity.Ftrevestado));


            command.ExecuteNonQuery();
            return entity.Ftedatcodi;
        }

        public int Save(FtExtEnvioRevisionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ftrevcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ftrevhtmlobscoes, DbType.String, entity.Ftrevhtmlobscoes);
            dbProvider.AddInParameter(command, helper.Ftrevhtmlrptaagente, DbType.String, entity.Ftrevhtmlrptaagente);
            dbProvider.AddInParameter(command, helper.Ftrevhtmlrptacoes, DbType.String, entity.Ftrevhtmlrptacoes);
            dbProvider.AddInParameter(command, helper.Ftrevestado, DbType.String, entity.Ftrevestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FtExtEnvioRevisionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlUpdate;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftrevhtmlobscoes, DbType.String, entity.Ftrevhtmlobscoes));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftrevhtmlrptaagente, DbType.String, entity.Ftrevhtmlrptaagente));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftrevhtmlrptacoes, DbType.String, entity.Ftrevhtmlrptacoes));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftrevestado, DbType.String, entity.Ftrevestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftrevcodi, DbType.Int32, entity.Ftrevcodi));

            command.ExecuteNonQuery();
        }

        public void Delete(int ftrevcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ftrevcodi, DbType.Int32, ftrevcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtEnvioRevisionDTO GetById(int ftrevcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ftrevcodi, DbType.Int32, ftrevcodi);
            FtExtEnvioRevisionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtEnvioRevisionDTO> List()
        {
            List<FtExtEnvioRevisionDTO> entitys = new List<FtExtEnvioRevisionDTO>();
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

        public List<FtExtEnvioRevisionDTO> GetByCriteria()
        {
            List<FtExtEnvioRevisionDTO> entitys = new List<FtExtEnvioRevisionDTO>();
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

        public List<FtExtEnvioRevisionDTO> ListByVersionYDato(int ftevercodi)
        {
            List<FtExtEnvioRevisionDTO> entitys = new List<FtExtEnvioRevisionDTO>();
            string sql = string.Format(helper.SqlListarPorDatos, ftevercodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioRevisionDTO entity = helper.Create(dr);

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

        public List<FtExtEnvioRevisionDTO> ListByVersionYReq(int ftevercodi)
        {
            List<FtExtEnvioRevisionDTO> entitys = new List<FtExtEnvioRevisionDTO>();
            string sql = string.Format(helper.SqlListarPorRequisitos, ftevercodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioRevisionDTO entity = helper.Create(dr);

                    int iFtereqcodi = dr.GetOrdinal(helper.Ftereqcodi);
                    if (!dr.IsDBNull(iFtereqcodi)) entity.Ftereqcodi = Convert.ToInt32(dr.GetValue(iFtereqcodi));

                    int iFevrqcodi = dr.GetOrdinal(helper.Fevrqcodi);
                    if (!dr.IsDBNull(iFevrqcodi)) entity.Fevrqcodi = Convert.ToInt32(dr.GetValue(iFevrqcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtEnvioRevisionDTO> ListByVersionYEq(int ftevercodi)
        {
            List<FtExtEnvioRevisionDTO> entitys = new List<FtExtEnvioRevisionDTO>();
            string sql = string.Format(helper.SqlListarPorModoOp, ftevercodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioRevisionDTO entity = helper.Create(dr);

                    int iFteeqcodi = dr.GetOrdinal(helper.Fteeqcodi);
                    if (!dr.IsDBNull(iFteeqcodi)) entity.Fteeqcodi = Convert.ToInt32(dr.GetValue(iFteeqcodi));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
