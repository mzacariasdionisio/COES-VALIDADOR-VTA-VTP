using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Scada
{
    /// <summary>
    /// Clase que mapea la tabla TR_OBSERVACION
    /// </summary>
    public class TrObservacionDTO : EntityBase
    {
        public int Obscancodi { get; set; } 
        public string Obscanusucreacion { get; set; } 
        public string Obscanusumodificacion { get; set; } 
        public DateTime? Obscanfeccreacion { get; set; } 
        public DateTime? Obscanfecmodificacion { get; set; } 
        public string Obscanestado { get; set; }
        public string Desestado { get; set; }
        public string Obscancomentario { get; set; } 
        public string Obscantipo { get; set; } 
        public int? Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public List<TrObservacionItemDTO> ListaItems { get; set; }
        public List<TrObservacionEstadoDTO> ListaHistoria { get; set; }
        public string IndEdicion { get; set; }

        #region FIT - Señales no Disponibles
        public string Obscanproceso { get; set; }
        public string Obscancomentarioagente { get; set; }
        #endregion
    }
}
