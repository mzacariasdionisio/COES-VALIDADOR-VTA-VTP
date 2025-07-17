using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PMPO_TIPO_OBRA
    /// </summary>
    public class PmpoTipoObraDTO : EntityBase
    {
        public int TObracodi { get; set; }
        public string TObradescripcion { get; set; }
        public string TObrausucreacion { get; set; }
        public DateTime TObrafeccreacion { get; set; }
        public string TObrausumodificacion { get; set; }
        public DateTime TObrafecmodificacion { get; set; }

    }
}
