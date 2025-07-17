using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_PTOENTREGA
    /// </summary>
    public class EvePtoentregaDTO : EntityBase
    {
        public int Clientecodi { get; set; } 
        public int Ptoentregacodi { get; set; } 
        public string Ptoentrenomb { get; set; } 
        public int Equicodi { get; set; } 
    }
}

