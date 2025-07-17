using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CAI_SDDP_GENMARG
    /// </summary>
    public class CaiSddpGenmargDTO : EntityBase
    {
        public int Sddpgmcodi { get; set; }
        public string Sddpgmtipo { get; set; }
        public int Caiajcodi { get; set; }
        public int Sddpgmetapa { get; set; }
        public int Sddpgmserie { get; set; }
        public int Sddpgmbloque { get; set; }
        public string Sddpgmnombre { get; set; }
        public decimal Sddpgmenergia { get; set; }
        public DateTime? Sddpgmfecha { get; set; }
        public string Sddpgmusucreacion { get; set; }
        public DateTime Sddpgmfeccreacion { get; set; } 
    }


}
