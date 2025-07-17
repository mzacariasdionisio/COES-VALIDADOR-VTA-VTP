using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_PTORELACION
    /// </summary>
    public class MePtorelacionDTO : EntityBase
    {
        public int Ptorelcodi { get; set; }
        public int Equicodi { get; set; }
        public int Ptorelpunto1 { get; set; }
        public int Ptorelpunto2 { get; set; }
        public string Ptoreltipo { get; set; }
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
        public int? Centralcodi { get; set; }
        public string Centralnomb { get; set; }
        public int? Ptomedicodi { get; set; }
        public int? Origlectcodi { get; set; }
        public int? Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public string Equinomb { get; set; }
        public string Equiabrev { get; set; }
        public int Seleccion { get; set; }
    }
}
