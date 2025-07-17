using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EN_ENSAYOFORMATO
    /// </summary>
    public class EnEnsayoformatoDTO : EntityBase
    {
        public int Formatocodi { get; set; }
        public int Enunidadcodi { get; set; }
        public string Ensformatnombfisico { get; set; }
        public string Ensformatnomblogico { get; set; }
        public DateTime? Lastdate { get; set; }
        public string Lastuser { get; set; }
        public int? Ensformtestado { get; set; }

        /// <summary>
        /// Campos adicionales no inluidos en BD
        /// </summary>
        public string Formatodesc { get; set; }
        public string Estenvnomb { get; set; }
        public string Estadocolor { get; set; }
        public string Imageextension { get; set; }
        public int Equicodi { get; set; }
        public int Ensayocodi { get; set; }
        public string Equinomb { get; set; }
        public string Colorfila { get; set; }
    }
}
