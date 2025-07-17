using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_CMVSTARIFA
    /// </summary>
    public class WbCmvstarifaHelper : HelperBase
    {
        public WbCmvstarifaHelper(): base(Consultas.WbCmvstarifaSql)
        {
        }

        public WbCmvstarifaDTO Create(IDataReader dr)
        {
            WbCmvstarifaDTO entity = new WbCmvstarifaDTO();

            int iCmtarusucreacion = dr.GetOrdinal(this.Cmtarusucreacion);
            if (!dr.IsDBNull(iCmtarusucreacion)) entity.Cmtarusucreacion = dr.GetString(iCmtarusucreacion);

            int iCmtarusumodificacion = dr.GetOrdinal(this.Cmtarusumodificacion);
            if (!dr.IsDBNull(iCmtarusumodificacion)) entity.Cmtarusumodificacion = dr.GetString(iCmtarusumodificacion);

            int iCmtarfeccreacion = dr.GetOrdinal(this.Cmtarfeccreacion);
            if (!dr.IsDBNull(iCmtarfeccreacion)) entity.Cmtarfeccreacion = dr.GetDateTime(iCmtarfeccreacion);

            int iCmtarfecmodificacion = dr.GetOrdinal(this.Cmtarfecmodificacion);
            if (!dr.IsDBNull(iCmtarfecmodificacion)) entity.Cmtarfecmodificacion = dr.GetDateTime(iCmtarfecmodificacion);

            int iCmtarcodi = dr.GetOrdinal(this.Cmtarcodi);
            if (!dr.IsDBNull(iCmtarcodi)) entity.Cmtarcodi = Convert.ToInt32(dr.GetValue(iCmtarcodi));

            int iCmtarcmprom = dr.GetOrdinal(this.Cmtarcmprom);
            if (!dr.IsDBNull(iCmtarcmprom)) entity.Cmtarcmprom = dr.GetDecimal(iCmtarcmprom);

            int iCmtartarifabarra = dr.GetOrdinal(this.Cmtartarifabarra);
            if (!dr.IsDBNull(iCmtartarifabarra)) entity.Cmtartarifabarra = dr.GetDecimal(iCmtartarifabarra);

            int iCmtarprommovil = dr.GetOrdinal(this.Cmtarprommovil);
            if (!dr.IsDBNull(iCmtarprommovil)) entity.Cmtarprommovil = dr.GetDecimal(iCmtarprommovil);

            int iCmtarfecha = dr.GetOrdinal(this.Cmtarfecha);
            if (!dr.IsDBNull(iCmtarfecha)) entity.Cmtarfecha = dr.GetDateTime(iCmtarfecha);

            return entity;
        }


        #region Mapeo de Campos

        public string Cmtarusucreacion = "CMTARUSUCREACION";
        public string Cmtarusumodificacion = "CMTARUSUMODIFICACION";
        public string Cmtarfeccreacion = "CMTARFECCREACION";
        public string Cmtarfecmodificacion = "CMTARFECMODIFICACION";
        public string Cmtarcodi = "CMTARCODI";
        public string Cmtarcmprom = "CMTARCMPROM";
        public string Cmtartarifabarra = "CMTARTARIFABARRA";
        public string Cmtarprommovil = "CMTARPROMMOVIL";
        public string Cmtarfecha = "CMTARFECHA";

        #endregion
    }
}
