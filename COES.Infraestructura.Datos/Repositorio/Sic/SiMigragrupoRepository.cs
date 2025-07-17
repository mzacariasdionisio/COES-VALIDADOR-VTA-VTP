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
    /// Clase de acceso a datos de la tabla SI_MIGRAGRUPO
    /// </summary>
    public class SiMigragrupoRepository: RepositoryBase, ISiMigragrupoRepository
    {
        private string strConexion;
        public SiMigragrupoRepository(string strConn): base(strConn)
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

        SiMigragrupoHelper helper = new SiMigragrupoHelper();

        public int Save(SiMigragrupoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            dbProvider.AddInParameter(command, helper.Miggrucodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Migempcodi, DbType.Int32, entity.Migempcodi);
            dbProvider.AddInParameter(command, helper.Grupocodimigra, DbType.Int32, entity.Grupocodimigra);
            dbProvider.AddInParameter(command, helper.Grupocodibajanuevo, DbType.Int32, entity.Grupocodibajanuevo);
            dbProvider.AddInParameter(command, helper.Miggruusucreacion, DbType.String, entity.Miggruusucreacion);
            dbProvider.AddInParameter(command, helper.Miggrufeccreacion, DbType.DateTime, entity.Miggrufeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiMigragrupoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Miggrucodi, DbType.Int32, entity.Miggrucodi);
            dbProvider.AddInParameter(command, helper.Migempcodi, DbType.Int32, entity.Migempcodi);
            dbProvider.AddInParameter(command, helper.Grupocodimigra, DbType.Int32, entity.Grupocodimigra);
            dbProvider.AddInParameter(command, helper.Grupocodibajanuevo, DbType.Int32, entity.Grupocodibajanuevo);
            dbProvider.AddInParameter(command, helper.Miggruusucreacion, DbType.String, entity.Miggruusucreacion);
            dbProvider.AddInParameter(command, helper.Miggrufeccreacion, DbType.DateTime, entity.Miggrufeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int miggrucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Miggrucodi, DbType.Int32, miggrucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiMigragrupoDTO GetById(int miggrucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Miggrucodi, DbType.Int32, miggrucodi);
            SiMigragrupoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiMigragrupoDTO> List()
        {
            List<SiMigragrupoDTO> entitys = new List<SiMigragrupoDTO>();
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

        public List<SiMigragrupoDTO> GetByCriteria()
        {
            List<SiMigragrupoDTO> entitys = new List<SiMigragrupoDTO>();
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
