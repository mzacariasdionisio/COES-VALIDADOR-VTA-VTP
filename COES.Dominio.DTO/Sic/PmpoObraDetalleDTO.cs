using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PMPO_OBRA_DETALLE
    /// </summary>
    public class PmpoObraDetalleDTO : EntityBase
    {
        public int Obradetcodi { get; set; }
        public int Obracodi { get; set; }
        public int Equicodi { get; set; }
        public String Emprnomb { get; set; }
        public String Equinomb { get; set; }
        public String Equiestado { get; set; }
        public int Barrcodi { get; set; }
        public String Barrnombre { get; set; }
        public String Barrsituacion { get; set; }
        public int Grupocodi { get; set; }
        public String Gruponombre { get; set; }
        public String Grupoestado { get; set; }
        public string Obradetdescripcion { get; set; }

        public string Obradetusucreacion { get; set; }
        public DateTime Obradetfeccreacion { get; set; }

        public string Obradetusumodificacion { get; set; }
        public DateTime Obradetfecmodificacion { get; set; }

        
    }
}
