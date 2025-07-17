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
    /// Clase de acceso a datos de la tabla SI_MIGRAQUERYPLANTILLA
    /// </summary>
    public class SiMigraqueryplantillaRepository : RepositoryBase, ISiMigraqueryplantillaRepository
    {
        public SiMigraqueryplantillaRepository(string strConn) : base(strConn)
        {
        }

        SiMigraqueryplantillaHelper helper = new SiMigraqueryplantillaHelper();

        public int Save(SiMigraqueryplantillaDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqplacodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqplanomb, DbType.String, entity.Miqplanomb));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqpladesc, DbType.String, entity.Miqpladesc));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqplacomentario, DbType.String, entity.Miqplacomentario));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqplausucreacion, DbType.String, entity.Miqplausucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqplafeccreacion, DbType.DateTime, entity.Miqplafeccreacion));

            command.ExecuteNonQuery();

            return id;
        }

        public void Update(SiMigraqueryplantillaDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlUpdate;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqplanomb, DbType.String, entity.Miqplanomb));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqpladesc, DbType.String, entity.Miqpladesc));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqplacomentario, DbType.String, entity.Miqplacomentario));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqplausucreacion, DbType.String, entity.Miqplausucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqplafeccreacion, DbType.DateTime, entity.Miqplafeccreacion));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqplacodi, DbType.Int32, entity.Miqplacodi));

            command.ExecuteNonQuery();
        }

        public void Delete(int miqplacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Miqplacodi, DbType.Int32, miqplacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiMigraqueryplantillaDTO GetById(int miqplacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Miqplacodi, DbType.Int32, miqplacodi);
            SiMigraqueryplantillaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiMigraqueryplantillaDTO> List()
        {
            List<SiMigraqueryplantillaDTO> entitys = new List<SiMigraqueryplantillaDTO>();
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

        public List<SiMigraqueryplantillaDTO> GetByCriteria()
        {
            List<SiMigraqueryplantillaDTO> entitys = new List<SiMigraqueryplantillaDTO>();
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
