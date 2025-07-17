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
    /// Clase de acceso a datos de la tabla SI_MIGRAPTOMEDICION
    /// </summary>
    public class SiMigraptomedicionRepository: RepositoryBase, ISiMigraptomedicionRepository
    {
        private string strConexion;
        public SiMigraptomedicionRepository(string strConn): base(strConn)
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

        SiMigraptomedicionHelper helper = new SiMigraptomedicionHelper();

        public int Save(SiMigraPtomedicionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            dbProvider.AddInParameter(command, helper.Mgpmedcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Migempcodi, DbType.Int32, entity.Migempcodi);
            dbProvider.AddInParameter(command, helper.Ptomedcodimigra, DbType.Int32, entity.Ptomedcodimigra);
            dbProvider.AddInParameter(command, helper.Ptomedbajanuevo, DbType.Int32, entity.Ptomedbajanuevo);
            dbProvider.AddInParameter(command, helper.Mgpmedusucreacion, DbType.String, entity.Mgpmedusucreacion);
            dbProvider.AddInParameter(command, helper.Mgpmedfeccreacion, DbType.DateTime, entity.Mgpmedfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiMigraPtomedicionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mgpmedcodi, DbType.Int32, entity.Mgpmedcodi);
            dbProvider.AddInParameter(command, helper.Migempcodi, DbType.Int32, entity.Migempcodi);
            dbProvider.AddInParameter(command, helper.Ptomedcodimigra, DbType.Int32, entity.Ptomedcodimigra);
            dbProvider.AddInParameter(command, helper.Ptomedbajanuevo, DbType.Int32, entity.Ptomedbajanuevo);
            dbProvider.AddInParameter(command, helper.Mgpmedusucreacion, DbType.String, entity.Mgpmedusucreacion);
            dbProvider.AddInParameter(command, helper.Mgpmedfeccreacion, DbType.DateTime, entity.Mgpmedfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mgpmedcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mgpmedcodi, DbType.Int32, mgpmedcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiMigraPtomedicionDTO GetById(int mgpmedcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mgpmedcodi, DbType.Int32, mgpmedcodi);
            SiMigraPtomedicionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiMigraPtomedicionDTO> List()
        {
            List<SiMigraPtomedicionDTO> entitys = new List<SiMigraPtomedicionDTO>();
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

        public List<SiMigraPtomedicionDTO> GetByCriteria()
        {
            List<SiMigraPtomedicionDTO> entitys = new List<SiMigraPtomedicionDTO>();
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
