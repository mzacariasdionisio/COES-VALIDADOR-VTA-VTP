using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_VERSION_CONCEPTO
    /// </summary>
    public class SiVersionConceptoDTO : EntityBase
    {
        public int Vercnpcodi { get; set; }
        public string Vercnpdesc { get; set; }
        public string Vercnptipo { get; set; }
    }
}
