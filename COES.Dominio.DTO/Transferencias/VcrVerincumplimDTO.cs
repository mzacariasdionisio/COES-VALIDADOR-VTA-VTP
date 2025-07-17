using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_VERINCUMPLIM
    /// </summary>
    public class VcrVerincumplimDTO : EntityBase
    {
        public int Vcrinccodi { get; set; } 
        public int Equicodicen { get; set; } 
        public int Equicodiuni { get; set; } 
        public int? Vcrvincodrpf { get; set; } 
        public DateTime Vcrvinfecha { get; set; } 
        public decimal Vcrvincumpli { get; set; } 
        public string Vcrvinobserv { get; set; } 
        public string Vcrvinusucreacion { get; set; } 
        public DateTime Vcrvinfeccreacion { get; set; } 
        public int Vcrvincodi { get; set; }
        //atributos adicionales
        public string CentralNombre { get; set; }
        public string UniNombre { get; set; }
    }
}
