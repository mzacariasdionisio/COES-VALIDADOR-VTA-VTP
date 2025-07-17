using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class SecuenciaEventoDTO : EntityBase
    {
        public int EVESECUENCIACODI { get; set; }
        public int EVENCODI { get; set; }
        public string EVESECFECHA { get; set; }
        public string EVESECHORA { get; set; }
        public string EVESECSECC { get; set; }
        public int EVECOEMP { get; set; }
        public string EVESECDESCRIPCION { get; set; }
        public bool EVESECINCMANIOB { get; set; }
        public string EVESECINCMANIOBVALOR { get; set; }
        public DateTime? LASTDATE { get; set; }
        public string LASTUSER { get; set; }

        public string LASTDATEstr
        {
            get
            {
                if (LASTDATE.HasValue)
                {
                    return LASTDATE.Value.ToString("dd/MM/yyyy HH:mm:ss");
                }
                return "";
            }
        }
    }
}
