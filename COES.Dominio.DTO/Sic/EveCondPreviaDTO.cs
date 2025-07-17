using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class EveCondPreviaDTO : EntityBase
    {
        public int EVECONDPRCODI { get; set; }
        public int EVENCODI { get; set; }
        public string EVECONDPRTIPO { get; set; }
        public string EVECONDPRCODIGOUNIDAD { get; set; }
        public string EVECONDPRSUBESTACIONA { get; set; }
        public int EVECONDPRSUBESTACIONCENT { get; set; }
        public string EVECONDPRUNIDAD { get; set; }
        public decimal EVECONDPRTENSION { get; set; }
        public string EVECONDPRPOTENCIAMW { get; set; }
        public string EVECONDPRPOTENCIAMVAR { get; set; }
        public DateTime LASTDATE { get; set; }
        public string LASTUSER { get; set; }
        public string EVECONDPRSUBESTACIONDE { get; set; }
        public string EVECONDPRCENTRALDE { get; set; }
    }
}
