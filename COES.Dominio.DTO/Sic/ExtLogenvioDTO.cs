using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EXT_LOGENVIO
    /// </summary>
    public class ExtLogenvioDTO : EntityBase
    {
        public int Logcodi { get; set; }
        public int Emprcodi { get; set; }
        public string Filenomb { get; set; }
        public int Origlectcodi { get; set; }
        public int Lectcodi { get; set; }
        public int Estenvcodi { get; set; }
        public DateTime? Feccarga { get; set; }
        public int? Nrosemana { get; set; }
        public DateTime? Fecproceso { get; set; }
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
        public string EmprNomb { get; set; }
        public string EstEnvNomb { get; set; }
        public int NroAnio { get; set; }
    }
}

