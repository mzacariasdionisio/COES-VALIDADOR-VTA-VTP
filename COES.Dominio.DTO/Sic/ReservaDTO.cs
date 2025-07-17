using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ABI_POTEFEC
    /// </summary>
    public class ReservaDTO : EntityBase
    {
        public string URS { get; set; }
        public string Empresa { get; set; }
        public string Central { get; set; }
        public string TipoEquipo { get; set; }
        public string Equipo { get; set; }
        public string Tipo { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }
        public Decimal ValorReserva { get; set; }
        public string Hora { get; set; }
    }
}
