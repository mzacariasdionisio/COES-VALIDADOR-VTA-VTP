using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_UNIDAD
    /// </summary>
    public partial class IndUnidadDTO : EntityBase
    {
        public int Iunicodi { get; set; }
        public int Equipadre { get; set; }
        public string Iuniunidadnomb { get; set; }
        public int? Equicodi { get; set; }
        public int? Grupocodi { get; set; }
        public string Iuninombcentral { get; set; }
        public string Iuninombunidad { get; set; }

        public int Iuniactivo { get; set; }
        public string Iuniusucreacion { get; set; }
        public DateTime? Iunifeccreacion { get; set; }
        public string Iuniusumodificacion { get; set; }
        public DateTime? Iunifecmodificacion { get; set; }
    }

    public partial class IndUnidadDTO 
    {
        public int Ipericodi { get; set; }
        public int Famcodi { get; set; }
        public string Emprnomb { get; set; }
        public string Central { get; set; }
        public string Equinomb { get; set; }
        public string Gruponomb { get; set; }

        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }
    }
}
