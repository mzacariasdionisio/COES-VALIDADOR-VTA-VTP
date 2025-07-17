using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RCA_CUADRO_PROG_DISTRIB
    /// </summary>
    public class RcaCuadroProgDistribDTO : EntityBase
    {
        public string Rcprodusumodificacion { get; set; } 
        public DateTime? Rcprodfecmodificacion { get; set; } 
        public int Rcprodcodi { get; set; } 
        public int Rccuadcodi { get; set; } 
        public int Emprcodi { get; set; } 
       
        public string Rcprodsubestacion { get; set; } 
        public decimal Rcproddemanda { get; set; } 
        public decimal Rcprodcargarechazar { get; set; } 
        public string Rcprodestregistro { get; set; } 
        public string Rcprodusucreacion { get; set; } 
        public DateTime Rcprodfeccreacion { get; set; }
        
        public string Empresa { get; set; }
       
        public string Subestacion { get; set; }
        
    }
}
