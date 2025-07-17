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
    /// Clase de acceso a datos de la tabla IN_FACTOR_REL_MMAYOR_ARCHIVO
    /// </summary>
    public class InFactorRelMmayorArchivoRepository : RepositoryBase, IInFactorRelMmayorArchivoRepository
    {
        public InFactorRelMmayorArchivoRepository(string strConn) : base(strConn)
        {
        }

        InFactorRelMmayorArchivoHelper helper = new InFactorRelMmayorArchivoHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(InFactorRelMmayorArchivoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irmarcodi, DbType.Int32, entity.Irmarcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmcodi, DbType.Int32, entity.Infmmcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Inarchcodi, DbType.Int32, entity.Inarchcodi));

            command.ExecuteNonQuery();
            return entity.Irmarcodi;
        }

        public void Update(InFactorRelMmayorArchivoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Irmarcodi, DbType.Int32, entity.Irmarcodi);
            dbProvider.AddInParameter(command, helper.Infmmcodi, DbType.Int32, entity.Infmmcodi);
            dbProvider.AddInParameter(command, helper.Inarchcodi, DbType.Int32, entity.Inarchcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int irmarcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Irmarcodi, DbType.Int32, irmarcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public InFactorRelMmayorArchivoDTO GetById(int irmarcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Irmarcodi, DbType.Int32, irmarcodi);
            InFactorRelMmayorArchivoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<InFactorRelMmayorArchivoDTO> List()
        {
            List<InFactorRelMmayorArchivoDTO> entitys = new List<InFactorRelMmayorArchivoDTO>();
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

        public List<InFactorRelMmayorArchivoDTO> GetByCriteria()
        {
            List<InFactorRelMmayorArchivoDTO> entitys = new List<InFactorRelMmayorArchivoDTO>();
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
