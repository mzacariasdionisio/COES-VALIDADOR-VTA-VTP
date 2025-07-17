using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla ST_FACTOR
    /// </summary>
    public class StFactorDTO : EntityBase
    {
        public int Stfactcodi { get; set; } 
        public int Strecacodi { get; set; } 
        public int Sistrncodi { get; set; } 
        public decimal Stfacttor { get; set; } 
        public string Stfactusucreacion { get; set; } 
        public DateTime Stfactfeccreacion { get; set; }
        //atributos de consultas
        public string SisTrnnombre { get; set; }
    }
}
