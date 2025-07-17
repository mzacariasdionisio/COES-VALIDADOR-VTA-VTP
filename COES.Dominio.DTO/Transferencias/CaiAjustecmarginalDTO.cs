using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CAI_AJUSTECMARGINAL
    /// </summary>
    public class CaiAjustecmarginalDTO : EntityBase
    {
        public int Caajcmcodi { get; set; } 
        public int Caiajcodi { get; set; } 
        public int Pericodi { get; set; } 
        public int Recacodi { get; set; } 
        public int Caajcmmes { get; set; } 
        public string Caajcmusucreacion { get; set; } 
        public DateTime Caajcmfeccreacion { get; set; } 
    }
}
