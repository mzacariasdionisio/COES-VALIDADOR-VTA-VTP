using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_FUENTEDATOS
    /// </summary>
    public class SiFuentedatosDTO : EntityBase
    {
        public int Fdatcodi { get; set; }
        public string Fdatnombre { get; set; }
        public string Fdattabla { get; set; }
        public string Fdatpk { get; set; } 
        public int? Fdatpadre { get; set; }

    }
}
