using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Scada
{
    /// <summary>
    /// Clase que mapea la tabla TR_OBSERVACION_ITEM_ESTADO
    /// </summary>
    public class TrObservacionItemEstadoDTO : EntityBase
    {
        public int? Obsitecodi { get; set; } 
        public int Obitescodi { get; set; } 
        public string Obitesestado { get; set; } 
        public string Obitescomentario { get; set; } 
        public string Obitesusuario { get; set; } 
        public DateTime? Obitesfecha { get; set; } 
    }
}
