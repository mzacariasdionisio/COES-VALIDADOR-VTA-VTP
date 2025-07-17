using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sp7
{
    /// <summary>
    /// Clase que mapea la tabla TR_VERSION_SP7
    /// </summary>
    public class TrVersionSp7DTO : EntityBase
    {
        public int Vercodi { get; set; } 
        public int Emprcodieje { get; set; }
        public string Emprenomb { get; set; } 
        public DateTime? Verfechaini { get; set; } 
        public DateTime? Verfechafin { get; set; } 
        public string Vernota { get; set; }
        public string Vernotaeje { get; set; } 
        public int? Vernumero { get; set; } 
        public string Verrepoficial { get; set; } 
        public string Verestado { get; set; } 
        public string Verreprocestadcanal { get; set; } 
        public string Verconsidexclus { get; set; } 
        public string Verestadisticacontabmae { get; set; } 
        public DateTime? Verestadisticamaefecha { get; set; }
        public string Verauditoria { get; set; }         
        public DateTime? Verultfechaejec { get; set; } 
        public string Versionaplic { get; set; } 
        public string Verusucreacion { get; set; } 
        public DateTime? Verfeccreacion { get; set; } 
        public string Verusumodificacion { get; set; } 
        public DateTime? Verfecmodificacion { get; set; }
        public string Verestadodescrip { get; set; } 
    }
}
