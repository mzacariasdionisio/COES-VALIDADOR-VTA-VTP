using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    public class TrnDemandaRepository : RepositoryBase, ITrnDemandaRepository
    {
        public TrnDemandaRepository(string strConn) : base(strConn)
        {

        }
        TrnDemandaHelper helper = new TrnDemandaHelper();

        public int Save(TrnDemandaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Demcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Valormaximo, DbType.Decimal, entity.Valormaximo);
            dbProvider.AddInParameter(command, helper.Periododemanda, DbType.String, entity.Periododemanda);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public TrnDemandaDTO ObtenerTrnDemanda(string periodo, int emprcodi)
        {
            String query = String.Format(helper.SqlListxEmpresaPeriodo, periodo, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            TrnDemandaDTO entity = new TrnDemandaDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public void Delete(TrnDemandaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Periododemanda, DbType.String, entity.Periododemanda);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.ExecuteNonQuery(command);            
        }
    }
}
