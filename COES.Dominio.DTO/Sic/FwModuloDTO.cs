using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class FwModuloDTO : EntityBase
    {
        public int Modcodi { get; set; }
        public string Modnombre { get; set; }
        public string Modestado { get; set; }
        public int Rolcode { get; set; }
        public string Pathfile { get; set; }
        public string Fuenterepositorio { get; set; }
        public string Usermanual { get; set; }
        public string Inddefecto { get; set; }
    }
}
