using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class VtdRetiroMensual
    {
        public int ReMeNDia { get; set; }
        public int ReMeNDSe { get; set; }
        public DateTime? ReMeSem { get; set; }
        public decimal ReMeProD { get; set; }
        public decimal ReMeReDP { get; set; }
        public decimal ReMeReDE { get; set; }
    }
}
