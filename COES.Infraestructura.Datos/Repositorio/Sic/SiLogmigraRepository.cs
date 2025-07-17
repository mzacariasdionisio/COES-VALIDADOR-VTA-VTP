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
    /// Clase de acceso a datos de la tabla SI_LOGMIGRA
    /// </summary>
    public class SiLogmigraRepository: RepositoryBase, ISiLogmigraRepository
    {
        private string strConexion;
      
        public SiLogmigraRepository(string strConn): base(strConn)
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


        SiLogmigraHelper helper = new SiLogmigraHelper();

        //Comentado para Save sin Transaccion
        //public void Save(SiLogmigraDTO entity, IDbConnection cnn,DbTransaction tran)
        //{
        //    DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

        //    command.Transaction = tran;
        //    command.Connection = (DbConnection)cnn;


        //    dbProvider.AddInParameter(command, helper.Migracodi, DbType.Int32, entity.Migracodi);
        //    dbProvider.AddInParameter(command, helper.Logcodi, DbType.Int32, entity.Logcodi);
        //    dbProvider.AddInParameter(command, helper.Logmigusucreacion, DbType.String, entity.Logmigusucreacion);
        //    dbProvider.AddInParameter(command, helper.Logmigfeccreacion, DbType.DateTime, entity.Logmigfeccreacion);
        //    dbProvider.ExecuteNonQuery(command);
        //}
        ///Save con Transacion
        public void Save(SiLogmigraDTO entity, IDbConnection cnn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            //command = (DbCommand)cnn.CreateCommand();
            command.Transaction = tran;
            command.Connection = (DbConnection)cnn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migracodi, DbType.Int32, entity.Migracodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Logcodi, DbType.Int32, entity.Logcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Logmigusucreacion, DbType.String, entity.Logmigusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Logmigfeccreacion, DbType.DateTime, entity.Logmigfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Logmigtipo, DbType.Int32, entity.Logmigtipo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubacodi, DbType.Int32, entity.Miqubacodi));

            command.ExecuteNonQuery();

        }



        public void Update(SiLogmigraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Migracodi, DbType.Int32, entity.Migracodi);
            dbProvider.AddInParameter(command, helper.Logcodi, DbType.Int32, entity.Logcodi);
            dbProvider.AddInParameter(command, helper.Logmigusucreacion, DbType.String, entity.Logmigusucreacion);
            dbProvider.AddInParameter(command, helper.Logmigfeccreacion, DbType.DateTime, entity.Logmigfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int migracodi, int logcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Migracodi, DbType.Int32, migracodi);
            dbProvider.AddInParameter(command, helper.Logcodi, DbType.Int32, logcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiLogmigraDTO GetById(int migracodi, int logcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Migracodi, DbType.Int32, migracodi);
            dbProvider.AddInParameter(command, helper.Logcodi, DbType.Int32, logcodi);
            SiLogmigraDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiLogmigraDTO> List()
        {
            List<SiLogmigraDTO> entitys = new List<SiLogmigraDTO>();
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

        public List<SiLogmigraDTO> GetByCriteria()
        {
            List<SiLogmigraDTO> entitys = new List<SiLogmigraDTO>();
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
    }
}
