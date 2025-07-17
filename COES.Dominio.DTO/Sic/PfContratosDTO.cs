using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PF_CONTRATOS
    /// </summary>
    public partial class PfContratosDTO : EntityBase
    {
        public int Pfcontcodi { get; set; }
        public decimal? Pfcontcantidad { get; set; }
        public DateTime? Pfcontvigenciaini { get; set; }
        public DateTime? Pfcontvigenciafin { get; set; }
        public string Pfcontobservacion { get; set; }
        public int? Pfcontcedente { get; set; }
        public int? Pfcontcesionario { get; set; }
        public int? Pfpericodi { get; set; }
        public int? Pfverscodi { get; set; }
    }

    public partial class PfContratosDTO
    {
        public string Pfcontnombcedente { get; set; }
        public string Pfcontnombcesionario { get; set; }

        public string PfcontvigenciainiDesc { get; set; }
        public string PfcontvigenciafinDesc { get; set; }
    }
}
