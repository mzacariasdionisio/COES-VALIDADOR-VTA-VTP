using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class EpoZonaDTO : EntityBase
    {
        public int Zoncodi { get; set; }
        public string ZonDescripcion { get; set; }
    }
}
