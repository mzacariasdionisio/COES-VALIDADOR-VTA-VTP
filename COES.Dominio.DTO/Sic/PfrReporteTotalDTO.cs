using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PFR_REPORTE_TOTAL
    /// </summary>
    public partial class PfrReporteTotalDTO : EntityBase
    {
        public int Pfrtotcodi { get; set; }
        public int? Emprcodi { get; set; }
        public int? Equipadre { get; set; }
        public int? Equicodi { get; set; }
        public int? Famcodi { get; set; }
        public int? Grupocodi { get; set; }
        public string Pfrtotunidadnomb { get; set; }        
        public int? Pfresccodi { get; set; } 
        public decimal? Pfrtotcv { get; set; } 
        public decimal? Pfrtotpe { get; set; } 
        public decimal? Pfrtotpea { get; set; } 
        public decimal? Pfrtotfi { get; set; } 
        public decimal? Pfrtotpf { get; set; } 
        public decimal? Pfrtotpfc { get; set; } 
        public decimal? Pfrtotpd { get; set; } 
        public decimal? Pfrtotcvf { get; set; } 
        public decimal? Pfrtotpdd { get; set; } 
        public decimal? Pfrtotpfr { get; set; }
        public int? Pfrtotcrmesant { get; set; }
        public decimal? Pfrtotfkmesant { get; set; }
        public int? Pfrtotficticio { get; set; }
    }

    public partial class PfrReporteTotalDTO : EntityBase, ICloneable
    {
        public string Famnomb { get; set; }
        public string Emprnomb { get; set; }
        public string Central { get; set; }
        public string Equinomb { get; set; }
        public string Gruponomb { get; set; }
        public string Grupotipocogen { get; set; }

        public string Validador { get; set; }
        public int RegistroDuplicadoAux2 { get; set; }
        public decimal? ValorPDParaDuplicadoAux2 { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
