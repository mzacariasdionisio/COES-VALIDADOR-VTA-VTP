using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTD_VALORIZACION y VTD_VALORIZACIONDETALLE
    /// </summary>
    public class VtdMontoPorCapacidadDTO
    {
        public int Emprcodi { get; set; }
        //commentary-agregado para el join de tablas
        public string Emprnomb { get; set; }
        public decimal Valomr { get; set; }
        public decimal Valopreciopotencia { get; set; }
        public decimal Valdpfirremun { get; set; }
        public decimal Valddemandacoincidente { get; set; }
        public decimal Valdmoncapacidad { get; set; }
        public DateTime Valofecha { get; set; }
    }
    
}
