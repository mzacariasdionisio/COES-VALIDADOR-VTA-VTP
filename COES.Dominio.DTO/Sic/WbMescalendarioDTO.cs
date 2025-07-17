using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_MESCALENDARIO
    /// </summary>
    public class WbMescalendarioDTO : EntityBase
    {
        public int Mescalcodi { get; set; } 
        public string Mescalcolorsat { get; set; } 
        public int? Mescalmes { get; set; } 
        public int? Mescalanio { get; set; } 
        public string Mescalcolorsun { get; set; } 
        public string Mescalcolor { get; set; } 
        public string Mescalinfo { get; set; } 
        public string Mescaltitulo { get; set; } 
        public string Mescaldescripcion { get; set; } 
        public string Mescalestado { get; set; } 
        public string Mescalusumodificacion { get; set; } 
        public DateTime? Mescalfecmodificacion { get; set; }
        public string Mescalnombmes { get; set; }
        public string Mescalcolortit { get; set; }
        public string Mescalcolorsubtit { get; set; }
        public string Mesdiacolor { get; set; }
    }
}
