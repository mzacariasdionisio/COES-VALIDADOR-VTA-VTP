using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_POTLIM
    /// </summary>
    public partial class IndPotlimDTO : EntityBase
    {
        public int Potlimcodi { get; set; }
        public decimal? Potlimmw { get; set; }
        public string Potlimnombre { get; set; }
        public DateTime? Potlimfechaini { get; set; }
        public DateTime? Potlimfechafin { get; set; }
        public int? Potlimestado { get; set; }
        public string Potlimusucreacion { get; set; }
        public DateTime? Potlimfeccreacion { get; set; }
        public string Potlimusumodificacion { get; set; }
        public DateTime? Potlimfecmodificacion { get; set; }

        public List<IndPotlimUnidadDTO> IndPotlimUnidades { get; set; }
    }

    public partial class IndPotlimDTO : EntityBase
    {
        public int? Equicodi { get; set; }
        public string Equinomb { get; set; }
        public string Equinomb2 { get; set; }
        public int? Grupocodi { get; set; }
        public string Gruponomb { get; set; }
        public decimal Equlimpotefectiva { get; set; }
    }
}
