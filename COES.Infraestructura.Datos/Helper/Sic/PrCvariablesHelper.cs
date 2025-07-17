using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_CVARIABLES
    /// </summary>
    public class PrCvariablesHelper : HelperBase
    {
        public PrCvariablesHelper(): base(Consultas.PrCvariablesSql)
        {
        }

        public PrCvariablesDTO Create(IDataReader dr)
        {
            PrCvariablesDTO entity = new PrCvariablesDTO();

            int iRepcodi = dr.GetOrdinal(this.Repcodi);
            if (!dr.IsDBNull(iRepcodi)) entity.Repcodi = Convert.ToInt32(dr.GetValue(iRepcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iCvc = dr.GetOrdinal(this.Cvc);
            if (!dr.IsDBNull(iCvc)) entity.Cvc = dr.GetDecimal(iCvc);

            int iCvnc = dr.GetOrdinal(this.Cvnc);
            if (!dr.IsDBNull(iCvnc)) entity.Cvnc = dr.GetDecimal(iCvnc);

            int iFpmin = dr.GetOrdinal(this.Fpmin);
            if (!dr.IsDBNull(iFpmin)) entity.Fpmin = dr.GetDecimal(iFpmin);

            int iFpmed = dr.GetOrdinal(this.Fpmed);
            if (!dr.IsDBNull(iFpmed)) entity.Fpmed = dr.GetDecimal(iFpmed);

            int iFpmax = dr.GetOrdinal(this.Fpmax);
            if (!dr.IsDBNull(iFpmax)) entity.Fpmax = dr.GetDecimal(iFpmax);

            int iCcomb = dr.GetOrdinal(this.Ccomb);
            if (!dr.IsDBNull(iCcomb)) entity.Ccomb = dr.GetDecimal(iCcomb);

            int iPe = dr.GetOrdinal(this.Pe);
            if (!dr.IsDBNull(iPe)) entity.Pe = dr.GetDecimal(iPe);

            int iEficbtukwh = dr.GetOrdinal(this.Eficbtukwh);
            if (!dr.IsDBNull(iEficbtukwh)) entity.Eficbtukwh = dr.GetDecimal(iEficbtukwh);

            int iEficterm = dr.GetOrdinal(this.Eficterm);
            if (!dr.IsDBNull(iEficterm)) entity.Eficterm = dr.GetDecimal(iEficterm);

            int iEscecodi = dr.GetOrdinal(this.Escecodi);
            if (!dr.IsDBNull(iEscecodi)) entity.Escecodi = Convert.ToInt32(dr.GetValue(iEscecodi));

            int iCecSi = dr.GetOrdinal(this.CecSi);
            if (!dr.IsDBNull(iCecSi)) entity.CecSi = dr.GetDecimal(iCecSi);

            int iRendSi = dr.GetOrdinal(this.RendSi);
            if (!dr.IsDBNull(iRendSi)) entity.RendSi = dr.GetDecimal(iRendSi);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            return entity;
        }


        #region Mapeo de Campos

        public string Repcodi = "REPCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Cvc = "CVC";
        public string Cvnc = "CVNC";
        public string Fpmin = "FPMIN";
        public string Fpmed = "FPMED";
        public string Fpmax = "FPMAX";
        public string Ccomb = "CCOMB";
        public string Pe = "PE";
        public string Eficbtukwh = "EFICBTUKWH";
        public string Eficterm = "EFICTERM";
        public string Escecodi = "ESCECODI";
        public string CecSi = "CEC_SI";
        public string RendSi = "REND_SI";
        public string Cv = "Cv";
        public string Gruponomb = "Gruponomb";
        public string Repfecha2 = "Repfecha2";
        public string Repfecha = "Repfecha";
        public string Reptipo = "REPTIPO";
        public string Fenergcodi = "Fenergcodi";
        public string Fenergnomb = "Fenergnomb";

        #region MonitoreoMME
        public string Emprcodi = "Emprcodi";
        public string Emprnomb = "EmprNomb";
        #endregion

        #region SIOSEIN
        public string Osinergcodi = "OSINERGCODI";
        /// <summary>
        /// Código osinergmin fuente de energia(Tipo de combustible)
        /// </summary>
        public string OsinergcodiFe = "OSINERGCODIFE";
        #endregion

        #endregion
        public string SqlCostosVariablesPorRepCV
        {
            get { return base.GetSqlXml("CostosVariablesPorRepCV"); }
        }

        public string SqlEliminarCostosVariablesPorRepCv
        {
            get { return base.GetSqlXml("EliminarCostosVariablesPorRepCv"); }
        }

        #region MonitoreoMME
        public string SqlListCostoVariablesxRangoFecha
        {
            get { return base.GetSqlXml("ListCostoVariablesxRangoFecha"); }
        }
        #endregion

        #region SIOSEIN
        public string SqlObtenerCVariablePorRepcodiYCatecodi
        {
            get { return base.GetSqlXml("ObtenerCVariablePorRepcodiYCatecodi"); }
        }
        #endregion
    }
}
