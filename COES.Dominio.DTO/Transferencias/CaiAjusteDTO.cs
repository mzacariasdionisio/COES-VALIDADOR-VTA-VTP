using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CAI_AJUSTE
    /// </summary>
    public class CaiAjusteDTO : EntityBase
    {
        public int Caiajcodi { get; set; } 
        public int Caiprscodi { get; set; } 
        public int Caiajanio { get; set; } 
        public int Caiajmes { get; set; } 
        public string Caiajnombre { get; set; } 
        public string Caiajusucreacion { get; set; } 
        public DateTime Caiajfeccreacion { get; set; } 
        public string Caiajusumodificacion { get; set; } 
        public DateTime Caiajfecmodificacion { get; set; } 
    }
}
