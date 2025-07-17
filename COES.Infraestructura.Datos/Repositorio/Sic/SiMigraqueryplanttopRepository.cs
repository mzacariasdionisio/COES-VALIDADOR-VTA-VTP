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
    /// Clase de acceso a datos de la tabla SI_MIGRAQUERYPLANTTOP
    /// </summary>
    public class SiMigraqueryplanttopRepository : RepositoryBase, ISiMigraqueryplanttopRepository
    {
        public SiMigraqueryplanttopRepository(string strConn) : base(strConn)
        {
        }

        SiMigraqueryplanttopHelper helper = new SiMigraqueryplanttopHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(SiMigraqueryplanttopDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miptopcodi, DbType.Int32, entity.Miptopcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqplacodi, DbType.Int32, entity.Miqplacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Tmopercodi, DbType.Int32, entity.Tmopercodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miptopactivo, DbType.Int32, entity.Miptopactivo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miptopusucreacion, DbType.String, entity.Miptopusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miptopfeccreacion, DbType.DateTime, entity.Miptopfeccreacion));

            command.ExecuteNonQuery();

            return entity.Miptopcodi;
        }

        public void Update(SiMigraqueryplanttopDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqplacodi, DbType.Int32, entity.Miqplacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Tmopercodi, DbType.Int32, entity.Tmopercodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miptopactivo, DbType.Int32, entity.Miptopactivo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miptopusucreacion, DbType.String, entity.Miptopusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miptopfeccreacion, DbType.DateTime, entity.Miptopfeccreacion));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miptopcodi, DbType.Int32, entity.Miptopcodi));

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int miptopcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Miptopcodi, DbType.Int32, miptopcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiMigraqueryplanttopDTO GetById(int miptopcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Miptopcodi, DbType.Int32, miptopcodi);
            SiMigraqueryplanttopDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiMigraqueryplanttopDTO> List()
        {
            List<SiMigraqueryplanttopDTO> entitys = new List<SiMigraqueryplanttopDTO>();
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

        public List<SiMigraqueryplanttopDTO> GetByCriteria(int miqplacodi)
        {
            List<SiMigraqueryplanttopDTO> entitys = new List<SiMigraqueryplanttopDTO>();

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
