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
    /// Clase de acceso a datos de la tabla IN_MENSAJE_REL_ARCHIVO
    /// </summary>
    public class InMensajeRelArchivoRepository : RepositoryBase, IInMensajeRelArchivoRepository
    {
        public InMensajeRelArchivoRepository(string strConn) : base(strConn)
        {
        }

        InMensajeRelArchivoHelper helper = new InMensajeRelArchivoHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(InMensajeRelArchivoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Msgcodi, DbType.Int32, entity.Msgcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Inarchcodi, DbType.Int32, entity.Inarchcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irmearcodi, DbType.Int32, entity.Irmearcodi));

            command.ExecuteNonQuery();
            return entity.Irmearcodi;
        }

        public void Update(InMensajeRelArchivoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Msgcodi, DbType.Int32, entity.Msgcodi);
            dbProvider.AddInParameter(command, helper.Inarchcodi, DbType.Int32, entity.Inarchcodi);
            dbProvider.AddInParameter(command, helper.Irmearcodi, DbType.Int32, entity.Irmearcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int irmarcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Irmearcodi, DbType.Int32, irmarcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public InMensajeRelArchivoDTO GetById(int irmarcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Irmearcodi, DbType.Int32, irmarcodi);
            InMensajeRelArchivoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<InMensajeRelArchivoDTO> List()
        {
            List<InMensajeRelArchivoDTO> entitys = new List<InMensajeRelArchivoDTO>();
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

        public List<InMensajeRelArchivoDTO> GetByCriteria()
        {
            List<InMensajeRelArchivoDTO> entitys = new List<InMensajeRelArchivoDTO>();
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
