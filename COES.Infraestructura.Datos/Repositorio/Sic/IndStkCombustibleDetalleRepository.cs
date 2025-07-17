using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla IND_STKCOMBUSTIBLE_DETALLE
    /// </summary>
    public class IndStkCombustibleDetalleRepository : RepositoryBase, IIndStkCombustibleDetalleRepository
    {
        private string strConexion;
        IndStkCombustibleDetalleHelper helper = new IndStkCombustibleDetalleHelper();

        public IndStkCombustibleDetalleRepository(string strConn) : base(strConn)
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
            return (DbTransaction) conn.BeginTransaction();
        }

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public IndStkCombustibleDetalleDTO GetById(int stkdetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Stkdetcodi, DbType.Int32, stkdetcodi);
            IndStkCombustibleDetalleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public void Save(IndStkCombustibleDetalleDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand) conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Stkdetcodi, DbType.Int32, entity.Stkdetcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Stkcmtcodi, DbType.Int32, entity.Stkcmtcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Stkdettipo, DbType.String, entity.Stkdettipo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D1, DbType.String, entity.D1));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D2, DbType.String, entity.D2));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D3, DbType.String, entity.D3));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D4, DbType.String, entity.D4));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D5, DbType.String, entity.D5));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D6, DbType.String, entity.D6));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D7, DbType.String, entity.D7));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D8, DbType.String, entity.D8));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D9, DbType.String, entity.D9));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D10, DbType.String, entity.D10));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D11, DbType.String, entity.D11));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D12, DbType.String, entity.D12));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D13, DbType.String, entity.D13));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D14, DbType.String, entity.D14));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D15, DbType.String, entity.D15));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D16, DbType.String, entity.D16));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D17, DbType.String, entity.D17));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D18, DbType.String, entity.D18));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D19, DbType.String, entity.D19));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D20, DbType.String, entity.D20));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D21, DbType.String, entity.D21));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D22, DbType.String, entity.D22));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D23, DbType.String, entity.D23));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D24, DbType.String, entity.D24));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D25, DbType.String, entity.D25));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D26, DbType.String, entity.D26));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D27, DbType.String, entity.D27));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D28, DbType.String, entity.D28));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D29, DbType.String, entity.D29));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D30, DbType.String, entity.D30));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D31, DbType.String, entity.D31));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Stkdetusucreacion, DbType.String, entity.Stkdetusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Stkdetfeccreacion, DbType.DateTime, entity.Stkdetfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Stkdetusumodificacion, DbType.String, entity.Stkdetusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Stkdetfecmodificacion, DbType.DateTime, entity.Stkdetfecmodificacion));
            command.ExecuteNonQuery();
        }

        public void UpdateDays(int stkdetcodi, string setupdates, string stkdetusumodificacion, DateTime stkdetfecmodificacion, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand) conn.CreateCommand();
            command.CommandText = string.Format(helper.SqlUpdateDays, setupdates);
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            dbProvider.AddInParameter(command, helper.Stkdetusumodificacion, DbType.String, stkdetusumodificacion);
            dbProvider.AddInParameter(command, helper.Stkdetfecmodificacion, DbType.DateTime, stkdetfecmodificacion);
            dbProvider.AddInParameter(command, helper.Stkdetcodi, DbType.Int32, stkdetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<IndStkCombustibleDetalleDTO> GetByCriteria(int stkcmtcodi, string stkdettipo)
        {
            List<IndStkCombustibleDetalleDTO> entitys = new List<IndStkCombustibleDetalleDTO>();

            string query = string.Format(helper.SqlGetByCriteria, stkcmtcodi, stkdettipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndStkCombustibleDetalleDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IndStkCombustibleDetalleDTO> GetByPeriod(int ipericodi, string emprcodi, string stkdettipo)
        {
            List<IndStkCombustibleDetalleDTO> entitys = new List<IndStkCombustibleDetalleDTO>();

            string query = string.Format(helper.SqlGetByPeriod, ipericodi, emprcodi, stkdettipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndStkCombustibleDetalleDTO entity = helper.CreateByPeriod(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }

}
