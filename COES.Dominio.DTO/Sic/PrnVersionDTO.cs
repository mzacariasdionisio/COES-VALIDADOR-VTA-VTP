using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class PrnVersionDTO
    {
        public int Prnvercodi { get; set; }
        public string Prnvernomb { get; set; }
        public string Prnverestado { get; set; }
        public string Prnverusucreacion { get; set; }
        public DateTime Prnverfeccreacion { get; set; }
        public string Prnverusumodificacion { get; set; }
        public DateTime Prnverfecmodificacion { get; set; }

        #region Adicionales

        public int Prnredbarracp { get; set; }
        public int Prnredbarrapm { get; set; }
        public decimal Prnredgauss { get; set; }
        public decimal Prnredperdida { get; set; }
        public int Ptomedicodi { get; set; }
        public int Origlectcodi { get; set; }
        public string Gruponomb { get; set; }
        public string Prnredtipo { get; set; }
        #endregion
    }
}
