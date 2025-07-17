using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla FT_EXT_ENVIO_VERSION
    /// </summary>
    public class FtExtEnvioVersionRepository : RepositoryBase, IFtExtEnvioVersionRepository
    {
        public FtExtEnvioVersionRepository(string strConn) : base(strConn)
        {
        }

        FtExtEnvioVersionHelper helper = new FtExtEnvioVersionHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(FtExtEnvioVersionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftevercodi, DbType.Int32, entity.Ftevercodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftevertipo, DbType.Int32, entity.Ftevertipo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteverconexion, DbType.Int32, entity.Fteverconexion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteveroperacion, DbType.Int32, entity.Fteveroperacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteverautoguardado, DbType.String, entity.Fteverautoguardado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteverdescripcion, DbType.String, entity.Fteverdescripcion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteverestado, DbType.String, entity.Fteverestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteverfeccreacion, DbType.DateTime, entity.Fteverfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteverusucreacion, DbType.String, entity.Fteverusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvcodi, DbType.Int32, entity.Ftenvcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Estenvcodi, DbType.Int32, entity.Estenvcodi));

            command.ExecuteNonQuery();
            return entity.Ftevercodi;
        }

        public void Update(FtExtEnvioVersionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlUpdate;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftevertipo, DbType.Int32, entity.Ftevertipo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteverconexion, DbType.Int32, entity.Fteverconexion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteveroperacion, DbType.Int32, entity.Fteveroperacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteverautoguardado, DbType.String, entity.Fteverautoguardado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteverdescripcion, DbType.String, entity.Fteverdescripcion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteverestado, DbType.String, entity.Fteverestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteverfeccreacion, DbType.DateTime, entity.Fteverfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteverusucreacion, DbType.String, entity.Fteverusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvcodi, DbType.Int32, entity.Ftenvcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Estenvcodi, DbType.Int32, entity.Estenvcodi));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftevercodi, DbType.Int32, entity.Ftevercodi));

            command.ExecuteNonQuery();
        }

        public void UpdateListaVersion(int ftenvcodiOrigen, int ftenvcodiDestino, IDbConnection conn, DbTransaction tran)
        {
            string sql = string.Format(helper.SqlUpdateListaVersion, ftenvcodiOrigen, ftenvcodiDestino);

            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = sql;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.ExecuteNonQuery();
        }

        public void Delete(int ftevercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ftevercodi, DbType.Int32, ftevercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtEnvioVersionDTO GetById(int ftevercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ftevercodi, DbType.Int32, ftevercodi);
            FtExtEnvioVersionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtEnvioVersionDTO> List()
        {
            List<FtExtEnvioVersionDTO> entitys = new List<FtExtEnvioVersionDTO>();
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

        public List<FtExtEnvioVersionDTO> GetByCriteria(string ftenvcodis, string tipos)
        {
            List<FtExtEnvioVersionDTO> entitys = new List<FtExtEnvioVersionDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, ftenvcodis, tipos);
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
