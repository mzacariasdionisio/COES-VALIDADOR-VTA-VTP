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
    /// Clase de acceso a datos de la tabla SI_MIGRACIONLOG
    /// </summary>
    public class SiMigracionlogRepository : RepositoryBase, ISiMigracionlogRepository
    {
        private string strConexion;

        public SiMigracionlogRepository(string strConn) : base(strConn)
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

        SiMigracionlogHelper helper = new SiMigracionlogHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(SiMigracionlogDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Logmigcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Migracodi, DbType.Int32, entity.Migracodi);
            dbProvider.AddInParameter(command, helper.Mqxtopcodi, DbType.Int32, entity.Mqxtopcodi);
            dbProvider.AddInParameter(command, helper.Logmigoperacion, DbType.String, entity.Logmigoperacion);
            dbProvider.AddInParameter(command, helper.Logmicodigo, DbType.String, entity.Logmicodigo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiMigracionlogDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Logmigcodi, DbType.Int32, entity.Logmigcodi);
            dbProvider.AddInParameter(command, helper.Migracodi, DbType.Int32, entity.Migracodi);
            dbProvider.AddInParameter(command, helper.Mqxtopcodi, DbType.Int32, entity.Mqxtopcodi);
            dbProvider.AddInParameter(command, helper.Logmigoperacion, DbType.String, entity.Logmigoperacion);
            dbProvider.AddInParameter(command, helper.Logmicodigo, DbType.String, entity.Logmicodigo);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int logmigcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Logmigcodi, DbType.Int32, logmigcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiMigracionlogDTO GetById(int logmigcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Logmigcodi, DbType.Int32, logmigcodi);
            SiMigracionlogDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiMigracionlogDTO> List()
        {
            List<SiMigracionlogDTO> entitys = new List<SiMigracionlogDTO>();
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

        public List<SiMigracionlogDTO> GetByCriteria()
        {
            List<SiMigracionlogDTO> entitys = new List<SiMigracionlogDTO>();
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

        public void EjecutarQuery(string query, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;
            //dbProvider.ExecuteNonQuery(command);
            command.CommandText = query;
            command.ExecuteNonQuery();
        }

        //Método optiene la cantidad de registros involucrados en las querys de una migración para el DBA
        public int? CantRegistrosMigraQuery(int migracodi, int mqxtopcodi)
        {
            string query = string.Format(helper.SqlCantRegistrosMigraQuery, migracodi, mqxtopcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            object count = dbProvider.ExecuteScalar(command);

            return Convert.ToInt32(count);
        }
    }
}
