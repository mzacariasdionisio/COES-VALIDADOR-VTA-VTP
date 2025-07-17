using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CB_REP_PROPIEDAD
    /// </summary>
    public class CbRepPropiedadDTO : EntityBase
    {
        public int Cbrprocodi { get; set; } 
        public string Cbrpronombre { get; set; } 
        public string Cbrprovalor { get; set; } 
    }
}
