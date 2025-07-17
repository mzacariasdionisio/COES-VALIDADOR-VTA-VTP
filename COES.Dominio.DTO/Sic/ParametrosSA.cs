using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class ParametrosSA
    {
        public string id { get; set; }
        public string label { get; set; }
        public List<string> data { get; set; }
        public string htrender { get; set; }
        public string hcrender { get; set; }

        public string Formulas { get; set; }
    }
}
