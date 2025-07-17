using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_AYUDAAPP
    /// </summary>
    public class WbAyudaappDTO : EntityBase
    {
        public int Ayuappcodi { get; set; } 
        public string Ayuappcodigoventana { get; set; } 
        public string Ayuappdescripcionventana { get; set; } 
        public string Ayuappmensaje { get; set; }
        public string Ayuappmensajeeng { get; set; }
        public string Ayuappestado { get; set; } 
        public string Ayuappusucreacion { get; set; } 
        public DateTime? Ayuappfeccreacion { get; set; } 
        public string Ayuappusumodificacion { get; set; } 
        public DateTime? Ayuappfecmodificacion { get; set; } 
    }
}
