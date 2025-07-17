using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class PrnRelacionTnaDTO
    {
        public int Reltnacodi { get; set; }
        public int Reltnaformula { get; set; }
        public string Reltnanom { get; set; }

        //adicionales
        public int Reltnadetcodi { get; set; }
        public int Barracodi { get; set; }
        public string Barranom { get; set; }
        public int Reltnadetformula { get; set; }
        public List<dynamic[]> Detalle { get; set; }
        public string Formulanomb { get; set; }
    }
}
