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
    public class IndCapacidadRepository : RepositoryBase, IIndCapacidadRepository
    {
        public IndCapacidadRepository(string strConn) : base(strConn)
        {
        }

        IndCapacidadHelper helper = new IndCapacidadHelper();

        public int Save(IndCapacidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Indcpccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Indcbrcodi, DbType.Int32, entity.Indcbrcodi);
            dbProvider.AddInParameter(command, helper.Equicodicentral, DbType.Int32, entity.Equicodicentral);
            dbProvider.AddInParameter(command, helper.Equicodiunidad, DbType.Int32, entity.Equicodiunidad);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi);
            dbProvider.AddInParameter(command, helper.Indcpcfecinicio, DbType.DateTime, entity.Indcpcfecinicio);
            dbProvider.AddInParameter(command, helper.Indcpcfecfin, DbType.DateTime, entity.Indcpcfecfin);
            dbProvider.AddInParameter(command, helper.Indcpctipo, DbType.Int32, entity.Indcpctipo);
            dbProvider.AddInParameter(command, helper.Indcpcusucreacion, DbType.String, entity.Indcpcusucreacion);
            dbProvider.AddInParameter(command, helper.Indcpcfeccreacion, DbType.DateTime, entity.Indcpcfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void UpdateDateById(IndCapacidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateDateById);

            dbProvider.AddInParameter(command, helper.Indcpcfecinicio, DbType.DateTime, entity.Indcpcfecinicio);
            dbProvider.AddInParameter(command, helper.Indcpcfecfin, DbType.DateTime, entity.Indcpcfecfin);
            dbProvider.AddInParameter(command, helper.Indcpccodi, DbType.Int32, entity.Indcpccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<IndCapacidadDTO> ListCapacidadByCabecera(int indcbrcodi)
        {
            IndCapacidadDTO entity = new IndCapacidadDTO();
            List<IndCapacidadDTO> entitys = new List<IndCapacidadDTO>();
            string query = string.Format(helper.SqlListCapacidadByCabecera, indcbrcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new IndCapacidadDTO();

                    int iIndcpccodi = dr.GetOrdinal(helper.Indcpccodi);
                    if (!dr.IsDBNull(iIndcpccodi)) entity.Indcpccodi = Convert.ToInt32(dr.GetValue(iIndcpccodi));

                    int iIndcbrcodi = dr.GetOrdinal(helper.Indcbrcodi);
                    if (!dr.IsDBNull(iIndcbrcodi)) entity.Indcbrcodi = Convert.ToInt32(dr.GetValue(iIndcbrcodi));

                    int iEquicodicentral = dr.GetOrdinal(helper.Equicodicentral);
                    if (!dr.IsDBNull(iEquicodicentral)) entity.Equicodicentral = Convert.ToInt32(dr.GetValue(iEquicodicentral));

                    int iEquicodiunidad = dr.GetOrdinal(helper.Equicodiunidad);
                    if (!dr.IsDBNull(iEquicodiunidad)) entity.Equicodiunidad = Convert.ToInt32(dr.GetValue(iEquicodiunidad));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iIndcpcfecinicio = dr.GetOrdinal(helper.Indcpcfecinicio);
                    if (!dr.IsDBNull(iIndcpcfecinicio)) entity.Indcpcfecinicio = dr.GetDateTime(iIndcpcfecinicio);

                    int iIndcpcfecfin = dr.GetOrdinal(helper.Indcpcfecfin);
                    if (!dr.IsDBNull(iIndcpcfecfin)) entity.Indcpcfecfin = dr.GetDateTime(iIndcpcfecfin);

                    int iIndcpctipo = dr.GetOrdinal(helper.Indcpctipo);
                    if (!dr.IsDBNull(iIndcpctipo)) entity.Indcpctipo = Convert.ToInt32(dr.GetValue(iIndcpctipo));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IndCapacidadDTO> ListCapacidadJoinCabecera(int emprcodi, int ipericodi, int indcbrtipo)
        {
            IndCapacidadDTO entity = new IndCapacidadDTO();
            List<IndCapacidadDTO> entitys = new List<IndCapacidadDTO>();
            string query = string.Format(helper.SqlListCapacidadJoinCabecera, emprcodi, ipericodi, indcbrtipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new IndCapacidadDTO();

                    int iIndcbrcodi = dr.GetOrdinal(helper.Indcbrcodi);
                    if (!dr.IsDBNull(iIndcbrcodi)) entity.Indcbrcodi = Convert.ToInt32(dr.GetValue(iIndcbrcodi));

                    int iIndcpccodi = dr.GetOrdinal(helper.Indcpccodi);
                    if (!dr.IsDBNull(iIndcpccodi)) entity.Indcpccodi = Convert.ToInt32(dr.GetValue(iIndcpccodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iIpericodi = dr.GetOrdinal(helper.Ipericodi);
                    if (!dr.IsDBNull(iIpericodi)) entity.Ipericodi = Convert.ToInt32(dr.GetValue(iIpericodi));

                    int iEquicodicentral = dr.GetOrdinal(helper.Equicodicentral);
                    if (!dr.IsDBNull(iEquicodicentral)) entity.Equicodicentral = Convert.ToInt32(dr.GetValue(iEquicodicentral));

                    int iEquicodiunidad = dr.GetOrdinal(helper.Equicodiunidad);
                    if (!dr.IsDBNull(iEquicodiunidad)) entity.Equicodiunidad = Convert.ToInt32(dr.GetValue(iEquicodiunidad));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iIndcpcfecinicio = dr.GetOrdinal(helper.Indcpcfecinicio);
                    if (!dr.IsDBNull(iIndcpcfecinicio)) entity.Indcpcfecinicio = dr.GetDateTime(iIndcpcfecinicio);

                    int iIndcpcfecfin = dr.GetOrdinal(helper.Indcpcfecfin);
                    if (!dr.IsDBNull(iIndcpcfecfin)) entity.Indcpcfecfin = dr.GetDateTime(iIndcpcfecfin);

                    int iIndcpctipo = dr.GetOrdinal(helper.Indcpctipo);
                    if (!dr.IsDBNull(iIndcpctipo)) entity.Indcpctipo = Convert.ToInt32(dr.GetValue(iIndcpctipo));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IndCapacidadDTO> ListByCriteria(int ipericodi, string emprcodi, string indcbrtipo, string equicodicentral, string equicodiunidad, string grupocodi, string famcodi, string indcpctipo)
        {
            List<IndCapacidadDTO> entitys = new List<IndCapacidadDTO>();
            string query = string.Format(helper.SqlListByCriteria, ipericodi, emprcodi, indcbrtipo, equicodicentral, equicodiunidad, grupocodi, famcodi, indcpctipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndCapacidadDTO entity = helper.CreateListByCriteria(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
