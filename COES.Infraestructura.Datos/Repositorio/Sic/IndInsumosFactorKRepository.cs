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
    public class IndInsumosFactorKRepository : RepositoryBase, IIndInsumosFactorKRepository
    {
        private string strConexion;
        IndInsumosFactorKHelper helper = new IndInsumosFactorKHelper();

        public IndInsumosFactorKRepository(string strConn) : base(strConn)
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

        public IndInsumosFactorKDTO GetById(int insfckcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Insfckcodi, DbType.Int32, insfckcodi);

            IndInsumosFactorKDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public IndInsumosFactorKDTO GetByCriteria(int ipericodi, int emprcodi, int equicodicentral, int equicodiunidad, int grupocodi, int famcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Ipericodi, DbType.Int32, ipericodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodicentral, DbType.Int32, equicodicentral);
            dbProvider.AddInParameter(command, helper.Equicodiunidad, DbType.Int32, equicodiunidad);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, famcodi);

            IndInsumosFactorKDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.CreateByCriteria(dr);
                }
            }

            return entity;
        }

        public List<IndInsumosFactorKDTO> GetByPeriodo(int ipericodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByPeriodo);
            dbProvider.AddInParameter(command, helper.Ipericodi, DbType.Int32, ipericodi);

            List<IndInsumosFactorKDTO> list = new List<IndInsumosFactorKDTO>();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndInsumosFactorKDTO entity = helper.CreateByCriteria(dr);
                    list.Add(entity);
                }
            }

            return list;
        }

        public void Save(IndInsumosFactorKDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand) conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Insfckcodi, DbType.Int32, entity.Insfckcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ipericodi, DbType.Int32, entity.Ipericodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodicentral, DbType.Int32, entity.Equicodicentral));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodiunidad, DbType.Int32, entity.Equicodiunidad));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Insfckfrc, DbType.Decimal, entity.Insfckfrc));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Insfckusucreacion, DbType.String, entity.Insfckusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Insfckfeccreacion, DbType.DateTime, entity.Insfckfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Insfckusumodificacion, DbType.String, entity.Insfckusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Insfckfecmodificacion, DbType.DateTime, entity.Insfckfecmodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Insfckusuultimp, DbType.String, entity.Insfckusuultimp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Insfckfecultimp, DbType.DateTime, entity.Insfckfecultimp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Insfckranfecultimp, DbType.String, entity.Insfckranfecultimp));
            command.ExecuteNonQuery();
        }

        public void UpdateFRC(int insfckcodi, decimal insfckfrc, string insfckusumodificacion, DateTime insfckfecmodificacion, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand) conn.CreateCommand();
            command.CommandText = helper.SqlUpdateFRC;
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            dbProvider.AddInParameter(command, helper.Insfckfrc, DbType.Decimal, insfckfrc);
            dbProvider.AddInParameter(command, helper.Insfckusumodificacion, DbType.String, insfckusumodificacion);
            dbProvider.AddInParameter(command, helper.Insfckfecmodificacion, DbType.DateTime, insfckfecmodificacion);
            dbProvider.AddInParameter(command, helper.Insfckcodi, DbType.Int32, insfckcodi);

            command.ExecuteNonQuery();
        }

        public void UpdateFRCByImport(int insfckcodi, decimal insfckfrc, string insfckusuultimp, DateTime insfckfecultimp, string insfckranfecultimp, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand) conn.CreateCommand();
            command.CommandText = helper.SqlUpdateFRCByImport;
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            dbProvider.AddInParameter(command, helper.Insfckfrc, DbType.Decimal, insfckfrc);
            dbProvider.AddInParameter(command, helper.Insfckusuultimp, DbType.String, insfckusuultimp);
            dbProvider.AddInParameter(command, helper.Insfckfecultimp, DbType.DateTime, insfckfecultimp);
            dbProvider.AddInParameter(command, helper.Insfckranfecultimp, DbType.String, insfckranfecultimp);
            dbProvider.AddInParameter(command, helper.Insfckcodi, DbType.Int32, insfckcodi);

            command.ExecuteNonQuery();
        }

    }
}
