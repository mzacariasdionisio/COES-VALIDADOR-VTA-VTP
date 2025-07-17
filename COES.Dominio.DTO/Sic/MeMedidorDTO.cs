using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_MEDIDOR
    /// </summary>
    public class MeMedidorDTO : EntityBase
    {
        public int Medicodi { get; set; } 
        public string Medinombre { get; set; } 
        public string Medimarca { get; set; } 
        public string Medimodelo { get; set; } 
        public string Medinserie { get; set; } 
        public string Medicprecision { get; set; } 
        public int? Ptomedicodi { get; set; } 
        public string Meditipo { get; set; } 
    }
}
