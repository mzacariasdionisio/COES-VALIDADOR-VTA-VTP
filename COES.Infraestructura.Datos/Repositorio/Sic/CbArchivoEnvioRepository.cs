using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CB_ARCHIVOENVIO
    /// </summary>
    public class CbArchivoenvioRepository : RepositoryBase, ICbArchivoenvioRepository
    {
        private string strConexion;
        public CbArchivoenvioRepository(string strConn) : base(strConn)
        {
            strConexion = strConn;
        }

        public IDbConnection BeginConnection()
        {
            Database db = DatabaseFactory.CreateDatabase(strConexion);
            IDbConnection conn = db.CreateConnection();
            conn.Open();
            return conn;
        }

        public DbTransaction StartTransaction(IDbConnection conn)
        {
            return (DbTransaction)conn.BeginTransaction();
        }

        readonly CbArchivoenvioHelper helper = new CbArchivoenvioHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(CbArchivoenvioDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbarchcodi, DbType.Int32, entity.Cbarchcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbvercodi, DbType.Int32, entity.Cbvercodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbarchnombreenvio, DbType.String, entity.Cbarchnombreenvio));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbarchnombrefisico, DbType.String, entity.Cbarchnombrefisico));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbarchorden, DbType.Int32, entity.Cbarchorden));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbarchestado, DbType.Int32, entity.Cbarchestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ccombcodi, DbType.Int32, entity.Ccombcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbarchconfidencial, DbType.Int32, entity.Cbarchconfidencial));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbarchobs, DbType.String, entity.Cbarchobs));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Corrcodi, DbType.Int32, entity.Corrcodi));

            command.ExecuteNonQuery();
            return entity.Cbarchcodi;
        }

        public void Update(CbArchivoenvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cbvercodi, DbType.Int32, entity.Cbvercodi);
            dbProvider.AddInParameter(command, helper.Cbarchcodi, DbType.Int32, entity.Cbarchcodi);
            dbProvider.AddInParameter(command, helper.Cbarchnombreenvio, DbType.String, entity.Cbarchnombreenvio);
            dbProvider.AddInParameter(command, helper.Cbarchnombrefisico, DbType.String, entity.Cbarchnombrefisico);
            dbProvider.AddInParameter(command, helper.Cbarchorden, DbType.Int32, entity.Cbarchorden);
            dbProvider.AddInParameter(command, helper.Cbarchestado, DbType.Int32, entity.Cbarchestado);
            dbProvider.AddInParameter(command, helper.Ccombcodi, DbType.Int32, entity.Ccombcodi);
            dbProvider.AddInParameter(command, helper.Cbarchconfidencial, DbType.Int32, entity.Cbarchconfidencial);
            dbProvider.AddInParameter(command, helper.Cbarchobs, DbType.String, entity.Cbarchobs);
            dbProvider.AddInParameter(command, helper.Corrcodi, DbType.Int32, entity.Corrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cbarchcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cbarchcodi, DbType.Int32, cbarchcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CbArchivoenvioDTO GetById(int cbarchcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cbarchcodi, DbType.Int32, cbarchcodi);
            CbArchivoenvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CbArchivoenvioDTO> List()
        {
            List<CbArchivoenvioDTO> entitys = new List<CbArchivoenvioDTO>();
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

        public List<CbArchivoenvioDTO> GetByCriteria(int cbvercodi)
        {
            List<CbArchivoenvioDTO> entitys = new List<CbArchivoenvioDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, cbvercodi);
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

        public List<CbArchivoenvioDTO> GetByCorreo(string corrcodis)
        {
            List<CbArchivoenvioDTO> entitys = new List<CbArchivoenvioDTO>();
            string sql = string.Format(helper.SqlGetByCorreo, corrcodis);
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
