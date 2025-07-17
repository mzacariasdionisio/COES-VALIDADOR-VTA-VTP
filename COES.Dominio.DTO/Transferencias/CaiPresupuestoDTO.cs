using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CAI_PRESUPUESTO
    /// </summary>
    public class CaiPresupuestoDTO : EntityBase
    {
        public int Caiprscodi { get; set; } 
        public int Caiprsanio { get; set; } 
        public int Caiprsmesinicio { get; set; } 
        public int Caiprsnromeses { get; set; } 
        public string Caiprsnombre { get; set; } 
        public string Caiprsusucreacion { get; set; } 
        public DateTime Caiprsfeccreacion { get; set; } 
        public string Caiprsusumodificacion { get; set; } 
        public DateTime Caiprsfecmodificacion { get; set; } 
    }
}
