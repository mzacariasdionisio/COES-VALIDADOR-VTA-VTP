using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_RECALCULO
    /// </summary>
    public partial class IndRecalculoDTO : EntityBase
    {
        public int Irecacodi { get; set; }
        public int Ipericodi { get; set; }
        public DateTime Irecafechaini { get; set; }
        public DateTime Irecafechafin { get; set; }
        public string Irecatipo { get; set; }
        public int Irecaesfinal { get; set; }
        public string Irecanombre { get; set; }
        public string Irecadescripcion { get; set; }
        public DateTime Irecafechalimite { get; set; }
        public DateTime Irecafechaobs { get; set; }
        public string Irecainforme { get; set; }
        public string Irecausucreacion { get; set; }
        public DateTime? Irecafeccreacion { get; set; }
        public string Irecausumodificacion { get; set; }
        public DateTime? Irecafecmodificacion { get; set; }
    }

    public partial class IndRecalculoDTO
    {
        public string IrecafechainiDesc { get; set; }
        public string IrecafechafinDesc { get; set; }
        public string IrecafechalimiteDesc { get; set; }
        public string IrecafechaobsDesc { get; set; }
        public string Estado { get; set; }
        public string IrecaestadoDesc { get; set; }
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }
        public int Orden { get; set; }

        public string FechaIniPeriodoDesc { get; set; }
        public string FechaFinPeriodoDesc { get; set; }
    }
}
