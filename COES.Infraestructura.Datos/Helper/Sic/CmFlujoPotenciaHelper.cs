using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_FLUJO_POTENCIA
    /// </summary>
    public class CmFlujoPotenciaHelper : HelperBase
    {
        public CmFlujoPotenciaHelper(): base(Consultas.CmFlujoPotenciaSql)
        {
        }

        public CmFlujoPotenciaDTO Create(IDataReader dr)
        {
            CmFlujoPotenciaDTO entity = new CmFlujoPotenciaDTO();

            int iFlupotcodi = dr.GetOrdinal(this.Flupotcodi);
            if (!dr.IsDBNull(iFlupotcodi)) entity.Flupotcodi = Convert.ToInt32(dr.GetValue(iFlupotcodi));

            int iCmgncorrelativo = dr.GetOrdinal(this.Cmgncorrelativo);
            if (!dr.IsDBNull(iCmgncorrelativo)) entity.Cmgncorrelativo = Convert.ToInt32(dr.GetValue(iCmgncorrelativo));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iFlupotvalor = dr.GetOrdinal(this.Flupotvalor);
            if (!dr.IsDBNull(iFlupotvalor)) entity.Flupotvalor = dr.GetDecimal(iFlupotvalor);

            int iFlupotoperativo = dr.GetOrdinal(this.Flupotoperativo);
            if (!dr.IsDBNull(iFlupotoperativo)) entity.Flupotoperativo = Convert.ToInt32(dr.GetValue(iFlupotoperativo));

            int iFlupotfecha = dr.GetOrdinal(this.Flupotfecha);
            if (!dr.IsDBNull(iFlupotfecha)) entity.Flupotfecha = dr.GetDateTime(iFlupotfecha);

            int iFlupotusucreacion = dr.GetOrdinal(this.Flupotusucreacion);
            if (!dr.IsDBNull(iFlupotusucreacion)) entity.Flupotusucreacion = dr.GetString(iFlupotusucreacion);

            int iFlupotfechacreacion = dr.GetOrdinal(this.Flupotfechacreacion);
            if (!dr.IsDBNull(iFlupotfechacreacion)) entity.Flupotfechacreacion = dr.GetDateTime(iFlupotfechacreacion);

            int iFlupotvalor1 = dr.GetOrdinal(this.Flupotvalor1);
            if (!dr.IsDBNull(iFlupotvalor1)) entity.Flupotvalor1 = dr.GetDecimal(iFlupotvalor1);

            int iFlupotvalor2 = dr.GetOrdinal(this.Flupotvalor2);
            if (!dr.IsDBNull(iFlupotvalor2)) entity.Flupotvalor2 = dr.GetDecimal(iFlupotvalor2);

            int iConfigcodi = dr.GetOrdinal(this.Configcodi);
            if (!dr.IsDBNull(iConfigcodi)) entity.Configcodi = Convert.ToInt32(dr.GetValue(iConfigcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            return entity;
        }


        #region Mapeo de Campos

        public string Flupotcodi = "FLUPOTCODI";
        public string Cmgncorrelativo = "CMGNCORRELATIVO";
        public string Equicodi = "EQUICODI";
        public string Flupotvalor = "FLUPOTVALOR";
        public string Flupotoperativo = "FLUPOTOPERATIVO";
        public string Flupotfecha = "FLUPOTFECHA";
        public string Flupotusucreacion = "FLUPOTUSUCREACION";
        public string Flupotfechacreacion = "FLUPOTFECHACREACION";
        public string Emprnomb = "EMPRNOMB";
        public string Equinomb = "EQUINOMB";
        public string Equiabrev = "EQUIABREV";
        public string Flupotvalor1 = "FLUPOTVALOR1";
        public string Flupotvalor2 = "FLUPOTVALOR2";
        public string Configcodi = "CONFIGCODI";
        public string Nodobarra1 = "NODOBARRA1";
        public string Nodobarra2 = "NODOBARRA2";
        public string Emprcodi = "EMPRCODI";
        public string Famcodi = "FAMCODI";

        public string SqlObtenerReporteFlujoPotencia
        {
            get { return base.GetSqlXml("ObtenerReporteFlujoPotencia"); }
        }

        #endregion
    }
}
