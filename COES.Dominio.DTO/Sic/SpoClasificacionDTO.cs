using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SPO_CLASIFICACION
    /// </summary>
    public class SpoClasificacionDTO : EntityBase
    {
        public int Clasicodi { get; set; } 
        public string Clasinombre { get; set; } 
    }
}
