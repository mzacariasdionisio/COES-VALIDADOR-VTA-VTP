using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_VERSIONIEOD
    /// </summary>
    public class SiVersionHelper : HelperBase
    {
        public SiVersionHelper()
            : base(Consultas.SiVersionSql)
        {
        }

        public SiVersionDTO Create(IDataReader dr)
        {
            SiVersionDTO entity = new SiVersionDTO();

            int iVerscodi = dr.GetOrdinal(this.Verscodi);
            if (!dr.IsDBNull(iVerscodi)) entity.Verscodi = Convert.ToInt32(dr.GetValue(iVerscodi));

            int iVersnombre = dr.GetOrdinal(this.Versnombre);
            if (!dr.IsDBNull(iVersnombre)) entity.Versnombre = dr.GetString(iVersnombre);

            int iVersusucreacion = dr.GetOrdinal(this.Versusucreacion);
            if (!dr.IsDBNull(iVersusucreacion)) entity.Versusucreacion = dr.GetString(iVersusucreacion);

            int iVersfeccreacion = dr.GetOrdinal(this.Versfeccreacion);
            if (!dr.IsDBNull(iVersfeccreacion)) entity.Versfeccreacion = dr.GetDateTime(iVersfeccreacion);

            int iVerscorrelativo = dr.GetOrdinal(this.Verscorrelativo);
            if (!dr.IsDBNull(iVerscorrelativo)) entity.Verscorrelativo = Convert.ToInt32(dr.GetValue(iVerscorrelativo));

            int iVersfechaperiodo = dr.GetOrdinal(this.Versfechaperiodo);
            if (!dr.IsDBNull(iVersfechaperiodo)) entity.Versfechaperiodo = dr.GetDateTime(iVersfechaperiodo);

            int iVersfecha = dr.GetOrdinal(this.Versfechaversion);
            if (!dr.IsDBNull(iVersfecha)) entity.Versfechaversion = dr.GetDateTime(iVersfecha);

            int iMprojcodi = dr.GetOrdinal(this.Mprojcodi);
            if (!dr.IsDBNull(iMprojcodi)) entity.Mprojcodi = dr.GetInt32(iMprojcodi);

            int iTmrepcodi = dr.GetOrdinal(this.Tmrepcodi);
            if (!dr.IsDBNull(iTmrepcodi)) entity.Tmrepcodi = dr.GetInt32(iTmrepcodi);

            int iVersmotivo = dr.GetOrdinal(this.Versmotivo);
            if (!dr.IsDBNull(iVersmotivo)) entity.Versmotivo = dr.GetString(iVersmotivo);

            return entity;
        }

        #region Mapeo de Campos

        public string Verscodi = "VERSCODI";
        public string Versnombre = "VERSNOMBRE";
        public string Verscorrelativo = "VERSCORRELATIVO";
        public string Versusucreacion = "VERSUSUCREACION";
        public string Versfeccreacion = "VERSFECCREACION";
        public string Versfechaperiodo = "VERSFECHAPERIODO";
        public string Versfechaversion = "VERSFECHAVERSION";

        public string Versdetnroreporte = "VERSDTNROREPORTE";
        public string Versdetdatos = "VERSDTDATOS";
        public string Versdetcodi = "VERSDTCODI";
        public string Mprojcodi = "Mprojcodi";
        public string Tmrepcodi = "Tmrepcodi";
        public string Versmotivo = "Versmotivo";

        #endregion

        public string SqlMaximoXFecha
        {
            get { return base.GetSqlXml("MaximoXFecha"); }
        }

        public string SqlGetMaxIdDetalle
        {
            get { return base.GetSqlXml("GetMaxIdDetalle"); }
        }
    }
}