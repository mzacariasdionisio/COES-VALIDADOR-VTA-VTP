using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CPA_AJUSTEPRESUPUESTAL
    /// </summary>
    public class CpaAjustePresupuestalRepository : RepositoryBase, ICpaAjustePresupuestalRepository
    {
        private readonly string strConexion;
        readonly CpaAjustePresupuestalHelper helper = new CpaAjustePresupuestalHelper();

        public CpaAjustePresupuestalRepository(string strConn)
            : base(strConn)
        {
            strConexion = strConn;
        }

        public IDbConnection BeginConnection()
        {
            Database db = DatabaseFactory.CreateDatabase(strConexion);
            IDbConnection conn = db.CreateConnection();
            conn.Open();
            return conn;
        }

        public DbTransaction StartTransaction(IDbConnection conn)
        {
            return (DbTransaction)conn.BeginTransaction();
        }

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(CpaAjustePresupuestalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpaapcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cpaapanio, DbType.Int32, entity.Cpaapanio);
            dbProvider.AddInParameter(command, helper.Cpaapajuste, DbType.String, entity.Cpaapajuste);
            dbProvider.AddInParameter(command, helper.Cpaapanioejercicio, DbType.Int32, entity.Cpaapanioejercicio);
            dbProvider.AddInParameter(command, helper.Cpaapusucreacion, DbType.String, entity.Cpaapusucreacion);
            dbProvider.AddInParameter(command, helper.Cpaapfeccreacion, DbType.DateTime, entity.Cpaapfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Save(CpaAjustePresupuestalDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpaapcodi, DbType.Int32, entity.Cpaapcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpaapanio, DbType.Int32, entity.Cpaapanio));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpaapajuste, DbType.String, entity.Cpaapajuste));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpaapanioejercicio, DbType.Int32, entity.Cpaapanioejercicio));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpaapusucreacion, DbType.String, entity.Cpaapusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpaapfeccreacion, DbType.DateTime, entity.Cpaapfeccreacion));
            command.ExecuteNonQuery();
        }

        public void Update(CpaAjustePresupuestalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cpaapanio, DbType.Int32, entity.Cpaapanio);
            dbProvider.AddInParameter(command, helper.Cpaapajuste, DbType.String, entity.Cpaapajuste);
            dbProvider.AddInParameter(command, helper.Cpaapanioejercicio, DbType.Int32, entity.Cpaapanioejercicio);
            dbProvider.AddInParameter(command, helper.Cpaapusucreacion, DbType.String, entity.Cpaapusucreacion);
            dbProvider.AddInParameter(command, helper.Cpaapfeccreacion, DbType.DateTime, entity.Cpaapfeccreacion);
            dbProvider.AddInParameter(command, helper.Cpaapcodi, DbType.Int32, entity.Cpaapcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cpaapcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cpaapcodi, DbType.Int32, cpaapcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<CpaAjustePresupuestalDTO> List()
        {
            List<CpaAjustePresupuestalDTO> entities = new List<CpaAjustePresupuestalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }

        public CpaAjustePresupuestalDTO GetById(int cpaapcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpaapcodi, DbType.Int32, cpaapcodi);
            CpaAjustePresupuestalDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpaAjustePresupuestalDTO> GetByCriteria(int cpaapanio)
        {
            string query = string.Format(helper.SqlGetByCriteria, cpaapanio);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<CpaAjustePresupuestalDTO> entitys = new List<CpaAjustePresupuestalDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpaAjustePresupuestalDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
