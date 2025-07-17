using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RI_SOLICITUDDETALLE
    /// </summary>
    public class RiSolicituddetalleDTO : EntityBase
    {
        public string Sdetvalor { get; set; } 
        public string Sdetadjunto { get; set; } 
        public string Sdetvaloradjunto { get; set; } 
        public int? Solicodi { get; set; } 
        public string Sdetusucreacion { get; set; } 
        public DateTime? Sdetfeccreacion { get; set; } 
        public string Sdetusumodificacion { get; set; } 
        public DateTime? Sdetfecmodificacion { get; set; } 
        public int Sdetcodi { get; set; } 
        public string Sdetcampo { get; set; } 
    }
}
