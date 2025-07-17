using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla ST_ELEMENTO_COMPENSADO
    /// </summary>
    public class StElementoCompensadoDTO : EntityBase
    {
        public int Elecmpcodi { get; set; } 
        public int Strecacodi { get; set; }
        public int Stfactcodi { get; set; } 
        public int Stcompcodi { get; set; } 
        public decimal Elecmpmonto { get; set; } 
        public string Elecmpusucreacion { get; set; } 
        public DateTime Elecmpfeccreacion { get; set; } 
    }
}
