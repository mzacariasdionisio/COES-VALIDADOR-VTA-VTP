using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.SGDoc
{
    public class SelloDTO
    {
        public string FljCadAtencion { get; set; }
        public int AreaPadre { get; set; }
        public int AreaCode { get; set; }
        public string DescripDeleg { get; set; }
        public DateTime? FljFechaMax { get; set; }
    }
}
