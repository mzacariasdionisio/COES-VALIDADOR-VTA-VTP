using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SPO_NUMDATCAMBIO
    /// </summary>
    public class SpoNumdatcambioHelper : HelperBase
    {
        public SpoNumdatcambioHelper(): base(Consultas.SpoNumdatcambioSql)
        {
        }

        public SpoNumdatcambioDTO Create(IDataReader dr)
        {
            SpoNumdatcambioDTO entity = new SpoNumdatcambioDTO();

            int iNumdcbcodi = dr.GetOrdinal(this.Numdcbcodi);
            if (!dr.IsDBNull(iNumdcbcodi)) entity.Numdcbcodi = Convert.ToInt32(dr.GetValue(iNumdcbcodi));

            int iVerncodi = dr.GetOrdinal(this.Verncodi);
            if (!dr.IsDBNull(iVerncodi)) entity.Verncodi = Convert.ToInt32(dr.GetValue(iVerncodi));

            int iSconcodi = dr.GetOrdinal(this.Sconcodi);
            if (!dr.IsDBNull(iSconcodi)) entity.Sconcodi = Convert.ToInt32(dr.GetValue(iSconcodi));

            int iClasicodi = dr.GetOrdinal(this.Clasicodi);
            if (!dr.IsDBNull(iClasicodi)) entity.Clasicodi = Convert.ToInt32(dr.GetValue(iClasicodi));

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            int iNumdcbvalor = dr.GetOrdinal(this.Numdcbvalor);
            if (!dr.IsDBNull(iNumdcbvalor)) entity.Numdcbvalor = dr.GetDecimal(iNumdcbvalor);

            int iNumdatfechainicio = dr.GetOrdinal(this.Numdcbfechainicio);
            if (!dr.IsDBNull(iNumdatfechainicio)) entity.Numdcbfechainicio = dr.GetDateTime(iNumdatfechainicio);

            int iNumdatfechafin = dr.GetOrdinal(this.Numdcbfechafin);
            if (!dr.IsDBNull(iNumdatfechafin)) entity.Numdcbfechafin = dr.GetDateTime(iNumdatfechafin);

            return entity;
        }


        #region Mapeo de Campos

        public string Numdcbcodi = "NUMDCBCODI";
        public string Verncodi = "VERNCODI";
        public string Sconcodi = "SCONCODI";
        public string Clasicodi = "CLASICODI";
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Numdcbvalor = "NUMDCBVALOR";
        public string Numdcbfechainicio = "NUMDCBFECHAINICIO";
        public string Numdcbfechafin = "NUMDCBFECHAFIN";

        #endregion
    }
}
