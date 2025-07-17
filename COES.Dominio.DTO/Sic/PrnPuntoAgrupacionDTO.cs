using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class PrnPuntoAgrupacionDTO
    {
        public int Ptogrpcodi { get; set; }
        public int Ptomedicodi { get; set; }
        public int Ptogrppronostico { get; set; }
        public DateTime Ptogrpfechaini { get; set; }
        public DateTime Ptogrpfechafin { get; set; }
        public string Ptogrpusumodificacion { get; set; }
    }
}
