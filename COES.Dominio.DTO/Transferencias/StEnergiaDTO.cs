using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla ST_ENERGIA
    /// </summary>
    public class StEnergiaDTO : EntityBase
    {
        public int Stenrgcodi { get; set; } 
        public int Strecacodi { get; set; } 
        public int Equicodi { get; set; } 
        public decimal Stenrgrgia { get; set; } 
        public string Stenrgusucreacion { get; set; } 
        public DateTime Stenrgfeccreacion { get; set; }
        //atributos de consultas
        public string Equinomb { get; set; }
    }
}
