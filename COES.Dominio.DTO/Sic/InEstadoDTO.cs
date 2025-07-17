using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IN_ESTADO
    /// </summary>
    public class InEstadoDTO : EntityBase
    {
        public int Estadocodi { get; set; }
        public string Estadonomb { get; set; }
        public string Estadousucreacion { get; set; }
        public DateTime? Estadofeccreacion { get; set; }
        public string Estadousumodificacion { get; set; }
        public DateTime? Estadofecmodificacion { get; set; } 
        public int Estadopadre { get; set; }
    }
}
