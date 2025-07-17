using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CPA_CALCULO_CENTRAL
    /// </summary>
    public class CpaCalculoCentralRepository : RepositoryBase, ICpaCalculoCentralRepository
    {
        readonly CpaCalculoCentralHelper helper = new CpaCalculoCentralHelper();

        public CpaCalculoCentralRepository(string strConn)
            : base(strConn)
        {
        }

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(CpaCalculoCentralDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpacccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cpacecodi, DbType.Int32, entity.Cpacecodi);
            dbProvider.AddInParameter(command, helper.Cpaccodi, DbType.Int32, entity.Cpaccodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi); //(object)entity.Barrcodi ?? DBNull.Value
            dbProvider.AddInParameter(command, helper.Cpacctotenemwh, DbType.Decimal, entity.Cpacctotenemwh);
            dbProvider.AddInParameter(command, helper.Cpacctotenesoles, DbType.Decimal, entity.Cpacctotenesoles);
            dbProvider.AddInParameter(command, helper.Cpacctotpotmwh, DbType.Decimal, entity.Cpacctotpotmwh);
            dbProvider.AddInParameter(command, helper.Cpacctotpotsoles, DbType.Decimal, entity.Cpacctotpotsoles);
            dbProvider.AddInParameter(command, helper.Cpaccusucreacion, DbType.String, entity.Cpaccusucreacion);
            dbProvider.AddInParameter(command, helper.Cpaccfeccreacion, DbType.DateTime, entity.Cpaccfeccreacion);

            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public void Save(CpaCalculoCentralDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpacccodi, DbType.Int32, entity.Cpacccodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpacecodi, DbType.Int32, entity.Cpacecodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpaccodi, DbType.Int32, entity.Cpaccodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpacctotenemwh, DbType.Decimal, entity.Cpacctotenemwh));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpacctotenesoles, DbType.Decimal, entity.Cpacctotenesoles));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpacctotpotmwh, DbType.Decimal, entity.Cpacctotpotmwh));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpacctotpotsoles, DbType.Decimal, entity.Cpacctotpotsoles));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpaccusucreacion, DbType.String, entity.Cpaccusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpaccfeccreacion, DbType.DateTime, entity.Cpaccfeccreacion));
            command.ExecuteNonQuery();
        }

        public void Update(CpaCalculoCentralDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cpacecodi, DbType.Int32, entity.Cpacecodi);
            dbProvider.AddInParameter(command, helper.Cpaccodi, DbType.Int32, entity.Cpaccodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Cpacctotenemwh, DbType.Decimal, entity.Cpacctotenemwh);
            dbProvider.AddInParameter(command, helper.Cpacctotenesoles, DbType.Decimal, entity.Cpacctotenesoles);
            dbProvider.AddInParameter(command, helper.Cpacctotpotmwh, DbType.Decimal, entity.Cpacctotpotmwh);
            dbProvider.AddInParameter(command, helper.Cpacctotpotsoles, DbType.Decimal, entity.Cpacctotpotsoles);
            dbProvider.AddInParameter(command, helper.Cpaccusucreacion, DbType.String, entity.Cpaccusucreacion);
            dbProvider.AddInParameter(command, helper.Cpaccfeccreacion, DbType.DateTime, entity.Cpaccfeccreacion);
            dbProvider.AddInParameter(command, helper.Cpacccodi, DbType.Int32, entity.Cpacccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cpacccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cpacccodi, DbType.Int32, cpacccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByRevision(int cparcodi, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlDeleteByRevision;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparcodi, DbType.Int32, cparcodi));
            command.ExecuteNonQuery();
        }

        public List<CpaCalculoCentralDTO> List()
        {
            List<CpaCalculoCentralDTO> entities = new List<CpaCalculoCentralDTO>();
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

        public CpaCalculoCentralDTO GetById(int cpacccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpacccodi, DbType.Int32, cpacccodi);
            CpaCalculoCentralDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpaCalculoCentralDTO> GetByCriteria(int cparcodi, string cpacemes)
        {
            string query = string.Format(helper.SqlGetByCriteria, cparcodi, cpacemes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<CpaCalculoCentralDTO> entities = new List<CpaCalculoCentralDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpaCalculoCentralDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iBarrBarraTransferencia = dr.GetOrdinal(helper.BarrBarraTransferencia);
                    if (!dr.IsDBNull(iBarrBarraTransferencia)) entity.BarrBarraTransferencia = dr.GetString(iBarrBarraTransferencia);

                    entities.Add(entity);
                }
            }

            return entities;
        }
    }
}
