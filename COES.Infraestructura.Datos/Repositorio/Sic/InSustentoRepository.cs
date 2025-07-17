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
    /// Clase de acceso a datos de la tabla IN_SUSTENTO
    /// </summary>
    public class InSustentoRepository : RepositoryBase, IInSustentoRepository
    {
        public InSustentoRepository(string strConn) : base(strConn)
        {
        }

        InSustentoHelper helper = new InSustentoHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(InSustentoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Instcodi, DbType.Int32, entity.Instcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Instestado, DbType.String, entity.Instestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Instusumodificacion, DbType.String, entity.Instusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Instfecmodificacion, DbType.DateTime, entity.Instfecmodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Intercodi, DbType.Int32, entity.Intercodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Inpstcodi, DbType.Int32, entity.Inpstcodi));

            command.ExecuteNonQuery();
            return entity.Instcodi;
        }

        public void Update(InSustentoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Instcodi, DbType.Int32, entity.Instcodi);
            dbProvider.AddInParameter(command, helper.Instestado, DbType.String, entity.Instestado);
            dbProvider.AddInParameter(command, helper.Instusumodificacion, DbType.String, entity.Instusumodificacion);
            dbProvider.AddInParameter(command, helper.Instfecmodificacion, DbType.DateTime, entity.Instfecmodificacion);
            dbProvider.AddInParameter(command, helper.Intercodi, DbType.Int32, entity.Intercodi);
            dbProvider.AddInParameter(command, helper.Inpstcodi, DbType.Int32, entity.Inpstcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int instcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Instcodi, DbType.Int32, instcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public InSustentoDTO GetById(int instcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Instcodi, DbType.Int32, instcodi);
            InSustentoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public InSustentoDTO GetByIntercodi(int intercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIntercodi);

            dbProvider.AddInParameter(command, helper.Instcodi, DbType.Int32, intercodi);
            InSustentoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iInpsttipo = dr.GetOrdinal(helper.Inpsttipo);
                    if (!dr.IsDBNull(iInpsttipo)) entity.Inpsttipo = Convert.ToInt32(dr.GetValue(iInpsttipo));

                }
            }

            return entity;
        }

        public List<InSustentoDTO> List()
        {
            List<InSustentoDTO> entitys = new List<InSustentoDTO>();
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

        public List<InSustentoDTO> GetByCriteria()
        {
            List<InSustentoDTO> entitys = new List<InSustentoDTO>();
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
