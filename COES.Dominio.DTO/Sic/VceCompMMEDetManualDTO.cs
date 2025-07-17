using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class VceCompMMEDetManualDetDTO
    {
        public string Cmmedmtipocalc { get; set; }
        //public decimal? Crdetcvtbajaefic { get; set; }
        public decimal? Cmmedmcompensacion { get; set; }
        public decimal? Cmmedmcmg { get; set; }
        public decimal? Cmmedmcvt { get; set; }
        public int? Subcausacodi { get; set; }
        //public decimal? Crdetvalor { get; set; }
        public DateTime Cmmedmhora { get; set; }
        public int Grupocodi { get; set; }
        public int PecaCodi { get; set; } 

        //Adicionales
        public string Emprnomb { get; set; }
        public string Gruponomb { get; set; }
        public string Subcausadesc { get; set; }

        public decimal? Cmmedmenergia { get; set; }
        public decimal? Cmmedmpotencia { get; set; }
        public decimal? Cmmedmconsumocomb { get; set; }

        public decimal? Cmmedmprecioaplic { get; set; }
        public decimal? Cmmedmcvc { get; set; }
        public decimal? Cmmedmcvnc { get; set; }
        public decimal? Cmmedmlhv { get; set; }
    }
}
