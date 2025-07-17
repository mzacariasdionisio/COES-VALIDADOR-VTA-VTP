using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CB_DATOS_DETALLE
    /// </summary>
    public partial class CbDatosDetalleDTO : EntityBase
    {
        public int Cbdetcodi { get; set; }
        public int Cbevdacodi { get; set; }
        public string Cbdetcompago { get; set; }
        public int Cbdettipo { get; set; }
        public DateTime? Cbdetfechaemision { get; set; }
        public decimal Cbdettipocambio { get; set; }
        public string Cbdetmoneda { get; set; }
        public decimal Cbdetvolumen { get; set; }
        public decimal Cbdetcosto { get; set; }
        public decimal Cbdetcosto2 { get; set; }
        public decimal Cbdetimpuesto { get; set; }
        public decimal Cbdetdmrg { get; set; }
    }

    public partial class CbDatosDetalleDTO 
    {
        public string CbdetfechaemisionDesc { get; set; }
        public string MensajeValidacion { get; set; } = string.Empty;
    }
}
