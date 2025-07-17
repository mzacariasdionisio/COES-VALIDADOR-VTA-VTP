using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_RECALCULO_POTENCIA
    /// </summary>
    public class VtpRecalculoPotenciaDTO : EntityBase
    {
        public int Pericodi { get; set; } 
        public int Recpotcodi { get; set; } 
        public string Recpotnombre { get; set; } 
        public string Recpotinfovtp { get; set; } 
        public decimal? Recpotfactincecontrantacion { get; set; } 
        public decimal? Recpotfactincedespacho { get; set; } 
        public decimal? Recpotmaxidemamensual { get; set; } 
        public DateTime? Recpotinterpuntames { get; set; } 
        public decimal? Recpotpreciopoteppm { get; set; }
        public decimal? Recpotpreciopeajeppm { get; set; }
        public decimal? Recpotpreciocostracionamiento { get; set; }
        public decimal? Recpotpreciodemaservauxiliares { get; set; }
        public decimal? Recpotconsumidademanda { get; set; }
        public DateTime? Recpotfechalimite { get; set; }
        public string Recpothoralimite { get; set; }
        public string Recpotcuadro1 { get; set; }
        public string Recpotnota1 { get; set; } 
        public string Recpotcomegeneral { get; set; } 
        public int? Recacodi { get; set; }
        public int? Pericodidestino { get; set; }
        public string Recpotestado { get; set; }
        public string Recpotusucreacion { get; set; } 
        public DateTime? Recpotfeccreacion { get; set; } 
        public string Recpotusumodificacion { get; set; } 
        public DateTime? Recpotfecmodificacion { get; set; }
        public int? Recpotcargapfr { get; set; }

        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Perinombre { get; set; }
        public string Recanombre { get; set; }
        public string Perinombredestino { get; set; }
        //MAPEA CPA - CU05
        public int Perianio { get; set; }
        public int Perimes { get; set; }

    }
}
