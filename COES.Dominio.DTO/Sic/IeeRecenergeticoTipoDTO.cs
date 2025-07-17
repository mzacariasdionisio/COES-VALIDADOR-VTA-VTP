using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IEE_RECENERGETICO_TIPO
    /// </summary>
    public class IeeRecenergeticoTipoDTO : EntityBase
    {
        public int Renertipcodi { get; set; } 
        public string Renerabrev { get; set; } 
        public string Renertipnomb { get; set; } 
    }
}
