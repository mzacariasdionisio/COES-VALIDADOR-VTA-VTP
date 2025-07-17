using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_CALTIPOVENTO
    /// </summary>
    public class WbCaltipoventoHelper : HelperBase
    {
        public WbCaltipoventoHelper(): base(Consultas.WbCaltipoventoSql)
        {
        }

        public WbCaltipoventoDTO Create(IDataReader dr)
        {
            WbCaltipoventoDTO entity = new WbCaltipoventoDTO();

            int iTipcalcodi = dr.GetOrdinal(this.Tipcalcodi);
            if (!dr.IsDBNull(iTipcalcodi)) entity.Tipcalcodi = Convert.ToInt32(dr.GetValue(iTipcalcodi));

            int iTipcaldesc = dr.GetOrdinal(this.Tipcaldesc);
            if (!dr.IsDBNull(iTipcaldesc)) entity.Tipcaldesc = dr.GetString(iTipcaldesc);

            int iTipcalcolor = dr.GetOrdinal(this.Tipcalcolor);
            if (!dr.IsDBNull(iTipcalcolor)) entity.Tipcalcolor = dr.GetString(iTipcalcolor);

            int iTipcalicono = dr.GetOrdinal(this.Tipcalicono);
            if (!dr.IsDBNull(iTipcalicono)) entity.Tipcalicono = dr.GetString(iTipcalicono);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            return entity;
        }


        #region Mapeo de Campos

        public string Tipcalcodi = "TIPCALCODI";
        public string Tipcaldesc = "TIPCALDESC";
        public string Tipcalcolor = "TIPCALCOLOR";
        public string Tipcalicono = "TIPCALICONO";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";

        #endregion
    }
}
