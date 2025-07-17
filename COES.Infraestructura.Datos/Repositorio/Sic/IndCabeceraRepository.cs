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
    public class IndCabeceraRepository : RepositoryBase, IIndCabeceraRepository
    {
        public IndCabeceraRepository(string strConn) : base(strConn)
        {
        }

        IndCabeceraHelper helper = new IndCabeceraHelper();

        public int Save(IndCabeceraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Indcbrcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.String, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Ipericodi, DbType.Int32, entity.Ipericodi);
            dbProvider.AddInParameter(command, helper.Indcbrtipo, DbType.Int32, entity.Indcbrtipo);
            dbProvider.AddInParameter(command, helper.Indcbrusucreacion, DbType.String, entity.Indcbrusucreacion);
            dbProvider.AddInParameter(command, helper.Indcbrfeccreacion, DbType.DateTime, entity.Indcbrfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public List<IndCabeceraDTO> ListCabecera(int emprcodi, int ipericodi)
        {
            IndCabeceraDTO entity = new IndCabeceraDTO();
            List<IndCabeceraDTO> entitys = new List<IndCabeceraDTO>();
            string query = string.Format(helper.SqlListCabecera, emprcodi, ipericodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new IndCabeceraDTO();

                    int iIndcbrcodi = dr.GetOrdinal(helper.Indcbrcodi);
                    if (!dr.IsDBNull(iIndcbrcodi)) entity.Indcbrcodi = Convert.ToInt32(dr.GetValue(iIndcbrcodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iIpericodi = dr.GetOrdinal(helper.Ipericodi);
                    if (!dr.IsDBNull(iIpericodi)) entity.Ipericodi = Convert.ToInt32(dr.GetValue(iIpericodi));

                    int iIndcbrtipo = dr.GetOrdinal(helper.Indcbrtipo);
                    if (!dr.IsDBNull(iIndcbrtipo)) entity.Indcbrtipo = Convert.ToInt32(dr.GetValue(iIndcbrtipo));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
