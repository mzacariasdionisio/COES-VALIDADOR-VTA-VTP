using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SIO_CABECERADET
    /// </summary>
    public class SioCabeceradetDTO : EntityBase
    {
        public int Cabpricodi { get; set; } 
        public int Tpriecodi { get; set; } 
        public DateTime Cabpriperiodo { get; set; }
        public string Cabpriusucreacion { get; set; }
        public DateTime Cabprifeccreacion { get; set; }
        public int? Cabpriversion { get; set; }
        public int Cabpritieneregistros { get; set; }

        public string CabprifeccreacionDesc { get; set; }
        public string EstadoDesc { get; set; }
    }
}
