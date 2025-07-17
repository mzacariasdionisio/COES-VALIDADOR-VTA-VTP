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
    /// Clase de acceso a datos de la tabla CB_OBSXARCHIVO
    /// </summary>
    public class CbObsxarchivoRepository : RepositoryBase, ICbObsxarchivoRepository
    {
        public CbObsxarchivoRepository(string strConn) : base(strConn)
        {
        }

        CbObsxarchivoHelper helper = new CbObsxarchivoHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(CbObsxarchivoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbobsacodi, DbType.Int32, entity.Cbobsacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbobscodi, DbType.Int32, entity.Cbobscodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbobsanombreenvio, DbType.String, entity.Cbobsanombreenvio));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbobsanombrefisico, DbType.String, entity.Cbobsanombrefisico));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbobsaorden, DbType.Int32, entity.Cbobsaorden));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbobsaestado, DbType.Int32, entity.Cbobsaestado));

            command.ExecuteNonQuery();
            return entity.Cbobsacodi;
        }

        public void Update(CbObsxarchivoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cbobscodi, DbType.Int32, entity.Cbobscodi);
            dbProvider.AddInParameter(command, helper.Cbobsacodi, DbType.Int32, entity.Cbobsacodi);
            dbProvider.AddInParameter(command, helper.Cbobsanombreenvio, DbType.String, entity.Cbobsanombreenvio);
            dbProvider.AddInParameter(command, helper.Cbobsanombrefisico, DbType.String, entity.Cbobsanombrefisico);
            dbProvider.AddInParameter(command, helper.Cbobsaorden, DbType.Int32, entity.Cbobsaorden);
            dbProvider.AddInParameter(command, helper.Cbobsaestado, DbType.Int32, entity.Cbobsaestado);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cbobsacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cbobsacodi, DbType.Int32, cbobsacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CbObsxarchivoDTO GetById(int cbobsacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cbobsacodi, DbType.Int32, cbobsacodi);
            CbObsxarchivoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CbObsxarchivoDTO> List(string cbcentcodis)
        {
            List<CbObsxarchivoDTO> entitys = new List<CbObsxarchivoDTO>();

            string sql = string.Format(helper.SqlList, cbcentcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCcombcodi = dr.GetOrdinal(helper.Ccombcodi);
                    if (!dr.IsDBNull(iCcombcodi)) entity.Ccombcodi = Convert.ToInt32(dr.GetValue(iCcombcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CbObsxarchivoDTO> ListByCbvercodi(int cbvercodi)
        {
            List<CbObsxarchivoDTO> entitys = new List<CbObsxarchivoDTO>();

            string sql = string.Format(helper.SqlListByCbvercodi, cbvercodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iCcombcodi = dr.GetOrdinal(helper.Ccombcodi);
                    if (!dr.IsDBNull(iCcombcodi)) entity.Ccombcodi = Convert.ToInt32(dr.GetValue(iCcombcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CbObsxarchivoDTO> GetByCriteria()
        {
            List<CbObsxarchivoDTO> entitys = new List<CbObsxarchivoDTO>();
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
