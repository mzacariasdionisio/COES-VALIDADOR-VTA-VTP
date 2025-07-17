using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_VERDEFICIT
    /// </summary>
    public class VcrVerdeficitDTO : EntityBase
    {
        public int Vcrvdecodi { get; set; } 
        public int Vcrdsrcodi { get; set; } 
        public DateTime? Vcrvdefecha { get; set; } 
        public DateTime? Vcrvdehorinicio { get; set; } 
        public DateTime? Vcrvdehorfinal { get; set; } 
        public int Emprcodi { get; set; } 
        public int Grupocodi { get; set; } 
        public string Gruponomb { get; set; } 
        public decimal Vcrvdedeficit { get; set; } 
        public string Vcrvdeusucreacion { get; set; } 
        public DateTime Vcrvdefeccreacion { get; set; }
        //atributos adicionales
        public string EmprNombre { get; set; }
    }
}
