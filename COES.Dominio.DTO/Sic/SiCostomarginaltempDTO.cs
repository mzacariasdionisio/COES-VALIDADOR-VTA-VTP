using System;
using System.Collections.Generic;
using COES.Base.Core;


namespace COES.Dominio.DTO.Sic
{
    public class SiCostomarginaltempDTO : EntityBase
    {
        public int Enviocodi { get; set; }
        public int Cmgtcodi { get; set; }
        public int Barrcodi { get; set; }
        public decimal? Cmgtenergia { get; set; }
        public decimal? Cmgtcongestion { get; set; }
        public int? Cmgtcorrelativo { get; set; }
        public decimal? Cmgttotal { get; set; }
        public DateTime Cmgtfecha { get; set; }
    }
}
