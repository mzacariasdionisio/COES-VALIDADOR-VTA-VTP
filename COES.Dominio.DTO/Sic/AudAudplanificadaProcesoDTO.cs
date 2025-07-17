using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AUD_AUDPLANIFICADA_PROCESO
    /// </summary>
    public class AudAudplanificadaprocesoDTO : EntityBase
    {
        public int Audppcodi { get; set; } 
        public int Audpcodi { get; set; } 
        public int Proccodi { get; set; } 
        public string Audppactivo { get; set; } 
        public string Audpphistorico { get; set; } 
        public string Audppusucreacion { get; set; } 
        public DateTime? Audppfeccreacion { get; set; } 
        public string Audppusumodificacion { get; set; } 
        public DateTime? Audppfecmodificacion { get; set; } 

        public string Procdescripcion { get; set; }
        public int Areacodi { get; set; }
    }
}
