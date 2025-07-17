using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class RequestReporte
    {
        public string empresas { get; set; }
        public string estado { get; set; }
        public string periodo { get; set; }

        public  string idreportes { get; set; }
        
        public  string tipoReporte { get; set; }

    }
}
