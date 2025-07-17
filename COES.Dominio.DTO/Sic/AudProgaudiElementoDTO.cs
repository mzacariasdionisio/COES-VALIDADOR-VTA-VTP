using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AUD_PROGAUDI_ELEMENTO
    /// </summary>
    public class AudProgaudiElementoDTO : EntityBase
    {
        public int Progaecodi { get; set; } 
        public int Progacodi { get; set; } 
        public int Elemcodi { get; set; } 
        public DateTime? Progaeiniciorevision { get; set; } 
        public DateTime? Progaefinrevision { get; set; } 
        public int? Progaetamanomuestra { get; set; } 
        public string Progaemuestraseleccionada { get; set; } 
        public string Progaeprocedimientoprueba { get; set; } 
        public string Progaeactivo { get; set; } 
        public string Progaehistorico { get; set; } 
        public string Progaeusucreacion { get; set; } 
        public DateTime? Progaefechacreacion { get; set; } 
        public string Progaeusumodificacion { get; set; } 
        public DateTime? Progaefechamodificacion { get; set; } 

        public int Proccodi { get; set; }
        public string Elemdescripcion { get; set; }
        public string Elemcodigo { get; set; }
        
    }
}
