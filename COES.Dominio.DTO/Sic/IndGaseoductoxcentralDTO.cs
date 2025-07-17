using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_GASEODUCTOXCENTRAL
    /// </summary>
    public partial class IndGaseoductoxcentralDTO : EntityBase
    {
        public int Gasctrcodi { get; set; } 
        public int Gaseoductoequicodi { get; set; } 
        public int Equicodi { get; set; } 
        public string Gasctrestado { get; set; } 
        public DateTime? Gasctrfeccreacion { get; set; } 
        public string Gasctrusucreacion { get; set; } 
        public string Gasctrusumodificacion { get; set; } 
        public DateTime? Gasctrfecmodificacion { get; set; }

    }

    public partial class IndGaseoductoxcentralDTO : EntityBase
    {
        public string Emprnomb { get; set; }
        public string Central { get; set; }
        public string Gaseoducto { get; set; }
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }
    }
}
