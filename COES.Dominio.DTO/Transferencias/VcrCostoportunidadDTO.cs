using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_COSTOPORTUNIDAD
    /// </summary>
    public class VcrCostoportunidadDTO : EntityBase
    {
        public int Vcrecacodi { get; set; } 
        public int Grupocodi { get; set; } 
        public string Gruponomb { get; set; } 
        public DateTime Vcrcopfecha { get; set; } 
        public decimal Vcrcopcosto { get; set; } 
        public string Vcrcopusucreacion { get; set; } 
        public DateTime Vcrcopfeccreacion { get; set; } 
        public int Vcrcopcodi { get; set; } 
    }
}
