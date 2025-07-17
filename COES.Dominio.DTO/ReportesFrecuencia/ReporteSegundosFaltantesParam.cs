using System;

namespace COES.Dominio.DTO.ReportesFrecuencia
{
    public class ReporteSegundosFaltantesParam
    {
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public int IdGPS { get; set; }
        public string Usuario { get; set; }
        public string IndOficial { get; set; }

    }
}

