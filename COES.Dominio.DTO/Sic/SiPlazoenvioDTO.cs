using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_PLAZOENVIO
    /// </summary>
    public class SiPlazoenvioDTO : EntityBase
    {
        public int Plazcodi { get; set; }
        public int Fdatcodi { get; set; }
        public int Plazperiodo { get; set; }
        public int Plazinimin { get; set; }
        public int Plazinidia { get; set; }
        public int Plazfindia { get; set; }
        public int Plazfinmin { get; set; }
        public int Plazfueradia { get; set; }
        public int Plazfueramin { get; set; }
        public string Plazusucreacion { get; set; }
        public DateTime? Plazfeccreacion { get; set; }
        public string Plazusumodificacion { get; set; }
        public DateTime? Plazfecmodificacion { get; set; }

        public string Fdatnombre { get; set; }
        public string Usuario { get; set; }
        public string PlazfeccreacionDesc { get; set; }
        public string PlazfecmodificacionDesc { get; set; }
        public string Periodo { get; set; }

        public string TipoPlazo { get; set; }
        public DateTime FechaProceso { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaPlazoIni { get; set; }//Fecha de inicio del Plazo
        public DateTime FechaPlazoFin { get; set; }// solo se podra enviar informacion esta entre FechaPlazoIni < fecha < FechaPlazo
        public DateTime FechaPlazoFuera { get; set; }
        public int IdEnvio { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public int Emprcodi { get; set; }
    }
}
