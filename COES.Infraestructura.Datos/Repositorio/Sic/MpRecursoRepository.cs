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
    /// Clase de acceso a datos de la tabla MP_RECURSO
    /// </summary>
    public class MpRecursoRepository: RepositoryBase, IMpRecursoRepository
    {
        public MpRecursoRepository(string strConn): base(strConn)
        {
        }

        MpRecursoHelper helper = new MpRecursoHelper();
        
        public int Save(MpRecursoDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = string.Format(helper.SqlGetMaxId, entity.Mtopcodi);
                
                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopcodi, DbType.Int32, entity.Mtopcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mcatcodi, DbType.Int32, entity.Mcatcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurnomb, DbType.String, entity.Mrecurnomb));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurtablasicoes, DbType.String, entity.Mrecurtablasicoes));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurcodisicoes, DbType.Int32, entity.Mrecurcodisicoes));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurlogico, DbType.Int32, entity.Mrecurlogico));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurestado, DbType.Int32, entity.Mrecurestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurpadre, DbType.Int32, entity.Mrecurpadre));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurorigen, DbType.Int32, entity.Mrecurorigen));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurorigen2, DbType.Int32, entity.Mrecurorigen2));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurorigen3, DbType.Int32, entity.Mrecurorigen3));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurorden, DbType.Int32, entity.Mrecurorden));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurusumodificacion, DbType.String, entity.Mrecurusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurfecmodificacion, DbType.DateTime, entity.Mrecurfecmodificacion));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }
        public void Update(MpRecursoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mtopcodi, DbType.Int32, entity.Mtopcodi);
            dbProvider.AddInParameter(command, helper.Mrecurcodi, DbType.Int32, entity.Mrecurcodi);
            dbProvider.AddInParameter(command, helper.Mcatcodi, DbType.Int32, entity.Mcatcodi);
            dbProvider.AddInParameter(command, helper.Mrecurnomb, DbType.String, entity.Mrecurnomb);
            dbProvider.AddInParameter(command, helper.Mrecurtablasicoes, DbType.String, entity.Mrecurtablasicoes);
            dbProvider.AddInParameter(command, helper.Mrecurcodisicoes, DbType.Int32, entity.Mrecurcodisicoes);
            dbProvider.AddInParameter(command, helper.Mrecurlogico, DbType.Int32, entity.Mrecurlogico);
            dbProvider.AddInParameter(command, helper.Mrecurestado, DbType.Int32, entity.Mrecurestado);
            dbProvider.AddInParameter(command, helper.Mrecurpadre, DbType.Int32, entity.Mrecurpadre);
            dbProvider.AddInParameter(command, helper.Mrecurorigen, DbType.Int32, entity.Mrecurorigen);
            dbProvider.AddInParameter(command, helper.Mrecurorigen2, DbType.Int32, entity.Mrecurorigen2);
            dbProvider.AddInParameter(command, helper.Mrecurorigen3, DbType.Int32, entity.Mrecurorigen3);
            dbProvider.AddInParameter(command, helper.Mrecurorden, DbType.Int32, entity.Mrecurorden);
            dbProvider.AddInParameter(command, helper.Mrecurusumodificacion, DbType.String, entity.Mrecurusumodificacion);
            dbProvider.AddInParameter(command, helper.Mrecurfecmodificacion, DbType.DateTime, entity.Mrecurfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mtopcodi, int mrecurcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mtopcodi, DbType.Int32, mtopcodi);
            dbProvider.AddInParameter(command, helper.Mrecurcodi, DbType.Int32, mrecurcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MpRecursoDTO GetById(int mtopcodi, int mrecurcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mtopcodi, DbType.Int32, mtopcodi);
            dbProvider.AddInParameter(command, helper.Mrecurcodi, DbType.Int32, mrecurcodi);
            MpRecursoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MpRecursoDTO> List()
        {
            List<MpRecursoDTO> entitys = new List<MpRecursoDTO>();
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

        public List<MpRecursoDTO> GetByCriteria()
        {
            List<MpRecursoDTO> entitys = new List<MpRecursoDTO>();
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

        
        public List<MpRecursoDTO> ListarRecursosPorTopologia(int mtopcodi)
        {
            List<MpRecursoDTO> entitys = new List<MpRecursoDTO>();
            string query = string.Format(helper.SqlListarRecursosByTopologia, mtopcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public int SaveCopia(MpRecursoDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopcodi, DbType.Int32, entity.Mtopcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurcodi, DbType.Int32, entity.Mrecurcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mcatcodi, DbType.Int32, entity.Mcatcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurnomb, DbType.String, entity.Mrecurnomb));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurtablasicoes, DbType.String, entity.Mrecurtablasicoes));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurcodisicoes, DbType.Int32, entity.Mrecurcodisicoes));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurlogico, DbType.Int32, entity.Mrecurlogico));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurestado, DbType.Int32, entity.Mrecurestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurpadre, DbType.Int32, entity.Mrecurpadre));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurorigen, DbType.Int32, entity.Mrecurorigen));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurorigen2, DbType.Int32, entity.Mrecurorigen2));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurorigen3, DbType.Int32, entity.Mrecurorigen3));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurorden, DbType.Int32, entity.Mrecurorden));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurusumodificacion, DbType.String, entity.Mrecurusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurfecmodificacion, DbType.DateTime, entity.Mrecurfecmodificacion));

                dbCommand.ExecuteNonQuery();
                return entity.Mrecurcodi;
            }
        }

        public void Update(MpRecursoDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlUpdate;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mcatcodi, DbType.Int32, entity.Mcatcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurnomb, DbType.String, entity.Mrecurnomb));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurtablasicoes, DbType.String, entity.Mrecurtablasicoes));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurcodisicoes, DbType.Int32, entity.Mrecurcodisicoes));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurlogico, DbType.Int32, entity.Mrecurlogico));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurestado, DbType.Int32, entity.Mrecurestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurpadre, DbType.Int32, entity.Mrecurpadre));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurorigen, DbType.Int32, entity.Mrecurorigen));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurorigen2, DbType.Int32, entity.Mrecurorigen2));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurorigen3, DbType.Int32, entity.Mrecurorigen3));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurorden, DbType.Int32, entity.Mrecurorden));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurusumodificacion, DbType.String, entity.Mrecurusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurfecmodificacion, DbType.DateTime, entity.Mrecurfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopcodi, DbType.Int32, entity.Mtopcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurcodi, DbType.Int32, entity.Mrecurcodi));

                dbCommand.ExecuteNonQuery();
            }
        }

        public void UpdateOrdenCentral(int orden, int topcodi, int recurcodi)
        {
            string sql = string.Format(helper.SqlUpdateOrden, orden, topcodi, recurcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            dbProvider.ExecuteNonQuery(command);
            //using (var dbCommand = (DbCommand)connection.CreateCommand())
            //{
            //    dbCommand.Transaction = (DbTransaction)transaction;
            //    dbCommand.Connection = (DbConnection)connection;

            //    var query = string.Format(helper.SqlUpdateOrden, orden, sddpCodi);
            //    dbCommand.CommandText = query;

            //    dbCommand.ExecuteNonQuery();
            //}
        }
    }
}
