using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_PROCESO_LOG
    /// </summary>
    public class SiProcesoLogDTO : EntityBase
    {
        public int Prcslgcodi { get; set; } 
        public int Prcscodi { get; set; }
        public DateTime? Prcslgfecha { get; set; } 
        public DateTime? Prcslginicio { get; set; } 
        public DateTime? Prcslgfin { get; set; } 
        public string Prcslgestado { get; set; } 
    }
}
