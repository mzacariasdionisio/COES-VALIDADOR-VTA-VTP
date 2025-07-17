using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class DesviacionDTO : EntityBase
    {
        public System.Int32 Lectcodi { get; set; }
        public System.DateTime Desvfecha { get; set; }
        public System.Int32 Ptomedicodi { get; set; }
        public System.Int32 Medorigdesv { get; set; }
        public System.String Lastuser { get; set; }
        public System.DateTime Lastdate { get; set; }
        public System.String Ptomedidesc { get; set; }

    }
}
