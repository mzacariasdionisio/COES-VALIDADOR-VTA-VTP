using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class CrEtapaCriterioDTO
    {
        public int CRETAPACRICODI { get; set; }
        public int CRETAPACODI { get; set; }
        public int CRCRITERIOCODI { get; set; }
        public DateTime LASTDATE { get; set; }
        public string LASTUSER { get; set; }
        public string CREDESCRIPCION { get; set; }
    }
}
