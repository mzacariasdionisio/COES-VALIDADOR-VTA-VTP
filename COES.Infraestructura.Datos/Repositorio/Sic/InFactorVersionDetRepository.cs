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
    /// Clase de acceso a datos de la tabla IN_FACTOR_VERSION_DET
    /// </summary>
    public class InFactorVersionDetRepository : RepositoryBase, IInFactorVersionDetRepository
    {
        public InFactorVersionDetRepository(string strConn) : base(strConn)
        {
        }

        InFactorVersionDetHelper helper = new InFactorVersionDetHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(InFactorVersionDetDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infvdtcodi, DbType.Int32, entity.Infvdtcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infvdtintercodis, DbType.String, entity.Infvdtintercodis));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infvdthorizonte, DbType.String, entity.Infvdthorizonte));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infvercodi, DbType.Int32, entity.Infvercodi));

            command.ExecuteNonQuery();
            return entity.Infvdtcodi;
        }

        public void Update(InFactorVersionDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Infvdtcodi, DbType.Int32, entity.Infvdtcodi);
            dbProvider.AddInParameter(command, helper.Infvdtintercodis, DbType.String, entity.Infvdtintercodis);
            dbProvider.AddInParameter(command, helper.Infvdthorizonte, DbType.String, entity.Infvdthorizonte);
            dbProvider.AddInParameter(command, helper.Infvercodi, DbType.Int32, entity.Infvercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int infvdtcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Infvdtcodi, DbType.Int32, infvdtcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public InFactorVersionDetDTO GetById(int infvdtcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Infvdtcodi, DbType.Int32, infvdtcodi);
            InFactorVersionDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<InFactorVersionDetDTO> List()
        {
            List<InFactorVersionDetDTO> entitys = new List<InFactorVersionDetDTO>();
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

        public List<InFactorVersionDetDTO> ListxInfvercodi(int infvercodi)
        {
            List<InFactorVersionDetDTO> entitys = new List<InFactorVersionDetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListxInfvercodi);

            dbProvider.AddInParameter(command, helper.Infvercodi, DbType.Int32, infvercodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        
        public List<InFactorVersionDetDTO> GetByCriteria()
        {
            List<InFactorVersionDetDTO> entitys = new List<InFactorVersionDetDTO>();
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
    }
}
