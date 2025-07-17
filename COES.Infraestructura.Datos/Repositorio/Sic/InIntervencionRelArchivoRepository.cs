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
    /// Clase de acceso a datos de la tabla IN_INTERVENCION_REL_ARCHIVO
    /// </summary>
    public class InIntervencionRelArchivoRepository : RepositoryBase, IInIntervencionRelArchivoRepository
    {
        public InIntervencionRelArchivoRepository(string strConn) : base(strConn)
        {
        }

        InIntervencionRelArchivoHelper helper = new InIntervencionRelArchivoHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(InIntervencionRelArchivoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irarchcodi, DbType.Int32, entity.Irarchcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Intercodi, DbType.Int32, entity.Intercodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Inarchcodi, DbType.Int32, entity.Inarchcodi));

            command.ExecuteNonQuery();
            return entity.Irarchcodi;
        }

        public void Update(InIntervencionRelArchivoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Irarchcodi, DbType.Int32, entity.Irarchcodi);
            dbProvider.AddInParameter(command, helper.Intercodi, DbType.Int32, entity.Intercodi);
            dbProvider.AddInParameter(command, helper.Inarchcodi, DbType.Int32, entity.Inarchcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int irarchcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Irarchcodi, DbType.Int32, irarchcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public InIntervencionRelArchivoDTO GetById(int irarchcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Irarchcodi, DbType.Int32, irarchcodi);
            InIntervencionRelArchivoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<InIntervencionRelArchivoDTO> List()
        {
            List<InIntervencionRelArchivoDTO> entitys = new List<InIntervencionRelArchivoDTO>();
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

        public List<InIntervencionRelArchivoDTO> GetByCriteria()
        {
            List<InIntervencionRelArchivoDTO> entitys = new List<InIntervencionRelArchivoDTO>();
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
