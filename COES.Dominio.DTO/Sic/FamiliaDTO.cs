using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class FamiliaDTO
    {
        public short FAMCODI { get; set; }
        public string FAMABREV { get; set; }
        public Nullable<short> TIPOECODI { get; set; }
        public Nullable<short> TAREACODI { get; set; }
        public string FAMNOMB { get; set; }
        public Nullable<short> FAMNUMCONEC { get; set; }
        public string FAMNOMBGRAF { get; set; }
    }
}

