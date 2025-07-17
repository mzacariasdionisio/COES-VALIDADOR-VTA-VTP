using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_BARRAGENERADOR
    /// </summary>
    public class CmBarrageneradorDTO : EntityBase
    {
        public int Bargercodi { get; set; } 
        public int? Relacioncodi { get; set; } 
        public int? Cnfbarcodi { get; set; } 
        public DateTime? Bargerfecha { get; set; } 
        public string Bargerusucreacion { get; set; } 
        public DateTime? Bargerfeccreacion { get; set; } 
    }
}
