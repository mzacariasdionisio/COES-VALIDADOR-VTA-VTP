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
    /// Clase de acceso a datos de la tabla SI_MIGRAQUERYBASE
    /// </summary>
    public class SiMigraquerybaseRepository : RepositoryBase, ISiMigraquerybaseRepository
    {
        public SiMigraquerybaseRepository(string strConn) : base(strConn)
        {
        }

        SiMigraquerybaseHelper helper = new SiMigraquerybaseHelper();

        public int Save(SiMigraquerybaseDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubacodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqplacodi, DbType.Int32, entity.Miqplacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubaquery, DbType.String, entity.Miqubaquery));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubanomtabla, DbType.String, entity.Miqubanomtabla));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubamensaje, DbType.String, entity.Miqubamensaje));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubaflag, DbType.Int32, entity.Miqubaflag));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubaactivo, DbType.Int32, entity.Miqubaactivo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubastr, DbType.String, entity.Miqubastr));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubaflagtbladicional, DbType.String, entity.Miqubaflagtbladicional));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubausucreacion, DbType.String, entity.Miqubausucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubafeccreacion, DbType.DateTime, entity.Miqubafeccreacion));

            command.ExecuteNonQuery();

            return id;
        }

        public void Update(SiMigraquerybaseDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlUpdate;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqplacodi, DbType.Int32, entity.Miqplacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubaquery, DbType.String, entity.Miqubaquery));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubanomtabla, DbType.String, entity.Miqubanomtabla));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubamensaje, DbType.String, entity.Miqubamensaje));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubaflag, DbType.String, entity.Miqubaflag));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubaactivo, DbType.Int32, entity.Miqubaactivo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubastr, DbType.String, entity.Miqubastr));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubaflagtbladicional, DbType.String, entity.Miqubaflagtbladicional));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubausucreacion, DbType.String, entity.Miqubausucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubafeccreacion, DbType.DateTime, entity.Miqubafeccreacion));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Miqubacodi, DbType.Int32, entity.Miqubacodi));

            command.ExecuteNonQuery();
        }

        public void Delete(int miqubacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Miqubacodi, DbType.Int32, miqubacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiMigraquerybaseDTO GetById(int miqubacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Miqubacodi, DbType.Int32, miqubacodi);
            SiMigraquerybaseDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiMigraquerybaseDTO> List()
        {
            List<SiMigraquerybaseDTO> entitys = new List<SiMigraquerybaseDTO>();
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

        public List<SiMigraquerybaseDTO> GetByCriteria()
        {
            List<SiMigraquerybaseDTO> entitys = new List<SiMigraquerybaseDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iMiqplanomb = dr.GetOrdinal(helper.Miqplanomb);
                    if (!dr.IsDBNull(iMiqplanomb)) entity.Miqplanomb = dr.GetString(iMiqplanomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<SiMigraquerybaseDTO> ListarSiQueryXMigraQueryTipo(int idTipoOperacionMigracion)
        {
            List<SiMigraquerybaseDTO> entitys = new List<SiMigraquerybaseDTO>();
            string strComando = string.Format(helper.SqlListarMigraQueryXTipoOperacion, idTipoOperacionMigracion);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiMigraquerybaseDTO entity = helper.Create(dr);

                    int iMqxtopcodi = dr.GetOrdinal(helper.Mqxtopcodi);
                    if (!dr.IsDBNull(iMqxtopcodi)) entity.Moxtopcodi = Convert.ToInt32(dr.GetValue(iMqxtopcodi));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

    }
}
