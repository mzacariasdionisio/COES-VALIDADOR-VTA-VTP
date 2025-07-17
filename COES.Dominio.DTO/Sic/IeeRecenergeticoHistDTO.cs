using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IEE_RECENERGETICO_HIST
    /// </summary>
    [Serializable]
    public class IeeRecenergeticoHistDTO : EntityBase
    {
        public int Renerhcodi { get; set; }
        public DateTime? Renerhfecha { get; set; }
        public decimal? Renerhvalor { get; set; }
        public int? Renertipcodi { get; set; }
        public string Renerhusucreacion { get; set; }
        public DateTime? Renerhfeccreacion { get; set; }
        public string Renerhusumodificacion { get; set; }
        public DateTime? Renerhfecmodificacion { get; set; }
        public string Renertipnomb { get; set; }
    }
}
