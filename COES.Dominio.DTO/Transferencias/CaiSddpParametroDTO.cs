using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CAI_SDDP_PARAMETRO
    /// </summary>

    public class CaiSddpParametroDTO : EntityBase
    {
        public int Sddppmcodi { get; set; }
        public int Caiajcodi { get; set; }
        public decimal Sddppmtc { get; set; }
        public int Sddppmsemini { get; set; }
        public int Sddppmnumsem { get; set; }
        public int Sddppmcantbloque { get; set; }
        public int Sddppmnumserie { get; set; }
        public string Sddppmusucreacion { get; set; }
        public DateTime Sddppmfeccreacion { get; set; }
    }
}
