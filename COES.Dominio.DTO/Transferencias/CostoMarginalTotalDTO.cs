using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class CostoMarginalTotalDTO
    {

        public string BarrNombre { get; set; }
        public string BarrCodi { get; set; }
        public List<decimal> ValorCMG { get; set; }
    }
}
