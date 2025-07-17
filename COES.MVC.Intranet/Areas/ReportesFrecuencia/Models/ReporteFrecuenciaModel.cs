using COES.Dominio.DTO.ReportesFrecuencia;
using System;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.ReportesFrecuencia.Models
{
    public class ReporteFrecuenciaModel
    {
        public string mensaje { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin{ get; set; }
        public List<ReporteEquipoGPS> ListaGPS { get; set; }
        public int IdGPS { get; set; }
        public string mesInicio { get; set; }
        public string Etapas { get; set; }
        public string IndOficial { get; set; }
    }

    public class ReporteEquipoGPS
    {
        public int Id { get; set; }
        public string GPS { get; set; }
        public string Oficial { get; set; }
    }

}