using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_PAGORSF
    /// </summary>
    public class VcrPagorsfDTO : EntityBase
    {
        public int Vcprsfcodi { get; set; } 
        public int Vcrecacodi { get; set; } 
        public int Equicodi { get; set; } 
        public decimal Vcprsfpagorsf { get; set; } 
        public string Vcprsfusucreacion { get; set; } 
        public DateTime Vcprsffeccreacion { get; set; } 
    }
}
