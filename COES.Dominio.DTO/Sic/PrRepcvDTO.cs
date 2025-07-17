using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_REPCV
    /// </summary>
    public class PrRepcvDTO : EntityBase
    {
        public int Repcodi { get; set; } 
        public DateTime Repfecha { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
        public string Repobserva { get; set; } 
        public string Reptipo { get; set; } 
        public string Repnomb { get; set; } 
        public string Repdetalle { get; set; } 
        public string Deleted { get; set; } 
        public DateTime? Repfechafp { get; set; } 
        public DateTime? Repfechaem { get; set; } 

        #region MigracionSGOCOES-GrupoB
        public string RepfechaDesc { get; set; }
        public string RepfechaemDesc { get; set; }
        public string ReptipoDesc { get; set; }
        #endregion

        #region SIOSEIN2

        public decimal CvBase { get; set; }
        public decimal CvMedia { get; set; }
        public int Grupopadre { get; set; }
        public int Grupocodi { get; set; }
        public decimal CvPunta { get; set; }

        #endregion
        public string OpcionEvento { get; set; }
    }
}
