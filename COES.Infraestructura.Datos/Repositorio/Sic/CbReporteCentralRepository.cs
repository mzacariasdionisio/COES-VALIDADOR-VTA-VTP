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
    /// Clase de acceso a datos de la tabla CB_REPORTE_CENTRAL
    /// </summary>
    public class CbReporteCentralRepository: RepositoryBase, ICbReporteCentralRepository
    {
        public CbReporteCentralRepository(string strConn): base(strConn)
        {
        }

        CbReporteCentralHelper helper = new CbReporteCentralHelper();

        public int getIdDisponible()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public void Save(CbReporteCentralDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cbrcencodi, DbType.Int32, entity.Cbrcencodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cbrepcodi, DbType.Int32, entity.Cbrepcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cbcentcodi, DbType.Int32, entity.Cbcentcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cbrcennombre, DbType.String, entity.Cbrcennombre));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cbrcencoloreado, DbType.Int32, entity.Cbrcencoloreado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cbrcenorigen, DbType.Int32, entity.Cbrcenorigen));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cbrcenorden, DbType.Int32, entity.Cbrcenorden));

                dbCommand.ExecuteNonQuery();
            }
        }
        

        public int Save(CbReporteCentralDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cbrcencodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cbrepcodi, DbType.Int32, entity.Cbrepcodi);
            dbProvider.AddInParameter(command, helper.Cbcentcodi, DbType.Int32, entity.Cbcentcodi);
            dbProvider.AddInParameter(command, helper.Cbrcennombre, DbType.String, entity.Cbrcennombre);
            dbProvider.AddInParameter(command, helper.Cbrcencoloreado, DbType.Int32, entity.Cbrcencoloreado);
            dbProvider.AddInParameter(command, helper.Cbrcenorigen, DbType.Int32, entity.Cbrcenorigen);
            dbProvider.AddInParameter(command, helper.Cbrcenorden, DbType.Int32, entity.Cbrcenorden);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CbReporteCentralDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cbrepcodi, DbType.Int32, entity.Cbrepcodi);            
            dbProvider.AddInParameter(command, helper.Cbcentcodi, DbType.Int32, entity.Cbcentcodi);
            dbProvider.AddInParameter(command, helper.Cbrcennombre, DbType.String, entity.Cbrcennombre);
            dbProvider.AddInParameter(command, helper.Cbrcencoloreado, DbType.Int32, entity.Cbrcencoloreado);
            dbProvider.AddInParameter(command, helper.Cbrcenorigen, DbType.Int32, entity.Cbrcenorigen);
            dbProvider.AddInParameter(command, helper.Cbrcenorden, DbType.Int32, entity.Cbrcenorden);
            dbProvider.AddInParameter(command, helper.Cbrcencodi, DbType.Int32, entity.Cbrcencodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cbrcencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cbrcencodi, DbType.Int32, cbrcencodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CbReporteCentralDTO GetById(int cbrcencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cbrcencodi, DbType.Int32, cbrcencodi);
            CbReporteCentralDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CbReporteCentralDTO> List()
        {
            List<CbReporteCentralDTO> entitys = new List<CbReporteCentralDTO>();
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

        public List<CbReporteCentralDTO> GetByCriteria()
        {
            List<CbReporteCentralDTO> entitys = new List<CbReporteCentralDTO>();
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

        public List<CbReporteCentralDTO> GetByIdReporte(int cbrepcodi)
        {
            List<CbReporteCentralDTO> entitys = new List<CbReporteCentralDTO>();
            var sqlQuery = string.Format(helper.SqlGetByIdReporte, cbrepcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

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
        
    }
}
