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
    /// Clase de acceso a datos de la tabla CB_REPORTE_DETALLE
    /// </summary>
    public class CbReporteDetalleRepository: RepositoryBase, ICbReporteDetalleRepository
    {
        public CbReporteDetalleRepository(string strConn): base(strConn)
        {
        }

        CbReporteDetalleHelper helper = new CbReporteDetalleHelper();

        public int getIdDisponible()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public void Save(CbReporteDetalleDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cbrepdcodi, DbType.Int32, entity.Cbrepdcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ccombcodi, DbType.Int32, entity.Ccombcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cbrcencodi, DbType.Int32, entity.Cbrcencodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cbrepdvalor, DbType.String, entity.Cbrepdvalor));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cbrepvalordecimal, DbType.Decimal, entity.Cbrepvalordecimal));
                dbCommand.ExecuteNonQuery();
            }
        }        

        public int Save(CbReporteDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cbrepdcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ccombcodi, DbType.Int32, entity.Ccombcodi);
            dbProvider.AddInParameter(command, helper.Cbrcencodi, DbType.Int32, entity.Cbrcencodi);
            dbProvider.AddInParameter(command, helper.Cbrepdvalor, DbType.String, entity.Cbrepdvalor);
            dbProvider.AddInParameter(command, helper.Cbrepvalordecimal, DbType.Decimal, entity.Cbrepvalordecimal);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CbReporteDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            
            dbProvider.AddInParameter(command, helper.Ccombcodi, DbType.Int32, entity.Ccombcodi);
            dbProvider.AddInParameter(command, helper.Cbrcencodi, DbType.Int32, entity.Cbrcencodi);
            dbProvider.AddInParameter(command, helper.Cbrepdvalor, DbType.String, entity.Cbrepdvalor);
            dbProvider.AddInParameter(command, helper.Cbrepvalordecimal, DbType.Decimal, entity.Cbrepvalordecimal);
            dbProvider.AddInParameter(command, helper.Cbrepdcodi, DbType.Int32, entity.Cbrepdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cbrepdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cbrepdcodi, DbType.Int32, cbrepdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CbReporteDetalleDTO GetById(int cbrepdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cbrepdcodi, DbType.Int32, cbrepdcodi);
            CbReporteDetalleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CbReporteDetalleDTO> List()
        {
            List<CbReporteDetalleDTO> entitys = new List<CbReporteDetalleDTO>();
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

        public List<CbReporteDetalleDTO> GetByCriteria()
        {
            List<CbReporteDetalleDTO> entitys = new List<CbReporteDetalleDTO>();
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

        public List<CbReporteDetalleDTO> GetByIdCentral(int cbrcencodi)
        {
            List<CbReporteDetalleDTO> entitys = new List<CbReporteDetalleDTO>();
            var sqlQuery = string.Format(helper.SqlGetByIdCentral, cbrcencodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

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
