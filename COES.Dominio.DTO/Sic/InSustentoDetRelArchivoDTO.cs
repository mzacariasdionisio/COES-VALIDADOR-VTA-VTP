using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IN_SUSTENTO_DET_REL_ARCHIVO
    /// </summary>
    public class InSustentoDetRelArchivoDTO : EntityBase
    {
        public int Instdcodi { get; set; }
        public int Inarchcodi { get; set; }
        public int Isdarcodi { get; set; }
    }
}
