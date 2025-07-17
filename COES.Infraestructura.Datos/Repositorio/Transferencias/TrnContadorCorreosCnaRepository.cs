using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    public class TrnContadorCorreosCnaRepository : RepositoryBase, ITrnContadorCorreosCnaRepository
    {
        public TrnContadorCorreosCnaRepository(string strConn) : base(strConn)
        {

        }

        TrnContadorCorreosCnaHelper helper = new TrnContadorCorreosCnaHelper();

        public void Delete(int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, emprcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public int ObtenerContadorParticipante(int emprcodi)
        {
            int contador = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerContadorParticipante);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, emprcodi);
            object result = dbProvider.ExecuteScalar(command);
            if (result != null)
            {
                contador = Convert.ToInt32(result);
                
            }
            //String query = String.Format(helper.SqlObtenerContadorParticipante, emprcodi);
            //DbCommand command = dbProvider.GetSqlStringCommand(query);
            //TrnContadorCorreosCnaDTO entity = new TrnContadorCorreosCnaDTO();

            //using (IDataReader dr = dbProvider.ExecuteReader(command))
            //{
            //    if (dr.Read())
            //    {
            //        entity = helper.Create(dr);
            //    }
            //}
            return contador;
        }

        public int Save(TrnContadorCorreosCnaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Contcnacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Cantcorreos, DbType.Int32, entity.Cantcorreos);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);         
            dbProvider.ExecuteNonQuery(command);
            return id;
        }
    }
}
