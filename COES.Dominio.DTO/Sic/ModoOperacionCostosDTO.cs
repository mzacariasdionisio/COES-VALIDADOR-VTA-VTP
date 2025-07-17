using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class ModoOperacionCostosDTO
    {
        public int Grupocodi { get; set; }
        public string Gruponomb { get; set; }
        public string Combustible { get; set; }
        public string PrecioCombustible { get; set; }
        public string PrecioTransporte { get; set; }
        public string PrecioTratMecanico { get; set; }
        public string PrecioTratQuimico { get; set; }
        public string CVNC { get; set; }
    }
}
