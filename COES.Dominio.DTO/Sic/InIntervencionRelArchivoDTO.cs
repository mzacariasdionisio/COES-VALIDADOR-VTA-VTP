using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IN_INTERVENCION_REL_ARCHIVO
    /// </summary>
    public class InIntervencionRelArchivoDTO : EntityBase
    {
        public int Irarchcodi { get; set; }
        public int Intercodi { get; set; }
        public int Inarchcodi { get; set; }
    }
}
