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
    public class TrnLogCnaRepository : RepositoryBase, ITrnLogCnaRepository
    {
        public TrnLogCnaRepository(string strConn) : base(strConn)
        {

        }
        TrnLogCnaHelper helper = new TrnLogCnaHelper();
        public List<TrnLogCnaDTO> List(DateTime fechaInicio, DateTime fechaFin)
        {
            String query = String.Format(helper.SqlList, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<TrnLogCnaDTO> entitys = new List<TrnLogCnaDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    //entitys.Add(helper.Create(dr));

                    TrnLogCnaDTO entity = helper.Create(dr);
                    int iCantCna = dr.GetOrdinal(this.helper.CantCna);
                    if (!dr.IsDBNull(iCantCna)) entity.Cantcna = Convert.ToInt32(dr.GetValue(iCantCna));

                    int iEmprrazsocial = dr.GetOrdinal(this.helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = Convert.ToString(dr.GetValue(iEmprrazsocial));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int Save(TrnLogCnaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Logcnacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.FechaProceso, DbType.DateTime, entity.FechaProceso);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }
    }
}
