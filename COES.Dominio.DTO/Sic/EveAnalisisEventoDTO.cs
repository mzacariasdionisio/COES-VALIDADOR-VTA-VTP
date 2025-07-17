using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class EveAnalisisEventoDTO : EntityBase
    {
        public int EVEANAEVECODI { get; set; }
        public int EVENCODI { get; set; }
        public string EVEANAEVEDESCNUMERAL { get; set; }
        public string EVEANAEVEDESCFIGURA { get; set; }
        public string EVEANAEVERUTAFIGURA { get; set; }
        public int EVENUMCODI { get; set; }
        public DateTime LASTDATE { get; set; }
        public string LASTUSER { get; set; }
        public string EVETIPNUMDESCRIPCION { get; set; }
        public int EVEANATIPO { get; set; }
        public string EVEANAHORA { get; set; }
        public int EVEANAPOSICION { get; set; }
    }
}
