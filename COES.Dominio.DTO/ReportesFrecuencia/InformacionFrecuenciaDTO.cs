using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.ReportesFrecuencia
{
    public class InformacionFrecuenciaDTO
    {
        public string FechaHora { get; set; }
        public int GPSCodi { get; set; }
        public string ColumnH { get; set; }
        public string Valor { get; set; }
        public string Frecuencia { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string GPSNombre { get; set; }
        public string GPSNombreComparar { get; set; }
        public string FrecuenciaComparar { get; set; }
        public double FrecuenciaDiferencia { get; set; }
    }
}
