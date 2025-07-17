using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FW_USERROL
    /// </summary>
    public class FwUserrolDTO : EntityBase
    {
        public int Usercode { get; set; } 
        public int Rolcode { get; set; } 
        public int? Userrolvalidate { get; set; } 
        public int? Userrolcheck { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
    }
}
