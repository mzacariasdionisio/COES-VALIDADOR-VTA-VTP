using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_CVARIABLES
    /// </summary>
    [Serializable]
    public class PrCvariablesDTO : EntityBase
    {
        public int Repcodi { get; set; } 
        public int Grupocodi { get; set; } 
        public decimal? Cvc { get; set; } 
        public decimal? Cvnc { get; set; } 
        public decimal? Fpmin { get; set; } 
        public decimal? Fpmed { get; set; } 
        public decimal? Fpmax { get; set; } 
        public decimal? Ccomb { get; set; } 
        public decimal? Pe { get; set; } 
        public decimal? Eficbtukwh { get; set; } 
        public decimal? Eficterm { get; set; } 
        public int Escecodi { get; set; } 
        public decimal? CecSi { get; set; } 
        public decimal? RendSi { get; set; }
        public decimal? Cv { get; set; } 

        //Campos de Otras Tablas
        public string Gruponomb { get; set; }
        public string Escenomb { get; set; }
        public string Emprnomb { get; set; }
        public string GruponombPadre { get; set; }
        public string Grupoabrev { get; set; }
        public string TipoModo { get; set; }

        public DateTime Repfecha2 { get; set; }
        public DateTime Repfecha { get; set; }
        public string Reptipo { get; set; }
        public int Fenergcodi { get; set; }
        public string Fenergnomb { get; set; }

        public string TipoGenerRer { get; set; }
        public string Grupotipocogen { get; set; }

        // 6 nuevos campos de CI
        public string Tramo1 { get; set; }
        public decimal? CIncremental1 { get; set; }
        public string Tramo2 { get; set; }
        public decimal? CIncremental2 { get; set; }
        public string Tramo3 { get; set; }
        public decimal? CIncremental3 { get; set; }

        public decimal? Pe1 { get; set; }
        public decimal? Pe2 { get; set; }
        public decimal? Pe3 { get; set; }
        public decimal? Pe4 { get; set; }

        public string TipoCombustible { get; set; }

        #region MigracionSGOCOES-GrupoB
        public string Grupotipo { get; set; }
        public int Emprcodi { get; set; }

        public string FormulaCvc { get; set; }
        public string FormulaCvnc { get; set; }
        public string FormulaCcomb { get; set; }
        public string FormulaPe { get; set; }
        public string FormulaCecSi { get; set; }
        public string FormulaRendSi { get; set; }
        public string FormulaCv { get; set; }

        public decimal TCambio { get; set; }
        public decimal CcombAlt { get; set; }
        public decimal PciSI { get; set; }
        public decimal Cbe { get; set; }
        public decimal CbeAlt { get; set; }
        public decimal CvncUS { get; set; }
        public decimal CMarrPar { get; set; }

        #endregion

        #region MonitoreoMME

        public int Barrcodi { get; set; }
        public string Barrnomb { get; set; }

        #endregion

        #region Siosein2
        public int Mocmtipocomb { get; set; }
        #endregion

        #region SIOSEIN
        public string Osinergcodi { get; set; }

        /// <summary>
        /// Código osinergmin fuente de energia(Tipo de combustible)
        /// </summary>
        public string OsinergcodiFe { get; set; }

        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }

        #endregion
    }
}
