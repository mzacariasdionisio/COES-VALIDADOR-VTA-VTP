using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_PERIODOCUADRO7
    /// </summary>
    public class IndPeriodocuadro7DTO : EntityBase
    {
        public int Percu7codi { get; set; }
        public int? Percu7annoini { get; set; }
        public DateTime Percu7mesini { get; set; }
        public int? Percu7semanaini { get; set; }
        public string Percu7usumodificacion { get; set; }
        public DateTime Percu7fecmodificacion { get; set; }
        public string Percu7estado { get; set; }
        public int? Percu7annofin { get; set; }
        public DateTime Percu7mesfin { get; set; }
        public int? Percu7semanafin { get; set; }
        public int Percu7modofiltro { get; set; }
        public int? Percuacodi { get; set; }
    }
}
