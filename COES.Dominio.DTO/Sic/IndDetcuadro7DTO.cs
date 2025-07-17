using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_DETCUADRO7
    /// </summary>
    public class IndDetcuadro7DTO : EntityBase
    {
        public int Cuadr7codi { get; set; }
        public int? Cuadr7narranque { get; set; }
        public decimal? Cuadr7hip { get; set; }
        public int? Cuadr7anno { get; set; }
        public string Cuadr7usumodificacion { get; set; }
        public DateTime? Cuadr7fecmodificacion { get; set; }
        public int? Cuadr7semana { get; set; }
        public decimal? Cuadr7hif { get; set; }
        public int? Cuadr7mes { get; set; }
        public int? Equicodi { get; set; }
        public int? Percu7codi { get; set; }
        public decimal? Hopcodi { get; set; }
        public int? Emprcodi { get; set; }

        public string Emprnomb { get; set; }
        public string Equinomb { get; set; }
        public string Gruponomb { get; set; }

    }
}
