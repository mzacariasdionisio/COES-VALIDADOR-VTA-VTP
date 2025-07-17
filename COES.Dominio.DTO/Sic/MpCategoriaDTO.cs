using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla MP_CATEGORIA
    /// </summary>
    public class MpCategoriaDTO : EntityBase
    {
        public int Mcatcodi { get; set; } 
        public string Mcatnomb { get; set; } 
        public string Mcatabrev { get; set; } 
        public int? Mcattipo { get; set; } 
        public string Mcatdesc { get; set; } 
    }
}
