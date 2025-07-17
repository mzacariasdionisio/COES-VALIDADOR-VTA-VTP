using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla ST_RESPAGO
    /// </summary>
    public class StRespagoDTO : EntityBase
    {
        public int Respagcodi { get; set; } 
        public int Strecacodi { get; set; } 
        //public int Sistrncodi { get; set; } 
        public int Stcntgcodi { get; set; } 
        public string Respagusucreacion { get; set; } 
        public DateTime Respagfeccreacion { get; set; }
        //variables para consultas
        public string Sistrnnombre { get; set; }
        public string Equinomb { get; set; }
        public string Stcompcodelemento { get; set; }
 
    }
}
