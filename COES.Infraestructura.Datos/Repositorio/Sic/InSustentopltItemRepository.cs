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
    /// Clase de acceso a datos de la tabla IN_SUSTENTOPLT_ITEM
    /// </summary>
    public class InSustentopltItemRepository : RepositoryBase, IInSustentopltItemRepository
    {
        public InSustentopltItemRepository(string strConn) : base(strConn)
        {
        }

        InSustentopltItemHelper helper = new InSustentopltItemHelper();

        public int Save(InSustentopltItemDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Inpsticodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Inpstidesc, DbType.String, entity.Inpstidesc);
            dbProvider.AddInParameter(command, helper.Inpstcodi, DbType.Int32, entity.Inpstcodi);
            dbProvider.AddInParameter(command, helper.Inpstiorden, DbType.Int32, entity.Inpstiorden);
            dbProvider.AddInParameter(command, helper.Inpstitipo, DbType.Int32, entity.Inpstitipo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(InSustentopltItemDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Inpstidesc, DbType.String, entity.Inpstidesc);
            dbProvider.AddInParameter(command, helper.Inpstcodi, DbType.Int32, entity.Inpstcodi);
            dbProvider.AddInParameter(command, helper.Inpstiorden, DbType.Int32, entity.Inpstiorden);
            dbProvider.AddInParameter(command, helper.Inpstitipo, DbType.Int32, entity.Inpstitipo);
            dbProvider.AddInParameter(command, helper.Inpsticodi, DbType.Int32, entity.Inpsticodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int inpsticodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Inpsticodi, DbType.Int32, inpsticodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public InSustentopltItemDTO GetById(int inpsticodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Inpsticodi, DbType.Int32, inpsticodi);
            InSustentopltItemDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<InSustentopltItemDTO> List()
        {
            List<InSustentopltItemDTO> entitys = new List<InSustentopltItemDTO>();
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

        public List<InSustentopltItemDTO> GetByCriteria(int inpstcodi)
        {
            List<InSustentopltItemDTO> entitys = new List<InSustentopltItemDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, inpstcodi);
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

        /// <summary>
        /// Guardado transaccional
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int Save(InSustentopltItemDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Inpsticodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Inpstidesc, DbType.String, entity.Inpstidesc));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Inpstcodi, DbType.Int32, entity.Inpstcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Inpstiorden, DbType.Int32, entity.Inpstiorden));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Inpstitipo, DbType.Int32, entity.Inpstitipo));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void UpdateOrdenRequisito(int orden, int inpsticodi)
        {
            string sql = string.Format(helper.SqlUpdateOrden, orden, inpsticodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            dbProvider.ExecuteNonQuery(command);
        }

    }
}
