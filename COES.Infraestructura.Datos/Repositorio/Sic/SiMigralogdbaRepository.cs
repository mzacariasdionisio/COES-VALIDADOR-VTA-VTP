using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla SI_MIGRALOGDBA
    /// </summary>
    public class SiMigralogdbaRepository: RepositoryBase, ISiMigralogdbaRepository
    {
        private string strConexion;

        public SiMigralogdbaRepository(string strConn): base(strConn)
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

        SiMigralogdbaHelper helper = new SiMigralogdbaHelper();


        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int SaveTransferencia(SiMigralogdbaDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSaveTransferencia;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migdbacodi, DbType.Int32, entity.Migdbacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migracodi, DbType.Int32, entity.Migracodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migdbaquery, DbType.String, entity.Migdbaquery));


            //Added
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migdbalogquery, DbType.String, entity.Migdbalogquery));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migdbausucreacion, DbType.String, entity.Migdbausucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migdbafeccreacion, DbType.DateTime, entity.Migdbafeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mqxtopcodi, DbType.Int32, entity.Mqxtopcodi));

            //command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migrausumodificacion, DbType.String, entity.Migrausumodificacion));
            //command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migrafecmodificacion, DbType.DateTime, entity.Migrafecmodificacion));


            command.ExecuteNonQuery();
            return 1;
        }

        public int Save(SiMigralogdbaDTO entity,IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            //command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            //command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migracodi, DbType.Int32, id));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migdbacodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migracodi, DbType.Int32, entity.Migracodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migdbaquery, DbType.String, entity.Migdbaquery));




            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiMigralogdbaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Migracodi, DbType.Int32, entity.Migracodi);
            dbProvider.AddInParameter(command, helper.Migdbacodi, DbType.Int32, entity.Migdbacodi);
            dbProvider.AddInParameter(command, helper.Migdbaquery, DbType.String, entity.Migdbaquery);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int migdbacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Migdbacodi, DbType.Int32, migdbacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiMigralogdbaDTO GetById(int migdbacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Migdbacodi, DbType.Int32, migdbacodi);
            SiMigralogdbaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiMigralogdbaDTO> List()
        {
            List<SiMigralogdbaDTO> entitys = new List<SiMigralogdbaDTO>();
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

        public List<SiMigralogdbaDTO> GetByCriteria()
        {
            List<SiMigralogdbaDTO> entitys = new List<SiMigralogdbaDTO>();
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

        //Método paar traer las querys de una migración para el DBA
        public List<SiMigralogdbaDTO> ListLogDbaByMigracion(int migracodi)
        {

            List<SiMigralogdbaDTO> entitys = new List<SiMigralogdbaDTO>();
            string query = string.Format(helper.SqlListLogDbaByMigracion, migracodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iMiqubanomtabla = dr.GetOrdinal(this.helper.Miqubanomtabla);
                    if (!dr.IsDBNull(iMiqubanomtabla)) entity.Miqubanomtabla = dr.GetString(iMiqubanomtabla);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
