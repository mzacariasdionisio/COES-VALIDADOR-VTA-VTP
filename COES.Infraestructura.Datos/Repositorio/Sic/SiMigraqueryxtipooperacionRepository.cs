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
    /// Clase de acceso a datos de la tabla SI_MIGRAQUERYXTIPOOPERACION
    /// </summary>
    public class SiMigraqueryxtipooperacionRepository : RepositoryBase, ISiMigraqueryxtipooperacionRepository
    {
        public SiMigraqueryxtipooperacionRepository(string strConn) : base(strConn)
        {
        }

        SiMigraqueryxtipooperacionHelper helper = new SiMigraqueryxtipooperacionHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(SiMigraqueryxtipooperacionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mqxtopcodi, DbType.Int32, entity.Mqxtopcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubacodi, DbType.Int32, entity.Miqubacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Tmopercodi, DbType.Int32, entity.Tmopercodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mqxtoporden, DbType.Int32, entity.Mqxtoporden));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mqxtopactivo, DbType.Int32, entity.Mqxtopactivo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mqxtopusucreacion, DbType.String, entity.Mqxtopusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mqxtopfeccreacion, DbType.DateTime, entity.Mqxtopfeccreacion));

            command.ExecuteNonQuery();

            return entity.Mqxtopcodi;
        }

        public void Update(SiMigraqueryxtipooperacionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlUpdate;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubacodi, DbType.Int32, entity.Miqubacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Tmopercodi, DbType.Int32, entity.Tmopercodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mqxtoporden, DbType.Int32, entity.Mqxtoporden));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mqxtopactivo, DbType.Int32, entity.Mqxtopactivo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mqxtopusucreacion, DbType.String, entity.Mqxtopusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mqxtopfeccreacion, DbType.DateTime, entity.Mqxtopfeccreacion));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mqxtopcodi, DbType.Int32, entity.Mqxtopcodi));

            command.ExecuteNonQuery();
        }

        public void Delete(int mqxtopcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mqxtopcodi, DbType.Int32, mqxtopcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiMigraqueryxtipooperacionDTO GetById(int mqxtopcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mqxtopcodi, DbType.Int32, mqxtopcodi);
            SiMigraqueryxtipooperacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiMigraqueryxtipooperacionDTO> List()
        {
            List<SiMigraqueryxtipooperacionDTO> entitys = new List<SiMigraqueryxtipooperacionDTO>();
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

        public List<SiMigraqueryxtipooperacionDTO> GetByCriteria(int miqubacodi)
        {
            List<SiMigraqueryxtipooperacionDTO> entitys = new List<SiMigraqueryxtipooperacionDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, miqubacodi);
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

        public List<SiMigraqueryxtipooperacionDTO> ListarMqxXTipoMigracion(int idTipoMigraOperacion)
        {

            List<SiMigraqueryxtipooperacionDTO> entitys = new List<SiMigraqueryxtipooperacionDTO>();
            string strComando = string.Format(helper.SqlListarMqxXTipoOperacionMigracion, idTipoMigraOperacion);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);
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
