using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SEG_REGIONEQUIPO
    /// </summary>
    public class SegRegionequipoHelper : HelperBase
    {
        public SegRegionequipoHelper(): base(Consultas.SegRegionequipoSql)
        {
        }

        public SegRegionequipoDTO Create(IDataReader dr)
        {
            SegRegionequipoDTO entity = new SegRegionequipoDTO();

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iRegcodi = dr.GetOrdinal(this.Regcodi);
            if (!dr.IsDBNull(iRegcodi)) entity.Regcodi = Convert.ToInt32(dr.GetValue(iRegcodi));

            int iRegecodi = dr.GetOrdinal(this.Regecodi);
            if (!dr.IsDBNull(iRegecodi)) entity.Regecodi = Convert.ToInt32(dr.GetValue(iRegecodi));

            int iSegcotipo = dr.GetOrdinal(this.Segcotipo);
            if (!dr.IsDBNull(iSegcotipo)) entity.Segcotipo = Convert.ToInt32(dr.GetValue(iSegcotipo));

            int iRegeusucreacion = dr.GetOrdinal(this.Regeusucreacion);
            if (!dr.IsDBNull(iRegeusucreacion)) entity.Regeusucreacion = dr.GetString(iRegeusucreacion);

            int iRegefeccreacion = dr.GetOrdinal(this.Regefeccreacion);
            if (!dr.IsDBNull(iRegefeccreacion)) entity.Regefeccreacion = dr.GetDateTime(iRegefeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Equicodi = "EQUICODI";
        public string Regcodi = "REGCODI";
        public string Regecodi = "REGECODI";
        public string Segcotipo = "SEGCOTIPO";
        public string Regefeccreacion = "Regefeccreacion";
        public string Regeusucreacion = "Regeusucreacion";

        public string Equinomb = "Equinomb";
        public string Tipoequipo = "TIPOEQUIPO";
        public string Tipo = "TIPO";

        #endregion
    }
}
