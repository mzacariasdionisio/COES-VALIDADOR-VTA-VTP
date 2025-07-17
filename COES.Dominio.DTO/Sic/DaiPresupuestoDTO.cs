using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla DAI_PRESUPUESTO
    /// </summary>
    public class DaiPresupuestoDTO : EntityBase
    {
        public int Prescodi { get; set; } 
        public string Presanio { get; set; } 
        public decimal? Presmonto { get; set; } 
        public int? Presamortizacion { get; set; } 
        public decimal? Presinteres { get; set; } 
        public string Presactivo { get; set; } 
        public string Presusucreacion { get; set; } 
        public DateTime? Presfeccreacion { get; set; } 
        public string Presusumodificacion { get; set; } 
        public DateTime? Presfecmodificacion { get; set; } 

        public int Tieneaportantes { get; set; }
        public string monto { get; set; }
        public string presprocesada { get; set; }
    }
}
