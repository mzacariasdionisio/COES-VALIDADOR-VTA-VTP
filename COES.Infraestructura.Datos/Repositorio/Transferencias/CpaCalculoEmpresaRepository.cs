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
    /// Clase de acceso a datos de la tabla CPA_CALCULO_EMPRESA
    /// </summary>
    public class CpaCalculoEmpresaRepository : RepositoryBase, ICpaCalculoEmpresaRepository
    {
        readonly CpaCalculoEmpresaHelper helper = new CpaCalculoEmpresaHelper();

        public CpaCalculoEmpresaRepository(string strConn)
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

        public int Save(CpaCalculoEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpacecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cpaccodi, DbType.Int32, entity.Cpaccodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Cpacetipo, DbType.String, entity.Cpacetipo);
            dbProvider.AddInParameter(command, helper.Cpacemes, DbType.Int32, entity.Cpacemes);
            dbProvider.AddInParameter(command, helper.Cpacetotenemwh, DbType.Decimal, entity.Cpacetotenemwh);
            dbProvider.AddInParameter(command, helper.Cpacetotenesoles, DbType.Decimal, entity.Cpacetotenesoles);
            dbProvider.AddInParameter(command, helper.Cpacetotpotmwh, DbType.Decimal, entity.Cpacetotpotmwh);
            dbProvider.AddInParameter(command, helper.Cpacetotpotsoles, DbType.Decimal, entity.Cpacetotpotsoles);
            dbProvider.AddInParameter(command, helper.Cpaceusucreacion, DbType.String, entity.Cpaceusucreacion);
            dbProvider.AddInParameter(command, helper.Cpacefeccreacion, DbType.DateTime, entity.Cpacefeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Save(CpaCalculoEmpresaDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpacecodi, DbType.Int32, entity.Cpacecodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpaccodi, DbType.Int32, entity.Cpaccodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpacetipo, DbType.String, entity.Cpacetipo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpacemes, DbType.Int32, entity.Cpacemes));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpacetotenemwh, DbType.Decimal, entity.Cpacetotenemwh));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpacetotenesoles, DbType.Decimal, entity.Cpacetotenesoles));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpacetotpotmwh, DbType.Decimal, entity.Cpacetotpotmwh));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpacetotpotsoles, DbType.Decimal, entity.Cpacetotpotsoles));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpaceusucreacion, DbType.String, entity.Cpaceusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpacefeccreacion, DbType.DateTime, entity.Cpacefeccreacion));

            command.ExecuteNonQuery();
        }

        public void Update(CpaCalculoEmpresaDTO entity)
        { 
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cpaccodi, DbType.Int32, entity.Cpaccodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Cpacetipo, DbType.String, entity.Cpacetipo);
            dbProvider.AddInParameter(command, helper.Cpacemes, DbType.Int32, entity.Cpacemes);
            dbProvider.AddInParameter(command, helper.Cpacetotenemwh, DbType.Decimal, entity.Cpacetotenemwh);
            dbProvider.AddInParameter(command, helper.Cpacetotenesoles, DbType.Decimal, entity.Cpacetotenesoles);
            dbProvider.AddInParameter(command, helper.Cpacetotpotmwh, DbType.Decimal, entity.Cpacetotpotmwh);
            dbProvider.AddInParameter(command, helper.Cpacetotpotsoles, DbType.Decimal, entity.Cpacetotpotsoles);
            dbProvider.AddInParameter(command, helper.Cpaceusucreacion, DbType.String, entity.Cpaceusucreacion);
            dbProvider.AddInParameter(command, helper.Cpacefeccreacion, DbType.DateTime, entity.Cpacefeccreacion);
            dbProvider.AddInParameter(command, helper.Cpacecodi, DbType.Int32, entity.Cpacecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cpacecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cpacecodi, DbType.Int32, cpacecodi);

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

        public List<CpaCalculoEmpresaDTO> List()
        {
            List<CpaCalculoEmpresaDTO> entities = new List<CpaCalculoEmpresaDTO>();
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

        public CpaCalculoEmpresaDTO GetById(int cpacecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpacecodi, DbType.Int32, cpacecodi);
            CpaCalculoEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpaCalculoEmpresaDTO> GetByCriteria(int cparcodi, string cpacemes)
        {
            string query = string.Format(helper.SqlGetByCriteria, cparcodi, cpacemes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<CpaCalculoEmpresaDTO> entities = new List<CpaCalculoEmpresaDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpaCalculoEmpresaDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entities.Add(entity);
                }
            }

            return entities;
        }

    }
}
