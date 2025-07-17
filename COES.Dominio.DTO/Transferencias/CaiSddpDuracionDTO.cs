using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CAI_SDDP_DURACION
    /// </summary>
    public class CaiSddpDuracionDTO : EntityBase
    {
        public int Sddpducodi { get; set; }
        public int Caiajcodi { get; set; }
        public int Sddpduetapa { get; set; }
        public int Sddpduserie { get; set; }
        public int Sddpdubloque { get; set; }
        public decimal Sddpduduracion { get; set; }
        public string Sddpduusucreacion { get; set; }
        public DateTime Sddpdufeccreacion { get; set; } 
    }
}
