using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_REDUCCPAGOEJE
    /// </summary>
    public class VcrReduccpagoejeDTO : EntityBase
    {
        public int Vcrpecodi { get; set; } 
        public int Vcrecacodi { get; set; } 
        public int Equicodi { get; set; } 
        public decimal Vcrpecumplmes { get; set; } 
        public decimal Vcrpereduccpagomax { get; set; } 
        public decimal Vcrpereduccpagoeje { get; set; } 
        public string Vcrpeusucreacion { get; set; } 
        public DateTime Vcrpefeccreacion { get; set; } 
    }
}
