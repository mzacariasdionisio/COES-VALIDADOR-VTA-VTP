using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_RETIRO_POTESC
    /// </summary>
    public class VtpRetiroPotescDTO : EntityBase
    {
        public int Rpsccodi { get; set; } 
        public int Pericodi { get; set; } 
        public int Recpotcodi { get; set; } 
        public int? Emprcodi { get; set; } 
        public int? Barrcodi { get; set; } 
        public string Rpsctipousuario { get; set; } 
        public string Rpsccalidad { get; set; }
        public decimal? Rpscprecioppb { get; set; }
        public decimal? Rpscpreciopote { get; set; }
        public decimal? Rpscpoteegreso { get; set; } 
        public decimal? Rpscpeajeunitario { get; set; } 
        public int? Barrcodifco { get; set; } 
        public decimal? Rpscpoteactiva { get; set; } 
        public decimal? Rpscpotereactiva { get; set; } 
        public string Rpscusucreacion { get; set; } 
        public DateTime Rpscfeccreacion { get; set; } 
        public string Rpscusumodificacion { get; set; } 
        public DateTime Rpscfecmodificacion { get; set; }
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnomb { get; set; }
        public string Barrnombre { get; set; }
        public string Barrnombrefco { get; set; }
        public string RpsCodiVTP { get; set; }
    }
}
