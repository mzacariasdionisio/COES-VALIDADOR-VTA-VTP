using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Scada
{
    /// <summary>
    /// Clase que mapea la tabla TR_OBSERVACION_ESTADO
    /// </summary>
    public class TrObservacionEstadoDTO : EntityBase
    {
        public int? Obscancodi { get; set; } 
        public int Obsestcodi { get; set; } 
        public string Obsestestado { get; set; } 
        public string Obsestcomentario { get; set; } 
        public string Obsestusuario { get; set; } 
        public DateTime? Obsestfecha { get; set; } 
    }
}
