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
    /// Clase de acceso a datos de la tabla IND_RELACION_RPT
    /// </summary>
    public class IndRelacionRptRepository : RepositoryBase, IIndRelacionRptRepository
    {
        public IndRelacionRptRepository(string strConn) : base(strConn)
        {
        }

        IndRelacionRptHelper helper = new IndRelacionRptHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(IndRelacionRptDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irelrpcodi, DbType.Int32, entity.Irelrpcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irelrpidprinc, DbType.Int32, entity.Irelrpidprinc));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irelpridsec, DbType.Int32, entity.Irelpridsec));

            command.ExecuteNonQuery();
            return entity.Irelrpcodi;
        }

        public void Update(IndRelacionRptDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Irelrpcodi, DbType.Int32, entity.Irelrpcodi);
            dbProvider.AddInParameter(command, helper.Irelrpidprinc, DbType.Int32, entity.Irelrpidprinc);
            dbProvider.AddInParameter(command, helper.Irelpridsec, DbType.Int32, entity.Irelpridsec);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int irelrpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Irelrpcodi, DbType.Int32, irelrpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public IndRelacionRptDTO GetById(int irelrpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Irelrpcodi, DbType.Int32, irelrpcodi);
            IndRelacionRptDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<IndRelacionRptDTO> List()
        {
            List<IndRelacionRptDTO> entitys = new List<IndRelacionRptDTO>();
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

        public List<IndRelacionRptDTO> GetByCriteria(int irptprinc)
        {
            List<IndRelacionRptDTO> entitys = new List<IndRelacionRptDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, irptprinc);
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
    }
}
