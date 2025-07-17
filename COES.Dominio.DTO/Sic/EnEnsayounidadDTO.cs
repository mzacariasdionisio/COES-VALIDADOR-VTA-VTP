using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EN_ENSAYOUNIDAD
    /// </summary>
    public class EnEnsayounidadDTO : EntityBase
    {
        public int Enunidadcodi { get; set; }
        public int Ensayocodi { get; set; }
        public int Equicodi { get; set; }
    }
}
