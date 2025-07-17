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
    public class IndCapacidadDiaRepository : RepositoryBase, IIndCapacidadDiaRepository
    {
        public IndCapacidadDiaRepository(string strConn) : base(strConn)
        {
        }

        IndCapacidadDiaHelper helper = new IndCapacidadDiaHelper();

        public int Save(IndCapacidadDiaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpcdiacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Indcpccodi, DbType.Int32, entity.Indcpccodi);
            dbProvider.AddInParameter(command, helper.Cpcdiafecha, DbType.DateTime, entity.Cpcdiafecha);
            dbProvider.AddInParameter(command, helper.Cpcdiavalor, DbType.Decimal, entity.Cpcdiavalor);
            dbProvider.AddInParameter(command, helper.Cpcdiausucreacion, DbType.String, entity.Cpcdiausucreacion);
            dbProvider.AddInParameter(command, helper.Cpcdiafeccreacion, DbType.DateTime, entity.Cpcdiafeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void UpdateValueByIdByDate(IndCapacidadDiaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateValueByIdByDate);

            dbProvider.AddInParameter(command, helper.Cpcdiavalor, DbType.Decimal, entity.Cpcdiavalor);
            dbProvider.AddInParameter(command, helper.Indcpccodi, DbType.Int32, entity.Indcpccodi);
            dbProvider.AddInParameter(command, helper.Cpcdiafecha, DbType.DateTime, entity.Cpcdiafecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<IndCapacidadDiaDTO> ListCapacidadDiaByCapacidad(int indcpccodi)
        {
            IndCapacidadDiaDTO entity = new IndCapacidadDiaDTO();
            List<IndCapacidadDiaDTO> entitys = new List<IndCapacidadDiaDTO>();
            string query = string.Format(helper.SqlListCapacidadDiaByCapacidad, indcpccodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new IndCapacidadDiaDTO();

                    int iCpcdiacodi = dr.GetOrdinal(helper.Cpcdiacodi);
                    if (!dr.IsDBNull(iCpcdiacodi)) entity.Cpcdiacodi = Convert.ToInt32(dr.GetValue(iCpcdiacodi));

                    int iIndcpccodi = dr.GetOrdinal(helper.Indcpccodi);
                    if (!dr.IsDBNull(iIndcpccodi)) entity.Indcpccodi = Convert.ToInt32(dr.GetValue(iIndcpccodi));

                    int iCpcdiafecha = dr.GetOrdinal(helper.Cpcdiafecha);
                    if (!dr.IsDBNull(iCpcdiafecha)) entity.Cpcdiafecha = dr.GetDateTime(iCpcdiafecha);

                    int iCpcdiavalor = dr.GetOrdinal(helper.Cpcdiavalor);
                    if (!dr.IsDBNull(iCpcdiavalor)) entity.Cpcdiavalor = Convert.ToDecimal(dr.GetValue(iCpcdiavalor));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IndCapacidadDiaDTO> ListCapacidadDiaJoinCapacidad(int indcbrcodi)
        {
            IndCapacidadDiaDTO entity = new IndCapacidadDiaDTO();
            List<IndCapacidadDiaDTO> entitys = new List<IndCapacidadDiaDTO>();
            string query = string.Format(helper.SqlListCapacidadDiaJoinCapacidad, indcbrcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new IndCapacidadDiaDTO();

                    int iCpcdiacodi = dr.GetOrdinal(helper.Cpcdiacodi);
                    if (!dr.IsDBNull(iCpcdiacodi)) entity.Cpcdiacodi = Convert.ToInt32(dr.GetValue(iCpcdiacodi));

                    int iIndcpccodi = dr.GetOrdinal(helper.Indcpccodi);
                    if (!dr.IsDBNull(iIndcpccodi)) entity.Indcpccodi = Convert.ToInt32(dr.GetValue(iIndcpccodi));

                    int iEquicodicentral = dr.GetOrdinal(helper.Equicodicentral);
                    if (!dr.IsDBNull(iEquicodicentral)) entity.Equicodicentral = Convert.ToInt32(dr.GetValue(iEquicodicentral));

                    int iEquicodiunidad = dr.GetOrdinal(helper.Equicodiunidad);
                    if (!dr.IsDBNull(iEquicodiunidad)) entity.Equicodiunidad = Convert.ToInt32(dr.GetValue(iEquicodiunidad));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iCpcdiafecha = dr.GetOrdinal(helper.Cpcdiafecha);
                    if (!dr.IsDBNull(iCpcdiafecha)) entity.Cpcdiafecha = dr.GetDateTime(iCpcdiafecha);

                    int iIndcpctipo = dr.GetOrdinal(helper.Indcpctipo);
                    if (!dr.IsDBNull(iIndcpctipo)) entity.Indcpctipo = Convert.ToInt32(dr.GetValue(iIndcpctipo));

                    int iCpcdiavalor = dr.GetOrdinal(helper.Cpcdiavalor);
                    if (!dr.IsDBNull(iCpcdiavalor)) entity.Cpcdiavalor = Convert.ToDecimal(dr.GetValue(iCpcdiavalor));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
