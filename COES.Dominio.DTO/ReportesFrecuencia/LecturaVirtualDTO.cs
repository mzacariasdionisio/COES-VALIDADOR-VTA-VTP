using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.ReportesFrecuencia
{
    public class LecturaVirtualDTO
    {
        public int IdCarga { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal Frecuencia { get; set; }
        public decimal Tension { get; set; }
        public string Mensaje { get; set; }
        public string FecHora { get; set; }
        public int Miliseg { get; set; }
        public string FechaHoraString { get; set; }
    }
}
