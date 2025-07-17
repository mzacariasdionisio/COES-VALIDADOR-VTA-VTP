using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PMO_SDDP_TIPO
    /// </summary>
    public class PmoSddpTipoDTO : EntityBase
    {
        public int Tsddpcodi { get; set; }
        public string Tsddpnomb { get; set; }
        public string Tsddpabrev { get; set; }
    }
}
