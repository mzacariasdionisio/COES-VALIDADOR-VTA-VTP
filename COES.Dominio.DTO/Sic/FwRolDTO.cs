using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FW_ROL
    /// </summary>
    public class FwRolDTO : EntityBase
    {
        public int Rolcode { get; set; } 
        public string Rolname { get; set; } 
        public int? Rolvalidate { get; set; } 
        public int? Rolcheck { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
    }
}
