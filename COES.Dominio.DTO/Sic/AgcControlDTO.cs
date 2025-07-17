using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AGC_CONTROL
    /// </summary>
    public class AgcControlDTO : EntityBase
    {
        public int Agcccodi { get; set; }
        public string Agcctipo { get; set; }
        public string Agccdescrip { get; set; }
        public int? Ptomedicodi { get; set; }
        public string Agccb2 { get; set; }
        public string Agccb3 { get; set; }
        public string Agccvalido { get; set; }
        public string Agccusucreacion { get; set; }
        public DateTime? Agccfeccreacion { get; set; }
        public string Agccusumodificacion { get; set; }
        public DateTime? Agccfecmodificacion { get; set; }
        public string Ptomedibarranomb { get; set; }


        public string Ptomedielenomb { get; set; }
        public string Ptomedidesc { get; set; }




    }
}
