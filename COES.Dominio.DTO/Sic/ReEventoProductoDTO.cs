using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_EVENTO_PRODUCTO
    /// </summary>
    public class ReEventoProductoDTO : EntityBase
    {
        public int Reevprcodi { get; set; } 
        public int? Reevpranio { get; set; } 
        public int? Reevprmes { get; set; } 
        public DateTime? Reevprfecinicio { get; set; } 
        public DateTime? Reevprfecfin { get; set; } 
        public string Reevprptoentrega { get; set; } 
        public decimal? Reevprtension { get; set; } 
        public int? Reevprempr1 { get; set; } 
        public int? Reevprempr2 { get; set; } 
        public int? Reevprempr3 { get; set; } 
        public decimal? Reevprporc1 { get; set; } 
        public decimal? Reevprporc2 { get; set; } 
        public decimal? Reevprporc3 { get; set; } 
        public string Reevprcomentario { get; set; } 
        public string Reevpracceso { get; set; }
        public string Reevprestado { get; set; } 
        public string Reevprusucreacion { get; set; } 
        public DateTime? Reevprfeccreacion { get; set; } 
        public string Reevprusumodificacion { get; set; } 
        public DateTime? Reevprfecmodificacion { get; set; } 
        public bool Indicadorpadre { get; set; }
        public int Rowspan { get; set; }
        public string Responsablenomb1 { get; set; }
        public string Responsablenomb2 { get; set; }
        public string Responsablenomb3 { get; set; }
        public string Suministrador { get; set; }
        public string Acceso { get; set; }
        public string Estadocarga { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public string Empresas { get; set; }
        public string Nombremes { get; set; }

    }
}
