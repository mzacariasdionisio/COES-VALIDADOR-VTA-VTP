using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_EMPRESAMENSAJE
    /// </summary>
    public class SiEmpresaMensajeDTO : EntityBase
    {
        public int Empmsjcodi { get; set; } 
        public int? Msgcodi { get; set; } 
        public int? Envdetcodi { get; set; } 
        public int? Emprcodi { get; set; } 
        public string Empmsjusucreacion { get; set; } 
        public DateTime? Empmsjfeccreacion { get; set; } 
        public string Empmsjusumodificacion { get; set; } 
        public DateTime? Empmsjfecmodificacion { get; set; } 
    }
}
