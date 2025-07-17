using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_TERMSUPERAVIT
    /// </summary>
    public class VcrTermsuperavitDTO : EntityBase
    {
        public int Vcrtscodi { get; set; } 
        public int Vcrecacodi { get; set; } 
        public int Grupocodi { get; set; } 
        public string Gruponomb { get; set; } 
        public DateTime Vcrtsfecha { get; set; }
        public decimal Vcrtsmpa { get; set; }
        public decimal Vcrtsdefmwe { get; set; }
        public decimal Vcrtssupmwe { get; set; }
        public decimal Vcrtsrnsmwe { get; set; }
        public decimal Vcrtsdeficit { get; set; }
        public decimal Vcrtssuperavit { get; set; }
        public decimal Vcrtsresrvnosumn { get; set; }
        public string Vcrtsusucreacion { get; set; } 
        public DateTime Vcrtsfeccreacion { get; set; } 
    }
}
