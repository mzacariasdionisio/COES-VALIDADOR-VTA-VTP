using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_VALIDACION
    /// </summary>
    public partial class MeValidacionDTO : EntityBase
    {
        public int Validcodi { get; set; }
        public int Formatcodi { get; set; }
        public int Emprcodi { get; set; }
        public DateTime Validfechaperiodo { get; set; }
        public int? Validestado { get; set; }
        public string Validusumodificacion { get; set; }
        public DateTime? Validfecmodificacion { get; set; }
        public string Validcomentario { get; set; }
        public string Validplazo { get; set; }
        public decimal? Validdataconsiderada { get; set; }
        public decimal? Validdatainformada { get; set; }
        public decimal? Validdatasinobs { get; set; }
    }

    public partial class MeValidacionDTO
    {
        public string Emprnomb { get; set; }
        public string Formatnombre { get; set; }
        public string ValidfechaperiodoDesc { get; set; }
        public string ValidplazoDesc { get; set; }
        public string ValiddataconsideradaDesc { get; set; }
        public string ValiddatainformadaDesc { get; set; }
        public string ValiddatasinobsDesc { get; set; }
    }
}
