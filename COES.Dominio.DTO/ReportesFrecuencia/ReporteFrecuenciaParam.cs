using System;

namespace COES.Dominio.DTO.ReportesFrecuencia
{
    public class ReporteFrecuenciaParam
    {
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public int IdGPS { get; set; }
        public string Usuario { get; set; }
        public string IndOficial { get; set; }

    }
}
