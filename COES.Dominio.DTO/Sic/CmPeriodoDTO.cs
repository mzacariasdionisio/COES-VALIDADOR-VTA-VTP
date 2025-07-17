using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_PERIODO
    /// </summary>
    public class CmPeriodoDTO : EntityBase
    {
        public int Cmpercodi { get; set; } 
        public string Cmperbase { get; set; } 
        public string Cmpermedia { get; set; } 
        public string Cmperpunta { get; set; } 
        public string Cmperestado { get; set; } 
        public DateTime? Cmpervigencia { get; set; } 
        public DateTime? Cmperexpira { get; set; } 
        public string Cmperusucreacion { get; set; } 
        public DateTime? Cmperfeccreacion { get; set; } 
        public string Cmperusumodificacion { get; set; } 
        public DateTime? Cmperfecmodificacion { get; set; } 
        public string Vigencia { get; set; }
        public string Modificacion { get; set; }
        public string Expiracion { get; set; }
    }
}
