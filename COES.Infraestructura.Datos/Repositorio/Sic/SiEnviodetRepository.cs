using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using COES.Framework.Base.Tools;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla ME_ENVIO
    /// </summary>
    public class SiEnviodetRepository : RepositoryBase, ISiEnviodetRepository
    {
        private string strConexion;
        SiEnviodetHelper helper = new SiEnviodetHelper();

        public SiEnviodetRepository(string strConn): base(strConn)
        {
            strConexion = strConn;
        }

        public IDbConnection BeginConnection()
        {
            Database db = DatabaseFactory.CreateDatabase(strConexion);
            IDbConnection conn = db.CreateConnection();
            conn.Open();
            return conn;
        }

        public DbTransaction StartTransaction(IDbConnection conn)
        {
            return (DbTransaction)conn.BeginTransaction();
        }
        
        public void Save(SiEnviodetDTO entity)
        {
            //DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            //object result = dbProvider.ExecuteScalar(command);
            //int id = 1;
            //if (result != null) id = Convert.ToInt32(result);
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, entity.Enviocodi);
            dbProvider.AddInParameter(command, helper.Fdatpkcodi, DbType.Int32, entity.Fdatpkcodi);

            dbProvider.ExecuteNonQuery(command);

            //return id;
        }

        public void Update(SiEnviodetDTO entity)
        {
            string stQuery = string.Format(helper.SqlUpdate, entity.Enviocodi, entity.Fdatpkcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(stQuery);
            dbProvider.ExecuteNonQuery(command);
        }
        
        public SiEnviodetDTO GetById(int idEnvio)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, idEnvio);
            SiEnviodetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiEnviodetDTO> List()
        {
            List<SiEnviodetDTO> entitys = new List<SiEnviodetDTO>();
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
    }
}

