using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_ACTIVIDAD
    /// </summary>
    public class SiActividadDTO : EntityBase
    {
        public int? Areacodi { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
        public int Actcodi { get; set; } 
        public string Actabrev { get; set; } 
        public string Actnomb { get; set; }
        public string Areaabrev { get; set; }
    }
}
