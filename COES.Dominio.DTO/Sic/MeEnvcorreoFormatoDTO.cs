using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_ENVCORREO_FORMATO
    /// </summary>
    public class MeEnvcorreoFormatoDTO : EntityBase
    {
        public int Ecformcodi { get; set; } 
        public int? Formatcodi { get; set; } 
        public int? Emprcodi { get; set; } 
        public string Ecformhabilitado { get; set; } 
        public string Ecformusucreacion { get; set; } 
        public DateTime? Ecformfeccreacion { get; set; } 
        public string Ecformusumodificacion { get; set; } 
        public DateTime? Ecformfecmodificacion { get; set; } 
        public string Empremail { get; set; }
        public int? Modcodi { get; set; }
    }
}
