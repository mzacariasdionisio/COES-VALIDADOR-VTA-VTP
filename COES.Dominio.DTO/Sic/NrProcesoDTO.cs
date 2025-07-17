using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla NR_PROCESO
    /// </summary>
    public class NrProcesoDTO : EntityBase
    {
        public int Nrprccodi { get; set; } 
        public int? Nrpercodi { get; set; } 
        public int? Grupocodi { get; set; } 
        public int? Nrcptcodi { get; set; } 
        public DateTime? Nrprcfechainicio { get; set; } 
        public DateTime? Nrprcfechafin { get; set; } 
        public decimal? Nrprchoraunidad { get; set; } 
        public decimal? Nrprchoracentral { get; set; } 
        public decimal? Nrprcpotencialimite { get; set; } 
        public decimal? Nrprcpotenciarestringida { get; set; } 
        public decimal? Nrprcpotenciaadjudicada { get; set; } 
        public decimal? Nrprcpotenciaefectiva { get; set; } 
        public decimal? Nrprcpotenciaprommedidor { get; set; } 
        public decimal? Nrprcprctjrestringefect { get; set; } 
        public decimal? Nrprcvolumencombustible { get; set; } 
        public decimal? Nrprcrendimientounidad { get; set; } 
        public decimal? Nrprcede { get; set; } 
        public int? Nrprcpadre { get; set; } 
        public string Nrprcexceptuacoes { get; set; } 
        public string Nrprcexceptuaosinergmin { get; set; } 
        public string Nrprctipoingreso { get; set; } 
        public string Nrprchorafalla { get; set; } 
        public decimal? Nrprcsobrecosto { get; set; } 
        public string Nrprcobservacion { get; set; } 
        public string Nrprcnota { get; set; } 
        public string Nrprcnotaautomatica { get; set; } 
        public string Nrprcfiltrado { get; set; } 
        public decimal? Nrprcrpf { get; set; } 
        public decimal? Nrprctolerancia { get; set; } 
        public string Nrprcusucreacion { get; set; } 
        public DateTime? Nrprcfeccreacion { get; set; } 
        public string Nrprcusumodificacion { get; set; } 
        public DateTime? Nrprcfecmodificacion { get; set; } 
        public string Nrpermes { get; set; }
        public string Gruponomb { get; set; }
        public string Nrcptabrev { get; set; }

        public string Emprnomb { get; set; }

    }
}
