using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CO_PROCESOCALCULO
    /// </summary>
    public class CoProcesocalculoDTO : EntityBase
    {
        public int Coprcacodi { get; set; } 
        public DateTime? Coprcafecproceso { get; set; } 
        public string Coprcausuproceso { get; set; } 
        public string Coprcaestado { get; set; } 
        public DateTime? Coprcafecinicio { get; set; } 
        public DateTime? Coprcafecfin { get; set; } 
        public int? Pericodi { get; set; } 
        public int? Vcrecaversion { get; set; } 
        public int? Copercodi { get; set; } 
        public int? Covercodi { get; set; } 
        public string Coprcafuentedato { get; set; }
    }
}
