using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_CATEGORIA
    /// </summary>
    public class PrCategoriaDTO : EntityBase
    {
        public int Catecodi { get; set; } 
        public string Cateabrev { get; set; } 
        public string Catenomb { get; set; } 
        public string Catetipo { get; set; } 
    }
}
