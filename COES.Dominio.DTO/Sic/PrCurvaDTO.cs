using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_CURVA
    /// </summary>
    public class PrCurvaDTO : EntityBase
    {
        public int Curvcodi { get; set; }
        public string Curvnombre { get; set; }
        public string Curvestado { get; set; }

        public string Curvusucreacion { get; set; }
        public DateTime? Curvfeccreacion { get; set; }
        public string Curvusumodificacion { get; set; }
        public DateTime? Curvfecmodificacion { get; set; }


    }
}
