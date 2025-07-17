using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_PEAJE_INGRESO
    /// </summary>
    public class VtpPeajeIngresoDTO : EntityBase
    {
        public int Pingcodi { get; set; }
        public int Pericodi { get; set; }
        public int Recpotcodi { get; set; }
        public string Pingtipo { get; set; }
        public string Pingnombre { get; set; }
        public int? Emprcodi { get; set; }
        public int? Rrpecodi { get; set; }
        public string Pingpago { get; set; }
        public string Pingtransmision { get; set; }
        public string Pingcodigo { get; set; }
        public decimal? Pingpeajemensual { get; set; }
        public decimal? Pingtarimensual { get; set; }
        public decimal? Pingregulado { get; set; }
        public decimal? Pinglibre { get; set; }
        public decimal? Pinggranusuario { get; set; }
        public decimal? Pingporctregulado { get; set; }
        public decimal? Pingporctlibre { get; set; }
        public decimal? Pingporctgranusuario { get; set; }
        public string Pingusucreacion { get; set; }
        public DateTime Pingfeccreacion { get; set; }
        public string Pingusumodificacion { get; set; }
        public DateTime Pingfecmodificacion { get; set; }

        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnomb { get; set; }
        public string Rrpenombre { get; set; }
    }
}
