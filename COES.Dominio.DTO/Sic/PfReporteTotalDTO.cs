using COES.Base.Core;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PF_REPORTE_TOTAL
    /// </summary>
    public partial class PfReporteTotalDTO : EntityBase
    {
        public int Pftotcodi { get; set; }
        public int Pfescecodi { get; set; }
        public int Emprcodi { get; set; }
        public int Equipadre { get; set; }
        public int Famcodi { get; set; }
        public int? Grupocodi { get; set; }
        public int? Equicodi { get; set; }
        public decimal? Pftotpe { get; set; }
        public decimal? Pftotpprom { get; set; }
        public decimal? Pftotenergia { get; set; }
        public int Pftotminsincu { get; set; }
        public decimal? Pftotfi { get; set; }
        public decimal? Pftotpg { get; set; }
        public decimal? Pftotfp { get; set; }
        public decimal? Pftotpf { get; set; }
        public int Pftotincremental { get; set; }
        public string Pftotunidadnomb { get; set; }
        public int Pftotnumdiapoc { get; set; }
    }

    public partial class PfReporteTotalDTO
    {
        public int Pfperianio { get; set; }
        public int Pfperimes { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }

        public string Famnomb { get; set; }
        public string Emprnomb { get; set; }
        public string Central { get; set; }
        public string Equinomb { get; set; }
        public string Gruponomb { get; set; }
        public string Grupotipocogen { get; set; }
        public DateTime? Equifechiniopcom { get; set; }
        public DateTime? Equifechfinopcom { get; set; }
        public string EquifechiniopcomDesc { get; set; }
        public bool Tiene36Meses { get; set; }
        public string ComentarioCalorUtil { get; set; }

        /// <summary>
        /// Grupocodi del equipo padre
        /// </summary>
        public int? Grupocodi2 { get; set; }

        /// <summary>
        /// Fecha inicial del escenario
        /// </summary>
        public DateTime Pfescefecini { get; set; }

        /// <summary>
        /// Fecha final del escenario
        /// </summary>
        public DateTime Pfescefecfin { get; set; }

        //PMEcc
        public decimal? PotenciaPromedio { get; set; }

        //Tcc 
        public int TotalMinutosCalorUtil { get; set; }

        //Tg
        public int TotalMinutosSinCalorUtil { get; set; }

        //T
        public int TotalMinutosPeriodo { get; set; }

        public decimal? Pftotpf1 { get; set; }
        public decimal? Pftotpf2 { get; set; }
        public decimal? Pftotpf3 { get; set; }
        public decimal? Pftotpf4 { get; set; }
        public decimal? Pftotpf5 { get; set; }
        public decimal? Pftotpf6 { get; set; }
        public decimal? Pftotpf7 { get; set; }
        public decimal? Pftotpf8 { get; set; }
        public decimal? Pftotpf9 { get; set; }

        public decimal Pftotpfpor1 { get; set; }
        public decimal Pftotpfpor2 { get; set; }
        public decimal Pftotpfpor3 { get; set; }
        public decimal Pftotpfpor4 { get; set; }
        public decimal Pftotpfpor5 { get; set; }
        public decimal Pftotpfpor6 { get; set; }
        public decimal Pftotpfpor7 { get; set; }
        public decimal Pftotpfpor8 { get; set; }
        public decimal Pftotpfpor9 { get; set; }



        public decimal? Pftotpe1 { get; set; }
        public decimal? Pftotpe2 { get; set; }
        public decimal? Pftotpe3 { get; set; }
        public decimal? Pftotpe4 { get; set; }
        public decimal? Pftotpe5 { get; set; }
        public decimal? Pftotpe6 { get; set; }
        public decimal? Pftotpe7 { get; set; }
        public decimal? Pftotpe8 { get; set; }
        public decimal? Pftotpe9 { get; set; }

        public List<PfReporteDetDTO> ListaDetalle { get; set; }
    }
}
