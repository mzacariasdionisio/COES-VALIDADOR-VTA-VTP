using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_INSUMO_VTP
    /// </summary>
    public class RerInsumoVtpDTO : EntityBase
    {
        public int Rerinpcodi { get; set; }
        public int Rerinscodi { get; set; }
        public int Rerpprcodi { get; set; }
        public int Emprcodi { get; set; }
        public int Equicodi { get; set; }
        public int Rerinpanio { get; set; }
        public int Rerinpmes { get; set; }
        public int Pericodi { get; set; }
        public int Recpotcodi { get; set; }
        public decimal Rerinpmestotal { get; set; }
        public string Rerinpmesusucreacion { get; set; }
        public DateTime Rerinpmesfeccreacion { get; set; }

        //Additional
        public string Perinombre { get; set; }
        public string Recpotnombre { get; set; }
    }
}

