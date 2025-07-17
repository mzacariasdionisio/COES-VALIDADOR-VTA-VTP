using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_COMPARATIVO_CAB
    /// </summary>
    public class RerComparativoCabDTO : EntityBase
    {
        public int Rerccbcodi { get; set; }
        public int Rerevacodi { get; set; }
        public int Reresecodi { get; set; }
        public int Rereeucodi { get; set; }
        public string Rerccboridatos { get; set; }
        public decimal Rerccbtotenesolicitada { get; set; }
        public decimal Rerccbtoteneestimada { get; set; }
        public string Rerccbusucreacion { get; set; }
        public DateTime Rerccbfeccreacion { get; set; }
        public string Rerccbusumodificacion { get; set; }
        public DateTime Rerccbfecmodificacion { get; set; }
    }
}