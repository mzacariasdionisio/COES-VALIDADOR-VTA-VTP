using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_REGIONSEGURIDAD_DETALLE
    /// </summary>
    public class CmRegionseguridadDetalleHelper : HelperBase
    {
        public CmRegionseguridadDetalleHelper(): base(Consultas.CmRegionseguridadDetalleSql)
        {
        }

        public CmRegionseguridadDetalleDTO Create(IDataReader dr)
        {
            CmRegionseguridadDetalleDTO entity = new CmRegionseguridadDetalleDTO();

            int iRegdetcodi = dr.GetOrdinal(this.Regdetcodi);
            if (!dr.IsDBNull(iRegdetcodi)) entity.Regdetcodi = Convert.ToInt32(dr.GetValue(iRegdetcodi));

            int iRegsegcodi = dr.GetOrdinal(this.Regsegcodi);
            if (!dr.IsDBNull(iRegsegcodi)) entity.Regsegcodi = Convert.ToInt32(dr.GetValue(iRegsegcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iRegsegusucreacion = dr.GetOrdinal(this.Regsegusucreacion);
            if (!dr.IsDBNull(iRegsegusucreacion)) entity.Regsegusucreacion = dr.GetString(iRegsegusucreacion);

            int iRegsegfeccreacion = dr.GetOrdinal(this.Regsegfeccreacion);
            if (!dr.IsDBNull(iRegsegfeccreacion)) entity.Regsegfeccreacion = dr.GetDateTime(iRegsegfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Regdetcodi = "REGDETCODI";
        public string Regsegcodi = "REGSEGCODI";
        public string Equicodi = "EQUICODI";
        public string Regsegusucreacion = "REGSEGUSUCREACION";
        public string Regsegfeccreacion = "REGSEGFECCREACION";
        public string Nombretna = "NOMBRETNA";
        public string Tipoequipo = "TIPOEQUIPO";
        public string Famcodi = "FAMCODI";

        public string SqlObtenerEquipos
        {
            get { return base.GetSqlXml("ObtenerEquipos"); }
        }

        // Mejoras Yupana
        public string SqlObtenerEquiposCentral
        {
            get { return base.GetSqlXml("ObtenerEquiposCentral"); }
        }

        public string SqlObtenerModoOperacion
        {
            get { return base.GetSqlXml("ObtenerModoOperacion"); }
        }

        public string SqlObtenerEquiposLinea
        {
            get { return base.GetSqlXml("ObtenerEquiposLinea"); }
        }

        #endregion
    }
}
