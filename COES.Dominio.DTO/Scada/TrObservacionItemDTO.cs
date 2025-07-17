using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Scada
{
    /// <summary>
    /// Clase que mapea la tabla TR_OBSERVACION_ITEM
    /// </summary>
    public class TrObservacionItemDTO : EntityBase
    {
        public int? Canalcodi { get; set; } 
        public int Obsitecodi { get; set; } 
        public string Obsiteestado { get; set; } 
        public string Obsitecomentario { get; set; }
        public string Obsitecomentarioagente { get; set; }
        public int? Obscancodi { get; set; } 
        public string Obsiteusuario { get; set; } 
        public DateTime? Obsitefecha { get; set; }
        public string Canalnomb { get; set; }
        public string Canaliccp { get; set; }
        public string Canalunidad { get; set; }
        public string Canalabrev { get; set; }
        public string Canalpointtype { get; set; }
        public string Emprnomb { get; set; }
        public string Zonanomb {get; set;}
        public string Descestado { get; set; }

        #region FIT - Señales no Disponibles
        public string Obsiteproceso { get; set; }
        public string FechaEmpresa { get; set; }
        public string FechaCoes { get; set; }

        #endregion
    }
}
