using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class PrnConfigbarraDTO : EntityBase
    {
        public int Grupocodi { get; set; }
        public DateTime Cfgbarfecha { get; set; }
        public decimal? Cfgbarpse { get; set; }
        public decimal? Cfgbarfactorf { get; set; }
        public string Cfgbarusucreacion { get; set; }
        public DateTime Cfgbarfeccreacion { get; set; }
        public string Cfgbarusumodificacion { get; set; }
        public DateTime Cfgbarfecmodificacion { get; set; }

        //Adicionales
        public string Gruponomb { get; set; }

        public string StrCfgbarfecha { get; set; }
        public int Cfgbartiporeg { get; set; }
        
        //Temporales
        public bool Prnprocess { get; set; }
        public int Ptomedicodi { get; set; }
    }
}
