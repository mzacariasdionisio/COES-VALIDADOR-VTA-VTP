using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PFR_RECALCULO
    /// </summary>
    public partial class PfrRecalculoDTO : EntityBase
    {
        public DateTime? Pfrrecfecmodificacion { get; set; }
        public int Pfrpercodi { get; set; }
        public int Pfrreccodi { get; set; }
        public string Pfrrecnombre { get; set; }
        public string Pfrrecdescripcion { get; set; }
        public string Pfrrecinforme { get; set; }
        public string Pfrrectipo { get; set; }
        public DateTime Pfrrecfechalimite { get; set; }
        public string Pfrrecusucreacion { get; set; }
        public DateTime? Pfrrecfeccreacion { get; set; }
        public string Pfrrecusumodificacion { get; set; }
    }

    public partial class PfrRecalculoDTO
    {
        public string Estado { get; set; }
        public string PfrrecestadoDesc { get; set; }
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }
        public string PfrrecfechalimiteDesc { get; set; }
    }
}
