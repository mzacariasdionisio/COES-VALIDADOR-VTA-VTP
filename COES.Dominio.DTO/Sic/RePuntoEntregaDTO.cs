using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_PUNTO_ENTREGA
    /// </summary>
    public partial class RePuntoEntregaDTO : EntityBase
    {
        public int Repentcodi { get; set; } 
        public string Repentnombre { get; set; } 
        public int? Rentcodi { get; set; } 
        public string Repentestado { get; set; } 
        public string Repentusucreacion { get; set; } 
        public DateTime? Repentfeccreacion { get; set; } 
        public string Repentusumodificacion { get; set; } 
        public DateTime? Repentfecmodificacion { get; set; } 
    }

    public partial class RePuntoEntregaDTO 
    {
        public string Rentabrev { get; set; }
        public string UsuarioCreaciónModificacion { get; set; }
        public string FechaCreaciónModificacionDesc { get; set; }
        public string EstadoDesc { get; set; }
    }
}
