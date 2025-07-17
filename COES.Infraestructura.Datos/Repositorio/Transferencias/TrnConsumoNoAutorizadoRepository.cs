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
    public class TrnConsumoNoAutorizadoRepository : RepositoryBase, ITrnConsumoNoAutorizadoRepository
    {
        public TrnConsumoNoAutorizadoRepository(string strConn) : base(strConn)
        {

        }
        TrnConsumoNoAutorizadoHelper helper = new TrnConsumoNoAutorizadoHelper();

        public int Save(TrnConsumoNoAutorizadoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Conscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, entity.Emprnomb);
            dbProvider.AddInParameter(command, helper.Valorcna, DbType.Decimal, entity.Valorcna);
            dbProvider.AddInParameter(command, helper.Fechacna, DbType.DateTime, entity.Fechacna);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Delete(int emprcodi, string fechacna)
        {
            String query = String.Format(helper.SqlDelete, emprcodi, fechacna);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<TrnConsumoNoAutorizadoDTO> ListTrnConsumoNoAutorizado(string fechainicio, string fechafin)
        {
            String query = String.Format(helper.SqlList, fechainicio, fechafin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<TrnConsumoNoAutorizadoDTO> entitys = new List<TrnConsumoNoAutorizadoDTO>();

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
