using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CO_URS_ESPECIAL
    /// </summary>
    public class CoUrsEspecialHelper : HelperBase
    {
        public CoUrsEspecialHelper(): base(Consultas.CoUrsEspecialSql)
        {
        }

        public CoUrsEspecialDTO Create(IDataReader dr)
        {
            CoUrsEspecialDTO entity = new CoUrsEspecialDTO();

            int iCourescodi = dr.GetOrdinal(this.Courescodi);
            if (!dr.IsDBNull(iCourescodi)) entity.Courescodi = Convert.ToInt32(dr.GetValue(iCourescodi));

            int iCopercodi = dr.GetOrdinal(this.Copercodi);
            if (!dr.IsDBNull(iCopercodi)) entity.Copercodi = Convert.ToInt32(dr.GetValue(iCopercodi));

            int iCovercodi = dr.GetOrdinal(this.Covercodi);
            if (!dr.IsDBNull(iCovercodi)) entity.Covercodi = Convert.ToInt32(dr.GetValue(iCovercodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iCouebacodi = dr.GetOrdinal(this.Couebacodi);
            if (!dr.IsDBNull(iCouebacodi)) entity.Couebacodi = Convert.ToInt32(dr.GetValue(iCouebacodi));

            int iCouresusucreacion = dr.GetOrdinal(this.Couresusucreacion);
            if (!dr.IsDBNull(iCouresusucreacion)) entity.Couresusucreacion = dr.GetString(iCouresusucreacion);

            int iCouresfeccreacion = dr.GetOrdinal(this.Couresfeccreacion);
            if (!dr.IsDBNull(iCouresfeccreacion)) entity.Couresfeccreacion = dr.GetDateTime(iCouresfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Courescodi = "COURESCODI";
        public string Copercodi = "COPERCODI";
        public string Covercodi = "COVERCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Couebacodi = "COUEBACODI";
        public string Couresusucreacion = "COURESUSUCREACION";
        public string Couresfeccreacion = "COURESFECCREACION";
        public string Gruponomb = "GRUPONOMB";

        #endregion
    }
}
