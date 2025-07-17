using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla MP_PROPIEDAD
    /// </summary>
    public class MpPropiedadDTO : EntityBase
    {
        public int Mpropcodi { get; set; } 
        public int Mcatcodi { get; set; } 
        public string Mpropnombre { get; set; } 
        public string Mpropabrev { get; set; } 
        public string Mpropunidad { get; set; } 
        public int? Mproporden { get; set; } 
        public string Mproptipo { get; set; } 
        public int? Mpropcodisicoes { get; set; } 
        public int? Mpropcodisicoes2 { get; set; } 
        public string Mpropusumodificacion { get; set; } 
        public DateTime? Mpropfecmodificacion { get; set; }
        public int Mpropancho { get; set; }
        public string Mpropvalordefault { get; set; }
        public string Mpropvalordefault2 { get; set; }
        public int Mpropprioridad { get; set; }
    }
}
