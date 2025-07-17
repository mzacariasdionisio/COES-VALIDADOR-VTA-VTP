using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_VERSION_DATDET
    /// </summary>
    public class SiVersionDatdetHelper : HelperBase
    {
        public SiVersionDatdetHelper() : base(Consultas.SiVersionDatdetSql)
        {
        }

        public SiVersionDatdetDTO Create(IDataReader dr)
        {
            SiVersionDatdetDTO entity = new SiVersionDatdetDTO();

            int iVdatdtcodi = dr.GetOrdinal(this.Vdatdtcodi);
            if (!dr.IsDBNull(iVdatdtcodi)) entity.Vdatdtcodi = Convert.ToInt32(dr.GetValue(iVdatdtcodi));

            int iVdatdtvalor = dr.GetOrdinal(this.Vdatdtvalor);
            if (!dr.IsDBNull(iVdatdtvalor)) entity.Vdatdtvalor = dr.GetString(iVdatdtvalor);
            entity.Vdatdtvalor = entity.Vdatdtvalor ?? "";

            int iVdatdtfecha = dr.GetOrdinal(this.Vdatdtfecha);
            if (!dr.IsDBNull(iVdatdtfecha)) entity.Vdatdtfecha = dr.GetDateTime(iVdatdtfecha);

            int iVerdatcodi = dr.GetOrdinal(this.Verdatcodi);
            if (!dr.IsDBNull(iVerdatcodi)) entity.Verdatcodi = Convert.ToInt32(dr.GetValue(iVerdatcodi));

            int iVercnpcodi = dr.GetOrdinal(this.Vercnpcodi);
            if (!dr.IsDBNull(iVercnpcodi)) entity.Vercnpcodi = Convert.ToInt32(dr.GetValue(iVercnpcodi));

            int iVdatdtid = dr.GetOrdinal(this.Vdatdtid);
            if (!dr.IsDBNull(iVdatdtid)) entity.Vdatdtid = Convert.ToInt32(dr.GetValue(iVdatdtid));

            return entity;
        }

        #region Mapeo de Campos

        public string Vdatdtcodi = "VDATDTCODI";
        public string Vdatdtvalor = "VDATDTVALOR";
        public string Vdatdtfecha = "VDATDTFECHA";
        public string Verdatcodi = "VERDATCODI";
        public string Vercnpcodi = "VERCNPCODI";
        public string Vdatdtid = "VDATDTID";

        #endregion
    }
}
