using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IN_MENSAJE_REL_ARCHIVO
    /// </summary>
    public class InMensajeRelArchivoDTO : EntityBase
    {
        public int Msgcodi { get; set; }
        public int Inarchcodi { get; set; }
        public int Irmearcodi { get; set; }
    }
}
