using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_CORREOAREA
    /// </summary>
    public partial class FtExtCorreoareaDTO : EntityBase
    {
        public int Faremcodi { get; set; } 
        public DateTime? Faremfeccreacion { get; set; } 
        public string Faremusucreacion { get; set; } 
        public DateTime? Faremfecmodificacion { get; set; } 
        public string Faremusumodificacion { get; set; } 
        public string Faremnombre { get; set; } 
        public string Faremestado { get; set; } 
    }

    public partial class FtExtCorreoareaDTO
    {
        public int CantidadCorreos { get; set; }
        public string FaremEstadoDesc { get; set; }
        public string FechaCreacionDesc { get; set; }
        public string FechaModificacionDesc { get; set; }
        public int? Ftitcodi { get; set; }
        public int? Fevrqcodi { get; set; }
    }
}
