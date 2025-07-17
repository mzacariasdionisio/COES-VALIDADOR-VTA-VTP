using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class PrnPtomedpropDTO : EntityBase
    {
        public int Ptomedicodi { get; set; }
        public string Prnpmpvarexoproceso { get; set; }
        public string Prnpmpusucreacion { get; set; }
        public DateTime Prnpmpfeccreacion { get; set; }
        public string Prnpmpusumodificacion { get; set; }
        public DateTime Prnpmpfecmodificacion { get; set; }
    }
}
