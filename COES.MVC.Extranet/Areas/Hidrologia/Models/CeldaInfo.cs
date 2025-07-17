using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Hidrologia.Models
{
    public class CeldaInfo
    {
        public string Celda { get; set; }
        public int PtoMedicion { get; set; }
        public int TipoInfo { get; set; }
        public string Tipoinfoabrev { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoObservacion { get; set; }
        public string Valor { get; set; }
        public string Central { get; set; }
        public string Grupo { get; set; }
        public bool EsNumero { get; set; }
    }
}