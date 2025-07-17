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
    /// Clase de acceso a datos de la tabla PFR_ESCENARIO
    /// </summary>
    public class PfrEscenarioRepository: RepositoryBase, IPfrEscenarioRepository
    {
        public PfrEscenarioRepository(string strConn): base(strConn)
        {
        }

        PfrEscenarioHelper helper = new PfrEscenarioHelper();

        //public int Save(PfrEscenarioDTO entity)
        //{
        //    DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
        //    object result = dbProvider.ExecuteScalar(command);
        //    int id = 1;
        //    if (result != null)id = Convert.ToInt32(result);

        //    command = dbProvider.GetSqlStringCommand(helper.SqlSave);

        //    dbProvider.AddInParameter(command, helper.Pfresccodi, DbType.Int32, id);
        //    dbProvider.AddInParameter(command, helper.Pfrrptcodi, DbType.Int32, entity.Pfrrptcodi);
        //    dbProvider.AddInParameter(command, helper.Pfrescdescripcion, DbType.String, entity.Pfrescdescripcion);
        //    dbProvider.AddInParameter(command, helper.Pfrescfecini, DbType.DateTime, entity.Pfrescfecini);
        //    dbProvider.AddInParameter(command, helper.Pfrescfecfin, DbType.DateTime, entity.Pfrescfecfin);
        //    dbProvider.AddInParameter(command, helper.Pfrescfrf, DbType.Decimal, entity.Pfrescfrf);
        //    dbProvider.AddInParameter(command, helper.Pfrescfrfr, DbType.Decimal, entity.Pfrescfrfr);
        //    dbProvider.AddInParameter(command, helper.Pfrescpfct, DbType.Decimal, entity.Pfrescpfct);

        //    dbProvider.ExecuteNonQuery(command);
        //    return id;
        //}

        public int Save(PfrEscenarioDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfresccodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrrptcodi, DbType.Int32, entity.Pfrrptcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrescdescripcion, DbType.String, entity.Pfrescdescripcion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrescfecini, DbType.DateTime, entity.Pfrescfecini));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrescfecfin, DbType.DateTime, entity.Pfrescfecfin));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrescfrf, DbType.Decimal, entity.Pfrescfrf));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrescfrfr, DbType.Decimal, entity.Pfrescfrfr));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrescpfct, DbType.Decimal, entity.Pfrescpfct));

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfrEscenarioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfresccodi, DbType.Int32, entity.Pfresccodi);
            dbProvider.AddInParameter(command, helper.Pfrrptcodi, DbType.Int32, entity.Pfrrptcodi);
            dbProvider.AddInParameter(command, helper.Pfrescdescripcion, DbType.String, entity.Pfrescdescripcion);
            dbProvider.AddInParameter(command, helper.Pfrescfecini, DbType.DateTime, entity.Pfrescfecini);
            dbProvider.AddInParameter(command, helper.Pfrescfecfin, DbType.DateTime, entity.Pfrescfecfin);
            dbProvider.AddInParameter(command, helper.Pfrescfrf, DbType.Decimal, entity.Pfrescfrf);
            dbProvider.AddInParameter(command, helper.Pfrescfrfr, DbType.Decimal, entity.Pfrescfrfr);
            dbProvider.AddInParameter(command, helper.Pfrescpfct, DbType.Decimal, entity.Pfrescpfct);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfresccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfresccodi, DbType.Int32, pfresccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfrEscenarioDTO GetById(int pfresccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfresccodi, DbType.Int32, pfresccodi);
            PfrEscenarioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfrEscenarioDTO> List()
        {
            List<PfrEscenarioDTO> entitys = new List<PfrEscenarioDTO>();
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
        public List<PfrEscenarioDTO> ListByReportecodi(int pfrrptcodi)
        {
            List<PfrEscenarioDTO> entitys = new List<PfrEscenarioDTO>();
            string sql = string.Format(helper.SqlListByReportecodi, pfrrptcodi);
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

        public List<PfrEscenarioDTO> GetByCriteria()
        {
            List<PfrEscenarioDTO> entitys = new List<PfrEscenarioDTO>();
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
