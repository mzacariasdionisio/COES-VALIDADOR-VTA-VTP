using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class DpoFormulaDTO
    {
        public string Ptomedibarranomb { get; set; }
        public string Nombre_P { get; set; }
        public string Formula_P { get; set; }
        public string Nombre_S { get; set; }
        public string Formula_S { get; set; }
    }

    public class DpoRelacionScoIeod
    {
        public int Ptomedicodi_Sco { get; set; }
        public int Ptomedicodi_Ieod { get; set; }
    }
}
