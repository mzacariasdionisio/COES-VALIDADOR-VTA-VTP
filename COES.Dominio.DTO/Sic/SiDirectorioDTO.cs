using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_EMPRESA
    /// </summary>
    public partial class SiDirectorioDTO : EntityBase
    {
        public int DirectorioCodigo { get; set; } 
        public string DirectorioNombre { get; set; } 
        public string Dircorreo { get; set; }
        public string iDirestado { get; set; }
    }
}
