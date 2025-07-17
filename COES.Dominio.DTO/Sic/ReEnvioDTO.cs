using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_ENVIO
    /// </summary>
    public partial class ReEnvioDTO : EntityBase
    {
        public int Reenvcodi { get; set; } 
        public int? Repercodi { get; set; } 
        public string Reenvtipo { get; set; } 
        public int? Emprcodi { get; set; } 
        public DateTime? Reenvfecha { get; set; } 
        public string Reenvplazo { get; set; } 
        public string Reenvestado { get; set; } 
        public string Reenvindicador { get; set; } 
        public string Reenvcomentario { get; set; } 
        public string Reenvusucreacion { get; set; } 
        public DateTime? Reenvfeccreacion { get; set; } 
    }

    public partial class ReEnvioDTO 
    {
        public string ReenvfechaDesc { get; set; }
        public string ReenvindicadorDesc { get; set; }
    }
    
}
