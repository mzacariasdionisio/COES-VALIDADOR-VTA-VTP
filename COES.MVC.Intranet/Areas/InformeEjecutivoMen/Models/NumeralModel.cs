using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.InformeEjecutivoMen.Models
{
    public class NumeralModel
    {
        public string TablaNum1 { get; set; }
        public string TablaNum2 { get; set; }
        public string TablaNum3 { get; set; }
        public string TablaNum4 { get; set; }
        public string TablaNum5 { get; set; }
        public string TablaNum6 { get; set; }
        public string TablaNum7 { get; set; }
        public string TablaNum8 { get; set; }
        public string TablaNum9 { get; set; }
        public string TablaNum10 { get; set; }
        public string TablaNum11 { get; set; }

        public List<SpoNumhistoriaDTO> ListaNumeral { get; set; }
        public string Fecha { get; set; }
    }
    public class VersionNumeralModel
    {
        public List<SpoVersionnumDTO> ListaVersionNumeral { get; set; }
    }

    public class ReporteNumeralModel
    {
        public string Fecha { get; set; }
        public List<SpoVersionrepDTO> ListaVersionReporte { get; set; }
        public List<SpoNumeralDTO> ListaNumeral { get; set; }
        public int NroEstadoNum { get; set; }
        public string Resultado { get; set; }
        public Exception Exception { get; set; }
    }

}