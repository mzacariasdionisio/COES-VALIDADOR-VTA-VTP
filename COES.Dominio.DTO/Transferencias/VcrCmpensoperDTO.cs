using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_CMPENSOPER
    /// </summary>
    public class VcrCmpensoperDTO : EntityBase
    {
        public int Vcmpopcodi { get; set; } 
        public int Vcrecacodi { get; set; } 
        public int Grupocodi { get; set; } 
        public string Gruponomb { get; set; } 
        public DateTime? Vcmpopfecha { get; set; } 
        public decimal Vcmpopporrsf { get; set; } 
        public decimal Vcmpopbajaefic { get; set; } 
        public string Vcmpopusucreacion { get; set; } 
        public DateTime Vcmpopfeccreacion { get; set; } 
    }
}
