using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_MENUREPORTEDET
    /// </summary>
    public class SiMenureportedetDTO : EntityBase
    {
        public int Mrepdcodigo { get; set; } 
        public int? Mrepcodi { get; set; } 
        public string Mrepdtitulo { get; set; } 
        public int? Mrepdestado { get; set; } 
        public int? Mrepdorden { get; set; } 
        public string Mrepdusucreacion { get; set; } 
        public DateTime? Mrepdfeccreacion { get; set; } 
        public string Mrepdusumodificacion { get; set; } 
        public DateTime? Mrepdfecmodificacion { get; set; } 
        public string Mrepddescripcion { get; set; } 
    }
}
