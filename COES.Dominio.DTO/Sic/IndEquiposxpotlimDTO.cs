using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_EQUIPOSXPOTLIM
    /// </summary>
    public class IndEquiposxpotlimDTO : EntityBase
    {
        public int Equlimcodi { get; set; }
        public decimal? Equlimpotefectiva { get; set; }
        public int? Cuadr3codi { get; set; }
        public string Equlimusumodificacion { get; set; }
        public DateTime? Equlimfecmodificacion { get; set; }
        public int? Equicodi { get; set; }
        public int? Emprcodi { get; set; }
        public string Equilimpotgrupomodoper { get; set; }
        public string Emprnomb { get; set; }
        public string Equinomb { get; set; }
    }
}
