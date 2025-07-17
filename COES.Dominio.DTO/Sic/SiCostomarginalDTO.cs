using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_COSTOMARGINAL
    /// </summary>
    [Serializable]
    public class SiCostomarginalDTO : EntityBase
    {
        public int Cmgrcodi { get; set; } 
        public int Barrcodi { get; set; }
        public decimal? Cmgrenergia { get; set; }
        public decimal? Cmgrcongestion { get; set; }
        public decimal? Cmgrtotal { get; set; }
        public int? Cmgrcorrelativo { get; set; }
        public DateTime Cmgrfecha { get; set; }
        public string Cmgrusucreacion { get; set; }
        public DateTime? Cmgrfeccreacion { get; set; }
        public int Cmgrtcodi { get; set; }
        public string Barrnomb { get; set; }

        public int FactorPresencia { get; set; }

        public string Fechahoracm { get; set; }

        #region Numerales Datos Base
        public string Dia { get; set; }
        public decimal? Valor { get; set; }
        #endregion

        #region SIOSEIN
        public string Osinergcodi { get; set; }
        #endregion
    }
}
