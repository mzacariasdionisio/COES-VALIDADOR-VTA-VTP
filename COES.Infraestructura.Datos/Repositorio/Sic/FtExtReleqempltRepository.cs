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
    /// Clase de acceso a datos de la tabla FT_EXT_RELEQEMPLT
    /// </summary>
    public class FtExtReleqempltRepository : RepositoryBase, IFtExtReleqempltRepository
    {
        public FtExtReleqempltRepository(string strConn) : base(strConn)
        {
        }

        FtExtReleqempltHelper helper = new FtExtReleqempltHelper();

        public int Save(FtExtReleqempltDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ftreqecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Ftreqeestado, DbType.Int32, entity.Ftreqeestado);
            dbProvider.AddInParameter(command, helper.Ftreqeusucreacion, DbType.String, entity.Ftreqeusucreacion);
            dbProvider.AddInParameter(command, helper.Ftreqefeccreacion, DbType.DateTime, entity.Ftreqefeccreacion);
            dbProvider.AddInParameter(command, helper.Ftreqeusumodificacion, DbType.String, entity.Ftreqeusumodificacion);
            dbProvider.AddInParameter(command, helper.Ftreqefecmodificacion, DbType.DateTime, entity.Ftreqefecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int Save(FtExtReleqempltDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftreqecodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equicodi, DbType.Int32, entity.Equicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftreqeestado, DbType.Int32, entity.Ftreqeestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftreqeusucreacion, DbType.String, entity.Ftreqeusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftreqefeccreacion, DbType.DateTime, entity.Ftreqefeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftreqeusumodificacion, DbType.String, entity.Ftreqeusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftreqefecmodificacion, DbType.DateTime, entity.Ftreqefecmodificacion));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void Update(FtExtReleqempltDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);


            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Ftreqeestado, DbType.Int32, entity.Ftreqeestado);
            dbProvider.AddInParameter(command, helper.Ftreqeusucreacion, DbType.String, entity.Ftreqeusucreacion);
            dbProvider.AddInParameter(command, helper.Ftreqefeccreacion, DbType.DateTime, entity.Ftreqefeccreacion);
            dbProvider.AddInParameter(command, helper.Ftreqeusumodificacion, DbType.String, entity.Ftreqeusumodificacion);
            dbProvider.AddInParameter(command, helper.Ftreqefecmodificacion, DbType.DateTime, entity.Ftreqefecmodificacion);
            dbProvider.AddInParameter(command, helper.Ftreqecodi, DbType.Int32, entity.Ftreqecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(FtExtReleqempltDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlUpdate;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equicodi, DbType.Int32, entity.Equicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftreqeestado, DbType.Int32, entity.Ftreqeestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftreqeusucreacion, DbType.String, entity.Ftreqeusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftreqefeccreacion, DbType.DateTime, entity.Ftreqefeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftreqeusumodificacion, DbType.String, entity.Ftreqeusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftreqefecmodificacion, DbType.DateTime, entity.Ftreqefecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftreqecodi, DbType.Int32, entity.Ftreqecodi));


                dbCommand.ExecuteNonQuery();

            }
        }

        public void Delete(int ftreqecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ftreqecodi, DbType.Int32, ftreqecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtReleqempltDTO GetById(int ftreqecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ftreqecodi, DbType.Int32, ftreqecodi);
            FtExtReleqempltDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtReleqempltDTO> List()
        {
            List<FtExtReleqempltDTO> entitys = new List<FtExtReleqempltDTO>();
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

        public List<FtExtReleqempltDTO> GetByCriteria()
        {
            List<FtExtReleqempltDTO> entitys = new List<FtExtReleqempltDTO>();
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

        public List<FtExtReleqempltDTO> ListarPorEquipo(string strEquicodis)
        {
            List<FtExtReleqempltDTO> entitys = new List<FtExtReleqempltDTO>();

            string query = string.Format(helper.SqlListarPorEquipo, strEquicodis);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtReleqempltDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
