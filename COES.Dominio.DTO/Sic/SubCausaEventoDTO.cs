using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class SubCausaEventoDTO
    {
        public short SUBCAUSACODI { get; set; }

        public int SUBCAUSACODI_ { get; set; }
        public Nullable<short> CAUSAEVENCODI { get; set; }
        public string SUBCAUSADESC { get; set; }
        public string SUBCAUSAABREV { get; set; }    
    }
}

