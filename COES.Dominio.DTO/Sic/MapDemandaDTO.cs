using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla MAP_Demanda
    /// </summary>
    public class MapDemandaDTO
    {
        public int Mapdemcodi { get; set; }
        public int Mapdemtipo { get; set; }
        public int Vermcodi { get; set; }
        public decimal? Mapdemvalor { get; set; }
        public DateTime Mapdemfechaperiodo { get; set; }
        public DateTime Mapdemfecha { get; set; }
        public int Mapdemperiodo { get; set; }
        public DateTime Mapdemfechafin { get; set; }
    }
}
