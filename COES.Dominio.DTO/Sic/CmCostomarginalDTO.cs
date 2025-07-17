using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_COSTOMARGINAL
    /// </summary>
    //[Serializable]
    public class CmCostomarginalDTO : EntityBase
    {
        public int Cmgncodi { get; set; } 
        public int Cnfbarcodi { get; set; } 
        public decimal? Cmgnenergia { get; set; } 
        public decimal? Cmgncongestion { get; set; } 
        public decimal? Cmgntotal { get; set; } 
        public int? Cmgncorrelativo { get; set; } 
        public DateTime Cmgnfecha { get; set; }
        public string Cmgnusucreacion { get; set; }
        public DateTime? Cmgnfeccreacion { get; set; }
        public decimal? Cmgndemanda { get; set; } 

        public string Cnfbarnodo { get; set; } 
        public string Cnfbarnombre { get; set; } 
        public string Cnfbarcoorx  { get; set; }
        public string Cnfbarcoory { get; set; }
        public string Cnfbarindpublicacion { get; set; }
        public string FechaProceso { get; set; }
        public string Color { get; set; }
        public int Indicador { get; set; }
        public string IndDefecto { get; set; }
        public string Cmgnreproceso { get; set; }
        public int Cmgnoperativo { get; set; }

        public string Folder { get; set; }
        public string TipoEstimador { get; set; }
        public string VersionPDO { get; set; }

        #region Mejoras CMgN
        public string TipoProceso { get; set; }
        public string CmgnfechaDesc { get; set; }
        public int? Topcodi { get; set; }
        public string Topnombre { get; set; }
        public int Periodo { get; set; }
        #endregion

        public int Cmveprversion { get; set; }
    }
}
