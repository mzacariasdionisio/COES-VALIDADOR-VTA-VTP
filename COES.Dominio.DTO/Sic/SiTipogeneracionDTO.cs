using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_TIPOGENERACION
    /// </summary>
    public class SiTipogeneracionDTO : EntityBase
    {
        public int Tgenercodi { get; set; } 
        public string Tgenerabrev { get; set; } 
        public string Tgenernomb { get; set; } 
        public string Tgenercolor { get; set; }

        public string ColorLetra { get; set; }
        public int Orden { get; set; }
    }
}

