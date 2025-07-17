using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CAI_AJUSTECMARGINAL
    /// </summary>
    public class CaiAjustecmarginalHelper : HelperBase
    {
        public CaiAjustecmarginalHelper(): base(Consultas.CaiAjustecmarginalSql)
        {
        }

        public CaiAjustecmarginalDTO Create(IDataReader dr)
        {
            CaiAjustecmarginalDTO entity = new CaiAjustecmarginalDTO();

            int iCaajcmcodi = dr.GetOrdinal(this.Caajcmcodi);
            if (!dr.IsDBNull(iCaajcmcodi)) entity.Caajcmcodi = Convert.ToInt32(dr.GetValue(iCaajcmcodi));

            int iCaiajcodi = dr.GetOrdinal(this.Caiajcodi);
            if (!dr.IsDBNull(iCaiajcodi)) entity.Caiajcodi = Convert.ToInt32(dr.GetValue(iCaiajcodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecacodi = dr.GetOrdinal(this.Recacodi);
            if (!dr.IsDBNull(iRecacodi)) entity.Recacodi = Convert.ToInt32(dr.GetValue(iRecacodi));

            int iCaajcmmes = dr.GetOrdinal(this.Caajcmmes);
            if (!dr.IsDBNull(iCaajcmmes)) entity.Caajcmmes = Convert.ToInt32(dr.GetValue(iCaajcmmes));

            int iCaajcmusucreacion = dr.GetOrdinal(this.Caajcmusucreacion);
            if (!dr.IsDBNull(iCaajcmusucreacion)) entity.Caajcmusucreacion = dr.GetString(iCaajcmusucreacion);

            int iCaajcmfeccreacion = dr.GetOrdinal(this.Caajcmfeccreacion);
            if (!dr.IsDBNull(iCaajcmfeccreacion)) entity.Caajcmfeccreacion = dr.GetDateTime(iCaajcmfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Caajcmcodi = "CAAJCMCODI";
        public string Caiajcodi = "CAIAJCODI";
        public string Pericodi = "PERICODI";
        public string Recacodi = "RECACODI";
        public string Caajcmmes = "CAAJCMMES";
        public string Caajcmusucreacion = "CAAJCMUSUCREACION";
        public string Caajcmfeccreacion = "CAAJCMFECCREACION";

        #endregion
    }
}
