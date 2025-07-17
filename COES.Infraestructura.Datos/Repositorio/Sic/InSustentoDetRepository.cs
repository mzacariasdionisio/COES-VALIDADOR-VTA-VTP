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
    /// Clase de acceso a datos de la tabla IN_SUSTENTO_DET
    /// </summary>
    public class InSustentoDetRepository : RepositoryBase, IInSustentoDetRepository
    {
        public InSustentoDetRepository(string strConn) : base(strConn)
        {
        }

        InSustentoDetHelper helper = new InSustentoDetHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(InSustentoDetDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Instcodi, DbType.Int32, entity.Instcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Inpsticodi, DbType.Int32, entity.Inpsticodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Instdcodi, DbType.Int32, entity.Instdcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Instdrpta, DbType.String, entity.Instdrpta));

            command.ExecuteNonQuery();
            return entity.Instdcodi;
        }

        public void Update(InSustentoDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Instcodi, DbType.Int32, entity.Instcodi);
            dbProvider.AddInParameter(command, helper.Inpsticodi, DbType.Int32, entity.Inpsticodi);
            dbProvider.AddInParameter(command, helper.Instdcodi, DbType.Int32, entity.Instdcodi);
            dbProvider.AddInParameter(command, helper.Instdrpta, DbType.String, entity.Instdrpta);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int instdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Instdcodi, DbType.Int32, instdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public InSustentoDetDTO GetById(int instdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Instdcodi, DbType.Int32, instdcodi);
            InSustentoDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<InSustentoDetDTO> List()
        {
            List<InSustentoDetDTO> entitys = new List<InSustentoDetDTO>();
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

        public List<InSustentoDetDTO> GetByCriteria(int instcodi)
        {
            List<InSustentoDetDTO> entitys = new List<InSustentoDetDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, instcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iInpstidesc = dr.GetOrdinal(helper.Inpstidesc);
                    if (!dr.IsDBNull(iInpstidesc)) entity.Inpstidesc = dr.GetString(iInpstidesc);

                    int iInpstitipo = dr.GetOrdinal(helper.Inpstitipo);
                    if (!dr.IsDBNull(iInpstitipo)) entity.Inpstitipo = Convert.ToInt32(dr.GetValue(iInpstitipo));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
