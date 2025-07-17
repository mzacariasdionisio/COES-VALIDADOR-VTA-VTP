using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_VERSUPERAVIT
    /// </summary>
    public class VcrVersuperavitDTO : EntityBase
    {
        public int Vcrvsacodi { get; set; } 
        public int Vcrdsrcodi { get; set; } 
        public DateTime? Vcrvsafecha { get; set; } 
        public DateTime? Vcrvsahorinicio { get; set; } 
        public DateTime? Vcrvsahorfinal { get; set; } 
        public int Emprcodi { get; set; } 
        public int Grupocodi { get; set; } 
        public string Gruponomb { get; set; } 
        public decimal Vcrvsasuperavit { get; set; } 
        public string Vcrvsausucreacion { get; set; } 
        public DateTime Vcrvsafeccreacion { get; set; }
        //atributos adicionales
        public string EmprNombre { get; set; }
    }
}
