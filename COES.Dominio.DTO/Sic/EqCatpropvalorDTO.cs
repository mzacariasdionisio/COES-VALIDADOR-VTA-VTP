using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EQ_CATPROPVALOR
    /// </summary>
    public class EqCatpropvalorDTO : EntityBase
    {
        public int Eqctpvcodi { get; set; } 
        public decimal? Eqctpvvalor { get; set; } 
        public string Eqctpvusucreacion { get; set; } 
        public DateTime? Eqctpvfeccreacion { get; set; } 
        public DateTime? Eqctpvfechadat { get; set; } 
        public int Eqcatpcodi { get; set; }
        public int Ctgdetcodi { get; set; }
        public string Eqcatpnomb { get; set; }
    }
}
