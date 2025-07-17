using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_HISEMPEQ_DATA
    /// </summary>
    public class SiHisempeqDataDTO : EntityBase
    {
        public DateTime Heqdatfecha { get; set; }
        public int Equicodi { get; set; }
        public int Emprcodi { get; set; }
        public int Equicodiold { get; set; }
        public int Equicodiactual { get; set; }
        public string Heqdatestado { get; set; }
        public string Heqdatusucreacion { get; set; }
        public DateTime? Heqdatfeccreacion { get; set; }

        public bool EstadoRecorrido { get; set; }
        public int Heqdatcodi { get; set; }
        public string Equinomb { get; set; }
        public string Emprnomb { get; set; }
    }
}
