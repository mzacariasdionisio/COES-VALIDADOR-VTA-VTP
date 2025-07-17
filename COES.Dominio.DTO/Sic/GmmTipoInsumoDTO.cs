using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    class GmmTipoInsumoDTO : EntityBase
    {
        /// <summary>
        /// Clase que mapea la tabla GMM_TIPOINSUMO
        /// </summary>
        public string TINSCODI { get; set; }
        public string TINSINSUMO { get; set; }
        public string TINSUSUCREACION { get; set; }
        public DateTime? TINSFECCREACION { get; set; }
        public string TINSUSUMODIFICACION { get; set; }
        public DateTime? TINSFECMODIFICACION { get; set; }

    }

}
