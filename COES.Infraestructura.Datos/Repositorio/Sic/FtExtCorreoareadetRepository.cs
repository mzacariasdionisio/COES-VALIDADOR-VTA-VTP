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
    /// Clase de acceso a datos de la tabla FT_EXT_CORREOAREADET
    /// </summary>
    public class FtExtCorreoareadetRepository: RepositoryBase, IFtExtCorreoareadetRepository
    {
        public FtExtCorreoareadetRepository(string strConn): base(strConn)
        {
        }

        FtExtCorreoareadetHelper helper = new FtExtCorreoareadetHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(FtExtCorreoareadetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Faremdcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Faremcodi, DbType.Int32, entity.Faremcodi);
            dbProvider.AddInParameter(command, helper.Faremdemail, DbType.String, entity.Faremdemail);
            dbProvider.AddInParameter(command, helper.Faremduserlogin, DbType.String, entity.Faremduserlogin);
            dbProvider.AddInParameter(command, helper.Faremdestado, DbType.String, entity.Faremdestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int Save(FtExtCorreoareadetDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremdcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremcodi, DbType.Int32, entity.Faremcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremdemail, DbType.String, entity.Faremdemail));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremduserlogin, DbType.String, entity.Faremduserlogin));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremdestado, DbType.String, entity.Faremdestado));


                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void Update(FtExtCorreoareadetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Faremcodi, DbType.Int32, entity.Faremcodi);
            dbProvider.AddInParameter(command, helper.Faremdemail, DbType.String, entity.Faremdemail);
            dbProvider.AddInParameter(command, helper.Faremduserlogin, DbType.String, entity.Faremduserlogin);
            dbProvider.AddInParameter(command, helper.Faremdestado, DbType.String, entity.Faremdestado);
            dbProvider.AddInParameter(command, helper.Faremdcodi, DbType.Int32, entity.Faremdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(FtExtCorreoareadetDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlUpdate;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremcodi, DbType.Int32, entity.Faremcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremdemail, DbType.String, entity.Faremdemail));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremduserlogin, DbType.String, entity.Faremduserlogin));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremdestado, DbType.String, entity.Faremdestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremdcodi, DbType.Int32, entity.Faremdcodi));

                dbCommand.ExecuteNonQuery();
            }
        }


        public void Delete(int faremdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Faremdcodi, DbType.Int32, faremdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int faremdcodi, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlDelete;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Faremdcodi, DbType.Int32, faremdcodi));
                dbCommand.ExecuteNonQuery();

            }
        }

        public FtExtCorreoareadetDTO GetById(int faremdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Faremdcodi, DbType.Int32, faremdcodi);
            FtExtCorreoareadetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtCorreoareadetDTO> List()
        {
            List<FtExtCorreoareadetDTO> entitys = new List<FtExtCorreoareadetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtCorreoareadetDTO entity = helper.Create(dr);

                    int iFaremnombre = dr.GetOrdinal(helper.Faremnombre);
                    if (!dr.IsDBNull(iFaremnombre)) entity.Faremnombre = dr.GetString(iFaremnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtCorreoareadetDTO> GetByCriteria()
        {
            List<FtExtCorreoareadetDTO> entitys = new List<FtExtCorreoareadetDTO>();
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

        public List<FtExtCorreoareadetDTO> ListarCorreosPorArea(string strFaremcodis)
        {
            List<FtExtCorreoareadetDTO> entitys = new List<FtExtCorreoareadetDTO>();
            string query = string.Format(helper.SqlListarCorreosPorArea, strFaremcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtCorreoareadetDTO entity = helper.Create(dr);

                    int iFaremnombre = dr.GetOrdinal(helper.Faremnombre);
                    if (!dr.IsDBNull(iFaremnombre)) entity.Faremnombre = dr.GetString(iFaremnombre);                    

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtCorreoareadetDTO> ListarPorCorreo(string correo)
        {
            List<FtExtCorreoareadetDTO> entitys = new List<FtExtCorreoareadetDTO>();
            string query = string.Format(helper.SqlListarPorCorreo, correo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtCorreoareadetDTO entity = helper.Create(dr);

                    int iFaremnombre = dr.GetOrdinal(helper.Faremnombre);
                    if (!dr.IsDBNull(iFaremnombre)) entity.Faremnombre = dr.GetString(iFaremnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
       
    }
}
