using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SIO_PRIE_COMP
    /// </summary>
    public partial class SioPrieCompDTO : EntityBase
    {
        public int Tbcompcodi { get; set; }
        public DateTime? Tbcompfecperiodo { get; set; }
        public int Emprcodi { get; set; }

        public decimal? Tbcompte { get; set; }
        public decimal? Tbcomppsr { get; set; }
        public decimal? Tbcomprscd { get; set; }
        public decimal? Tbcomprscul { get; set; }
        public decimal? Tbcompcbec { get; set; }
        public decimal? Tbcompcrf { get; set; }
        public decimal? Tbcompcio { get; set; }
        public decimal? Tbcompsma { get; set; }
        public decimal? Tbcompoc { get; set; }
        public decimal? Tbcompcpa { get; set; }
        public string Tbcompcodosinergmin { get; set; }
        public string Tbcompusucreacion { get; set; }
        public DateTime? Tbcompfeccreacion { get; set; }
        public string Tbcompusumodificacion { get; set; }
        public DateTime? Tbcompfecmodificacion { get; set; }
    }

    public partial class SioPrieCompDTO
    {
        public string Emprnomb { get; set; }
        public string Emprcodosinergmin { get; set; }
        public string TbcompfecperiodoDesc { get; set; }

    }
}
