using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PF_DISPCALORUTIL
    /// </summary>
    public partial class PfDispcalorutilDTO : EntityBase
    {
        public int Pfcucodi { get; set; }
        public int Emprcodi { get; set; }
        public int Equipadre { get; set; }
        public int Equicodi { get; set; }
        public DateTime Pfcufecha { get; set; }
        public int Pfcuh { get; set; }
        public int Pfcutienedisp { get; set; }
        public int Irptcodi { get; set; }
        public int? Pfcumin { get; set; }
    }

    public partial class PfDispcalorutilDTO
    {
        public DateTime FechaHora { get; set; }
        public string Emprnomb { get; set; }
        public int Grupocodi { get; set; }
        public string Central { get; set; }
        public string Equinomb { get; set; }
        public decimal? Mw { get; set; }
        public string PfcufechaDesc { get; set; }
    }
}
