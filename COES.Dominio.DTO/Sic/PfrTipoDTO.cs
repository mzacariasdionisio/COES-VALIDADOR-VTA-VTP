using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PFR_TIPO
    /// </summary>
    public class PfrTipoDTO : EntityBase
    {
        public int Pfrcatcodi { get; set; }
        public string Pfrcatnomb { get; set; }
    }
}
