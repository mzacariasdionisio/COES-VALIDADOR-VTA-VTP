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
    /// Clase de acceso a datos de la tabla CCC_VERSION
    /// </summary>
    public class CccVersionRepository : RepositoryBase, ICccVersionRepository
    {
        public CccVersionRepository(string strConn) : base(strConn)
        {
        }

        CccVersionHelper helper = new CccVersionHelper();

        public int Save(CccVersionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cccvercodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cccverhorizonte, DbType.String, entity.Cccverhorizonte));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cccverfecha, DbType.DateTime, entity.Cccverfecha));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cccvernumero, DbType.Int32, entity.Cccvernumero));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cccverestado, DbType.String, entity.Cccverestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cccverobs, DbType.String, entity.Cccverobs));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cccverrptcodis, DbType.String, entity.Cccverrptcodis));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cccverusucreacion, DbType.String, entity.Cccverusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cccverfeccreacion, DbType.DateTime, entity.Cccverfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cccverusumodificacion, DbType.String, entity.Cccverusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cccverfecmodificacion, DbType.DateTime, entity.Cccverfecmodificacion));

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CccVersionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cccverfecha, DbType.DateTime, entity.Cccverfecha);
            dbProvider.AddInParameter(command, helper.Cccvernumero, DbType.Int32, entity.Cccvernumero);
            dbProvider.AddInParameter(command, helper.Cccverestado, DbType.String, entity.Cccverestado);
            dbProvider.AddInParameter(command, helper.Cccverusucreacion, DbType.String, entity.Cccverusucreacion);
            dbProvider.AddInParameter(command, helper.Cccverfeccreacion, DbType.DateTime, entity.Cccverfeccreacion);
            dbProvider.AddInParameter(command, helper.Cccverusumodificacion, DbType.String, entity.Cccverusumodificacion);
            dbProvider.AddInParameter(command, helper.Cccverfecmodificacion, DbType.DateTime, entity.Cccverfecmodificacion);

            dbProvider.AddInParameter(command, helper.Cccvercodi, DbType.Int32, entity.Cccvercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cccvercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cccvercodi, DbType.Int32, cccvercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CccVersionDTO GetById(int cccvercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cccvercodi, DbType.Int32, cccvercodi);
            CccVersionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CccVersionDTO> List()
        {
            List<CccVersionDTO> entitys = new List<CccVersionDTO>();
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

        public List<CccVersionDTO> GetByCriteria(DateTime fecha, DateTime fechaFin, string horizonte)
        {
            List<CccVersionDTO> entitys = new List<CccVersionDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, fecha.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), horizonte);
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
