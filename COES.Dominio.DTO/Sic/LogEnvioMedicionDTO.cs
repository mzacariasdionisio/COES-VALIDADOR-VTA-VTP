using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class LogEnvioMedicionDTO
    {
        public int LogCodi { get; set; }
        public int EmprCodi { get; set; }
        public string LastUser { get; set; }
        public DateTime LastDate { get; set; }
        public string FilNomb { get; set; }
        public string LogDesc { get; set; }
        public int PtoMediCodi { get; set; }
        public DateTime Fecha { get; set; }
    }
}

