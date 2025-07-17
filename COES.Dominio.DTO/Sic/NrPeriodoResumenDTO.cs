using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla NR_PERIODO_RESUMEN
    /// </summary>
    public class NrPeriodoResumenDTO : EntityBase
    {
        public int Nrperrcodi { get; set; }
        public int? Nrpercodi { get; set; }
        public int? Nrcptcodi { get; set; }
        public int? Nrperrnumobservacion { get; set; }
        public string Nrperrobservacion { get; set; }
        public string Nrperreliminado { get; set; }
        public string Nrperrusucreacion { get; set; }
        public DateTime? Nrperrfeccreacion { get; set; }
        public string Nrperrusumodificacion { get; set; }
        public DateTime? Nrperrfecmodificacion { get; set; }
        public DateTime? Nrpermes { get; set; }
        public string Nrcptabrev { get; set; }

        public int Nrsmodcodi { get; set; }
        public string Nrsmodnombre { get; set; }
        //public int nrpercodi { get; set; }
        //public DateTime? nrpermes { get; set; }
        public int Pendiente { get; set; }
        public int Observaciones { get; set; }
        public int Terminado { get; set; }
        public int Proceso { get; set; }

        
        public string Nrcptdescripcion { get; set; }

    }
}
