using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class EveRecomobservDTO
    {
        public int EVERECOMOBSERVCODI { get; set; }
        public int EVENCODI { get; set; }
        public int EMPRCODI { get; set; }
        public int EVERECOMOBSERVTIPO { get; set; }
        public string EVERECOMOBSERVDESC { get; set; }
        public string EVERECOMOBSERVESTADO { get; set; }
        public DateTime? LASTDATE { get; set; }
        public string LASTUSER { get; set; }
        public string EMPRNOMB { get; set; }
        public string LASTDATEstr
        {
            get
            {
                if (LASTDATE.HasValue)
                {
                    return LASTDATE.Value.ToString("yyyy-MM-dd");
                }
                return "";
            }
        }
    }
}
