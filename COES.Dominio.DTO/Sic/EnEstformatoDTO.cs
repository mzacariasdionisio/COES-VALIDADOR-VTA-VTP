using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EN_ESTFORMATO
    /// </summary>
    public class EnEstformatoDTO : EntityBase
    {
        public int Estfmtcodi { get; set; }
        public int Enunidadcodi { get; set; }
        public int Formatocodi { get; set; }
        public int Estadocodi { get; set; }
        public DateTime? Estfmtlastdate { get; set; }
        public string Estfmtlastuser { get; set; }
        public string Estfmtdescrip { get; set; }

        /// <summary>
        /// Campos adicionales no inluidos en BD
        /// </summary>
        public string Estadonombre { get; set; }
        public string Estadocolor { get; set; }
        public string Formatodesc { get; set; }
    }
}
