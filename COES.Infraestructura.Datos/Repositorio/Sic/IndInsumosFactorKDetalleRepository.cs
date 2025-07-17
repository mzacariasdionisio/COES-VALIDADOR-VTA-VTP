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
    /// Clase de acceso a datos de la tabla IND_INSUMOS_FACTORK
    /// </summary>
    public class IndInsumosFactorKDetalleRepository : RepositoryBase, IIndInsumosFactorKDetalleRepository
    {
        private string strConexion;
        IndInsumosFactorKDetalleHelper helper = new IndInsumosFactorKDetalleHelper();

        public IndInsumosFactorKDetalleRepository(string strConn) : base(strConn)
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

        public IndInsumosFactorKDetalleDTO GetById(int infkdtcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Infkdtcodi, DbType.Int32, infkdtcodi);

            IndInsumosFactorKDetalleDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public IndInsumosFactorKDetalleDTO GetByCriteria(int insfckcodi, int infkdttipo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Insfckcodi, DbType.Int32, insfckcodi);
            dbProvider.AddInParameter(command, helper.Infkdttipo, DbType.String, infkdttipo);

            IndInsumosFactorKDetalleDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public void Save(IndInsumosFactorKDetalleDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand) conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infkdtcodi, DbType.Int32, entity.Infkdtcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Insfckcodi, DbType.Int32, entity.Insfckcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infkdttipo, DbType.String, entity.Infkdttipo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D1, DbType.Decimal, entity.D1));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D2, DbType.Decimal, entity.D2));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D3, DbType.Decimal, entity.D3));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D4, DbType.Decimal, entity.D4));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D5, DbType.Decimal, entity.D5));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D6, DbType.Decimal, entity.D6));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D7, DbType.Decimal, entity.D7));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D8, DbType.Decimal, entity.D8));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D9, DbType.Decimal, entity.D9));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D10, DbType.Decimal, entity.D10));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D11, DbType.Decimal, entity.D11));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D12, DbType.Decimal, entity.D12));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D13, DbType.Decimal, entity.D13));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D14, DbType.Decimal, entity.D14));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D15, DbType.Decimal, entity.D15));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D16, DbType.Decimal, entity.D16));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D17, DbType.Decimal, entity.D17));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D18, DbType.Decimal, entity.D18));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D19, DbType.Decimal, entity.D19));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D20, DbType.Decimal, entity.D20));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D21, DbType.Decimal, entity.D21));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D22, DbType.Decimal, entity.D22));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D23, DbType.Decimal, entity.D23));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D24, DbType.Decimal, entity.D24));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D25, DbType.Decimal, entity.D25));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D26, DbType.Decimal, entity.D26));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D27, DbType.Decimal, entity.D27));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D28, DbType.Decimal, entity.D28));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D29, DbType.Decimal, entity.D29));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D30, DbType.Decimal, entity.D30));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D31, DbType.Decimal, entity.D31));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infkdtusucreacion, DbType.String, entity.Infkdtusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infkdtfeccreacion, DbType.DateTime, entity.Infkdtfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infkdtusumodificacion, DbType.String, entity.Infkdtusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infkdtfecmodificacion, DbType.DateTime, entity.Infkdtfecmodificacion));
            command.ExecuteNonQuery();
        }

        public void UpdateDays(int infkdtcodi, string setupdates, string infkdtusumodificacion, DateTime infkdtfecmodificacion, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand) conn.CreateCommand();
            command.CommandText = string.Format(helper.SqlUpdateDays, setupdates);
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            dbProvider.AddInParameter(command, helper.Infkdtusumodificacion, DbType.String, infkdtusumodificacion);
            dbProvider.AddInParameter(command, helper.Infkdtfecmodificacion, DbType.DateTime, infkdtfecmodificacion);
            dbProvider.AddInParameter(command, helper.Infkdtcodi, DbType.Int32, infkdtcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByCriteria(string insfckcodi, string infkdttipo, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand) conn.CreateCommand();
            command.CommandText = string.Format(helper.SqlDeleteByCriteria, insfckcodi, infkdttipo);
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            dbProvider.ExecuteNonQuery(command);
        }

        public List<IndInsumosFactorKDetalleDTO> GetByInsumosFactorK(int ipericodi, int emprcodi, int equicodicentral, int equicodiunidad, int grupocodi, int famcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByInsumosFactorK);
            dbProvider.AddInParameter(command, helper.Ipericodi, DbType.Int32, ipericodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodicentral, DbType.Int32, equicodicentral);
            dbProvider.AddInParameter(command, helper.Equicodiunidad, DbType.Int32, equicodiunidad);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, famcodi);

            List<IndInsumosFactorKDetalleDTO> list = new List<IndInsumosFactorKDetalleDTO>();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndInsumosFactorKDetalleDTO entity = helper.Create(dr);
                    list.Add(entity);
                }
            }

            return list;
        }

        public List<IndInsumosFactorKDetalleDTO> GetByPeriodo(int ipericodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByPeriodo);
            dbProvider.AddInParameter(command, helper.Ipericodi, DbType.Int32, ipericodi);

            List<IndInsumosFactorKDetalleDTO> list = new List<IndInsumosFactorKDetalleDTO>();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndInsumosFactorKDetalleDTO entity = helper.Create(dr);
                    list.Add(entity);
                }
            }

            return list;
        }
    }
}
