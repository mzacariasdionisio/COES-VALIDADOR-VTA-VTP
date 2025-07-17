using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class TrnConsumoNoAutorizadoDTO
    {
        public int Conscodi { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public decimal Valorcna { get; set; }
        public DateTime Fechacna { get; set; }       
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
    }
}
