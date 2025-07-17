using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class GmmTipInformeDTO : EntityBase
    {
        /// <summary>
        /// Clase que mapea la tabla GMM_TIPINFORME
        /// </summary>
        public string TINFCODI { get; set; }
        public string TINFINFORME { get; set; }
        public string TINFUSUCREACION { get; set; }
        public DateTime? TINFFECCREACION { get; set; }
        public string TINFUSUMODIFICACION { get; set; }
        public DateTime? TINFFECMODIFICACION { get; set; }
    }
}
