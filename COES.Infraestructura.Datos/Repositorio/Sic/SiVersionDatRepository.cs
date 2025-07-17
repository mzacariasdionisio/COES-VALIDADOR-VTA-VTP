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
    /// Clase de acceso a datos de la tabla SI_VERSION_DAT
    /// </summary>
    public class SiVersionDatRepository : RepositoryBase, ISiVersionDatRepository
    {
        public SiVersionDatRepository(string strConn) : base(strConn)
        {
        }

        SiVersionDatHelper helper = new SiVersionDatHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(SiVersionDatDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Verdatcodi, DbType.Int32, entity.Verdatcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Vercnpcodi, DbType.Int32, entity.Vercnpcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Verdatvalor, DbType.String, entity.Verdatvalor));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Verdatvalor2, DbType.String, entity.Verdatvalor2));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Verdatid, DbType.Int32, entity.Verdatid));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Versdtcodi, DbType.Int32, entity.Versdtcodi));

            command.ExecuteNonQuery();
            return entity.Verdatcodi;
        }

        public void Update(SiVersionDatDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Verdatcodi, DbType.Int32, entity.Verdatcodi);
            dbProvider.AddInParameter(command, helper.Vercnpcodi, DbType.Int32, entity.Vercnpcodi);
            dbProvider.AddInParameter(command, helper.Verdatvalor, DbType.String, entity.Verdatvalor);
            dbProvider.AddInParameter(command, helper.Verdatvalor2, DbType.String, entity.Verdatvalor2);
            dbProvider.AddInParameter(command, helper.Verdatid, DbType.Int32, entity.Verdatid);
            dbProvider.AddInParameter(command, helper.Versdtcodi, DbType.Int32, entity.Versdtcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int verdatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Verdatcodi, DbType.Int32, verdatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiVersionDatDTO GetById(int verdatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Verdatcodi, DbType.Int32, verdatcodi);
            SiVersionDatDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiVersionDatDTO> List()
        {
            List<SiVersionDatDTO> entitys = new List<SiVersionDatDTO>();
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

        public List<SiVersionDatDTO> GetByCriteria(int versdtcodi)
        {
            List<SiVersionDatDTO> entitys = new List<SiVersionDatDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, versdtcodi);
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
