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
    /// Clase de acceso a datos de la tabla CB_REP_CABECERA
    /// </summary>
    public class CbRepCabeceraRepository: RepositoryBase, ICbRepCabeceraRepository
    {
        public CbRepCabeceraRepository(string strConn): base(strConn)
        {
        }

        CbRepCabeceraHelper helper = new CbRepCabeceraHelper();

        public int getIdDisponible()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public void Save(CbRepCabeceraDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cbrcabcodi, DbType.Int32, entity.Cbrcabcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cbrprocodi, DbType.Int32, entity.Cbrprocodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cbrepcodi, DbType.Int32, entity.Cbrepcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cbrcabdescripcion, DbType.String, entity.Cbrcabdescripcion));

                dbCommand.ExecuteNonQuery();
            }
        }       

        public int Save(CbRepCabeceraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cbrcabcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cbrprocodi, DbType.Int32, entity.Cbrprocodi);
            dbProvider.AddInParameter(command, helper.Cbrepcodi, DbType.Int32, entity.Cbrepcodi);
            dbProvider.AddInParameter(command, helper.Cbrcabdescripcion, DbType.String, entity.Cbrcabdescripcion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CbRepCabeceraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cbrcabcodi, DbType.Int32, entity.Cbrcabcodi);
            dbProvider.AddInParameter(command, helper.Cbrprocodi, DbType.Int32, entity.Cbrprocodi);
            dbProvider.AddInParameter(command, helper.Cbrepcodi, DbType.Int32, entity.Cbrepcodi);
            dbProvider.AddInParameter(command, helper.Cbrcabdescripcion, DbType.String, entity.Cbrcabdescripcion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cbrcabcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cbrcabcodi, DbType.Int32, cbrcabcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CbRepCabeceraDTO GetById(int cbrcabcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cbrcabcodi, DbType.Int32, cbrcabcodi);
            CbRepCabeceraDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CbRepCabeceraDTO> List()
        {
            List<CbRepCabeceraDTO> entitys = new List<CbRepCabeceraDTO>();
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

        public List<CbRepCabeceraDTO> GetByCriteria()
        {
            List<CbRepCabeceraDTO> entitys = new List<CbRepCabeceraDTO>();
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

        public List<CbRepCabeceraDTO> GetByTipoReporte(int tipoReporte)
        {
            List<CbRepCabeceraDTO> entitys = new List<CbRepCabeceraDTO>();
            var sqlQuery = string.Format(helper.SqlGetByTipoReporte, tipoReporte);
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

        public List<CbRepCabeceraDTO> GetByIdReporte(int cbrepcodi)
        {
            List<CbRepCabeceraDTO> entitys = new List<CbRepCabeceraDTO>();
            var sqlQuery = string.Format(helper.SqlGetByIdReporte, cbrepcodi);
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

        public List<CbRepCabeceraDTO> GetByTipoReporteYMesVigencia(int cbreptipo, string mesVigencia)
        {
            List<CbRepCabeceraDTO> entitys = new List<CbRepCabeceraDTO>();
            var sqlQuery = string.Format(helper.SqlGetByTipoReporteYMesVigencia, cbreptipo, mesVigencia);
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
