using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class PropiedadEquipoHistDTO
    {
        public int PROPCODI { get; set; }
        public int EQUICODI { get; set; }
        public DateTime FECHAPROPEQUI { get; set; }
        public string VALOR { get; set; }
        public string PROPABREV { get; set; }
        public string PROPNOMB { get; set; }
        public string PROPUNIDAD { get; set; }
        public string PROPPADRENOM { get; set; }
        public string PROPFILE { get; set; }
        public string FechapropequiDesc { get; set; }
    }
}

