using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_VERSION_DAT
    /// </summary>
    public class SiVersionDatHelper : HelperBase
    {
        public SiVersionDatHelper() : base(Consultas.SiVersionDatSql)
        {
        }

        public SiVersionDatDTO Create(IDataReader dr)
        {
            SiVersionDatDTO entity = new SiVersionDatDTO();

            int iVerdatcodi = dr.GetOrdinal(this.Verdatcodi);
            if (!dr.IsDBNull(iVerdatcodi)) entity.Verdatcodi = Convert.ToInt32(dr.GetValue(iVerdatcodi));

            int iVercnpcodi = dr.GetOrdinal(this.Vercnpcodi);
            if (!dr.IsDBNull(iVercnpcodi)) entity.Vercnpcodi = Convert.ToInt32(dr.GetValue(iVercnpcodi));

            int iVerdatvalor = dr.GetOrdinal(this.Verdatvalor);
            if (!dr.IsDBNull(iVerdatvalor)) entity.Verdatvalor = dr.GetString(iVerdatvalor);

            int iVerdatvalor2 = dr.GetOrdinal(this.Verdatvalor2);
            if (!dr.IsDBNull(iVerdatvalor2)) entity.Verdatvalor2 = dr.GetString(iVerdatvalor2);

            int iVersdtcodi = dr.GetOrdinal(this.Versdtcodi);
            if (!dr.IsDBNull(iVersdtcodi)) entity.Versdtcodi = Convert.ToInt32(dr.GetValue(iVersdtcodi));

            int iVerdatid = dr.GetOrdinal(this.Verdatid);
            if (!dr.IsDBNull(iVerdatid)) entity.Verdatid = Convert.ToInt32(dr.GetValue(iVerdatid));

            return entity;
        }

        #region Mapeo de Campos

        public string Verdatcodi = "VERDATCODI";
        public string Vercnpcodi = "VERCNPCODI";
        public string Verdatvalor = "VERDATVALOR";
        public string Verdatvalor2 = "VERDATVALOR2";
        public string Verdatid = "VERDATID";
        public string Versdtcodi = "VERSDTCODI";

        #endregion
    }
}
