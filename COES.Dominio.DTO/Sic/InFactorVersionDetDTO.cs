using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IN_FACTOR_VERSION_DET
    /// </summary>
    public class InFactorVersionDetDTO : EntityBase
    {
        public int Infvdtcodi { get; set; }
        public string Infvdtintercodis { get; set; }
        public string Infvdthorizonte { get; set; }
        public int Infvercodi { get; set; }
    }
}
