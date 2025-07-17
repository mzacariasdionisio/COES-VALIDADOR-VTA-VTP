using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AUD_REQUERIMIENTO_INFORM
    /// </summary>
    public class AudRequerimientoInfoArchivoDTO : EntityBase
    {
        public int Reqicodiarch { get; set; }
        public int Reqicodi { get; set; }
        public int Archcodi { get; set; }
    }
}
