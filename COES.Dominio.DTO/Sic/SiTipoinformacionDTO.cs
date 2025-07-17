using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_TIPOINFORMACION
    /// </summary>
    public class SiTipoinformacionDTO : EntityBase
    {
        public string Tipoinfoabrev { get; set; } 
        public string Tipoinfodesc { get; set; } 
        public int Tipoinfocodi { get; set; }
        public string Tinfcanalunidad { get; set; }

        public string Canalunidad { get; set; }
    }
}
