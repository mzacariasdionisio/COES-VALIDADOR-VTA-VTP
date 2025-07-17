using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class PerfilScadaDTO
    {
        public System.Int32 PERFCODI { get; set; }
        public System.String PERFDESC { get; set; }
        public System.DateTime? FECREGISTRO { get; set; }
        public System.DateTime? FECINICIO { get; set; }
        public System.DateTime? FECFIN { get; set; }
        public System.String LASTUSER { get; set; }
        public System.DateTime? LASTDATE { get; set; }
        public System.Int32 EJRUCODI { get; set; }
        public System.Int32 PERFCLASI { get; set; }
        public System.String PRRUNOMB { get; set; }
        public System.String PRRUABREV { get; set; }
        public List<ScadaDTO> LISTADETALLE { get; set; }
        public List<PerfilScadaDetDTO> LISTAITEMS { get; set; }
        public System.String PERFORIG { get; set; }
    }

}
