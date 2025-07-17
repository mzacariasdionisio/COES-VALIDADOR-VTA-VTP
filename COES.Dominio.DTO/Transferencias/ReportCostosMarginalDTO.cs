using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class CostoMarginalGraficosDTO
    {
        public string Hora { get; set; }
        public string Codigo{ get; set; }
        public decimal CostoMarginal1 { get; set; }
        public decimal CostoMarginal2 { get; set; }
        public decimal Desviacion { get; set; }
        public string BarrNombre { get; set; }
        public DateTime FechaIntervalo { get; set; }
        public List<CostoMarginalGraficoValoresDTO> CostosMarginales { get; set; }

    }
    
}
