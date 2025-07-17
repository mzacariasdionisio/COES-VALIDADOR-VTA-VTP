using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RI_HISTORICO
    /// </summary>
    [Serializable]
    public class RiHistoricoDTO : EntityBase
    {
        public int Hisricodi { get; set; } 
        public int? Hisrianio { get; set; } 
        public string Hisritipo { get; set; } 
        public string Hisridesc { get; set; } 
        public DateTime? Hisrifecha { get; set; } 
        public string Hisriestado { get; set; } 
        public string Hisriusucreacion { get; set; } 
        public DateTime? Hisrifeccreacion { get; set; } 
        public string Hisriusumodificacion { get; set; } 
        public DateTime? Hisrifecmodificacion { get; set; } 
    }
}
