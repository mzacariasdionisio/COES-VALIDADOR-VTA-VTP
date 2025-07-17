using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_PROCESO
    /// </summary>
    public class WbProcesoDTO : EntityBase
    {
        public int Procesocodi { get; set; }
        public string Procesoname { get; set; }
        public string Procesoestado { get; set; }
        public string Procesousucreacion { get; set; }
        public string Procesousumodificacion { get; set; }
        public DateTime? Procesofeccreacion { get; set; }
        public DateTime? Procesofecmodificacion { get; set; }
    }
}
