using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_ENVCORREO_CONF
    /// </summary>
    public class MeEnvcorreoConfDTO : EntityBase
    {
        public int Ecconfcodi { get; set; } 
        public string Ecconfnombre { get; set; } 
        public string Ecconfcargo { get; set; } 
        public string Ecconfanexo { get; set; } 
        public string Ecconfestadonot { get; set; } 
        public string Ecconfhoraenvio { get; set; } 
        public string Ecconfusucreacion { get; set; } 
        public DateTime? Ecconffeccreacion { get; set; } 
        public string Ecconfusumodificacion { get; set; } 
        public DateTime? Ecconffecmodificacion { get; set; } 
    }
}
