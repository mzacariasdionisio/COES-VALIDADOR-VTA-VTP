using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class RpfEnergiaPotenciaDTO : EntityBase
    {
        public System.Decimal Rpfenetotal { get; set; }
        public System.DateTime Rpfhidfecha { get; set; }
        public System.DateTime RpfhidfechaIni { get; set; }
        public System.DateTime RpfhidfechaFin { get; set; }
        public System.Decimal Rpfpotmedia { get; set; }
        public System.Decimal Eneindhidra { get; set; }
        public System.Decimal Potindhidra { get; set; }
        public System.String Lastuser { get; set; }
        public System.DateTime Lastdate { get; set; }

    }
}

