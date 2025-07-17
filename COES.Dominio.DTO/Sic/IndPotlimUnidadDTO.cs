using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_POTLIM_UNIDAD
    /// </summary>
    public partial class IndPotlimUnidadDTO : EntityBase
    {
        public int Equlimcodi { get; set; }
        public int Potlimcodi { get; set; }
        public decimal? Equlimpotefectiva { get; set; }
        public int? Equipadre { get; set; }
        public int? Grupocodi { get; set; }
        public int? Equicodi { get; set; }
        public string Equlimusumodificacion { get; set; }
        public DateTime? Equlimfecmodificacion { get; set; }
    }

    public partial class IndPotlimUnidadDTO : EntityBase
    {
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public string Central { get; set; }
        public string NombreUnidad { get; set; }
    }
}
