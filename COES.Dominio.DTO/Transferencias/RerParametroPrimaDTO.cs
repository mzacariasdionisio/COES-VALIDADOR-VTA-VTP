using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_PARAMETRO_PRIMA
    /// </summary>
    public class RerParametroPrimaDTO : EntityBase
    {
        public int Rerpprcodi { get; set; }
        public int Reravcodi { get; set; }
        public int Rerpprmes { get; set; }
        public string Rerpprmesaniodesc { get; set; }
        public decimal? Rerpprtipocambio { get; set; }
        public string Rerpprorigen { get; set; }
        public string Rerpprrevision { get; set; }
        public string Rerpprusucreacion { get; set; }
        public DateTime Rerpprfeccreacion { get; set; }
        public string Rerpprusumodificacion { get; set; }
        public DateTime Rerpprfecmodificacion { get; set; }
        public int? Pericodi { get; set; }
        public int? Recacodi { get; set; }
    }
}