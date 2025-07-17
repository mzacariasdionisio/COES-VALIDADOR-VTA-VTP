using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_TIPOGRUPO
    /// </summary>
    public class PrTipogrupoDTO : EntityBase
    {
        public int Tipogrupocodi { get; set; } 
        public string Tipogruponomb { get; set; } 
        public string Tipogrupoabrev { get; set; } 
    }
}

