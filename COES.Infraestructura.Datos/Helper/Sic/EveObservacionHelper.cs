using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_OBSERVACION
    /// </summary>
    public class EveObservacionHelper : HelperBase
    {
        public EveObservacionHelper(): base(Consultas.EveObservacionSql)
        {
        }

        public EveObservacionDTO Create(IDataReader dr)
        {
            EveObservacionDTO entity = new EveObservacionDTO();

            int iObscodi = dr.GetOrdinal(this.Obscodi);
            if (!dr.IsDBNull(iObscodi)) entity.Obscodi = Convert.ToInt32(dr.GetValue(iObscodi));

            int iSubcausacodi = dr.GetOrdinal(this.Subcausacodi);
            if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

            int iObsfecha = dr.GetOrdinal(this.Obsfecha);
            if (!dr.IsDBNull(iObsfecha)) entity.Obsfecha = dr.GetDateTime(iObsfecha);

            int iObsdescrip = dr.GetOrdinal(this.Obsdescrip);
            if (!dr.IsDBNull(iObsdescrip)) entity.Obsdescrip = dr.GetString(iObsdescrip);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iEvenclasecodi = dr.GetOrdinal(this.Evenclasecodi);
            if (!dr.IsDBNull(iEvenclasecodi)) entity.Evenclasecodi = Convert.ToInt32(dr.GetValue(iEvenclasecodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Obscodi = "OBSCODI";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Obsfecha = "OBSFECHA";
        public string Obsdescrip = "OBSDESCRIP";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Evenclasecodi = "EVENCLASECODI";

        #endregion
    }
}
