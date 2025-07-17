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

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla IND_HISTORICO_STOCKCOMBUST
    /// </summary>
    public class IndHistoricoStockCombustRepository : RepositoryBase, IIndHistoricoStockCombustRepository
    {
        public IndHistoricoStockCombustRepository(string strConn) : base(strConn)
        {
        }

        IndHistoricoStockCombustHelper helper = new IndHistoricoStockCombustHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public void Save(IndHistoricoStockCombustDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand) conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hststkcodi, DbType.Int32, entity.Hststkcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Stkcmtcodi, DbType.Int32, entity.Stkcmtcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ipericodi, DbType.Int32, entity.Ipericodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodicentral, DbType.Int32, entity.Equicodicentral));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodiunidad, DbType.Int32, entity.Equicodiunidad));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hststkperiodo, DbType.String, entity.Hststkperiodo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hststkempresa, DbType.String, entity.Hststkempresa));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hststkcentral, DbType.String, entity.Hststkcentral));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hststkunidad, DbType.String, entity.Hststkunidad));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hststktipoinfo, DbType.String, entity.Hststktipoinfo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hststkfecha, DbType.DateTime, entity.Hststkfecha));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hststkoriginal, DbType.String, entity.Hststkoriginal));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hststkmodificado, DbType.String, entity.Hststkmodificado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hststktipaccion, DbType.String, entity.Hststktipaccion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hststkusucreacion, DbType.String, entity.Hststkusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hststkfeccreacion, DbType.DateTime, entity.Hststkfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hststkusumodificacion, DbType.String, entity.Hststkusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hststkfecmodificacion, DbType.DateTime, entity.Hststkfecmodificacion));
            command.ExecuteNonQuery();
        }

        public List<IndHistoricoStockCombustDTO> GetByCriteria(int ipericodi)
        {
            List<IndHistoricoStockCombustDTO> entitys = new List<IndHistoricoStockCombustDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ipericodi, DbType.Int32, ipericodi));

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
