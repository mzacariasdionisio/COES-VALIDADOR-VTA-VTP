using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla MMM_FACTABLE
    /// </summary>
    public class MmmDatoDTO : EntityBase
    {
        public int Mmmdatcodi { get; set; }
        public DateTime Mmmdatfecha { get; set; }
        public int Mmmdathoraindex { get; set; }
        public int Emprcodi { get; set; }
        public int Grupocodi { get; set; }
        public int? Mogrupocodi { get; set; }
        public int? Barrcodi { get; set; }
        public int? Cnfbarcodi { get; set; }
        public decimal? Mmmdatmwejec { get; set; }
        public decimal? Mmmdatcmgejec { get; set; }
        public decimal? Mmmdatmwprog { get; set; }
        public decimal? Mmmdatcmgprog { get; set; }
        public decimal? Mmmdatocvar { get; set; }

        public string Gruponomb { get; set; }
        public int Grupopadre { get; set; }
        public string Barrnombre { get; set; }
        public int Catecodi { get; set; }
    }
}
