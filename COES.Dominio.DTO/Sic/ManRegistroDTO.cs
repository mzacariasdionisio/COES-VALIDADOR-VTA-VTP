using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla MAN_REGISTRO
    /// </summary>
    public class ManRegistroDTO : EntityBase
    {
        public int Regcodi { get; set; }
        public string Regabrev { get; set; }
        public string Regnomb { get; set; }
        public DateTime Fechaini { get; set; }
        public DateTime? Fechafin { get; set; }
        public int Tregcodi { get; set; }
        public int? Evenclasecodi { get; set; }
        public int? Version { get; set; }
        public int? Sololectura { get; set; }
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
        public DateTime? Fechalim { get; set; }
    }
}

