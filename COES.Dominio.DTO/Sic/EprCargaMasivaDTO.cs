using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EPR_CARGA_MASIVA
    /// </summary>
    public class EprCargaMasivaDTO : EntityBase
    {
        public int Epcamacodi { get; set; } 
        public int Epcamatipuso { get; set; }
        public string Epcamatipusonombre { get; set; }
        public DateTime Epcamafeccarga { get; set; }        
        public string Epcamausucreacion { get; set; } 
        public DateTime Epcamafeccreacion { get; set; } 
        public string Epcamausumodificacion { get; set; } 
        public DateTime? Epcamafecmodificacion { get; set; }     

        public int Epcamatotalregistro { get; set; }
    }
}
