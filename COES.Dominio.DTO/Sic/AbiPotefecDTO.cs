using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ABI_POTEFEC
    /// </summary>
    public class AbiPotefecDTO : EntityBase
    {
        public int Pefeccodi { get; set; }
        public int Emprcodi { get; set; }
        public int? Ctgdetcodi { get; set; }
        public int Tgenercodi { get; set; }
        public int Fenergcodi { get; set; }
        public int Equipadre { get; set; }
        public int? Ctgdetcodi2 { get; set; }
        public int? Grupocodi { get; set; }
        public int Equicodi { get; set; }
        public DateTime Pefecfechames { get; set; }
        public decimal Pefecvalorpinst { get; set; }
        public decimal Pefecvalorpe { get; set; }
        public string Pefectipogenerrer { get; set; }
        public string Pefecintegrante { get; set; }
        public DateTime? Pefecfecmodificacion { get; set; }
        public string Pefecusumodificacion { get; set; }

        public int Famcodi { get; set; }
    }
}
