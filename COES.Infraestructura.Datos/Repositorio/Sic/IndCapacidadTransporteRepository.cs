using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class IndCapacidadTransporteRepository : RepositoryBase, IIndCapacidadTransporteRepository
    {
        public IndCapacidadTransporteRepository(string strConn) : base(strConn)
        {
        }

        IndCapacidadTransporteHelper helper = new IndCapacidadTransporteHelper();

        public int Save(IndCapacidadTransporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpctnscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.IPericodi, DbType.Int32, entity.Ipericodi);
            dbProvider.AddInParameter(command, helper.Cpctnsusucreacion, DbType.String, entity.Cpctnsusucreacion);
            dbProvider.AddInParameter(command, helper.Cpctnsfeccreacion, DbType.DateTime, entity.Cpctnsfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public List<IndCapacidadTransporteDTO> ListCapacidadTransporte(int emprcodi, int ipericodi)
        {
            IndCapacidadTransporteDTO entity = new IndCapacidadTransporteDTO();
            List<IndCapacidadTransporteDTO> entitys = new List<IndCapacidadTransporteDTO>();
            string query = string.Format(helper.SqlListCapacidadTransporte, emprcodi, ipericodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new IndCapacidadTransporteDTO();

                    int iCpctnscodi = dr.GetOrdinal(helper.Cpctnscodi);
                    if (!dr.IsDBNull(iCpctnscodi)) entity.Cpctnscodi = Convert.ToInt32(dr.GetValue(iCpctnscodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iIpericodi = dr.GetOrdinal(helper.IPericodi);
                    if (!dr.IsDBNull(iIpericodi)) entity.Ipericodi = Convert.ToInt32(dr.GetValue(iIpericodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
