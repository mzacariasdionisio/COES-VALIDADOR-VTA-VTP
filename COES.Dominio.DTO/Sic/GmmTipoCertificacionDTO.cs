using System;
using System.Collections.Generic;
using COES.Base.Core;


namespace COES.Dominio.DTO.Sic
{
    class GmmTipoCertificacionDTO : EntityBase
    {
        /// <summary>
        /// Clase que mapea la tabla GMM_TIPOCERTICIFICACION
        /// </summary>
        public string TCERCODI { get; set; }
        public string TCERCERTIFICACION { get; set; }
        public string TCERUSUCREACION { get; set; }
        public DateTime? TCERFECCREACION { get; set; }
        public string TCERUSUMODIFICACION { get; set; }
        public DateTime? TCERFWFECMODIFICACION { get; set; }
    }
}
