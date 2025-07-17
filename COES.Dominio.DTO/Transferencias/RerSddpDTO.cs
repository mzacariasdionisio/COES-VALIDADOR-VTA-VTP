using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_SDDP
    /// </summary>
    public class RerSddpDTO : EntityBase
    {
        public int Resddpcodi { get; set; }
        public int Reravcodi { get; set; }
        public string Resddpnomarchivo { get; set; }
        public int Resddpsemanaini { get; set; }
        public int Resddpanioini { get; set; }
        public int Resddpnroseries { get; set; }
        public DateTime Resddpdiainicio { get; set; }
        public string Resddpusucreacion { get; set; }
        public DateTime Resddpfeccreacion { get; set; }
    }
}

