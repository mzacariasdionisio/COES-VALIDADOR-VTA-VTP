using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PF_RECALCULO
    /// </summary>
    public class PfRecalculoHelper : HelperBase
    {
        public PfRecalculoHelper(): base(Consultas.PfRecalculoSql)
        {
        }

        public PfRecalculoDTO Create(IDataReader dr)
        {
            PfRecalculoDTO entity = new PfRecalculoDTO();

            int iPfrecacodi = dr.GetOrdinal(this.Pfrecacodi);
            if (!dr.IsDBNull(iPfrecacodi)) entity.Pfrecacodi = Convert.ToInt32(dr.GetValue(iPfrecacodi));

            int iPfpericodi = dr.GetOrdinal(this.Pfpericodi);
            if (!dr.IsDBNull(iPfpericodi)) entity.Pfpericodi = Convert.ToInt32(dr.GetValue(iPfpericodi));

            int iIrecacodi = dr.GetOrdinal(this.Irecacodi);
            if (!dr.IsDBNull(iIrecacodi)) entity.Irecacodi = Convert.ToInt32(dr.GetValue(iIrecacodi));

            int iPfrecanombre = dr.GetOrdinal(this.Pfrecanombre);
            if (!dr.IsDBNull(iPfrecanombre)) entity.Pfrecanombre = dr.GetString(iPfrecanombre);

            int iPfrecadescripcion = dr.GetOrdinal(this.Pfrecadescripcion);
            if (!dr.IsDBNull(iPfrecadescripcion)) entity.Pfrecadescripcion = dr.GetString(iPfrecadescripcion);

            int iPfrecausucreacion = dr.GetOrdinal(this.Pfrecausucreacion);
            if (!dr.IsDBNull(iPfrecausucreacion)) entity.Pfrecausucreacion = dr.GetString(iPfrecausucreacion);

            int iPfrecafeccreacion = dr.GetOrdinal(this.Pfrecafeccreacion);
            if (!dr.IsDBNull(iPfrecafeccreacion)) entity.Pfrecafeccreacion = dr.GetDateTime(iPfrecafeccreacion);

            int iPfrecausumodificacion = dr.GetOrdinal(this.Pfrecausumodificacion);
            if (!dr.IsDBNull(iPfrecausumodificacion)) entity.Pfrecausumodificacion = dr.GetString(iPfrecausumodificacion);

            int iPfrecafecmodificacion = dr.GetOrdinal(this.Pfrecafecmodificacion);
            if (!dr.IsDBNull(iPfrecafecmodificacion)) entity.Pfrecafecmodificacion = dr.GetDateTime(iPfrecafecmodificacion);

            int iPfrecainforme = dr.GetOrdinal(this.Pfrecainforme);
            if (!dr.IsDBNull(iPfrecainforme)) entity.Pfrecainforme = dr.GetString(iPfrecainforme);   
            
            int iPfrecatipo = dr.GetOrdinal(this.Pfrecatipo);
            if (!dr.IsDBNull(iPfrecatipo)) entity.Pfrecatipo = dr.GetString(iPfrecatipo);

            int iPfrecafechalimite = dr.GetOrdinal(this.Pfrecafechalimite);
            if (!dr.IsDBNull(iPfrecafechalimite)) entity.Pfrecafechalimite = dr.GetDateTime(iPfrecafechalimite);

            return entity;
        }

        #region Mapeo de Campos

        public string Pfrecacodi = "PFRECACODI";
        public string Pfrecanombre = "PFRECANOMBRE";
        public string Pfrecadescripcion = "PFRECADESCRIPCION";
        public string Pfrecausucreacion = "PFRECAUSUCREACION";
        public string Pfrecafeccreacion = "PFRECAFECCREACION";
        public string Pfrecausumodificacion = "PFRECAUSUMODIFICACION";
        public string Pfrecafecmodificacion = "PFRECAFECMODIFICACION";
        public string Pfpericodi = "PFPERICODI";
        public string Irecacodi = "IRECACODI";
        public string Pfrecainforme = "PFRECAINFORME";
        public string Pfrecatipo = "PFRECATIPO";
        public string Pfrecafechalimite = "PFRECAFECHALIMITE";

        public string Pfperianio = "PFPERIANIO";
        public string Pfperimes = "PFPERIMES";

        #endregion
    }
}
