using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RPF_ENVIO
    /// </summary>
    public class RpfEnvioDTO : EntityBase
    {
        public int Rpfenvcodi { get; set; } 
        public DateTime? Rpfenvfecha { get; set; } 
        public string Rpfenvestado { get; set; } 
        public string Rpfenvusucreacion { get; set; } 
        public DateTime? Rpfenvfeccreacion { get; set; } 
        public string Rpfenvusumodificacion { get; set; } 
        public DateTime? Rpfenvfecmodificacion { get; set; } 
        public int? Emprcodi { get; set; }
    }
}
