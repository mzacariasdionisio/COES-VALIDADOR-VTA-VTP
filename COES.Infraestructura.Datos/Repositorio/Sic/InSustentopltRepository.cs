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
    /// Clase de acceso a datos de la tabla IN_SUSTENTOPLT
    /// </summary>
    public class InSustentopltRepository : RepositoryBase, IInSustentopltRepository
    {
        public InSustentopltRepository(string strConn) : base(strConn)
        {
        }

        InSustentopltHelper helper = new InSustentopltHelper();

        public int Save(InSustentopltDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Inpstcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Inpsttipo, DbType.Int32, entity.Inpsttipo);
            dbProvider.AddInParameter(command, helper.Inpstnombre, DbType.String, entity.Inpstnombre);
            dbProvider.AddInParameter(command, helper.Inpstestado, DbType.String, entity.Inpstestado);
            dbProvider.AddInParameter(command, helper.Inpstusumodificacion, DbType.String, entity.Inpstusumodificacion);
            dbProvider.AddInParameter(command, helper.Inpstfecmodificacion, DbType.DateTime, entity.Inpstfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(InSustentopltDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Inpstcodi, DbType.Int32, entity.Inpstcodi);
            dbProvider.AddInParameter(command, helper.Inpsttipo, DbType.Int32, entity.Inpsttipo);
            dbProvider.AddInParameter(command, helper.Inpstnombre, DbType.String, entity.Inpstnombre);
            dbProvider.AddInParameter(command, helper.Inpstestado, DbType.String, entity.Inpstestado);
            dbProvider.AddInParameter(command, helper.Inpstusumodificacion, DbType.String, entity.Inpstusumodificacion);
            dbProvider.AddInParameter(command, helper.Inpstfecmodificacion, DbType.DateTime, entity.Inpstfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int inpstcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Inpstcodi, DbType.Int32, inpstcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public InSustentopltDTO GetById(int inpstcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Inpstcodi, DbType.Int32, inpstcodi);
            InSustentopltDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public InSustentopltDTO GetVigenteByTipo(int tipo)
        {
            string sql = string.Format(helper.SqlGetVigenteByTipo, tipo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            InSustentopltDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<InSustentopltDTO> List()
        {
            List<InSustentopltDTO> entitys = new List<InSustentopltDTO>();
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

        public List<InSustentopltDTO> GetByCriteria()
        {
            List<InSustentopltDTO> entitys = new List<InSustentopltDTO>();
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

        /// <summary>
        /// Guardado transaccional
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int Save(InSustentopltDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Inpstcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Inpsttipo, DbType.Int32, entity.Inpsttipo));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Inpstnombre, DbType.String, entity.Inpstnombre));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Inpstestado, DbType.String, entity.Inpstestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Inpstusumodificacion, DbType.String, entity.Inpstusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Inpstfecmodificacion, DbType.DateTime, entity.Inpstfecmodificacion));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void UpdateEstadoPlantilla(InSustentopltDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                var query = string.Format(helper.SqlUpdateEstado, entity.Inpstestado, entity.Inpstcodi);
                dbCommand.CommandText = query;

                dbCommand.ExecuteNonQuery();
            }
        }

    }
}
