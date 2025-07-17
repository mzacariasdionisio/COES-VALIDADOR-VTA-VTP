using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_INTERRUPCION_INSUMO
    /// </summary>
    public partial class ReInterrupcionInsumoDTO : EntityBase
    {
        public int Reinincodi { get; set; } 
        public int? Repercodi { get; set; } 
        public int? Reinincorrelativo { get; set; } 
        public int? Repentcodi { get; set; } 
        public DateTime? Reininifecinicio { get; set; } 
        public DateTime? Reininfecfin { get; set; } 
        public DateTime? Reininprogifecinicio { get; set; } 
        public DateTime? Reininprogfecfin { get; set; } 
        public int? Retintcodi { get; set; } 
        public string Reninitipo { get; set; } 
        public string Reninicausa { get; set; } 
        public int? Recintcodi { get; set; } 
        public string Reinincodosi { get; set; } 
        public int? Reinincliente { get; set; } 
        public int? Reininsuministrador { get; set; } 
        public string Reininobservacion { get; set; } 
        public int? Reininresponsable1 { get; set; } 
        public decimal? Reininporcentaje1 { get; set; } 
        public int? Reininresponsable2 { get; set; } 
        public decimal? Reininporcentaje2 { get; set; } 
        public int? Reininresponsable3 { get; set; } 
        public decimal? Reininporcentaje3 { get; set; } 
        public int? Reininresponsable4 { get; set; } 
        public decimal? Reininporcentaje4 { get; set; } 
        public int? Reininresponsable5 { get; set; } 
        public decimal? Reininporcentaje5 { get; set; } 
        public string Reininusucreacion { get; set; } 
        public DateTime? Reininfeccreacion { get; set; } 
        public string Reininusumodificacion { get; set; } 
        public DateTime? Reininfecmodificacion { get; set; } 
    }
    public partial class ReInterrupcionInsumoDTO 
    {
        public DateTime? ReininifecinicioMinuto { get; set; }
        public DateTime? ReininfecfinMinuto { get; set; }
        public DateTime? ReininprogifecinicioMinuto { get; set; }
        public DateTime? ReininprogfecfinMinuto { get; set; }
        public string PuntoEntregaBarraNombre { get; set; }
        public string ReininifecinicioMinutoDesc { get; set; }
        public string Suministrador { get; set; }
        public string TipoNombre { get; set; }
        public string TiempoEjecutadoFin { get; set; }
        public int? InterrupcionTipoId { get; set; } //contraste
        public string Responsable1Nombre { get; set; }
        public decimal? Responsable1Porcentaje { get; set; }
        public string Responsable2Nombre { get; set; }
        public decimal? Responsable2Porcentaje { get; set; }
        public string Responsable3Nombre { get; set; }
        public decimal? Responsable3Porcentaje { get; set; }
        public string Responsable4Nombre { get; set; }
        public decimal? Responsable4Porcentaje { get; set; }
        public string Responsable5Nombre { get; set; }
        public string Observacion { get; set; }
        public decimal? Responsable5Porcentaje { get; set; }
    }
 }
