using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class EtapaDTO
    {
        public decimal? Etpcodi { get; set; }
        public decimal Etpdelta { get; set; }
        public decimal Etpacumulado { get; set; }
        public string Etpdescripcion { get; set; }
        public DateTime EtapaInicio { get; set; }
        public string Etpformatohora { get; set; }
    }
}
