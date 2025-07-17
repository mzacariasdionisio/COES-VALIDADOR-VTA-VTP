using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class CostoMarginalDesviacionDTO
    {

        public int TipCosto { get; set; }
        public int PeriCodi1 { get; set; }
        public List<int> Version1Array { get; set; }
        public List<int> Dia1Array { get; set; }
        public int PeriCodi2 { get; set; }
        public List<int> Version2Array { get; set; }
        public List<int> Dia2Array { get; set; }
        public List<int> BarrasArray { get; set; }

        public string TipoCostoMarginal { get; set; }
    }
}
