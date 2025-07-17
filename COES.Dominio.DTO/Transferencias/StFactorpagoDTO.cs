using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla ST_FACTORPAGO
    /// </summary>
    public class StFactorpagoDTO : EntityBase
    {
        public int Facpagcodi { get; set; } 
        public int Strecacodi { get; set; }
        public int Stcntgcodi { get; set; }
        public int Stcompcodi { get; set; }

        public decimal Facpagfggl { get; set; } 
        public decimal Facpagreajuste { get; set; }
        public decimal Facpagfgglajuste { get; set; }
        public string Facpagusucreacion { get; set; } 
        public DateTime Facpagfeccreacion { get; set; } 

        //variables para reportes
        public string Equinomb { get; set; }
        public decimal Elecmpmonto { get; set; }
        public string Stcompcodelemento { get; set; }
        public decimal Pagasgcmggl { get; set; }
    }
}
