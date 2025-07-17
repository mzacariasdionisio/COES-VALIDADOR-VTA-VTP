using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PFR_CONCEPTO
    /// </summary>
    public class PfrConceptoDTO : EntityBase
    {
        public int Pfrcnpcodi { get; set; }
        public string Pfrcnpnomb { get; set; }
    }
}
