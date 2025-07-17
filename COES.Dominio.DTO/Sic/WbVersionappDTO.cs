using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_VERSIONAPP
    /// </summary>
    public class WbVersionappDTO : EntityBase
    {
        public int Verappcodi { get; set; } 
        public decimal? Verappios { get; set; } 
        public decimal? Verappandroid { get; set; } 
        public string Verappdescripcion { get; set; } 
        public string Verappusucreacion { get; set; } 
        public DateTime? Verappfeccreacion { get; set; } 
        public string Verappusumodificacion { get; set; } 
        public DateTime? Verappfecmodificacion { get; set; }
        public string Verappvigente { get; set; }
        public int Verappupdate { get; set; }
    }
}
