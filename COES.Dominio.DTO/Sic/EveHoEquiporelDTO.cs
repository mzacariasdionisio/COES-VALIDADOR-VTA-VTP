using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_HO_EQUIPOREL 
    /// </summary>
    public class EveHoEquiporelDTO : EntityBase
    {
        public int Hoequicodi { get; set; }
        public int Hopcodi { get; set; }
        public int Equicodi { get; set; }
        public int Iccodi { get; set; }
        public int Hoequitipo { get; set; }

        public DateTime Ichorini { get; set; }
        public DateTime Ichorfin { get; set; }
        public int Subcausacodi { get; set; }
        public string Subcausadesc { get; set; }
        public decimal Icvalor1 { get; set; }

        public int TipoDesglose { get; set; }
        public string IchoriniDesc { get; set; }
        public string IchorfinDesc { get; set; }

        public string FechaIni { get; set; }
        public string HoraIni { get; set; }
        public string FechaFin { get; set; }
        public string HoraFin { get; set; }
    }
}
