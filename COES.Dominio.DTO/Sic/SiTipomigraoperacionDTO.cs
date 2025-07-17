using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_TIPOMIGRAOPERACION
    /// </summary>
    public class SiTipomigraoperacionDTO : EntityBase
    {
        public int Tmopercodi { get; set; }
        public string Tmoperdescripcion { get; set; } 
        public string Tmoperusucreacion { get; set; }

        
        public DateTime? Tmoperfeccreacion { get; set; } 



    }
}
