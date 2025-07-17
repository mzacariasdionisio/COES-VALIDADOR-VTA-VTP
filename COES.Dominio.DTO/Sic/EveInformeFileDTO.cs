using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_INFORME_FILE
    /// </summary>
    public class EveInformeFileDTO : EntityBase
    {
        public int Eveninfcodi { get; set; } 
        public int Eveninffilecodi { get; set; } 
        public string Desfile { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
        public string Extfile { get; set; }
        public string FileName { get; set; }
    }
}
