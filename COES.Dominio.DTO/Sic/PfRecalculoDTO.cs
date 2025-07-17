using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PF_RECALCULO
    /// </summary>
    public partial class PfRecalculoDTO : EntityBase
    {
        public int Pfrecacodi { get; set; }
        public int Pfpericodi { get; set; }
        public int Irecacodi { get; set; }
        public string Pfrecanombre { get; set; }
        public string Pfrecadescripcion { get; set; }
        public string Pfrecainforme { get; set; }
        public string Pfrecatipo { get; set; }
        public DateTime Pfrecafechalimite { get; set; }
        public string Pfrecausucreacion { get; set; }
        public DateTime? Pfrecafeccreacion { get; set; }
        public string Pfrecausumodificacion { get; set; }
        public DateTime? Pfrecafecmodificacion { get; set; }
    }

    public partial class PfRecalculoDTO
    {
        public int Pfperianio { get; set; }
        public int Pfperimes { get; set; }

        public string Estado { get; set; }
        public string PfrecaestadoDesc { get; set; }
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }
        public string PfrecafechalimiteDesc { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
    }

}
