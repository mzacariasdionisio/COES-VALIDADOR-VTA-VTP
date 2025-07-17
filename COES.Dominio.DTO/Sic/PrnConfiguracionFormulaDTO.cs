using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class PrnConfiguracionFormulaDTO
    {
        public int Cnffrmcodi { get; set; }
        public int Ptomedicodi { get; set; }
        public string Ptomedidesc { get; set; }
        public DateTime Cnffrmfecha { get; set; }
        public string Cnffrmferiado { get; set; }
        public string Cnffrmatipico { get; set; }
        public string Cnffrmveda { get; set; }
        public string Cnffrmdepauto { get; set; }
        public decimal? Cnffrmcargamax { get; set; }
        public decimal? Cnffrmcargamin { get; set; }
        public decimal? Cnffrmtolerancia { get; set; }
        public string Cnffrmusucreacion { get; set; }
        public DateTime Cnffrmfeccreacion { get; set; }
        public string Cnffrmusumodificacion { get; set; }
        public DateTime Cnffrmfecmodificacion { get; set; }
        //adicional
        public string Strcnffrmfecha { get; set; }
        public int Cnffrmformula { get; set; }
        public int? Cnffrmdiapatron { get; set; }
        public string Cnffrmpatron { get; set; }
        public string Cnffrmdefecto { get; set; }
        public int Prrucodi { get; set; }
        public string Prruabrev { get; set; }
    }
}
