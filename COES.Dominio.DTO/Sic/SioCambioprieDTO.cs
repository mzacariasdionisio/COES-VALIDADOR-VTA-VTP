using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SIO_CAMBIOPRIE
    /// </summary>
    public class SioCambioprieDTO : EntityBase
    {
        public int Campricodi { get; set; }
        public int? Cabpricodi { get; set; }
        public int? Grupocodi { get; set; }
        public int? Equicodi { get; set; }
        public int? Barrcodi { get; set; }
        public int? Emprcodi { get; set; }
        public int? Emprcodi2 { get; set; }
        public DateTime? Camprifecmodificacion { get; set; }
        public string Campriusumodificacion { get; set; }
        public string Camprivalor { get; set; }
        public int Ptomedicodi { get; set; }
    }
}
