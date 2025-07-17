using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_CENTRAL_LVTP
    /// </summary>
    public class RerCentralLvtpDTO : EntityBase
    {
        public int Rerctpcodi { get; set; }
        public int Rercencodi { get; set; }
        public int Equicodi { get; set; }
        public string Rerctpusucreacion { get; set; }
        public DateTime Rerctpfeccreacion { get; set; }

        public string Equinomb { get; set; }
    }
}