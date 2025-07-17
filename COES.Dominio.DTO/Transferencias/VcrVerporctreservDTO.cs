using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_VERPORCTRESERV
    /// </summary>
    public class VcrVerporctreservDTO : EntityBase
    {
        public int Vcrvprcodi { get; set; } 
        public int Equicodicen { get; set; } 
        public int Equicodiuni { get; set; } 
        public DateTime Vcrvprfecha { get; set; } 
        public decimal Vcrvprrpns { get; set; } 
        public string Vcrvprusucreacion { get; set; } 
        public DateTime Vcrvprfeccreacion { get; set; } 
        public int Vcrinccodi { get; set; }
        //atributos adicionales
        public string CentralNombre { get; set; }
        public string UnidadNombre { get; set; }
    }
}
