using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PSU_RPFHID
    /// </summary>
    public class PsuRpfhidDTO : EntityBase
    {
        public DateTime Rpfhidfecha { get; set; } 
        public decimal Rpfenetotal { get; set; } 
        public decimal Rpfpotmedia { get; set; } 
        public decimal Eneindhidra { get; set; } 
        public decimal Potindhidra { get; set; }
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
    }
}

