using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class PrnReduccionRedDTO
    {
        public int Prnredcodi { get; set; }
        public int Prnvercodi { get; set; }
        public int Prnredbarracp { get; set; }
        public int Prnredbarrapm { get; set; }
        public decimal? Prnredgauss {get; set; }
        public decimal? Prnredperdida { get; set; }
        public DateTime Prnredfecha { get; set; }
        public string Prnredusucreacion { get; set; }
        public DateTime Prnredfeccreacion { get; set; }
        public string Prnredusumodificacion { get; set; }
        public DateTime Prnredfecmodificacion { get; set; }
        public string Prnrednombre { get; set; }
        public string Prnredtipo { get; set; }

        //Adicionales
        public string Nombrecp { get; set; }
        public string Nombrepm { get; set; }
        public int Ptomedicodi { get; set; }
        public int Grupocodibarra { get; set; }
        public string Ptomedidesc { get; set; }
        public string Nombre { get; set; }
        public string Ptomedibarranomb { get; set; }
        
    }
}
