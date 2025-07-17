using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class EveintdescargaDTO : EntityBase
    {
        public int EVEINTDESCODI { get; set; }
        public int EVENCODI { get; set; }
        public int EVEINTDESTIPO { get; set; }
        public string EVEINTDESCELDA { get; set; }
        public string EVEINTDESCODIGO { get; set; }
        public string EVEINTDESSUBESTACION { get; set; }
        public int EVEINTDESR_ANTES { get; set; }
        public int EVEINTDESS_ANTES { get; set; }
        public int EVEINTDEST_ANTES { get; set; }
        public int EVEINTDESR_DESPUES { get; set; }
        public int EVEINTDESS_DESPUES { get; set; }
        public int EVEINTDEST_DESPUES { get; set; }      
        public DateTime LASTDATE { get; set; }
        public string LASTUSER { get; set; }
    }
}
