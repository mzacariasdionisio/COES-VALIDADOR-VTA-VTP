using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_COSTVARIABLE
    /// </summary>
    public class VcrCostvariableDTO : EntityBase
    {
        public int Vcvarcodi { get; set; } 
        public int Vcrecacodi { get; set; } 
        public int Grupocodi { get; set; } 
        public string Gruponomb { get; set; } 
        public int Equicodi { get; set; } 
        public DateTime? Vcvarfecha { get; set; } 
        public decimal Vcvarcostvar { get; set; } 
        public string Vcvarusucreacion { get; set; } 
        public DateTime Vcvarfeccreacion { get; set; } 
    }
}
