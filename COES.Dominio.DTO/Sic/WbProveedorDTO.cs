using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_PROVEEDOR
    /// </summary>
    public class WbProveedorDTO : EntityBase
    {
        public int Provcodi { get; set; } 
        public string Provnombre { get; set; } 
        public string Provtipo { get; set; } 
        public DateTime? Provfechainscripcion { get; set; } 
    }
}
