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
    /// Clase de acceso a datos de la tabla CB_DATOS
    /// </summary>
    public class CbDatosRepository : RepositoryBase, ICbDatosRepository
    {
        public CbDatosRepository(string strConn) : base(strConn)
        {
        }

        readonly CbDatosHelper helper = new CbDatosHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(CbDatosDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbevdacodi, DbType.Int32, entity.Cbevdacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbvercodi, DbType.Int32, entity.Cbvercodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ccombcodi, DbType.Int32, entity.Ccombcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbevdavalor, DbType.String, entity.Cbevdavalor));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbevdatipo, DbType.String, entity.Cbevdatipo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbevdavalor2, DbType.String, entity.Cbevdavalor2));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbevdatipo2, DbType.String, entity.Cbevdatipo2));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbevdaconfidencial, DbType.Int32, entity.Cbevdaconfidencial));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbevdaestado, DbType.Int32, entity.Cbevdaestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbevdanumdecimal, DbType.Int32, entity.Cbevdanumdecimal));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbcentcodi, DbType.Int32, entity.Cbcentcodi));

            command.ExecuteNonQuery();
            return entity.Cbevdacodi;
        }

        public void Update(CbDatosDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cbvercodi, DbType.Int32, entity.Cbvercodi);            
            dbProvider.AddInParameter(command, helper.Ccombcodi, DbType.Int32, entity.Ccombcodi);
            dbProvider.AddInParameter(command, helper.Cbevdavalor, DbType.String, entity.Cbevdavalor);
            dbProvider.AddInParameter(command, helper.Cbevdatipo, DbType.String, entity.Cbevdatipo);
            dbProvider.AddInParameter(command, helper.Cbevdavalor2, DbType.String, entity.Cbevdavalor2);
            dbProvider.AddInParameter(command, helper.Cbevdatipo2, DbType.String, entity.Cbevdatipo2);
            dbProvider.AddInParameter(command, helper.Cbevdacodi, DbType.Int32, entity.Cbevdacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cbevdacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cbevdacodi, DbType.Int32, cbevdacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CbDatosDTO GetById(int cbevdacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cbevdacodi, DbType.Int32, cbevdacodi);
            CbDatosDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CbDatosDTO> List(string cbcentcodis)
        {
            List<CbDatosDTO> entitys = new List<CbDatosDTO>();

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

        public List<CbDatosDTO> GetByCriteria(string cbvercodis, string ccombcodis)
        {
            List<CbDatosDTO> entitys = new List<CbDatosDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, cbvercodis, ccombcodis);
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


        public CbDatosDTO GetCostoCombustibleSolicitado(int ccombcodi, int idEnvio)
        {
            List<CbDatosDTO> entitys = new List<CbDatosDTO>();
            string sql = string.Format(helper.SqlGetCostoCombustibleSolicitado, ccombcodi, idEnvio);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            CbDatosDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CbDatosDTO> GetDatosReporteCV(string concepcodiS, string ccentcodis)
        {
            List<CbDatosDTO> entitys = new List<CbDatosDTO>();
            string sql = string.Format(helper.SqlGetDataReporteCV, concepcodiS, ccentcodis);
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
