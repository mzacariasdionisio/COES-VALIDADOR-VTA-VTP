using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_RESERVASIGN
    /// </summary>
    public class VcrReservasignDTO : EntityBase
    {
        public int Vcrasgcodi { get; set; } 
        public int Vcrecacodi { get; set; } 
        public int Grupocodi { get; set; } 
        public string Gruponomb { get; set; } 
        public DateTime? Vcrasgfecha { get; set; } 
        public DateTime? Vcrasghorinicio { get; set; } 
        public DateTime? Vcrasghorfinal { get; set; } 
        public decimal Vcrasgreservasign { get; set; } 
        public string Vcrasgtipo { get; set; } 
        public string Vcrasgusucreacion { get; set; } 
        public DateTime Vcrasgfeccreacion { get; set; }
        public decimal Vcrasgreservasignb { get; set; }
    }
}
