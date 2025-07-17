using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AGC_CONTROL_PUNTO
    /// </summary>
    public class AgcControlPuntoDTO : EntityBase
    {
        public int Agccpcodi { get; set; }
        public int? Agcccodi { get; set; }
        public int? Ptomedicodi { get; set; }
        public int? Equicodi { get; set; }
        public string Agccpb2 { get; set; }
        public string Agccpb3 { get; set; }
        public string Agccpusucreacion { get; set; }
        public DateTime? Agccpfeccreacion { get; set; }
        public string Agccpusumodificacion { get; set; }
        public DateTime? Agccpfecmodificacion { get; set; }
        public string Agcctipo { get; set; }
        public string Ptomedibarranomb { get; set; }
        public string Equiabrev { get; set; }
    }
}
