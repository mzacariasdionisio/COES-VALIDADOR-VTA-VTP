using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IND_RECALCULO
    /// </summary>
    public class IndRecalculoHelper : HelperBase
    {
        public IndRecalculoHelper() : base(Consultas.IndRecalculoSql)
        {
        }

        public IndRecalculoDTO Create(IDataReader dr)
        {
            IndRecalculoDTO entity = new IndRecalculoDTO();

            int iIrecacodi = dr.GetOrdinal(this.Irecacodi);
            if (!dr.IsDBNull(iIrecacodi)) entity.Irecacodi = Convert.ToInt32(dr.GetValue(iIrecacodi));

            int iIrecadescripcion = dr.GetOrdinal(this.Irecadescripcion);
            if (!dr.IsDBNull(iIrecadescripcion)) entity.Irecadescripcion = dr.GetString(iIrecadescripcion);

            int iIrecanombre = dr.GetOrdinal(this.Irecanombre);
            if (!dr.IsDBNull(iIrecanombre)) entity.Irecanombre = dr.GetString(iIrecanombre);

            int iIpericodi = dr.GetOrdinal(this.Ipericodi);
            if (!dr.IsDBNull(iIpericodi)) entity.Ipericodi = Convert.ToInt32(dr.GetValue(iIpericodi));

            int iIrecausucreacion = dr.GetOrdinal(this.Irecausucreacion);
            if (!dr.IsDBNull(iIrecausucreacion)) entity.Irecausucreacion = dr.GetString(iIrecausucreacion);

            int iIrecafeccreacion = dr.GetOrdinal(this.Irecafeccreacion);
            if (!dr.IsDBNull(iIrecafeccreacion)) entity.Irecafeccreacion = dr.GetDateTime(iIrecafeccreacion);

            int iIrecausumodificacion = dr.GetOrdinal(this.Irecausumodificacion);
            if (!dr.IsDBNull(iIrecausumodificacion)) entity.Irecausumodificacion = dr.GetString(iIrecausumodificacion);

            int iIrecafecmodificacion = dr.GetOrdinal(this.Irecafecmodificacion);
            if (!dr.IsDBNull(iIrecafecmodificacion)) entity.Irecafecmodificacion = dr.GetDateTime(iIrecafecmodificacion);

            int iIrecainforme = dr.GetOrdinal(this.Irecainforme);
            if (!dr.IsDBNull(iIrecainforme)) entity.Irecainforme = dr.GetString(iIrecainforme);

            int iIrecafechalimite = dr.GetOrdinal(this.Irecafechalimite);
            if (!dr.IsDBNull(iIrecafechalimite)) entity.Irecafechalimite = dr.GetDateTime(iIrecafechalimite);

            int iIrecafechaini = dr.GetOrdinal(this.Irecafechaini);
            if (!dr.IsDBNull(iIrecafechaini)) entity.Irecafechaini = dr.GetDateTime(iIrecafechaini);

            int iIrecafechafin = dr.GetOrdinal(this.Irecafechafin);
            if (!dr.IsDBNull(iIrecafechafin)) entity.Irecafechafin = dr.GetDateTime(iIrecafechafin);

            int iIrecafechaobs = dr.GetOrdinal(this.Irecafechaobs);
            if (!dr.IsDBNull(iIrecafechaobs)) entity.Irecafechaobs = dr.GetDateTime(iIrecafechaobs);

            int iIrecatipo = dr.GetOrdinal(this.Irecatipo);
            if (!dr.IsDBNull(iIrecatipo)) entity.Irecatipo = dr.GetString(iIrecatipo);

            int iIrecaesfinal = dr.GetOrdinal(this.Irecaesfinal);
            if (!dr.IsDBNull(iIrecaesfinal)) entity.Irecaesfinal = Convert.ToInt32(dr.GetValue(iIrecaesfinal));

            return entity;
        }

        #region Mapeo de Campos

        public string Irecacodi = "IRECACODI";
        public string Irecadescripcion = "IRECADESCRIPCION";
        public string Irecanombre = "IRECANOMBRE";
        public string Ipericodi = "IPERICODI";
        public string Irecausucreacion = "IRECAUSUCREACION";
        public string Irecafeccreacion = "IRECAFECCREACION";
        public string Irecausumodificacion = "IRECAUSUMODIFICACION";
        public string Irecafecmodificacion = "IRECAFECMODIFICACION";
        public string Irecainforme = "IRECAINFORME";
        public string Irecafechalimite = "IRECAFECHALIMITE";
        public string Irecafechaini = "IRECAFECHAINI";
        public string Irecafechafin = "IRECAFECHAFIN";
        public string Irecafechaobs = "IRECAFECHAOBS";
        public string Irecatipo = "Irecatipo";
        public string Irecaesfinal = "Irecaesfinal";

        #endregion

        public string SqlListXMes
        {
            get { return base.GetSqlXml("ListXMes"); }
        }

    }
}
