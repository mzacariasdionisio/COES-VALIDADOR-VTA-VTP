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
    /// Clase de acceso a datos de la tabla IND_INSUMO_LOG
    /// </summary>
    public class IndInsumoLogRepository : RepositoryBase, IIndInsumoLogRepository
    {
        public IndInsumoLogRepository(string strConn) : base(strConn)
        {
        }

        IndInsumoLogHelper helper = new IndInsumoLogHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(IndInsumoLogDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ilogcodi, DbType.Int32, entity.Ilogcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptcodi, DbType.Int32, entity.Irptcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Iloginsumo, DbType.Int32, entity.Iloginsumo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ilogcodigo, DbType.Int32, entity.Ilogcodigo));

            command.ExecuteNonQuery();
            return entity.Ilogcodi;
        }

        public void Update(IndInsumoLogDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ilogcodi, DbType.Int32, entity.Ilogcodi);
            dbProvider.AddInParameter(command, helper.Irptcodi, DbType.Int32, entity.Irptcodi);
            dbProvider.AddInParameter(command, helper.Iloginsumo, DbType.Int32, entity.Iloginsumo);
            dbProvider.AddInParameter(command, helper.Ilogcodigo, DbType.Int32, entity.Ilogcodigo);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ilogcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ilogcodi, DbType.Int32, ilogcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public IndInsumoLogDTO GetById(int ilogcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ilogcodi, DbType.Int32, ilogcodi);
            IndInsumoLogDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<IndInsumoLogDTO> List()
        {
            List<IndInsumoLogDTO> entitys = new List<IndInsumoLogDTO>();
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

        public List<IndInsumoLogDTO> GetByCriteria()
        {
            List<IndInsumoLogDTO> entitys = new List<IndInsumoLogDTO>();
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
