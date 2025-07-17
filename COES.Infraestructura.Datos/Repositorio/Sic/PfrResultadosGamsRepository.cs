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
    /// Clase de acceso a datos de la tabla PFR_RESULTADOS_GAMS
    /// </summary>
    public class PfrResultadosGamsRepository : RepositoryBase, IPfrResultadosGamsRepository
    {
        public PfrResultadosGamsRepository(string strConn) : base(strConn)
        {
        }

        PfrResultadosGamsHelper helper = new PfrResultadosGamsHelper();

        //public int Save(PfrResultadosGamsDTO entity)
        //{
        //    DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
        //    object result = dbProvider.ExecuteScalar(command);
        //    int id = 1;
        //    if (result != null) id = Convert.ToInt32(result);

        //    command = dbProvider.GetSqlStringCommand(helper.SqlSave);

        //    dbProvider.AddInParameter(command, helper.Pfrrgcodi, DbType.Int32, id);
        //    dbProvider.AddInParameter(command, helper.Pfresccodi, DbType.Int32, entity.Pfresccodi);
        //    dbProvider.AddInParameter(command, helper.Pfrrgecodi, DbType.Int32, entity.Pfrrgecodi);
        //    dbProvider.AddInParameter(command, helper.Pfreqpcodi, DbType.Int32, entity.Pfreqpcodi);
        //    dbProvider.AddInParameter(command, helper.Pfrcgtcodi, DbType.Int32, entity.Pfrcgtcodi);
        //    dbProvider.AddInParameter(command, helper.Pfrrgtipo, DbType.Int32, entity.Pfrrgtipo);
        //    dbProvider.AddInParameter(command, helper.Pfrrgresultado, DbType.Decimal, entity.Pfrrgresultado);

        //    dbProvider.ExecuteNonQuery(command);
        //    return id;
        //}

        public int Save(PfrResultadosGamsDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrrgcodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfresccodi, DbType.Int32, entity.Pfresccodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrrgecodi, DbType.Int32, entity.Pfrrgecodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfreqpcodi, DbType.Int32, entity.Pfreqpcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrcgtcodi, DbType.Int32, entity.Pfrcgtcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrrgtipo, DbType.Int32, entity.Pfrrgtipo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrrgresultado, DbType.Decimal, entity.Pfrrgresultado));

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfrResultadosGamsDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfresccodi, DbType.Int32, entity.Pfresccodi);
            dbProvider.AddInParameter(command, helper.Pfrrgecodi, DbType.Int32, entity.Pfrrgecodi);
            dbProvider.AddInParameter(command, helper.Pfreqpcodi, DbType.Int32, entity.Pfreqpcodi);
            dbProvider.AddInParameter(command, helper.Pfrcgtcodi, DbType.Int32, entity.Pfrcgtcodi);
            dbProvider.AddInParameter(command, helper.Pfrrgtipo, DbType.Int32, entity.Pfrrgtipo);
            dbProvider.AddInParameter(command, helper.Pfrrgresultado, DbType.Decimal, entity.Pfrrgresultado);
            dbProvider.AddInParameter(command, helper.Pfrrgcodi, DbType.Int32, entity.Pfrrgcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfrrgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfrrgcodi, DbType.Int32, pfrrgcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfrResultadosGamsDTO GetById(int pfrrgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfrrgcodi, DbType.Int32, pfrrgcodi);
            PfrResultadosGamsDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfrResultadosGamsDTO> List()
        {
            List<PfrResultadosGamsDTO> entitys = new List<PfrResultadosGamsDTO>();
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

        public List<PfrResultadosGamsDTO> GetByCriteria()
        {
            List<PfrResultadosGamsDTO> entitys = new List<PfrResultadosGamsDTO>();
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

        /// <summary>
        /// Listar pote escenario y por tipo
        /// </summary>
        /// <param name="pfresccodi"></param>
        /// <param name="pfrrgtipo"></param>
        /// <returns></returns>
        public List<PfrResultadosGamsDTO> ListByTipoYEscenario(int pfresccodi, int pfrrgtipo)
        {            
            List<PfrResultadosGamsDTO> entitys = new List<PfrResultadosGamsDTO>();

            string query = string.Format(helper.SqlListaPorTipoYEscenario, pfresccodi, pfrrgtipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

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
