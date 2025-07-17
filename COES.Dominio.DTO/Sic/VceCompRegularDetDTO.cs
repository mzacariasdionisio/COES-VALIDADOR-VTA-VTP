using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class VceCompRegularDetDTO
    {
        public string Crdettipocalc { get; set; }
        public decimal? Crdetcvtbajaefic { get; set; }
        public decimal? Crdetcompensacion { get; set; }
        public decimal? Crdetcmg { get; set; }
        public decimal? Crdetcvt { get; set; }
        public int? Subcausacodi { get; set; }
        public decimal? Crdetvalor { get; set; }
        public DateTime Crdethora { get; set; }
        public int Grupocodi { get; set; }
        public int PecaCodi { get; set; } 

        //Adicionales
        public string Emprnomb { get; set; }
        public string Gruponomb { get; set; }
    }
}
