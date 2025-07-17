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
    /// Clase de acceso a datos de la tabla CP_ARBOL_CONTINUO
    /// </summary>
    public class CpArbolContinuoRepository : RepositoryBase, ICpArbolContinuoRepository
    {
        public CpArbolContinuoRepository(string strConn) : base(strConn)
        {
        }

        CpArbolContinuoHelper helper = new CpArbolContinuoHelper();

        public int Save(CpArbolContinuoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparbcodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparbtag, DbType.String, entity.Cparbtag));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparbfecregistro, DbType.DateTime, entity.Cparbfecregistro));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparbusuregistro, DbType.String, entity.Cparbusuregistro));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparbestado, DbType.String, entity.Cparbestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparbfecha, DbType.DateTime, entity.Cparbfecha));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparbbloquehorario, DbType.Int32, entity.Cparbbloquehorario));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparbdetalleejec, DbType.String, entity.Cparbdetalleejec));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparbidentificador, DbType.String, entity.Cparbidentificador));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparbfeciniproceso, DbType.DateTime, entity.Cparbfeciniproceso));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparbfecfinproceso, DbType.DateTime, entity.Cparbfecfinproceso));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparbmsjproceso, DbType.String, entity.Cparbmsjproceso));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparbporcentaje, DbType.Decimal, entity.Cparbporcentaje));


            dbProvider.ExecuteNonQuery(command);
            return id;
        }


        public void Update(CpArbolContinuoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);


            dbProvider.AddInParameter(command, helper.Cparbtag, DbType.String, entity.Cparbtag);
            dbProvider.AddInParameter(command, helper.Cparbfecregistro, DbType.DateTime, entity.Cparbfecregistro);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.AddInParameter(command, helper.Cparbusuregistro, DbType.String, entity.Cparbusuregistro);
            dbProvider.AddInParameter(command, helper.Cparbestado, DbType.String, entity.Cparbestado);
            dbProvider.AddInParameter(command, helper.Cparbfecha, DbType.DateTime, entity.Cparbfecha);
            dbProvider.AddInParameter(command, helper.Cparbbloquehorario, DbType.Int32, entity.Cparbbloquehorario);
            dbProvider.AddInParameter(command, helper.Cparbdetalleejec, DbType.String, entity.Cparbdetalleejec);
            dbProvider.AddInParameter(command, helper.Cparbidentificador, DbType.String, entity.Cparbidentificador);
            dbProvider.AddInParameter(command, helper.Cparbfeciniproceso, DbType.DateTime, entity.Cparbfeciniproceso);
            dbProvider.AddInParameter(command, helper.Cparbfecfinproceso, DbType.DateTime, entity.Cparbfecfinproceso);
            dbProvider.AddInParameter(command, helper.Cparbmsjproceso, DbType.String, entity.Cparbmsjproceso);
            dbProvider.AddInParameter(command, helper.Cparbporcentaje, DbType.Decimal, entity.Cparbporcentaje);
            dbProvider.AddInParameter(command, helper.Cparbcodi, DbType.Int32, entity.Cparbcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cparbcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cparbcodi, DbType.Int32, cparbcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpArbolContinuoDTO GetById(int cparbcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cparbcodi, DbType.Int32, cparbcodi);
            CpArbolContinuoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpArbolContinuoDTO> List()
        {
            List<CpArbolContinuoDTO> entitys = new List<CpArbolContinuoDTO>();
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

        public List<CpArbolContinuoDTO> GetByCriteria(int topcodi)
        {
            List<CpArbolContinuoDTO> entitys = new List<CpArbolContinuoDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, topcodi);
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

        public CpArbolContinuoDTO GetUltimoArbol()
        {
            CpArbolContinuoDTO entity = null;
            string sql = String.Format(helper.SqlGetUltimoArbol);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
    }
}
