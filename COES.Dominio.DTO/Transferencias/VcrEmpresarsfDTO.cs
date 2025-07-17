using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_EMPRESARSF
    /// </summary>
    public class VcrEmpresarsfDTO : EntityBase
    {
        public int Vcersfcodi { get; set; } 
        public int Vcrecacodi { get; set; } 
        public int Emprcodi { get; set; } 
        public decimal Vcersfresvnosumins { get; set; } 
        public decimal Vcersftermsuperavit { get; set; }
        public decimal Vcersfcostoportun { get; set; }
        public decimal Vcersfcompensacion { get; set; }
        public decimal Vcersfasignreserva { get; set; }
        public decimal Vcersfpagoincumpl { get; set; } 
        public decimal Vcersfpagorsf { get; set; } 
        public string Vcersfusucreacion { get; set; } 
        public DateTime Vcersffeccreacion { get; set; }

        //Atributos para consultas
        public string Emprnomb { get; set; }
    }
}
