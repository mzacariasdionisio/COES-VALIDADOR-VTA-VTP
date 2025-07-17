using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class WbConvocatoriasDTO: EntityBase
    {
        public int Convcodi { get; set; }
        public string Convabrev { get; set; }
        public string Convnomb { get; set; }
        public string Convdesc { get; set; }
        public string Convlink { get; set; }
        public DateTime? Convfechaini { get; set; }
        public DateTime? Convfechafin { get; set; }
        public string Convestado { get; set; }
        public DateTime? Datecreacion { get; set; }
        public string Usercreacion { get; set; }
        public DateTime? Lastdate { get; set; }
        public string Lastuser { get; set; }
        public string ConvfechainiDesc { get; set; }
        public string ConvfechafinDesc { get; set; }


    }
}
