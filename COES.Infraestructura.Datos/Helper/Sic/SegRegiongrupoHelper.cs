using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SEG_REGIONGRUPO
    /// </summary>
    public class SegRegiongrupoHelper : HelperBase
    {
        public SegRegiongrupoHelper(): base(Consultas.SegRegiongrupoSql)
        {
        }

        public SegRegiongrupoDTO Create(IDataReader dr)
        {
            SegRegiongrupoDTO entity = new SegRegiongrupoDTO();

            int iRegcodi = dr.GetOrdinal(this.Regcodi);
            if (!dr.IsDBNull(iRegcodi)) entity.Regcodi = Convert.ToInt32(dr.GetValue(iRegcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iReggcodi = dr.GetOrdinal(this.Reggcodi);
            if (!dr.IsDBNull(iReggcodi)) entity.Reggcodi = Convert.ToInt32(dr.GetValue(iReggcodi));

            int iSegcotipo = dr.GetOrdinal(this.Segcotipo);
            if (!dr.IsDBNull(iSegcotipo)) entity.Segcotipo = Convert.ToInt32(dr.GetValue(iSegcotipo));

            int iReggusucreacion = dr.GetOrdinal(this.Reggusucreacion);
            if (!dr.IsDBNull(iReggusucreacion)) entity.Reggusucreacion = dr.GetString(iReggusucreacion);

            int iReggfeccreacion = dr.GetOrdinal(this.Reggfeccreacion);
            if (!dr.IsDBNull(iReggfeccreacion)) entity.Reggfeccreacion = dr.GetDateTime(iReggfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Regcodi = "REGCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Reggcodi = "REGGCODI";
        public string Segcotipo = "SEGCOTIPO";
        public string Reggfeccreacion = "Reggfeccreacion";
        public string Reggusucreacion = "Reggusucreacion";

        public string Equinomb = "Equinomb";
        public string Tipoequipo = "TIPOEQUIPO";
        #endregion
    }
}
