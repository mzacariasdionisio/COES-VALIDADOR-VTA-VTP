using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla HT_CENTRAL_CFGDET
    /// </summary>
    public class HtCentralCfgdetHelper : HelperBase
    {
        public HtCentralCfgdetHelper(): base(Consultas.HtCentralCfgdetSql)
        {
        }

        public HtCentralCfgdetDTO Create(IDataReader dr)
        {
            HtCentralCfgdetDTO entity = new HtCentralCfgdetDTO();

            int iHtcentcodi = dr.GetOrdinal(this.Htcentcodi);
            if (!dr.IsDBNull(iHtcentcodi)) entity.Htcentcodi = Convert.ToInt32(dr.GetValue(iHtcentcodi));

            int iHtcdetcodi = dr.GetOrdinal(this.Htcdetcodi);
            if (!dr.IsDBNull(iHtcdetcodi)) entity.Htcdetcodi = Convert.ToInt32(dr.GetValue(iHtcdetcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iCanalcodi = dr.GetOrdinal(this.Canalcodi);
            if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

            int iHtcdetfactor = dr.GetOrdinal(this.Htcdetfactor);
            if (!dr.IsDBNull(iHtcdetfactor)) entity.Htcdetfactor = dr.GetDecimal(iHtcdetfactor);

            int iHtcdetactivo = dr.GetOrdinal(this.Htcdetactivo);
            if (!dr.IsDBNull(iHtcdetactivo)) entity.Htcdetactivo = Convert.ToInt32(dr.GetValue(iHtcdetactivo));

            return entity;
        }


        #region Mapeo de Campos

        public string Htcentcodi = "HTCENTCODI";
        public string Htcdetcodi = "HTCDETCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Canalcodi = "CANALCODI";
        public string Htcdetfactor = "HTCDETFACTOR";
        public string Htcdetactivo = "HTCDETACTIVO";

        public string Ptomedielenomb = "PTOMEDIELENOMB";
        public string Canalnomb = "CANALNOMB";

        #endregion
    }
}
