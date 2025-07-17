using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_ORIGEN
    /// </summary>
    public class RerOrigenDTO : EntityBase
    {
        public int Reroricodi { get; set; }
        public string Reroridesc { get; set; }
        public string Reroriusucreacion { get; set; }
        public DateTime Rerorifeccreacion { get; set; }
        public string Reroriusumodificacion { get; set; }
        public DateTime Rerorifecmodificacion { get; set; }
    }
}