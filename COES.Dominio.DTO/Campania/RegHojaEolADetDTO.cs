using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class RegHojaEolADetDTO
    {
        public int CentralADetCodi { get; set; }
        public int CentralACodi { get; set; }

        public Decimal? Speed { get; set; }

        public Decimal? Acciona { get; set; }

        public string UsuCreacion { get; set; }
        public DateTime? FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }


    }
}
