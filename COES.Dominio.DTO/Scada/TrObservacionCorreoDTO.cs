using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Scada
{
    /// <summary>
    /// Clase que mapea la tabla TR_OBSERVACION_CORREO
    /// </summary>
    public class TrObservacionCorreoDTO : EntityBase
    {
        public int Obscorcodi { get; set; } 
        public int Emprcodi { get; set; } 
        public string Obscoremail { get; set; } 
        public string Obscorestado { get; set; } 
        public string Obscornombre { get; set; } 
        public string Obscorusumodificacion { get; set; } 
        public DateTime? Obscorfecmodificacion { get; set; }
        public string Emprnomb { get; set; }
    }
}
