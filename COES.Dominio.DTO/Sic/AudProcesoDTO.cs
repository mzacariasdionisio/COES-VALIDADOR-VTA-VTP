using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AUD_PROCESO
    /// </summary>
    public class AudProcesoDTO : EntityBase
    {
        public int Proccodi { get; set; } 
        public int Areacodi { get; set; } 
        public string Proccodigo { get; set; } 
        public string Procdescripcion { get; set; } 
        public int? Proctienesuperior { get; set; } 
        public int? Procprocesosuperior { get; set; } 
        public string Procactivo { get; set; } 
        public string Prochistorico { get; set; } 
        public string Procusucreacion { get; set; } 
        public DateTime? Procfeccreacion { get; set; } 
        public string Procusumodificacion { get; set; } 
        public DateTime? Procfecmodificacion { get; set; }
        public string Procsuperior { get; set; }

        public string Areanomb { get; set; }
        public string Procsuperiordescripcion { get; set; }

        public int Existerelacion { get; set; }
        
    }
}
