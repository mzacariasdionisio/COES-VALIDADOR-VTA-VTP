using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CO_URS_ESPECIALBASE
    /// </summary>
    public class CoUrsEspecialbaseHelper : HelperBase
    {
        public CoUrsEspecialbaseHelper(): base(Consultas.CoUrsEspecialbaseSql)
        {
        }

        public CoUrsEspecialbaseDTO Create(IDataReader dr)
        {
            CoUrsEspecialbaseDTO entity = new CoUrsEspecialbaseDTO();

            int iCouebacodi = dr.GetOrdinal(this.Couebacodi);
            if (!dr.IsDBNull(iCouebacodi)) entity.Couebacodi = Convert.ToInt32(dr.GetValue(iCouebacodi));

            int iCouebausucreacion = dr.GetOrdinal(this.Couebausucreacion);
            if (!dr.IsDBNull(iCouebausucreacion)) entity.Couebausucreacion = dr.GetString(iCouebausucreacion);

            int iCouebafeccreacion = dr.GetOrdinal(this.Couebafeccreacion);
            if (!dr.IsDBNull(iCouebafeccreacion)) entity.Couebafeccreacion = dr.GetDateTime(iCouebafeccreacion);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Couebacodi = "COUEBACODI";
        public string Couebausucreacion = "COUEBAUSUCREACION";
        public string Couebafeccreacion = "COUEBAFECCREACION";
        public string Grupocodi = "GRUPOCODI";
        public string Gruponomb = "GRUPONOMB";

        #endregion
    }
}
