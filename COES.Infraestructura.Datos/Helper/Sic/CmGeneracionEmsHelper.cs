using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_GENERACION_EMS
    /// </summary>
    public class CmGeneracionEmsHelper : HelperBase
    {
        public CmGeneracionEmsHelper(): base(Consultas.CmGeneracionEmsSql)
        {
        }

        public CmGeneracionEmsDTO Create(IDataReader dr)
        {
            CmGeneracionEmsDTO entity = new CmGeneracionEmsDTO();

            int iGenemscodi = dr.GetOrdinal(this.Genemscodi);
            if (!dr.IsDBNull(iGenemscodi)) entity.Genemscodi = Convert.ToInt32(dr.GetValue(iGenemscodi));

            int iCmgncorrelativo = dr.GetOrdinal(this.Cmgncorrelativo);
            if (!dr.IsDBNull(iCmgncorrelativo)) entity.Cmgncorrelativo = Convert.ToInt32(dr.GetValue(iCmgncorrelativo));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iGenemsgeneracion = dr.GetOrdinal(this.Genemsgeneracion);
            if (!dr.IsDBNull(iGenemsgeneracion)) entity.Genemsgeneracion = dr.GetDecimal(iGenemsgeneracion);

            int iGenemsoperativo = dr.GetOrdinal(this.Genemsoperativo);
            if (!dr.IsDBNull(iGenemsoperativo)) entity.Genemsoperativo = Convert.ToInt32(dr.GetValue(iGenemsoperativo));

            int iGenemsfecha = dr.GetOrdinal(this.Genemsfecha);
            if (!dr.IsDBNull(iGenemsfecha)) entity.Genemsfecha = dr.GetDateTime(iGenemsfecha);

            int iGenemsusucreacion = dr.GetOrdinal(this.Genemsusucreacion);
            if (!dr.IsDBNull(iGenemsusucreacion)) entity.Genemsusucreacion = dr.GetString(iGenemsusucreacion);

            int iGenemsfechacreacion = dr.GetOrdinal(this.Genemsfechacreacion);
            if (!dr.IsDBNull(iGenemsfechacreacion)) entity.Genemsfechacreacion = dr.GetDateTime(iGenemsfechacreacion);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            return entity;
        }


        #region Mapeo de Campos

        public string Genemscodi = "GENEMSCODI";
        public string Cmgncorrelativo = "CMGNCORRELATIVO";
        public string Equicodi = "EQUICODI";
        public string Genemsgeneracion = "GENEMSGENERACION";
        public string Genemsoperativo = "GENEMSOPERATIVO";
        public string Genemsfecha = "GENEMSFECHA";
        public string Genemsusucreacion = "GENEMSUSUCREACION";
        public string Genemsfechacreacion = "GENEMSFECHACREACION";
        public string Central = "CENTRAL";
        public string Equipadre = "EQUIPADRE";
        public string Famcodipadre = "FAMCODIPADRE";
        public string Emprcodi = "EMPRCODI";
        public string Genemstipoestimado = "GENEMSTIPOESTIMADOR";
        public string Genemspotmax = "GENEMSPOTMAX";
        public string Genemspotmin = "GENEMSPOTMIN";

        #endregion

        public string SqlDeleteByFecha
        {
            get { return base.GetSqlXml("DeleteByFecha"); }
        }
        
        public string SqlGeneracionPorCorrelativo
        {
            get { return base.GetSqlXml("GeneracionPorCorrelativo"); }
        }

        public string SqlGeneracionPorFechas
        {
            get { return base.GetSqlXml("GeneracionPorFechas"); }
        }

        public string SqlActualizarModoOperacion
        {
            get { return base.GetSqlXml("ActualizarModoOperacion"); }
        }

        #region EMS
        public string SqlGetListaGeneracionByEquipoFecha
        {
            get { return base.GetSqlXml("GetListaGeneracionByEquipoFecha"); }
        }
        #endregion

        #region Mejoras CMgN

        public string SqlObtenerGeneracionCostoIncremental
        {
            get { return base.GetSqlXml("ObtenerGeneracionCostoIncremental"); }
        }

        #endregion

    }
}
