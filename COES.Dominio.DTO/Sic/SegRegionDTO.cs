using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SEG_REGION
    /// </summary>
    public class SegRegionDTO : EntityBase
    {
        public int Regcodi { get; set; }
        public string Regnombre { get; set; }
        public string Regusumodificacion { get; set; }
        public DateTime? Regfecmodificacion { get; set; }
        public string Regusucreacion { get; set; }
        public DateTime? Regfeccreacion { get; set; }

        public string Regestado { get; set; }

        public string RegfecmodificacionDesc { get; set; }
        public string RegestadoDesc { get; set; }
        public List<SegCoordenadaDTO> Listatipo { get; set; }
        public int RegcodiExcel { get; set; }
    }

}
