using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CB_REPORTE
    /// </summary>
    public class CbReporteRepository: RepositoryBase, ICbReporteRepository
    {
        
        private string strConexion;
        public CbReporteRepository(string strConn) : base(strConn)
        {
            strConexion = strConn;
        }

        CbReporteHelper helper = new CbReporteHelper();

        public IDbConnection BeginConnection()
        {
            Database db = DatabaseFactory.CreateDatabase(strConexion);
            IDbConnection conn = db.CreateConnection();
            conn.Open();
            return conn;
        }

        public DbTransaction StartTransaction(IDbConnection conn)
        {
            return (DbTransaction)conn.BeginTransaction();
        }

        public int Save(CbReporteDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbrepcodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbrepmesvigencia, DbType.DateTime, entity.Cbrepmesvigencia));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbrepversion, DbType.Int32, entity.Cbrepversion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbrepnombre, DbType.String, entity.Cbrepnombre));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbrepusucreacion, DbType.String, entity.Cbrepusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbrepfeccreacion, DbType.DateTime, entity.Cbrepfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbrepusumodificacion, DbType.String, entity.Cbrepusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbrepfecmodificacion, DbType.DateTime, entity.Cbrepfecmodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbreptipo, DbType.Int32, entity.Cbreptipo));                        

            command.ExecuteNonQuery();
            return id;
        }

        public int Save(CbReporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cbrepcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cbrepmesvigencia, DbType.DateTime, entity.Cbrepmesvigencia);
            dbProvider.AddInParameter(command, helper.Cbrepversion, DbType.Int32, entity.Cbrepversion);
            dbProvider.AddInParameter(command, helper.Cbrepnombre, DbType.String, entity.Cbrepnombre);
            dbProvider.AddInParameter(command, helper.Cbrepusucreacion, DbType.String, entity.Cbrepusucreacion);
            dbProvider.AddInParameter(command, helper.Cbrepfeccreacion, DbType.DateTime, entity.Cbrepfeccreacion);
            dbProvider.AddInParameter(command, helper.Cbrepusumodificacion, DbType.String, entity.Cbrepusumodificacion);
            dbProvider.AddInParameter(command, helper.Cbrepfecmodificacion, DbType.DateTime, entity.Cbrepfecmodificacion);
            dbProvider.AddInParameter(command, helper.Cbreptipo, DbType.Int32, entity.Cbreptipo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CbReporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cbrepcodi, DbType.Int32, entity.Cbrepcodi);
            dbProvider.AddInParameter(command, helper.Cbrepmesvigencia, DbType.DateTime, entity.Cbrepmesvigencia);
            dbProvider.AddInParameter(command, helper.Cbrepversion, DbType.Int32, entity.Cbrepversion);
            dbProvider.AddInParameter(command, helper.Cbrepnombre, DbType.String, entity.Cbrepnombre);
            dbProvider.AddInParameter(command, helper.Cbrepusucreacion, DbType.String, entity.Cbrepusucreacion);
            dbProvider.AddInParameter(command, helper.Cbrepfeccreacion, DbType.DateTime, entity.Cbrepfeccreacion);
            dbProvider.AddInParameter(command, helper.Cbrepusumodificacion, DbType.String, entity.Cbrepusumodificacion);
            dbProvider.AddInParameter(command, helper.Cbrepfecmodificacion, DbType.DateTime, entity.Cbrepfecmodificacion);
            dbProvider.AddInParameter(command, helper.Cbreptipo, DbType.Int32, entity.Cbreptipo);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cbrepcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cbrepcodi, DbType.Int32, cbrepcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CbReporteDTO GetById(int cbrepcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cbrepcodi, DbType.Int32, cbrepcodi);
            CbReporteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CbReporteDTO> List()
        {
            List<CbReporteDTO> entitys = new List<CbReporteDTO>();
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

        public List<CbReporteDTO> GetByCriteria()
        {
            List<CbReporteDTO> entitys = new List<CbReporteDTO>();
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

        public List<CbReporteDTO> GetByTipoYMesVigencia(int tipoReporte, string mesVigencia)
        {
            List<CbReporteDTO> entitys = new List<CbReporteDTO>();
            var sqlQuery = string.Format(helper.SqlGetByTipoYMesVigencia, tipoReporte, mesVigencia);
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
