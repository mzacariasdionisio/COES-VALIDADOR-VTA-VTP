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
    /// Clase de acceso a datos de la tabla FT_EXT_FORMATO
    /// </summary>
    public class FtExtFormatoRepository : RepositoryBase, IFtExtFormatoRepository
    {
        public FtExtFormatoRepository(string strConn) : base(strConn)
        {
        }

        FtExtFormatoHelper helper = new FtExtFormatoHelper();

        public int Save(FtExtFormatoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ftfmtcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Fteqcodi, DbType.Int32, entity.Fteqcodi);
            dbProvider.AddInParameter(command, helper.Ftetcodi, DbType.Int32, entity.Ftetcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int Save(FtExtFormatoDTO entity, IDbConnection connection, IDbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftfmtcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fteqcodi, DbType.Int32, entity.Fteqcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftetcodi, DbType.Int32, entity.Ftetcodi));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void Update(FtExtFormatoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Fteqcodi, DbType.Int32, entity.Fteqcodi);
            dbProvider.AddInParameter(command, helper.Ftetcodi, DbType.Int32, entity.Ftetcodi);
            dbProvider.AddInParameter(command, helper.Ftfmtcodi, DbType.Int32, entity.Ftfmtcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(FtExtFormatoDTO entity, IDbConnection connection, IDbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlUpdate;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fteqcodi, DbType.Int32, entity.Fteqcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftetcodi, DbType.Int32, entity.Ftetcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftfmtcodi, DbType.Int32, entity.Ftfmtcodi));

                dbCommand.ExecuteNonQuery();

            }
        }

        public void Delete(int ftfmtcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ftfmtcodi, DbType.Int32, ftfmtcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtFormatoDTO GetById(int ftfmtcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ftfmtcodi, DbType.Int32, ftfmtcodi);
            FtExtFormatoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtFormatoDTO> List()
        {
            List<FtExtFormatoDTO> entitys = new List<FtExtFormatoDTO>();
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

        public List<FtExtFormatoDTO> GetByCriteria()
        {
            List<FtExtFormatoDTO> entitys = new List<FtExtFormatoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iCatecodi = dr.GetOrdinal(helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public FtExtFormatoDTO GetByEtapaYTipoEquipo(int fteqcodi, int ftetcodi)
        {
            FtExtFormatoDTO entity = null;

            string query = string.Format(helper.SqlListarPorEtapaYTipoEquipo, fteqcodi, ftetcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
    }
}
