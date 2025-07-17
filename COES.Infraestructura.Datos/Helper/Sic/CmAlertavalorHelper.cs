using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_ALERTAVALOR
    /// </summary>
    public class CmAlertavalorHelper : HelperBase
    {
        public CmAlertavalorHelper() : base(Consultas.CmAlertavalorSql)
        {
        }

        public CmAlertavalorDTO Create(IDataReader dr)
        {
            CmAlertavalorDTO entity = new CmAlertavalorDTO();

            int iAlevalcodi = dr.GetOrdinal(this.Alevalcodi);
            if (!dr.IsDBNull(iAlevalcodi)) entity.Alevalcodi = Convert.ToInt32(dr.GetValue(iAlevalcodi));

            int iAlevalindicador = dr.GetOrdinal(this.Alevalindicador);
            if (!dr.IsDBNull(iAlevalindicador)) entity.Alevalindicador = dr.GetString(iAlevalindicador);

            int iAlevalmax = dr.GetOrdinal(this.Alevalmax);
            if (!dr.IsDBNull(iAlevalmax)) entity.Alevalmax = dr.GetDecimal(iAlevalmax);

            int iAlevalmaxconconge = dr.GetOrdinal(this.Alevalmaxconconge);
            if (!dr.IsDBNull(iAlevalmaxconconge)) entity.Alevalmaxconconge = dr.GetDecimal(iAlevalmaxconconge);

            int iAlevalmaxsinconge = dr.GetOrdinal(this.Alevalmaxsinconge);
            if (!dr.IsDBNull(iAlevalmaxsinconge)) entity.Alevalmaxsinconge = dr.GetDecimal(iAlevalmaxsinconge);

            int iAlevalciconconge = dr.GetOrdinal(this.Alevalciconconge);
            if (!dr.IsDBNull(iAlevalciconconge)) entity.Alevalciconconge = dr.GetDecimal(iAlevalciconconge);

            int iAlevalcisinconge = dr.GetOrdinal(this.Alevalcisinconge);
            if (!dr.IsDBNull(iAlevalcisinconge)) entity.Alevalcisinconge = dr.GetDecimal(iAlevalcisinconge);

            int iAlevalfechaproceso = dr.GetOrdinal(this.Alevalfechaproceso);
            if (!dr.IsDBNull(iAlevalfechaproceso)) entity.Alevalfechaproceso = dr.GetDateTime(iAlevalfechaproceso);


            return entity;
        }


        #region Mapeo de Campos

        public string Alevalcodi = "ALEVALCODI";
        public string Alevalindicador = "ALEVALINDICADOR";
        public string Alevalmax = "ALEVALMAXVALOR";
        public string Alevalmaxconconge = "ALEVALMAXCONCONGE";
        public string Alevalmaxsinconge = "ALEVALMAXSINCONGE";
        public string Alevalcisinconge = "ALEVALCISINCONGE";
        public string Alevalciconconge = "ALEVALCICONCONGE";
        public string Alevalfechaproceso = "ALEVALFECHAPROCESO";

        #endregion
    }
}
