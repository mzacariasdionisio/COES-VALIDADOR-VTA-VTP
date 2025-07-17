using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_PERIODO_ETAPA
    /// </summary>
    public class RePeriodoEtapaDTO : EntityBase
    {
        public int Repeetcodi { get; set; } 
        public int? Repercodi { get; set; } 
        public int? Reetacodi { get; set; } 
        public DateTime? Repeetfecha { get; set; } 
        public string Repeetestado { get; set; } 
        public string Repeetusucreacion { get; set; } 
        public DateTime? Repeetfeccreacion { get; set; } 
        public string Repeetusumodificacion { get; set; } 
        public DateTime? Repeetfecmodificacion { get; set; } 
    }
}
