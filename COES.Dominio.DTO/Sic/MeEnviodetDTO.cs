using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_ENVIODET
    /// </summary>
    public class MeEnviodetDTO : EntityBase
    {
        public int Envdetcodi { get; set; }
        public int Enviocodi { get; set; }
        public int Envdetfpkcodi { get; set; }
        public string Envdetusucreacion { get; set; }
        public DateTime? Envdetfeccreacion { get; set; }
        public string Envdetusumodificacion { get; set; }
        public DateTime? Envdetfecmodificacion { get; set; }
    }
}
