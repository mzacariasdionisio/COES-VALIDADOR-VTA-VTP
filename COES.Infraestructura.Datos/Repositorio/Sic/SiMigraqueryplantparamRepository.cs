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
    /// Clase de acceso a datos de la tabla SI_MIGRAQUERYPLANTPARAM
    /// </summary>
    public class SiMigraqueryplantparamRepository : RepositoryBase, ISiMigraqueryplantparamRepository
    {
        public SiMigraqueryplantparamRepository(string strConn) : base(strConn)
        {
        }

        SiMigraqueryplantparamHelper helper = new SiMigraqueryplantparamHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(SiMigraqueryplantparamDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miplprcodi, DbType.Int32, entity.Miplprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miplpractivo, DbType.Int32, entity.Miplpractivo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miplprusucreacion, DbType.String, entity.Miplprusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miplprfeccreacion, DbType.DateTime, entity.Miplprfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqplacodi, DbType.Int32, entity.Miqplacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migparcodi, DbType.Int32, entity.Migparcodi));

            command.ExecuteNonQuery();

            return entity.Miplprcodi;
        }

        public void Update(SiMigraqueryplantparamDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlUpdate;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miplpractivo, DbType.Int32, entity.Miplpractivo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miplprusucreacion, DbType.String, entity.Miplprusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miplprfeccreacion, DbType.DateTime, entity.Miplprfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqplacodi, DbType.Int32, entity.Miqplacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migparcodi, DbType.Int32, entity.Migparcodi));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miplprcodi, DbType.Int32, entity.Miplprcodi));

            command.ExecuteNonQuery();
        }

        public void Delete(int miplprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Miplprcodi, DbType.Int32, miplprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiMigraqueryplantparamDTO GetById(int miplprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Miplprcodi, DbType.Int32, miplprcodi);
            SiMigraqueryplantparamDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiMigraqueryplantparamDTO> List()
        {
            List<SiMigraqueryplantparamDTO> entitys = new List<SiMigraqueryplantparamDTO>();
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

        public List<SiMigraqueryplantparamDTO> GetByCriteria(int miqplacodi)
        {
            List<SiMigraqueryplantparamDTO> entitys = new List<SiMigraqueryplantparamDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, miqplacodi);
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
