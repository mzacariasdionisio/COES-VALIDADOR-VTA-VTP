using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_RESTRIC_TIPO
    /// </summary>
    public class PrRestricTipoDTO : EntityBase
    {
        public int Restipcodi { get; set; } 
        public string Restipnombre { get; set; } 
        public string Restipusucreacion { get; set; } 
        public DateTime? Restipfeccreacion { get; set; } 
    }
}
