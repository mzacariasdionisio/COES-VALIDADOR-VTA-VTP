using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class AreaDTO
    {
        public short AREACODI { get; set; }
        public string AREANOMB { get; set; }
        public string AREAABREV { get; set; }
        public string LASTUSER { get; set; }
        public Nullable<short> EMPRCODI { get; set; }
        public string TAREAABREV { get; set; }
    }
}

