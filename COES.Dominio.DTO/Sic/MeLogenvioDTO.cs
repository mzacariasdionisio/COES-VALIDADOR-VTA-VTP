using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_LOGENVIO
    /// </summary>
    public class MeLogenvioDTO : EntityBase
    {
        public int Enviocodi { get; set; } 
        public int Logenvsec { get; set; } 
        public DateTime? Logenvfecha { get; set; } 
        public string Lastuser { get; set; }
        public string Logenvdescrip { get; set; } 
        public int? Mencodi { get; set; } 
        public int? Logenvmencant { get; set; } 
    }
}
