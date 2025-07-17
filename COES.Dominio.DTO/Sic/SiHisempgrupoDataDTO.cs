using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_HISEMPGRUPO_DATA
    /// </summary>
    public class SiHisempgrupoDataDTO : EntityBase
    {
        public DateTime Hgrdatfecha { get; set; }
        public int Emprcodi { get; set; }
        public string Hgrdatestado { get; set; }
        public int? Grupocodiold { get; set; }
        public int? Grupocodiactual { get; set; }
        public int Grupocodi { get; set; }
        public int Hgrdatcodi { get; set; }
        public string Hgrdatusucreacion { get; set; }
        public DateTime Hgrdatfeccreacion { get; set; }

        public string Gruponomb { get; set; }
        public string Emprnomb { get; set; }

    }
}
