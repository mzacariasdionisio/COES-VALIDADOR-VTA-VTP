using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_FACTORCONVERSION
    /// </summary>
    public partial class SiFactorconversionDTO : EntityBase
    {
        public int Tconvcodi { get; set; }
        public int Tinforigen { get; set; }
        public int Tinfdestino { get; set; }
        public decimal Tconvfactor { get; set; }
    }

    public partial class SiFactorconversionDTO
    {
        public string Tinforigenabrev { get; set; }
        public string Tinfdestinoabrev { get; set; }
        public string Tinforigendesc { get; set; }
        public string Tinfdestinodesc { get; set; }
    }
}
