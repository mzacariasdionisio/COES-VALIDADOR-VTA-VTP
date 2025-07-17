using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_MAILS
    /// </summary>
    public class EveMailsDTO : EntityBase
    {
        public int Mailcodi { get; set; } 
        public int? Mailturnonum { get; set; } 
        public string Mailreprogcausa { get; set; } 
        public string Mailcheck1 { get; set; } 
        public string Mailhoja { get; set; } 
        public string Mailprogramador { get; set; } 
        public int? Mailbloquehorario { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
        public DateTime Mailfecha { get; set; } 
        public string Mailcheck2 { get; set; } 
        public string Mailemitido { get; set; } 
        public int? Equicodi { get; set; } 
        public DateTime? Mailfechaini { get; set; } 
        public DateTime? Mailfechafin { get; set; } 
        public string Lastuserproc { get; set; } 
        public string Mailespecialista { get; set; } 
        public int? Mailtipoprograma { get; set; } 
        public int? Topcodi { get; set; }
        public int? Subcausacodi { get; set; }
        public string Subcausadesc { get; set; }
        public string T { get; set; }
        public string CoordinadorTurno { get; set; }

        public DateTime? Mailhora { get; set; }
        public string Mailconsecuencia;

        public string Hora { get; set; }
        public string Reprograma { get; set; }        
    }
}
