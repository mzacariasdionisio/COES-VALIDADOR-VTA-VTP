using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RCA_PARAM_ESQUEMA
    /// </summary>
    public class RcaParamEsquemaHelper : HelperBase
    {
        public RcaParamEsquemaHelper()
            : base(Consultas.RcaParamEsquemaSql)
        {
        }

        public RcaParamEsquemaDTO Create(IDataReader dr)
        {
            RcaParamEsquemaDTO entity = new RcaParamEsquemaDTO();

            int iRcparecodi = dr.GetOrdinal(this.Rcparecodi);
            if (!dr.IsDBNull(iRcparecodi)) entity.Rcparecodi = Convert.ToInt32(dr.GetValue(iRcparecodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iRcparehperacmf = dr.GetOrdinal(this.Rcparehperacmf);
            if (!dr.IsDBNull(iRcparehperacmf)) entity.Rcparehperacmf = dr.GetDecimal(iRcparehperacmf);

            int iRcparehperacmt = dr.GetOrdinal(this.Rcparehperacmt);
            if (!dr.IsDBNull(iRcparehperacmt)) entity.Rcparehperacmt = dr.GetDecimal(iRcparehperacmt);

            int iRcparehfperacmf = dr.GetOrdinal(this.Rcparehfperacmf);
            if (!dr.IsDBNull(iRcparehfperacmf)) entity.Rcparehfperacmf = dr.GetDecimal(iRcparehfperacmf);

            int iRcparehfperacmt = dr.GetOrdinal(this.Rcparehfperacmt);
            if (!dr.IsDBNull(iRcparehfperacmt)) entity.Rcparehfperacmt = dr.GetDecimal(iRcparehfperacmt);

            int iRcpareestregistro = dr.GetOrdinal(this.Rcpareestregistro);
            if (!dr.IsDBNull(iRcpareestregistro)) entity.Rcpareestregistro = dr.GetString(iRcpareestregistro);

            int iRcpareusucreacion = dr.GetOrdinal(this.Rcpareusucreacion);
            if (!dr.IsDBNull(iRcpareusucreacion)) entity.Rcpareusucreacion = dr.GetString(iRcpareusucreacion);

            int iRcparefeccreacion = dr.GetOrdinal(this.Rcparefeccreacion);
            if (!dr.IsDBNull(iRcparefeccreacion)) entity.Rcparefeccreacion = dr.GetDateTime(iRcparefeccreacion);

            int iRcpareusumodificacion = dr.GetOrdinal(this.Rcpareusumodificacion);
            if (!dr.IsDBNull(iRcpareusumodificacion)) entity.Rcpareusumodificacion = dr.GetString(iRcpareusumodificacion);

            int iRcparefecmodificacion = dr.GetOrdinal(this.Rcparefecmodificacion);
            if (!dr.IsDBNull(iRcparefecmodificacion)) entity.Rcparefecmodificacion = dr.GetDateTime(iRcparefecmodificacion);

            int iRcapareanio = dr.GetOrdinal(this.Rcpareanio);
            if (!dr.IsDBNull(iRcapareanio)) entity.Rcpareanio = dr.GetInt32(iRcapareanio);

            return entity;
        }


        #region Mapeo de Campos

        public string Rcparecodi = "RCPARECODI";
        public string Rcpareanio = "RCPAREANIO";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Rcparehperacmf = "RCPAREHPERACMF";
        public string Rcparehperacmt = "RCPAREHPERACMT";
        public string Rcparehfperacmf = "RCPAREHFPERACMF";
        public string Rcparehfperacmt = "RCPAREHFPERACMT";
        public string Rcpareestregistro = "RCPAREESTREGISTRO";
        public string Rcpareusucreacion = "RCPAREUSUCREACION";
        public string Rcparefeccreacion = "RCPAREFECCREACION";
        public string Rcpareusumodificacion = "RCPAREUSUMODIFICACION";
        public string Rcparefecmodificacion = "RCPAREFECMODIFICACION";

        public string Emprrazsocial = "EMPRRAZSOCIAL";
        public string Areanomb = "AREANOMB";
        public string Equinomb = "EQUINOMB";

        public string Rcparehperacmf2 = "RCPAREHPERACMF2";
        public string Rcparehfperacmf2 = "RCPAREHFPERACMF2";
        public string Rcparenroesquema = "RCPARENROESQUEMA";

        public string Rcparehpdemandaref = "RCPAREHPDEMANDAREF";
        public string Rcparehfpdemandaref = "RCPAREHFPDEMANDAREF";
        #endregion

        public string SqlListaPorFiltros
        {
            get { return base.GetSqlXml("ListaPorFiltros"); }
        }

        public string SqlListarAnios
        {
            get { return base.GetSqlXml("ListarAnios"); }
        }

        public string SqlListarPorPuntoMedicion
        {
            get { return base.GetSqlXml("ListarPorPuntoMedicion"); }
        }
    }
}
