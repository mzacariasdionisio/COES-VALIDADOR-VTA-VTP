using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.SGDoc
{
    public class AtencionDTO
    {
        public int Fljdetcodi { get; set; }
        public DateTime FechaMsg { get; set; }
        public string NombAreaOrig { get; set; }
        public string NombAreaDest { get; set; }
        public string Msg { get; set; }
        public string Estado { get; set; }
        public string Fileruta { get; set; }
        public bool IndArchivo { get; set; }
    }
}
