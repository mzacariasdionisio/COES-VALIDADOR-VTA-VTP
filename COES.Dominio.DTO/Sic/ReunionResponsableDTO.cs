using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class ReunionResponsableDTO : EntityBase
    {
        public int RESPCOD { get; set; }
        public string RESPNAME { get; set; }
        public int RESEVENCODI { get; set; }
        public int EVERESPONCODI { get; set; }
        public int EVENCODI { get; set; }
        public string EVEPARTICIPANTE { get; set; }
        public int EMPRCODI { get; set; }
        public string EMPRNOMB { get; set; }
        public string REPRUTAFIRMA { get; set; }

    }
}
