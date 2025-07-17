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
    /// Clase de acceso a datos de la tabla CB_OBS
    /// </summary>
    public class CbObsRepository : RepositoryBase, ICbObsRepository
    {
        public CbObsRepository(string strConn) : base(strConn)
        {
        }

        CbObsHelper helper = new CbObsHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(CbObsDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbobscodi, DbType.Int32, entity.Cbobscodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbevdacodi, DbType.Int32, entity.Cbevdacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbobshtml, DbType.String, entity.Cbobshtml));

            command.ExecuteNonQuery();
            return entity.Cbobscodi;
        }

        public void Update(CbObsDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cbevdacodi, DbType.Int32, entity.Cbevdacodi);
            dbProvider.AddInParameter(command, helper.Cbobscodi, DbType.Int32, entity.Cbobscodi);
            dbProvider.AddInParameter(command, helper.Cbobshtml, DbType.String, entity.Cbobshtml);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cbobscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cbobscodi, DbType.Int32, cbobscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CbObsDTO GetById(int cbobscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cbobscodi, DbType.Int32, cbobscodi);
            CbObsDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CbObsDTO> List(string cbcentcodis)
        {
            List<CbObsDTO> entitys = new List<CbObsDTO>();

            string sql = string.Format(helper.SqlList, cbcentcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CbObsDTO> ListByCbvercodi(int cbvercodi)
        {
            List<CbObsDTO> entitys = new List<CbObsDTO>();

            string sql = string.Format(helper.SqlListByCbvercodi, cbvercodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iCcombcodi = dr.GetOrdinal(helper.Ccombcodi);
                    if (!dr.IsDBNull(iCcombcodi)) entity.Ccombcodi = Convert.ToInt32(dr.GetValue(iCcombcodi));

                    int iCcombnombre = dr.GetOrdinal(helper.Ccombnombre);
                    if (!dr.IsDBNull(iCcombnombre)) entity.Ccombnombre = dr.GetString(iCcombnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CbObsDTO> GetByCriteria()
        {
            List<CbObsDTO> entitys = new List<CbObsDTO>();
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
