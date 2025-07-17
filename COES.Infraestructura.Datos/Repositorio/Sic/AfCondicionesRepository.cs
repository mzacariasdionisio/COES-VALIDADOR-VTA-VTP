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
    /// Clase de acceso a datos de la tabla AF_CONDICIONES
    /// </summary>
    public class AfCondicionesRepository: RepositoryBase, IAfCondicionesRepository
    {
        public AfCondicionesRepository(string strConn): base(strConn)
        {
        }

        AfCondicionesHelper helper = new AfCondicionesHelper();

        public int Save(AfCondicionesDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Afcondcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Afcondfecmodificacion, DbType.DateTime, entity.Afcondfecmodificacion);
            dbProvider.AddInParameter(command, helper.Afcondusumodificacion, DbType.String, entity.Afcondusumodificacion);
            dbProvider.AddInParameter(command, helper.Afcondfeccreacion, DbType.DateTime, entity.Afcondfeccreacion);
            dbProvider.AddInParameter(command, helper.Afcondusucreacion, DbType.String, entity.Afcondusucreacion);
            dbProvider.AddInParameter(command, helper.Afcondestado, DbType.Int32, entity.Afcondestado);
            dbProvider.AddInParameter(command, helper.Afcondzona, DbType.String, entity.Afcondzona);
            dbProvider.AddInParameter(command, helper.Afcondnumetapa, DbType.Int32, entity.Afcondnumetapa);
            dbProvider.AddInParameter(command, helper.Afcondfuncion, DbType.String, entity.Afcondfuncion);
            dbProvider.AddInParameter(command, helper.Afecodi, DbType.Int32, entity.Afecodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AfCondicionesDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Afcondfecmodificacion, DbType.DateTime, entity.Afcondfecmodificacion);
            dbProvider.AddInParameter(command, helper.Afcondusumodificacion, DbType.String, entity.Afcondusumodificacion);
            dbProvider.AddInParameter(command, helper.Afcondfeccreacion, DbType.DateTime, entity.Afcondfeccreacion);
            dbProvider.AddInParameter(command, helper.Afcondusucreacion, DbType.String, entity.Afcondusucreacion);
            dbProvider.AddInParameter(command, helper.Afcondestado, DbType.Int32, entity.Afcondestado);
            dbProvider.AddInParameter(command, helper.Afcondzona, DbType.String, entity.Afcondzona);
            dbProvider.AddInParameter(command, helper.Afcondnumetapa, DbType.Int32, entity.Afcondnumetapa);
            dbProvider.AddInParameter(command, helper.Afcondfuncion, DbType.String, entity.Afcondfuncion);
            dbProvider.AddInParameter(command, helper.Afcondcodi, DbType.Int32, entity.Afcondcodi);
            dbProvider.AddInParameter(command, helper.Afecodi, DbType.Int32, entity.Afecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int afcondcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Afcondcodi, DbType.Int32, afcondcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public AfCondicionesDTO GetById(int afcondcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Afcondcodi, DbType.Int32, afcondcodi);
            AfCondicionesDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<AfCondicionesDTO> List()
        {
            List<AfCondicionesDTO> entitys = new List<AfCondicionesDTO>();
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

        public List<AfCondicionesDTO> GetByCriteria()
        {
            List<AfCondicionesDTO> entitys = new List<AfCondicionesDTO>();
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


        #region Intranet CTAF

        public List<AfCondicionesDTO> ListByAfecodi(int afecodi)
        {
            List<AfCondicionesDTO> entitys = new List<AfCondicionesDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByAfecodi);
            dbProvider.AddInParameter(command, helper.Afecodi, DbType.Int32, afecodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public void DeleteByAfecodi(int afecodi, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlDeleteByAfecodi;
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Afecodi, DbType.Int32, afecodi));
                dbCommand.ExecuteNonQuery();
            }
        }

        public int Save(AfCondicionesDTO entity, IDbConnection connection, IDbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Afcondfecmodificacion, DbType.DateTime, entity.Afcondfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Afcondusumodificacion, DbType.String, entity.Afcondusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Afcondfeccreacion, DbType.DateTime, entity.Afcondfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Afcondusucreacion, DbType.String, entity.Afcondusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Afcondestado, DbType.Int32, entity.Afcondestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Afcondzona, DbType.String, entity.Afcondzona));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Afcondnumetapa, DbType.Int32, entity.Afcondnumetapa));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Afcondfuncion, DbType.String, entity.Afcondfuncion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Afcondcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Afecodi, DbType.Int32, entity.Afecodi));

                dbCommand.ExecuteNonQuery();
                dbCommand.Dispose();
                return id;
            }
        }

        #endregion

    }
}
