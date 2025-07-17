using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla SI_MIGRAQUERYPARAMETRO
    /// </summary>
    public class SiMigraqueryparametroRepository : RepositoryBase, ISiMigraqueryparametroRepository
    {
        public SiMigraqueryparametroRepository(string strConn) : base(strConn)
        {
        }

        SiMigraqueryparametroHelper helper = new SiMigraqueryparametroHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(SiMigraqueryparametroDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mgqparcodi, DbType.Int32, entity.Mgqparcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubacodi, DbType.Int32, entity.Miqubacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migparcodi, DbType.Int32, entity.Migparcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mgqparvalor, DbType.String, entity.Mgqparvalor));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mgqparactivo, DbType.Int32, entity.Mgqparactivo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mgqparusucreacion, DbType.String, entity.Mgqparusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mgqparfeccreacion, DbType.DateTime, entity.Mgqparfeccreacion));

            command.ExecuteNonQuery();

            return entity.Mgqparcodi;
        }

        public void Update(SiMigraqueryparametroDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlUpdate;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubacodi, DbType.Int32, entity.Miqubacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migparcodi, DbType.Int32, entity.Migparcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mgqparvalor, DbType.String, entity.Mgqparvalor));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mgqparactivo, DbType.Int32, entity.Mgqparactivo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mgqparusucreacion, DbType.String, entity.Mgqparusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mgqparfeccreacion, DbType.DateTime, entity.Mgqparfeccreacion));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mgqparcodi, DbType.Int32, entity.Mgqparcodi));

            command.ExecuteNonQuery();
        }

        public void Delete(int mgqparcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mgqparcodi, DbType.Int32, mgqparcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiMigraqueryparametroDTO GetById(int mgqparcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mgqparcodi, DbType.Int32, mgqparcodi);
            SiMigraqueryparametroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiMigraqueryparametroDTO> List()
        {
            List<SiMigraqueryparametroDTO> entitys = new List<SiMigraqueryparametroDTO>();
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

        public List<SiMigraqueryparametroDTO> GetByCriteria(int miqubacodi)
        {
            List<SiMigraqueryparametroDTO> entitys = new List<SiMigraqueryparametroDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, miqubacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiMigraqueryparametroDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
