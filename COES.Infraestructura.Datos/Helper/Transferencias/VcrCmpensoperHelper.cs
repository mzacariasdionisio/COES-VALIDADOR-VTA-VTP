using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_CMPENSOPER
    /// </summary>
    public class VcrCmpensoperHelper : HelperBase
    {
        public VcrCmpensoperHelper(): base(Consultas.VcrCmpensoperSql)
        {
        }

        public VcrCmpensoperDTO Create(IDataReader dr)
        {
            VcrCmpensoperDTO entity = new VcrCmpensoperDTO();

            int iVcmpopcodi = dr.GetOrdinal(this.Vcmpopcodi);
            if (!dr.IsDBNull(iVcmpopcodi)) entity.Vcmpopcodi = Convert.ToInt32(dr.GetValue(iVcmpopcodi));

            int iVcrecacodi = dr.GetOrdinal(this.Vcrecacodi);
            if (!dr.IsDBNull(iVcrecacodi)) entity.Vcrecacodi = Convert.ToInt32(dr.GetValue(iVcrecacodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iGruponomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

            int iVcmpopfecha = dr.GetOrdinal(this.Vcmpopfecha);
            if (!dr.IsDBNull(iVcmpopfecha)) entity.Vcmpopfecha = dr.GetDateTime(iVcmpopfecha);

            int iVcmpopporrsf = dr.GetOrdinal(this.Vcmpopporrsf);
            if (!dr.IsDBNull(iVcmpopporrsf)) entity.Vcmpopporrsf = dr.GetDecimal(iVcmpopporrsf);

            int iVcmpopbajaefic = dr.GetOrdinal(this.Vcmpopbajaefic);
            if (!dr.IsDBNull(iVcmpopbajaefic)) entity.Vcmpopbajaefic = dr.GetDecimal(iVcmpopbajaefic);

            int iVcmpopusucreacion = dr.GetOrdinal(this.Vcmpopusucreacion);
            if (!dr.IsDBNull(iVcmpopusucreacion)) entity.Vcmpopusucreacion = dr.GetString(iVcmpopusucreacion);

            int iVcmpopfeccreacion = dr.GetOrdinal(this.Vcmpopfeccreacion);
            if (!dr.IsDBNull(iVcmpopfeccreacion)) entity.Vcmpopfeccreacion = dr.GetDateTime(iVcmpopfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Vcmpopcodi = "VCMPOPCODI";
        public string Vcrecacodi = "VCRECACODI";
        public string Grupocodi = "GRUPOCODI";
        public string Gruponomb = "GRUPONOMB";
        public string Vcmpopfecha = "VCMPOPFECHA";
        public string Vcmpopporrsf = "VCMPOPPORRSF";
        public string Vcmpopbajaefic = "VCMPOPBAJAEFIC";
        public string Vcmpopusucreacion = "VCMPOPUSUCREACION";
        public string Vcmpopfeccreacion = "VCMPOPFECCREACION";

        #endregion
    }
}
