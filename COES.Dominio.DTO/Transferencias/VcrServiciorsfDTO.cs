using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_SERVICIORSF
    /// </summary>
    public class VcrServiciorsfDTO : EntityBase
    {
        public int Vcsrsfcodi { get; set; } 
        public int Vcrecacodi { get; set; } 
        public DateTime Vcsrsffecha { get; set; } 
        public decimal Vcsrsfasignreserva { get; set; } 
        public decimal Vcsrsfcostportun { get; set; } 
        public decimal Vcsrsfcostotcomps { get; set; } 
        public decimal Vcsrsfresvnosumn { get; set; } 
        public decimal Vcsrscostotservrsf { get; set; } 
        public string Vcsrsfusucreacion { get; set; } 
        public DateTime Vcsrsffeccreacion { get; set; }

        //Adicionales
        public decimal VcsrscostotservrsfSRns { get; set; }
    }
}
