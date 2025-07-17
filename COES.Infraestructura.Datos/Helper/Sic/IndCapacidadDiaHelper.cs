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
    public class IndCapacidadDiaHelper : HelperBase
    {
        public IndCapacidadDiaHelper() : base(Consultas.IndCapacidadDiaSql)
        {
        }

        #region Mapeo de Campos
        public string Cpcdiacodi = "CPCDIACODI";
        public string Indcpccodi = "INDCPCCODI";
        public string Cpcdiafecha = "CPCDIAFECHA";
        public string Cpcdiavalor = "CPCDIAVALOR";
        public string Cpcdiausucreacion = "CPCDIAUSUCREACION";
        public string Cpcdiafeccreacion = "CPCDIAFECCREACION";

        //Additional
        public string Indcbrcodi = "INDCBRCODI";
        public string Ipericodi = "IPERICODI";
        public string Emprcodi = "EMPRCODI";
        public string Indcbrtipo = "INDCBRTIPO";
        public string Equicodicentral = "EQUICODICENTRAL";
        public string Equicodiunidad = "EQUICODIUNIDAD";
        public string Grupocodi = "GRUPOCODI";
        public string Famcodi = "FAMCODI";
        public string Indcpctipo = "INDCPCTIPO";
        #endregion

        #region Consultas
        public string SqlListCapacidadDiaByCapacidad
        {
            get { return base.GetSqlXml("ListCapacidadDiaByCapacidad"); }
        }

        public string SqlListCapacidadDiaJoinCapacidad
        {
            get { return base.GetSqlXml("ListCapacidadDiaJoinCapacidad"); }
        }
        public string SqlUpdateValueByIdByDate
        {
            get { return base.GetSqlXml("UpdateValueByIdByDate"); }
        }
        #endregion

        #region Crear Datos
        private void SetCreate(IDataReader dr, IndCapacidadDiaDTO entity)
        {
            int iCpcdiacodi = dr.GetOrdinal(this.Cpcdiacodi);
            if (!dr.IsDBNull(iCpcdiacodi)) entity.Cpcdiacodi = Convert.ToInt32(dr.GetValue(iCpcdiacodi));

            int iIndcpccodi = dr.GetOrdinal(this.Indcpccodi);
            if (!dr.IsDBNull(iIndcpccodi)) entity.Indcpccodi = Convert.ToInt32(dr.GetValue(iIndcpccodi));

            int iCpcdiafecha = dr.GetOrdinal(this.Cpcdiafecha);
            if (!dr.IsDBNull(iCpcdiafecha)) entity.Cpcdiafecha = dr.GetDateTime(iCpcdiafecha);

            int iCpcdiavalor = dr.GetOrdinal(this.Cpcdiavalor);
            if (!dr.IsDBNull(iCpcdiavalor)) entity.Cpcdiavalor = Convert.ToDecimal(dr.GetValue(iCpcdiavalor));
        }

        #endregion

    }
}
