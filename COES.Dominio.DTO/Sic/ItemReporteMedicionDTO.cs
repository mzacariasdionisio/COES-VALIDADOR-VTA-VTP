using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase para precalculo de reporte
    /// </summary>
    public class ItemReporteMedicionDTO 
    {
        public decimal MaximaDemanda { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public DateTime FechaMaximaDemanda { get; set; }
        public string HoraMaximaDemanda { get; set; }
        public int Indice { get; set; }
        public decimal Interconexion { get; set; }
                
    }
}
