using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_VERSIONIEOD_DET
    /// </summary>
    public class SiVersionDetHelper : HelperBase
    {
        public SiVersionDetHelper()
            : base(Consultas.SiVersionDetSql)
        {
        }

        public SiVersionDetDTO Create(IDataReader dr)
        {
            SiVersionDetDTO entity = new SiVersionDetDTO();

            int iVersdtcodi = dr.GetOrdinal(this.Versdtcodi);
            if (!dr.IsDBNull(iVersdtcodi)) entity.Versdtcodi = Convert.ToInt32(dr.GetValue(iVersdtcodi));

            int iMrepcodi = dr.GetOrdinal(this.Mrepcodi);
            if (!dr.IsDBNull(iMrepcodi)) entity.Mrepcodi = Convert.ToInt32(dr.GetValue(iMrepcodi));

            int iVerscodi = dr.GetOrdinal(this.Verscodi);
            if (!dr.IsDBNull(iVerscodi)) entity.Verscodi = Convert.ToInt32(dr.GetValue(iVerscodi));

            int iVersdtnroreporte = dr.GetOrdinal(this.Versdtnroreporte);
            if (!dr.IsDBNull(iVersdtnroreporte)) entity.Versdtnroreporte = dr.GetDecimal(iVersdtnroreporte);

            int iVersdtdatos = dr.GetOrdinal(this.Versdtdatos);
            if (!dr.IsDBNull(iVersdtdatos)) entity.Versdtdatos = (byte[])dr.GetValue(iVersdtdatos);

            return entity;
        }

        #region Mapeo de Campos

        public string Versdtcodi = "VERSDTCODI";
        public string Verscodi = "VERSCODI";
        public string Versdtnroreporte = "VERSDTNROREPORTE";
        public string Versdtdatos = "VERSDTDATOS";
        public string Mrepcodi = "MREPCODI";

        #endregion

        public string SqlGetByVersionDetIEOD
        {
            get { return base.GetSqlXml("GetByVersionDetIEOD"); }
        }

    }
}
