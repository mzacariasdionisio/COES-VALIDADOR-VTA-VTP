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
    /// Clase de acceso a datos de la tabla FT_EXT_ETEMPDETPRY
    /// </summary>
    public class FtExtEtempdetpryRepository : RepositoryBase, IFtExtEtempdetpryRepository
    {
        public FtExtEtempdetpryRepository(string strConn) : base(strConn)
        {
        }

        FtExtEtempdetpryHelper helper = new FtExtEtempdetpryHelper();

        public int Save(FtExtEtempdetpryDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Feeprycodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Fetempcodi, DbType.Int32, entity.Fetempcodi);
            dbProvider.AddInParameter(command, helper.Ftprycodi, DbType.Int32, entity.Ftprycodi);
            dbProvider.AddInParameter(command, helper.Feepryestado, DbType.String, entity.Feepryestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int Save(FtExtEtempdetpryDTO entity, IDbConnection connection, IDbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feeprycodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fetempcodi, DbType.Int32, entity.Fetempcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftprycodi, DbType.Int32, entity.Ftprycodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feepryestado, DbType.String, entity.Feepryestado));


                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void Update(FtExtEtempdetpryDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Fetempcodi, DbType.Int32, entity.Fetempcodi);
            dbProvider.AddInParameter(command, helper.Ftprycodi, DbType.Int32, entity.Ftprycodi);
            dbProvider.AddInParameter(command, helper.Feepryestado, DbType.String, entity.Feepryestado);
            dbProvider.AddInParameter(command, helper.Feeprycodi, DbType.Int32, entity.Feeprycodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(FtExtEtempdetpryDTO entity, IDbConnection connection, IDbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlUpdate;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fetempcodi, DbType.Int32, entity.Fetempcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftprycodi, DbType.Int32, entity.Ftprycodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feepryestado, DbType.String, entity.Feepryestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feeprycodi, DbType.Int32, entity.Feeprycodi));


                dbCommand.ExecuteNonQuery();

            }
        }

        public void Delete(int feeprycodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Feeprycodi, DbType.Int32, feeprycodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtEtempdetpryDTO GetById(int feeprycodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Feeprycodi, DbType.Int32, feeprycodi);
            FtExtEtempdetpryDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public FtExtEtempdetpryDTO GetByEmpresaEtapaProyecto(int emprcodi, int ftetcodi, int ftprycodi)
        {
            string sql = string.Format(helper.SqlGetByEmpresaYEtapa, emprcodi, ftetcodi, ftprycodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            FtExtEtempdetpryDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iFtetnombre = dr.GetOrdinal(helper.Ftetnombre);
                    if (!dr.IsDBNull(iFtetnombre)) entity.Ftetnombre = dr.GetString(iFtetnombre);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iFtetcodi = dr.GetOrdinal(helper.Ftetcodi);
                    if (!dr.IsDBNull(iFtetcodi)) entity.Ftetcodi = Convert.ToInt32(dr.GetValue(iFtetcodi));

                    int iFtprynombre = dr.GetOrdinal(helper.Ftprynombre);
                    if (!dr.IsDBNull(iFtprynombre)) entity.Ftprynombre = dr.GetString(iFtprynombre);

                }
            }

            return entity;
        }

        public List<FtExtEtempdetpryDTO> List()
        {
            List<FtExtEtempdetpryDTO> entitys = new List<FtExtEtempdetpryDTO>();
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

        public List<FtExtEtempdetpryDTO> GetByCriteria()
        {
            List<FtExtEtempdetpryDTO> entitys = new List<FtExtEtempdetpryDTO>();
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

        public List<FtExtEtempdetpryDTO> ListarPorRelEmpresaEtapa(string estado, int fetempcodi)
        {
            List<FtExtEtempdetpryDTO> entitys = new List<FtExtEtempdetpryDTO>();

            string query = string.Format(helper.SqlListarPorRelEmpresaEtapa, estado, fetempcodi);
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

        public List<FtExtEtempdetpryDTO> GetByProyectos(string strFtprycodis)
        {
            List<FtExtEtempdetpryDTO> entitys = new List<FtExtEtempdetpryDTO>();

            string query = string.Format(helper.SqlGetByProyectos, strFtprycodis);
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
        
    }
}
