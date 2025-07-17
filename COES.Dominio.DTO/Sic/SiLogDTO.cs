using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class SiLogDTO : EntityBase
    {
        public int LogCodi { get; set; }
        public int ModCodi { get; set; }
        public string LogDesc { get; set; }
        public DateTime? LogFecha { get; set; }
        public string LogUser { get; set; }
        public DateTime? LogFechaMod { get; set; }
        public string LogUserMod { get; set; }

        public int? Logmigtipo { get; set; }
        public string LogmigtipoDesc { get; set; }
        public string Miqubamensaje { get; set; }
        public int Miqubaflag { get; set; }
        public string FechaDesc { get; set; }
        public string HoraDesc { get; set; }
        public int? Miqubacodi { get; set; }
    }
}
