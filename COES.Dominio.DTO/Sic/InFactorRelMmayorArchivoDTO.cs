using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IN_FACTOR_REL_MMAYOR_ARCHIVO
    /// </summary>
    public partial class InFactorRelMmayorArchivoDTO : EntityBase
    {
        public int Irmarcodi { get; set; } 
        public int Infmmcodi { get; set; } 
        public int Inarchcodi { get; set; } 
    }

    public partial class InFactorRelMmayorArchivoDTO
    {
        public int Infvercodi { get; set; }
        public InArchivoDTO Archivo { get; set; }
    }
}
