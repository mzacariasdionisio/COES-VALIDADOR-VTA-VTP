using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class EveTiposNumeralDTO : EntityBase
    {
        public int EVETIPNUMCODI { get; set; }
        public string EVETIPNUMDESCRIPCION { get; set; }
        public string EVETIPNUMESTADO { get; set; }
        public DateTime LASTDATE { get; set; }
        public string LASTUSER { get; set; }
        public string ESTADO { get; set; }
    }
}
