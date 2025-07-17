using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_FORMATOHOJA
    /// </summary>
    public class MeFormatohojaHelper : HelperBase
    {
        public MeFormatohojaHelper(): base(Consultas.MeFormatohojaSql)
        {
        }

        public MeFormatohojaDTO Create(IDataReader dr)
        {
            MeFormatohojaDTO entity = new MeFormatohojaDTO();

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLectcodi = dr.GetOrdinal(this.Lectcodi);
            if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

            int iHojatitulo = dr.GetOrdinal(this.Hojatitulo);
            if (!dr.IsDBNull(iHojatitulo)) entity.Hojatitulo = dr.GetString(iHojatitulo);

            int iHojanumero = dr.GetOrdinal(this.Hojanumero);
            if (!dr.IsDBNull(iHojanumero)) entity.Hojanumero = Convert.ToInt32(dr.GetValue(iHojanumero));

            int iFormatcodi = dr.GetOrdinal(this.Formatcodi);
            if (!dr.IsDBNull(iFormatcodi)) entity.Formatcodi = Convert.ToInt32(dr.GetValue(iFormatcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";
        public string Lectcodi = "LECTCODI";
        public string Hojatitulo = "HOJATITULO";
        public string Hojanumero = "HOJANUMERO";
        public string Formatcodi = "FORMATCODI";

        #endregion
    }
}
