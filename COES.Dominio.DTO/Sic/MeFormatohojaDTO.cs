using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_FORMATOHOJA
    /// </summary>
    public class MeFormatohojaDTO : EntityBase
    {
        public DateTime? Lastdate { get; set; } 
        public string Lastuser { get; set; } 
        public int? Lectcodi { get; set; } 
        public string Hojatitulo { get; set; } 
        public int Hojanumero { get; set; } 
        public int Formatcodi { get; set; } 
    }
}
