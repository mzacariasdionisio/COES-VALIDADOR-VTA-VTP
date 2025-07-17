using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_ENERGIAUNIDAD
    /// </summary>
    public class RerEnergiaUnidadDTO : EntityBase
    {
        public int Rereucodi { get; set; }
        public int Rersedcodi { get; set; }
        public int Equicodi { get; set; }
        public string Rereuenergiaunidad { get; set; }
        public decimal Rereutotenergia { get; set; }
        public string Rereuusucreacion { get; set; }
        public DateTime Rereufeccreacion { get; set; }

        //
        public int Centralcodi { get; set; }
        public string Equinombre { get; set; }
        public DateTime Intervaloinicio { get; set; }
        public DateTime Intervalofin { get; set; }
    }
}