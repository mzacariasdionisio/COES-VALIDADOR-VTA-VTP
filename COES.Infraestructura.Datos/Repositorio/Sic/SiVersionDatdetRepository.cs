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
    /// Clase de acceso a datos de la tabla SI_VERSION_DATDET
    /// </summary>
    public class SiVersionDatdetRepository : RepositoryBase, ISiVersionDatdetRepository
    {
        public SiVersionDatdetRepository(string strConn) : base(strConn)
        {
        }

        SiVersionDatdetHelper helper = new SiVersionDatdetHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(SiVersionDatdetDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Vdatdtcodi, DbType.Int32, entity.Vdatdtcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Vdatdtvalor, DbType.String, entity.Vdatdtvalor));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Vdatdtfecha, DbType.DateTime, entity.Vdatdtfecha));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Verdatcodi, DbType.Int32, entity.Verdatcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Vercnpcodi, DbType.Int32, entity.Vercnpcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Vercnpcodi, DbType.Int32, entity.Vdatdtid));
            command.ExecuteNonQuery();
            return entity.Vdatdtcodi;
        }

        public void Update(SiVersionDatdetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vdatdtcodi, DbType.Int32, entity.Vdatdtcodi);
            dbProvider.AddInParameter(command, helper.Vdatdtvalor, DbType.String, entity.Vdatdtvalor);
            dbProvider.AddInParameter(command, helper.Vdatdtfecha, DbType.DateTime, entity.Vdatdtfecha);
            dbProvider.AddInParameter(command, helper.Verdatcodi, DbType.Int32, entity.Verdatcodi);
            dbProvider.AddInParameter(command, helper.Verdatcodi, DbType.Int32, entity.Vdatdtid);
            dbProvider.AddInParameter(command, helper.Vercnpcodi, DbType.Int32, entity.Vercnpcodi);            

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vdatdtcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vdatdtcodi, DbType.Int32, vdatdtcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiVersionDatdetDTO GetById(int vdatdtcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vdatdtcodi, DbType.Int32, vdatdtcodi);
            SiVersionDatdetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiVersionDatdetDTO> List()
        {
            List<SiVersionDatdetDTO> entitys = new List<SiVersionDatdetDTO>();
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

        public List<SiVersionDatdetDTO> GetByCriteria(int versdtcodi)
        {
            List<SiVersionDatdetDTO> entitys = new List<SiVersionDatdetDTO>();

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
