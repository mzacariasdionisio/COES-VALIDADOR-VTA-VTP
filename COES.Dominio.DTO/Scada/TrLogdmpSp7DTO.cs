using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Scada
{
    /// <summary>
    /// Clase que mapea la tabla TR_LOGDMP_SP7
    /// </summary>
    public class TrLogdmpSp7DTO : EntityBase
    {
        public int Ldmcodi { get; set; } 
        public DateTime? Ldmfecha { get; set; } 
        public int? Vercodi { get; set; } 
        public DateTime? Ldmfechapub { get; set; } 
        public DateTime? Ldmfechaimp { get; set; } 
        public string Ldmnomb { get; set; } 
        public string Ldmtipo { get; set; } 
        public string Ldmestadoserv { get; set; } 
        public string Ldmestadocli { get; set; } 
        public string Ldmnotaexp { get; set; } 
        public string Ldmnotaimp { get; set; } 
        public string Ldmmedioimp { get; set; } 
        public string Ldmcomandoexp { get; set; } 
        public string Ldmcomandoimp { get; set; }
        public string Ldmenlace { get; set; } 
        public string Ldmusucreacion { get; set; } 
        public DateTime? Ldmfeccreacion { get; set; } 
        public string Ldmusumodificacion { get; set; } 
        public DateTime? Ldmfecmodificacion { get; set; }

        public string Ldmtipotexto { get; set; }
        public string Ldmmedioimptexto { get; set; }
        public string Ldmestadotexto { get; set; } 
    }
}
