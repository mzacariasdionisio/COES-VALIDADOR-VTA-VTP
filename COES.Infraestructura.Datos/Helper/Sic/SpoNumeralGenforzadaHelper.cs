using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SPO_NUMERAL_GENFORZADA
    /// </summary>
    public class SpoNumeralGenforzadaHelper : HelperBase
    {
        public SpoNumeralGenforzadaHelper() : base(Consultas.SpoNumeralGenforzadaSql)
        {
        }

        public SpoNumeralGenforzadaDTO Create(IDataReader dr)
        {
            SpoNumeralGenforzadaDTO entity = new SpoNumeralGenforzadaDTO();

            int iGenforcodi = dr.GetOrdinal(this.Genforcodi);
            if (!dr.IsDBNull(iGenforcodi)) entity.Genforcodi = Convert.ToInt32(dr.GetValue(iGenforcodi));

            int iVerncodi = dr.GetOrdinal(this.Verncodi);
            if (!dr.IsDBNull(iVerncodi)) entity.Verncodi = Convert.ToInt32(dr.GetValue(iVerncodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iHopcausacodi = dr.GetOrdinal(this.Hopcausacodi);
            if (!dr.IsDBNull(iHopcausacodi)) entity.Hopcausacodi = Convert.ToInt32(dr.GetValue(iHopcausacodi));

            int iGenforhorini = dr.GetOrdinal(this.Genforhorini);
            if (!dr.IsDBNull(iGenforhorini)) entity.Genforhorini = dr.GetDateTime(iGenforhorini);

            int iGenforhorfin = dr.GetOrdinal(this.Genforhorfin);
            if (!dr.IsDBNull(iGenforhorfin)) entity.Genforhorfin = dr.GetDateTime(iGenforhorfin);

            int iGenformw = dr.GetOrdinal(this.Genformw);
            if (!dr.IsDBNull(iGenformw)) entity.Genformw = dr.GetDecimal(iGenformw);

            int iGenforusucreacion = dr.GetOrdinal(this.Genforusucreacion);
            if (!dr.IsDBNull(iGenforusucreacion)) entity.Genforusucreacion = dr.GetString(iGenforusucreacion);

            int iGenforfeccreacion = dr.GetOrdinal(this.Genforfeccreacion);
            if (!dr.IsDBNull(iGenforfeccreacion)) entity.Genforfeccreacion = dr.GetDateTime(iGenforfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Genforcodi = "GENFORCODI";
        public string Verncodi = "VERNCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Grupocodi = "GRUPOCODI";
        public string Hopcausacodi = "HOPCAUSACODI";
        public string Genforhorini = "GENFORHORINI";
        public string Genforhorfin = "GENFORHORFIN";
        public string Genformw = "GENFORMW";
        public string Genforusucreacion = "GENFORUSUCREACION";
        public string Genforfeccreacion = "GENFORFECCREACION";

        public string Gruponomb = "GRUPONOMB";

        #endregion
    }
}
