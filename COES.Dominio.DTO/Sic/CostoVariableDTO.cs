using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ABI_POTEFEC
    /// </summary>
    public class CostoVariableDTO : EntityBase
    {
        public string MODO_OPERACION { get; set; }
        public string ESCENARIO { get; set; }
        public decimal? PE { get; set; }
        public decimal? EFICBTUKWH { get; set; }
        public decimal? EFICTERM { get; set; }
        public decimal? CCOMB { get; set; }
        public decimal? CVC { get; set; }
        public decimal? CVNC { get; set; }
    }
}
