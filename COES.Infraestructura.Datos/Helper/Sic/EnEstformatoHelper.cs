using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EN_ESTFORMATO
    /// </summary>
    public class EnEstformatoHelper : HelperBase
    {
        public EnEstformatoHelper()
            : base(Consultas.EnEstformatoSql)
        {
        }

        public EnEstformatoDTO Create(IDataReader dr)
        {
            EnEstformatoDTO entity = new EnEstformatoDTO();

            int iEstfmtcodi = dr.GetOrdinal(this.Estfmtcodi);
            if (!dr.IsDBNull(iEstfmtcodi)) entity.Estfmtcodi = Convert.ToInt32(dr.GetValue(iEstfmtcodi));

            int iEnunidadcodi = dr.GetOrdinal(this.Enunidadcodi);
            if (!dr.IsDBNull(iEnunidadcodi)) entity.Enunidadcodi = Convert.ToInt32(dr.GetValue(iEnunidadcodi));

            int iFormatocodi = dr.GetOrdinal(this.Formatocodi);
            if (!dr.IsDBNull(iFormatocodi)) entity.Formatocodi = Convert.ToInt32(dr.GetValue(iFormatocodi));

            int iEstadocodi = dr.GetOrdinal(this.Estadocodi);
            if (!dr.IsDBNull(iEstadocodi)) entity.Estadocodi = Convert.ToInt32(dr.GetValue(iEstadocodi));

            int iEstfmtlastdate = dr.GetOrdinal(this.Estfmtlastdate);
            if (!dr.IsDBNull(iEstfmtlastdate)) entity.Estfmtlastdate = dr.GetDateTime(iEstfmtlastdate);

            int iEstfmtlastuser = dr.GetOrdinal(this.Estfmtlastuser);
            if (!dr.IsDBNull(iEstfmtlastuser)) entity.Estfmtlastuser = dr.GetString(iEstfmtlastuser);

            int iEstfmtdescrip = dr.GetOrdinal(this.Estfmtdescrip);
            if (!dr.IsDBNull(iEstfmtdescrip)) entity.Estfmtdescrip = dr.GetString(iEstfmtdescrip);

            return entity;
        }



        #region Mapeo de Campos
        public string Estfmtcodi = "ESTFMTCODI";
        public string Enunidadcodi = "ENUNIDCODI";
        public string Formatocodi = "ENFMTCODI";
        public string Estadocodi = "ESTADOCODI";
        public string Estfmtlastdate = "ESTFMTLASTDATE";
        public string Estfmtlastuser = "ESTFMTLASTUSER";
        public string Estfmtdescrip = "ESTFMTDESCRIP";

        #endregion

        public string SqlListFormatoXEstado
        {
            get { return GetSqlXml("ListFormatoXEstado"); }
        }
    }
}
