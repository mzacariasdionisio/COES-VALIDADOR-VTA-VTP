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
    /// Clase de acceso a datos de la tabla PMO_QN_ENVIO
    /// </summary>
    public class PmoQnEnvioRepository : RepositoryBase, IPmoQnEnvioRepository
    {
        public PmoQnEnvioRepository(string strConn) : base(strConn)
        {
        }

        PmoQnEnvioHelper helper = new PmoQnEnvioHelper();

        public int Save(PmoQnEnvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Qnbenvcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Qnbenvanho, DbType.Int32, entity.Qnbenvanho);
            dbProvider.AddInParameter(command, helper.Qnbenvnomb, DbType.String, entity.Qnbenvnomb);
            dbProvider.AddInParameter(command, helper.Qnbenvestado, DbType.Int32, entity.Qnbenvestado);
            dbProvider.AddInParameter(command, helper.Qnbenvversion, DbType.Int32, entity.Qnbenvversion);
            dbProvider.AddInParameter(command, helper.Qnbenvfechaperiodo, DbType.DateTime, entity.Qnbenvfechaperiodo);
            dbProvider.AddInParameter(command, helper.Qnbenvusucreacion, DbType.String, entity.Qnbenvusucreacion);
            dbProvider.AddInParameter(command, helper.Qnbenvfeccreacion, DbType.DateTime, entity.Qnbenvfeccreacion);
            dbProvider.AddInParameter(command, helper.Qnbenvusumodificacion, DbType.String, entity.Qnbenvusumodificacion);
            dbProvider.AddInParameter(command, helper.Qnbenvfecmodificacion, DbType.DateTime, entity.Qnbenvfecmodificacion);
            dbProvider.AddInParameter(command, helper.Qnlectcodi, DbType.Int32, entity.Qnlectcodi);
            dbProvider.AddInParameter(command, helper.Qncfgecodi, DbType.Int32, entity.Qncfgecodi);
            dbProvider.AddInParameter(command, helper.Qnbenvidentificador, DbType.Int32, entity.Qnbenvidentificador);
            dbProvider.AddInParameter(command, helper.Qnbenvdeleted, DbType.Int32, entity.Qnbenvdeleted);
            dbProvider.AddInParameter(command, helper.Qnbenvbase, DbType.Int32, entity.Qnbenvbase);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PmoQnEnvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Qnbenvanho, DbType.Int32, entity.Qnbenvanho);
            dbProvider.AddInParameter(command, helper.Qnbenvnomb, DbType.String, entity.Qnbenvnomb);
            dbProvider.AddInParameter(command, helper.Qnbenvestado, DbType.Int32, entity.Qnbenvestado);
            dbProvider.AddInParameter(command, helper.Qnbenvversion, DbType.Int32, entity.Qnbenvversion);
            dbProvider.AddInParameter(command, helper.Qnbenvfechaperiodo, DbType.DateTime, entity.Qnbenvfechaperiodo);
            dbProvider.AddInParameter(command, helper.Qnbenvusucreacion, DbType.String, entity.Qnbenvusucreacion);
            dbProvider.AddInParameter(command, helper.Qnbenvfeccreacion, DbType.DateTime, entity.Qnbenvfeccreacion);
            dbProvider.AddInParameter(command, helper.Qnbenvusumodificacion, DbType.String, entity.Qnbenvusumodificacion);
            dbProvider.AddInParameter(command, helper.Qnbenvfecmodificacion, DbType.DateTime, entity.Qnbenvfecmodificacion);
            dbProvider.AddInParameter(command, helper.Qnlectcodi, DbType.Int32, entity.Qnlectcodi);
            dbProvider.AddInParameter(command, helper.Qncfgecodi, DbType.Int32, entity.Qncfgecodi);
            dbProvider.AddInParameter(command, helper.Qnbenvidentificador, DbType.Int32, entity.Qnbenvidentificador);
            dbProvider.AddInParameter(command, helper.Qnbenvdeleted, DbType.Int32, entity.Qnbenvdeleted);
            dbProvider.AddInParameter(command, helper.Qnbenvbase, DbType.Int32, entity.Qnbenvbase);
            dbProvider.AddInParameter(command, helper.Qnbenvcodi, DbType.Int32, entity.Qnbenvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int qnbenvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Qnbenvcodi, DbType.Int32, qnbenvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PmoQnEnvioDTO GetById(int qnbenvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Qnbenvcodi, DbType.Int32, qnbenvcodi);
            PmoQnEnvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PmoQnEnvioDTO> List()
        {
            List<PmoQnEnvioDTO> entitys = new List<PmoQnEnvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            PmoQnEnvioDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                //while (dr.Read())
                //{
                //    entitys.Add(helper.Create(dr));
                //}

                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iQnlectnomb = dr.GetOrdinal(helper.Qnlectnomb);
                    if (!dr.IsDBNull(iQnlectnomb)) entity.Qnlectnomb = dr.GetString(iQnlectnomb);

                    entitys.Add(entity);
                }

            }

            return entitys;
        }

        public List<PmoQnEnvioDTO> GetByCriteria(int anio, int tipo)
        {
            List<PmoQnEnvioDTO> entitys = new List<PmoQnEnvioDTO>();
            //DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            string sql = string.Format(helper.SqlGetByCriteria, anio, tipo);
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

        public int Save(PmoQnEnvioDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnbenvcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnbenvanho, DbType.Int32, entity.Qnbenvanho));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnbenvnomb, DbType.String, entity.Qnbenvnomb));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnbenvestado, DbType.Int32, entity.Qnbenvestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnbenvversion, DbType.Int32, entity.Qnbenvversion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnbenvfechaperiodo, DbType.DateTime, entity.Qnbenvfechaperiodo));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnbenvusucreacion, DbType.String, entity.Qnbenvusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnbenvfeccreacion, DbType.DateTime, entity.Qnbenvfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnbenvusumodificacion, DbType.String, entity.Qnbenvusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnbenvfecmodificacion, DbType.DateTime, entity.Qnbenvfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnlectcodi, DbType.Int32, entity.Qnlectcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qncfgecodi, DbType.Int32, entity.Qncfgecodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnbenvidentificador, DbType.Int32, entity.Qnbenvidentificador));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnbenvdeleted, DbType.Int32, entity.Qnbenvdeleted));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnbenvbase, DbType.Int32, entity.Qnbenvbase));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void UpdateBajaEnvio(int codigoEnvio, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                var query = string.Format(helper.SqlUpdateEstadoBaja, codigoEnvio);
                dbCommand.CommandText = query;

                dbCommand.ExecuteNonQuery();
            }
        }

        public void UpdateVigente(PmoQnEnvioDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlUpdateAprobar;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnbenvestado, DbType.Int32, entity.Qnbenvestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnbenvusumodificacion, DbType.String, entity.Qnbenvusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnbenvfecmodificacion, DbType.DateTime, entity.Qnbenvfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnbenvcodi, DbType.Int32, entity.Qnbenvcodi));

                dbCommand.ExecuteNonQuery();

            }
        }

        public void UpdateOficial(PmoQnEnvioDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlUpdateOficial;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnbenvidentificador, DbType.Int32, entity.Qnbenvidentificador));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnbenvusumodificacion, DbType.String, entity.Qnbenvusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnbenvfecmodificacion, DbType.DateTime, entity.Qnbenvfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnbenvcodi, DbType.Int32, entity.Qnbenvcodi));

                dbCommand.ExecuteNonQuery();

            }
        }
    }
}
