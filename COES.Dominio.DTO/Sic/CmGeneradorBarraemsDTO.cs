using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_GENERADOR_BARRAEMS
    /// </summary>
    public class CmGeneradorBarraemsDTO : EntityBase
    {
        public int Genbarcodi { get; set; } 
        public int? Relacioncodi { get; set; } 
        public int? Cnfbarcodi { get; set; } 
        public string Genbarusucreacion { get; set; } 
        public DateTime? Genbarfeccreacion { get; set; } 
        public string Genbarusumodificacion { get; set; } 
        public DateTime? Genbarfecmodificacion { get; set; } 
        public string Cnfbarnombre { get; set; }
    }
}
