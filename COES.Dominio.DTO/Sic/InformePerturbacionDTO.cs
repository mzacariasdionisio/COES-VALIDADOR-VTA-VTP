using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class InformePerturbacionDTO
    {
        public int PERTURBACIONCODI { get; set; }
        public int EVENCODI { get; set; }
        public int? SUBCAUSACODI { get; set; }
        public string ITEMTIPO { get; set; }
        public string ITEMTIME { get; set; }
        public string ITEMDESCRIPCION { get; set; }
        public int? EQUICODI { get; set; }
        public int? INTERRUPTORCODI { get; set; }
        public string ITEMSENALIZACION { get; set; }
        public string ITEMAC { get; set; }
        public decimal? ITEMORDEN { get; set; }
        public string SUBCAUSADESC { get; set; }
        public string EQUINOMB { get; set; }
        public string SUBESTACION { get; set; }
        public string INTERRUPTORNOMB { get; set; }
        public DateTime LASTDATE { get; set; }
        public string LASTUSER { get; set; }
    }

}
