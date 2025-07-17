using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_HTRABAJO_ESTADO
    /// </summary>
    public class PrHtrabajoEstadoDTO : EntityBase
    {
        public int Htestcodi { get; set; } 
        public string Htestcolor { get; set; } 
        public string Htestdesc { get; set; } 
    }
}
