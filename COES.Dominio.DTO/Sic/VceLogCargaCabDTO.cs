using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla VCE_LOG_CARGA_CAB
    /// </summary>
    public class VceLogCargaCabDTO : EntityBase
    {
        public int Crlccorden { get; set; } 
        public string Crlccentidad { get; set; } 
        public string Crlccnombtabla { get; set; } 
        public int PecaCodi { get; set; } 
        public int Crlcccodi { get; set; } 
    }
}
