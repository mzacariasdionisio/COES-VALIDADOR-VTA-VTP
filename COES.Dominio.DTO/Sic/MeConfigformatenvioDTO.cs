using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_CONFIGFORMATENVIO
    /// </summary>
    public class MeConfigformatenvioDTO : EntityBase
    {
        public int Cfgenvcodi { get; set; }
        public string Cfgenvptos { get; set; }
        public string Cfgenvorden { get; set; }
        public DateTime? Cfgenvfecha { get; set; }
        public int? Formatcodi { get; set; }
        public int? Emprcodi { get; set; }
        public string Cfgenvtipoinf { get; set; }
        public string Cfgenvhojas { get; set; }
        public string Cfgenvtipopto { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
