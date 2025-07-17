using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    class GmmTipoModalidadDTO : EntityBase
    {
        /// <summary>
        /// Clase que mapea la tabla GMM_TIPOMODALIDAD
        /// </summary>
        public string TMODCODI { get; set; }
        public string TMODMODALIDAD { get; set; }
        public string TMODUSUCREACION { get; set; }
        public DateTime? TMODFECCREACION { get; set; }
        public string TMODUSUMODIFICACION { get; set; }
        public DateTime? TMODFECMODIFICACION { get; set; }
    }

}
