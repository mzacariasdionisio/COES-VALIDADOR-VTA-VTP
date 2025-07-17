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
    /// Clase de acceso a datos de la tabla CB_DATOS_DETALLE
    /// </summary>
    public class CbDatosDetalleRepository : RepositoryBase, ICbDatosDetalleRepository
    {
        public CbDatosDetalleRepository(string strConn) : base(strConn)
        {
        }

        readonly CbDatosDetalleHelper helper = new CbDatosDetalleHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(CbDatosDetalleDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbdetcodi, DbType.Int32, entity.Cbdetcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbevdacodi, DbType.Int32, entity.Cbevdacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbdetcompago, DbType.String, entity.Cbdetcompago));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbdettipo, DbType.Int32, entity.Cbdettipo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbdetfechaemision, DbType.DateTime, entity.Cbdetfechaemision));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbdettipocambio, DbType.Decimal, entity.Cbdettipocambio));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbdetmoneda, DbType.String, entity.Cbdetmoneda));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbdetvolumen, DbType.Decimal, entity.Cbdetvolumen));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbdetcosto, DbType.Decimal, entity.Cbdetcosto));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbdetcosto2, DbType.Decimal, entity.Cbdetcosto2));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbdetimpuesto, DbType.Decimal, entity.Cbdetimpuesto));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbdetdmrg, DbType.Decimal, entity.Cbdetdmrg));

            command.ExecuteNonQuery();
            return entity.Cbdetcodi;
        }

        public void Update(CbDatosDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cbdetcodi, DbType.Int32, entity.Cbdetcodi);
            dbProvider.AddInParameter(command, helper.Cbevdacodi, DbType.Int32, entity.Cbevdacodi);
            dbProvider.AddInParameter(command, helper.Cbdetcompago, DbType.String, entity.Cbdetcompago);
            dbProvider.AddInParameter(command, helper.Cbdettipo, DbType.Int32, entity.Cbdettipo);
            dbProvider.AddInParameter(command, helper.Cbdetfechaemision, DbType.DateTime, entity.Cbdetfechaemision);
            dbProvider.AddInParameter(command, helper.Cbdettipocambio, DbType.Decimal, entity.Cbdettipocambio);
            dbProvider.AddInParameter(command, helper.Cbdetmoneda, DbType.String, entity.Cbdetmoneda);
            dbProvider.AddInParameter(command, helper.Cbdetvolumen, DbType.Decimal, entity.Cbdetvolumen);
            dbProvider.AddInParameter(command, helper.Cbdetcosto, DbType.Decimal, entity.Cbdetcosto);
            dbProvider.AddInParameter(command, helper.Cbdetcosto2, DbType.Decimal, entity.Cbdetcosto2);
            dbProvider.AddInParameter(command, helper.Cbdetimpuesto, DbType.Decimal, entity.Cbdetimpuesto);
            dbProvider.AddInParameter(command, helper.Cbdetdmrg, DbType.Decimal, entity.Cbdetdmrg);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cbdetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cbdetcodi, DbType.Int32, cbdetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CbDatosDetalleDTO GetById(int cbdetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cbdetcodi, DbType.Int32, cbdetcodi);
            CbDatosDetalleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CbDatosDetalleDTO> List()
        {
            List<CbDatosDetalleDTO> entitys = new List<CbDatosDetalleDTO>();
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

        public List<CbDatosDetalleDTO> GetByCriteria(int cbvercodi)
        {
            List<CbDatosDetalleDTO> entitys = new List<CbDatosDetalleDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, cbvercodi);
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
