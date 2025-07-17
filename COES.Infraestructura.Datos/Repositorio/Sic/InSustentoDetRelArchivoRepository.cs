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
    /// Clase de acceso a datos de la tabla IN_SUSTENTO_DET_REL_ARCHIVO
    /// </summary>
    public class InSustentoDetRelArchivoRepository : RepositoryBase, IInSustentoDetRelArchivoRepository
    {
        public InSustentoDetRelArchivoRepository(string strConn) : base(strConn)
        {
        }

        InSustentoDetRelArchivoHelper helper = new InSustentoDetRelArchivoHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(InSustentoDetRelArchivoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Instdcodi, DbType.Int32, entity.Instdcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Inarchcodi, DbType.Int32, entity.Inarchcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Isdarcodi, DbType.Int32, entity.Isdarcodi));

            command.ExecuteNonQuery();
            return entity.Isdarcodi;
        }

        public void Update(InSustentoDetRelArchivoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Instdcodi, DbType.Int32, entity.Instdcodi);
            dbProvider.AddInParameter(command, helper.Inarchcodi, DbType.Int32, entity.Inarchcodi);
            dbProvider.AddInParameter(command, helper.Isdarcodi, DbType.Int32, entity.Isdarcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int isdarcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Isdarcodi, DbType.Int32, isdarcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public InSustentoDetRelArchivoDTO GetById(int isdarcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Isdarcodi, DbType.Int32, isdarcodi);
            InSustentoDetRelArchivoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<InSustentoDetRelArchivoDTO> List()
        {
            List<InSustentoDetRelArchivoDTO> entitys = new List<InSustentoDetRelArchivoDTO>();
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

        public List<InSustentoDetRelArchivoDTO> GetByCriteria()
        {
            List<InSustentoDetRelArchivoDTO> entitys = new List<InSustentoDetRelArchivoDTO>();
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
