using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SEG_COORDENADA
    /// </summary>
    public class SegCoordenadaDTO : EntityBase
    {
        public int Segcocodi { get; set; } 
        public int? Segconro { get; set; } 
        public decimal? Segcoflujo1 { get; set; } 
        public decimal? Segcoflujo2 { get; set; } 
        public decimal? Segcogener1 { get; set; } 
        public decimal? Segcogener2 { get; set; } 
        public int? Zoncodi { get; set; } 
        public int? Regcodi { get; set; } 
        public int? Segcotipo { get; set; }
        public int Regsegcodi { get; set; }
        public string Segcousucreacion { get; set; }
        public string Segcousumodificacion { get; set; }
        public DateTime Segcofeccreacion { get; set; }
        public DateTime Segcofecmodificacion { get; set; }
        public string Segcoestado { get; set; }

        public string Zonnombre { get; set; }
        public string TipoDesc { get; set; }
        public int Totalrestriccion { get; set; }
        //Movisoft 26-04-2022
        public int RegcodiExcel { get; set; }
        public int RegsegcodiExcel { get; set; }
    }
}
