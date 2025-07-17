using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_RFRIA_UNIDADRESTRIC
    /// </summary>
    public class MeRfriaUnidadrestricDTO : EntityBase
    {
        public int Urfriacodi { get; set; }
        public int? Grupocodi { get; set; }
        public DateTime? Urfriafechaperiodo { get; set; }
        public DateTime? Urfriafechaini { get; set; }
        public DateTime? Urfriafechafin { get; set; }
        public string Urfriausucreacion { get; set; }
        public DateTime? Urfriafeccreacion { get; set; }
        public string Urfriausumodificacion { get; set; }
        public DateTime? Urfriafecmodificacion { get; set; }
        public int? Urfriaactivo { get; set; }
        public string Empresanomb { get; set; }
        public string Centralnomb { get; set; }
        public string Unidadnomb { get; set; }
        public string Urfriaobservacion { get; set; }
    }
}
