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
    public class TrnPeriodoCnaRepository : RepositoryBase, ITrnPeriodoCnaRepository
    {
        public TrnPeriodoCnaRepository(string strConn) : base(strConn)
        {

        }
        TrnPeriodoCnaHelper helper = new TrnPeriodoCnaHelper();
        public TrnPeriodoCnaDTO ObtenerSemana(string semanaperiodo)
        {
            String query = String.Format(helper.SqlObtenerSemana, semanaperiodo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            TrnPeriodoCnaDTO entity = new TrnPeriodoCnaDTO();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public int Save(TrnPeriodoCnaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Percnacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Dd, DbType.String, entity.Dd);
            dbProvider.AddInParameter(command, helper.Dl, DbType.String, entity.Dl);
            dbProvider.AddInParameter(command, helper.Dm, DbType.String, entity.Dm);
            dbProvider.AddInParameter(command, helper.Dmm, DbType.String, entity.Dmm);
            dbProvider.AddInParameter(command, helper.Dj, DbType.String, entity.Dj);
            dbProvider.AddInParameter(command, helper.Dvr, DbType.String, entity.Dvr);
            dbProvider.AddInParameter(command, helper.Ds, DbType.String, entity.Ds);
            dbProvider.AddInParameter(command, helper.Semperiodo, DbType.String, entity.Semperiodo);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Delete(TrnPeriodoCnaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Semperiodo, DbType.String, entity.Semperiodo);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}
