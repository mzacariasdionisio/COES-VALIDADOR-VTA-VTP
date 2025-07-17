using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AUD_PLANAUDITORIA
    /// </summary>
    public class AudPlanauditoriaDTO : EntityBase
    {
        public int Plancodi { get; set; }
        public int Plancodigenerado { get; set; }
        public string Plancodigo { get; set; }
        public string Planano { get; set; } 
        public string Plananovigencia { get; set; } 
        public string Planactivo { get; set; } 
        public string Planhistorico { get; set; } 
        public string Planusucreacion { get; set; } 
        public DateTime? Planfeccreacion { get; set; } 
        public string Planusumodificacion { get; set; } 
        public DateTime? Planfecmodificacion { get; set; } 
    }
}
