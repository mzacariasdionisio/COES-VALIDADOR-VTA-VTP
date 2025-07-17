using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CO_VERSIONBASE
    /// </summary>
    public class CoVersionbaseHelper : HelperBase
    {
        public CoVersionbaseHelper(): base(Consultas.CoVersionbaseSql)
        {
        }

        public CoVersionbaseDTO Create(IDataReader dr)
        {
            CoVersionbaseDTO entity = new CoVersionbaseDTO();

            int iCovebacodi = dr.GetOrdinal(this.Covebacodi);
            if (!dr.IsDBNull(iCovebacodi)) entity.Covebacodi = Convert.ToInt32(dr.GetValue(iCovebacodi));

            int iCovebadesc = dr.GetOrdinal(this.Covebadesc);
            if (!dr.IsDBNull(iCovebadesc)) entity.Covebadesc = dr.GetString(iCovebadesc);

            int iCovebatipo = dr.GetOrdinal(this.Covebatipo);
            if (!dr.IsDBNull(iCovebatipo)) entity.Covebatipo = dr.GetString(iCovebatipo);

            int iCovebadiainicio = dr.GetOrdinal(this.Covebadiainicio);
            if (!dr.IsDBNull(iCovebadiainicio)) entity.Covebadiainicio = Convert.ToInt32(dr.GetValue(iCovebadiainicio));

            int iCovebadiafin = dr.GetOrdinal(this.Covebadiafin);
            if (!dr.IsDBNull(iCovebadiafin)) entity.Covebadiafin = Convert.ToInt32(dr.GetValue(iCovebadiafin));

            int iCovebausucreacion = dr.GetOrdinal(this.Covebausucreacion);
            if (!dr.IsDBNull(iCovebausucreacion)) entity.Covebausucreacion = dr.GetString(iCovebausucreacion);

            int iCovebafeccreacion = dr.GetOrdinal(this.Covebafeccreacion);
            if (!dr.IsDBNull(iCovebafeccreacion)) entity.Covebafeccreacion = dr.GetDateTime(iCovebafeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Covebacodi = "COVEBACODI";
        public string Covebadesc = "COVEBADESC";
        public string Covebatipo = "COVEBATIPO";
        public string Covebadiainicio = "COVEBADIAINICIO";
        public string Covebadiafin = "COVEBADIAFIN";
        public string Covebausucreacion = "COVEBAUSUCREACION";
        public string Covebafeccreacion = "COVEBAFECCREACION";

        #endregion
    }
}
