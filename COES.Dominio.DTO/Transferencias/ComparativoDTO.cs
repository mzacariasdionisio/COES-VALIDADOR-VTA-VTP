using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class ComparativoDTO
    {
        public int Dia { get; set; }
        public int PeriMes { get; set; }
        public int PeriAnio { get; set; }
        public int Version { get; set; }

        public decimal valorInicial { get; set; }
        public decimal valorFinal { get; set; }
        public decimal tiempo { get; set; }
        public string fecha { get; set; }


        public decimal variacion { get; set; }

        #region Comparativo
        public string Hora { get; set; }
        public decimal EntregaRetiro1 { get; set; }
        public decimal EntregaRetiro2 { get; set; }
        public decimal Desviacion { get; set; }
        public string BarrNombre { get; set; }
        public DateTime FechaIntervalo { get; set; }
        #endregion

    }

    public class ComparativoPeriodosDTO
    {
        public string PeriodoMesVersion { get; set; }
        public int PERIANIO { get; set; }
        public int PERIMES { get; set; }
        public int VTRANVERSION { get; set; }
        public int Dia { get; set; }
        public List<ComparativoDTO> ListaComparativos { get; set; }
    }
}
