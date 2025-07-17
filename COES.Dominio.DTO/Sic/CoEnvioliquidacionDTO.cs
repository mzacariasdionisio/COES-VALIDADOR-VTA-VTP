using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CO_ENVIOLIQUIDACION
    /// </summary>
    public class CoEnvioliquidacionDTO : EntityBase
    {
        public int Coenlicodi { get; set; } 
        public DateTime? Coenlifecha { get; set; } 
        public string Coenliusuario { get; set; } 
        public int? Pericodi { get; set; } 
        public int? Vcrecacodi { get; set; } 
        public int? Covercodi { get; set; } 
        public int? Copercodi { get; set; } 
        public string Periodonomb { get; set; }
        public string Versionnomb { get; set; }
        public string Periododesc { get; set; }
        public string Versiondesc { get; set; }

    }
}
