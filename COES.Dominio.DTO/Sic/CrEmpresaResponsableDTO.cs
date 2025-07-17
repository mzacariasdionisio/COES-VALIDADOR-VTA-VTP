using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class CrEmpresaResponsableDTO
    {
        public int CRRESPEMPRCODI { get; set; }
        public int EMPRCODI { get; set; }
        public int CRETAPACODI { get; set; }
        public DateTime? LASTDATE { get; set; }
        public string LASTUSER { get; set; }
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
        public string EMPRNOMB { get; set; }
    }
}
