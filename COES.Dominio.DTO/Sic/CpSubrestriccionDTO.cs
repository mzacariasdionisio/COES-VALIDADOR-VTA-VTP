using COES.Base.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class CpSubrestriccionDTO
    {
        public short Srestcodi { get; set; }
        public Nullable<short> Restriccodi { get; set; }
        public string Srestnombre { get; set; }
        public string Srestnombregams { get; set; }
        public string Srestunidad { get; set; }
        public short Catcodi { get; set; }
        public  decimal Srestactivo {get;set;}
        public int Topcodi { get; set; }
    }
}
