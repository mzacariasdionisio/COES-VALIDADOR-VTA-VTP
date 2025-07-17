using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_INFORME_ITEM
    /// </summary>
    public class EveInformeItemDTO : EntityBase
    {
        public int Infitemcodi { get; set; } 
        public int Eveninfcodi { get; set; } 
        public int Itemnumber { get; set; } 
        public int? Subitemnumber { get; set; } 
        public int? Nroitem { get; set; } 
        public decimal? Potactiva { get; set; } 
        public decimal? Potreactiva { get; set; } 
        public int? Equicodi { get; set; } 
        public decimal? Niveltension { get; set; } 
        public string Desobservacion { get; set; } 
        public string Itemhora { get; set; } 
        public string Senializacion { get; set; } 
        public int? Interrcodi { get; set; } 
        public string Ac { get; set; } 
        public int? Ra { get; set; } 
        public int? Sa { get; set; } 
        public int? Ta { get; set; } 
        public int? Rd { get; set; } 
        public int? Sd { get; set; } 
        public int? Td { get; set; } 
        public string Descomentario { get; set; } 
        public string Sumininistro { get; set; } 
        public decimal? Potenciamw { get; set; } 
        public string Proteccion { get; set; } 
        public DateTime? Intinicio { get; set; } 
        public DateTime? Intfin { get; set; }      
        public string Subestacionde { get; set; }
        public string Subestacionhasta { get; set; }
        public string Equinomb { get; set; }
        public string Areanomb { get; set; }        
        public string Internomb { get; set; }
        public decimal Duracion { get; set; }
        public string DesIntInicio { get; set; }
        public string DesIntFin { get; set; }
        public int Ptointerrcodi { get; set; }
    }
}
