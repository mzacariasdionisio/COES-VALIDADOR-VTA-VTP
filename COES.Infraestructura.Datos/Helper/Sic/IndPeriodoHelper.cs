using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IND_PERIODO
    /// </summary>
    public class IndPeriodoHelper : HelperBase
    {
        public IndPeriodoHelper() : base(Consultas.IndPeriodoSql)
        {
        }

        public IndPeriodoDTO Create(IDataReader dr)
        {
            IndPeriodoDTO entity = new IndPeriodoDTO();

            int iIpericodi = dr.GetOrdinal(this.Ipericodi);
            if (!dr.IsDBNull(iIpericodi)) entity.Ipericodi = Convert.ToInt32(dr.GetValue(iIpericodi));

            int iIperinombre = dr.GetOrdinal(this.Iperinombre);
            if (!dr.IsDBNull(iIperinombre)) entity.Iperinombre = dr.GetString(iIperinombre);

            int iIperianio = dr.GetOrdinal(this.Iperianio);
            if (!dr.IsDBNull(iIperianio)) entity.Iperianio = Convert.ToInt32(dr.GetValue(iIperianio));

            int iIperimes = dr.GetOrdinal(this.Iperimes);
            if (!dr.IsDBNull(iIperimes)) entity.Iperimes = Convert.ToInt32(dr.GetValue(iIperimes));
            
            int iIperianiofin = dr.GetOrdinal(this.Iperianiofin);
            if (!dr.IsDBNull(iIperianiofin)) entity.Iperianiofin = Convert.ToInt32(dr.GetValue(iIperianiofin));

            int iIperimesfin = dr.GetOrdinal(this.Iperimesfin);
            if (!dr.IsDBNull(iIperimesfin)) entity.Iperimesfin = Convert.ToInt32(dr.GetValue(iIperimesfin));

            int iIperianiomes = dr.GetOrdinal(this.Iperianiomes);
            if (!dr.IsDBNull(iIperianiomes)) entity.Iperianiomes = Convert.ToInt32(dr.GetValue(iIperianiomes));

            int iIperiestado = dr.GetOrdinal(this.Iperiestado);
            if (!dr.IsDBNull(iIperiestado)) entity.Iperiestado = dr.GetString(iIperiestado);

            int iIperihorizonte = dr.GetOrdinal(this.Iperihorizonte);
            if (!dr.IsDBNull(iIperihorizonte)) entity.Iperihorizonte = dr.GetString(iIperihorizonte);

            int iIperiusucreacion = dr.GetOrdinal(this.Iperiusucreacion);
            if (!dr.IsDBNull(iIperiusucreacion)) entity.Iperiusucreacion = dr.GetString(iIperiusucreacion);

            int iIperifeccreacion = dr.GetOrdinal(this.Iperifeccreacion);
            if (!dr.IsDBNull(iIperifeccreacion)) entity.Iperifeccreacion = dr.GetDateTime(iIperifeccreacion);

            int iIperiusumodificacion = dr.GetOrdinal(this.Iperiusumodificacion);
            if (!dr.IsDBNull(iIperiusumodificacion)) entity.Iperiusumodificacion = dr.GetString(iIperiusumodificacion);

            int iIperifecmodificacion = dr.GetOrdinal(this.Iperifecmodificacion);
            if (!dr.IsDBNull(iIperifecmodificacion)) entity.Iperifecmodificacion = dr.GetDateTime(iIperifecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Ipericodi = "IPERICODI";
        public string Iperinombre = "IPERINOMBRE";
        public string Iperianio = "IPERIANIO";
        public string Iperimes = "IPERIMES";
        public string Iperianiofin = "IPERIANIOFIN";
        public string Iperimesfin = "IPERIMESFIN";
        public string Iperianiomes = "IPERIANIOMES";
        public string Iperiestado = "IPERIESTADO";
        public string Iperihorizonte = "IPERIHORIZONTE";
        public string Iperiusucreacion = "IPERIUSUCREACION";
        public string Iperifeccreacion = "IPERIFECCREACION";
        public string Iperiusumodificacion = "IPERIUSUMODIFICACION";
        public string Iperifecmodificacion = "IPERIFECMODIFICACION";

        #endregion
    }
}
