using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class IndCapacidadHelper : HelperBase
    {
        public IndCapacidadHelper() : base(Consultas.IndCapacidadSql)
        {
        }

        #region Mapeo de Campos
        public string Indcpccodi = "INDCPCCODI";
        public string Indcbrcodi = "INDCBRCODI";
        public string Equicodicentral = "EQUICODICENTRAL";
        public string Equicodiunidad = "EQUICODIUNIDAD";
        public string Grupocodi = "GRUPOCODI";
        public string Famcodi = "FAMCODI";
        public string Indcpcfecinicio = "INDCPCFECINICIO";
        public string Indcpcfecfin = "INDCPCFECFIN";
        public string Indcpctipo = "INDCPCTIPO";
        public string Indcpcusucreacion = "INDCPCUSUCREACION";
        public string Indcpcfeccreacion = "INDCPCFECCREACION";
        public string Indcpcusumodificacion = "INDCPCUSUMODIFICACION";
        public string Indcpcfecmodificacion = "INDCPCFECMODIFICACION";

        //Additional
        public string Ipericodi = "IPERICODI";
        public string Emprcodi = "EMPRCODI";
        public string Indcbrtipo = "INDCBRTIPO";
        #endregion

        #region Consultas
        public string SqlListCapacidadByCabecera
        {
            get { return base.GetSqlXml("ListCapacidadByCabecera"); }
        }
        public string SqlListCapacidadJoinCabecera
        {
            get { return base.GetSqlXml("ListCapacidadJoinCabecera"); }
        }
        public string SqlUpdateDateById
        {
            get { return base.GetSqlXml("UpdateDateById"); }
        }

        public string SqlListByCriteria
        {
            get { return base.GetSqlXml("ListByCriteria"); }
        }
        #endregion

        #region Crear Datos
        private void SetCreate(IDataReader dr, IndCapacidadDTO entity)
        {
            int iIndcpccodi = dr.GetOrdinal(this.Indcpccodi);
            if (!dr.IsDBNull(iIndcpccodi)) entity.Indcpccodi = Convert.ToInt32(dr.GetValue(iIndcpccodi));

            int iIndcbrcodi = dr.GetOrdinal(this.Indcbrcodi);
            if (!dr.IsDBNull(iIndcbrcodi)) entity.Indcbrcodi = Convert.ToInt32(dr.GetValue(iIndcbrcodi));

            int iEquicodicentral = dr.GetOrdinal(this.Equicodicentral);
            if (!dr.IsDBNull(iEquicodicentral)) entity.Equicodicentral = Convert.ToInt32(dr.GetValue(iEquicodicentral));

            int iEquicodiunidad = dr.GetOrdinal(this.Equicodiunidad);
            if (!dr.IsDBNull(iEquicodiunidad)) entity.Equicodiunidad = Convert.ToInt32(dr.GetValue(iEquicodiunidad));

            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

            int iIndcpcfecinicio = dr.GetOrdinal(this.Indcpcfecinicio);
            if (!dr.IsDBNull(iIndcpcfecinicio)) entity.Indcpcfecinicio = dr.GetDateTime(iIndcpcfecinicio);

            int iIndcpcfecfin = dr.GetOrdinal(this.Indcpcfecfin);
            if (!dr.IsDBNull(iIndcpcfecfin)) entity.Indcpcfecfin = dr.GetDateTime(iIndcpcfecfin);

            int iIndcpctipo = dr.GetOrdinal(this.Indcpctipo);
            if (!dr.IsDBNull(iIndcpctipo)) entity.Indcpctipo = Convert.ToInt32(dr.GetValue(iIndcpctipo));
        }

        public IndCapacidadDTO CreateListByCriteria(IDataReader dr)
        {
            IndCapacidadDTO entity = new IndCapacidadDTO();
            SetCreate(dr, entity);

            int iIpericodi = dr.GetOrdinal(this.Ipericodi);
            if (!dr.IsDBNull(iIpericodi)) entity.Ipericodi = Convert.ToInt32(dr.GetValue(iIpericodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iIndcbrtipo = dr.GetOrdinal(this.Indcbrtipo);
            if (!dr.IsDBNull(iIndcbrtipo)) entity.Indcbrtipo = Convert.ToInt32(dr.GetValue(iIndcbrtipo));


            return entity;
        }
        #endregion

    }
}
