using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PMO_LOG
    /// </summary>
    public partial class PmoLogDTO : EntityBase
    {
        public int Pmologcodi { get; set; }
        public int Enviocodi { get; set; }
        public int Logcodi { get; set; }
        public int Pmologtipo { get; set; }
        public int? Pmftabcodi { get; set; }
    }

    public partial class PmoLogDTO
    {
        public string LogDesc { get; set; }
        public DateTime LogFecha { get; set; }
        public string LogFechaDesc { get; set; }
        public string LogUser { get; set; }
        public string FechaDesc { get; set; }
        public string HoraDesc { get; set; }
        public string PmologtipoDesc { get; set; }
    }
}
