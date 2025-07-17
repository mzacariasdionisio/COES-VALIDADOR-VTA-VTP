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
    /// Clase de acceso a datos de la tabla PF_ESCENARIO
    /// </summary>
    public class PfEscenarioRepository : RepositoryBase, IPfEscenarioRepository
    {
        public PfEscenarioRepository(string strConn) : base(strConn)
        {
        }

        PfEscenarioHelper helper = new PfEscenarioHelper();

        public int Save(PfEscenarioDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfescecodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfescefecini, DbType.DateTime, entity.Pfescefecini));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfescefecfin, DbType.DateTime, entity.Pfescefecfin));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfescedescripcion, DbType.String, entity.Pfescedescripcion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrptcodi, DbType.Int32, entity.Pfrptcodi));

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfEscenarioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfescecodi, DbType.Int32, entity.Pfescecodi);
            dbProvider.AddInParameter(command, helper.Pfescefecini, DbType.DateTime, entity.Pfescefecini);
            dbProvider.AddInParameter(command, helper.Pfescefecfin, DbType.DateTime, entity.Pfescefecfin);
            dbProvider.AddInParameter(command, helper.Pfescedescripcion, DbType.String, entity.Pfescedescripcion);
            dbProvider.AddInParameter(command, helper.Pfrptcodi, DbType.Int32, entity.Pfrptcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfescecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfescecodi, DbType.Int32, pfescecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfEscenarioDTO GetById(int pfescecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfescecodi, DbType.Int32, pfescecodi);
            PfEscenarioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfEscenarioDTO> List()
        {
            List<PfEscenarioDTO> entitys = new List<PfEscenarioDTO>();
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

        public List<PfEscenarioDTO> GetByCriteria(int pfrptcodi)
        {
            List<PfEscenarioDTO> entitys = new List<PfEscenarioDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, pfrptcodi);

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
