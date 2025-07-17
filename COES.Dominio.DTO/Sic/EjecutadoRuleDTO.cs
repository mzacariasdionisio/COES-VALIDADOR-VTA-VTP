using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class EjecutadoRuleDTO
    {
        public short EJRUCODI { get; set; }
        public string EJRUABREV { get; set; }
        public string EJRUDETALLE { get; set; }
        public string EJRUFORMULA { get; set; }
        public string EJRUACTIVA { get; set; }
        public string EJRULASTUSER { get; set; }
        public Nullable<System.DateTime> EJRULASTDATE { get; set; }
    }
}

