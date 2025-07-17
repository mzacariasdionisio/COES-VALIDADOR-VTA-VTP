using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FW_ACCESO_MODELO
    /// </summary>
    public class FwAccesoModeloDTO : EntityBase
    {
        public int Acmodcodi { get; set; } 
        public int? Emprcodi { get; set; } 
        public DateTime? Acmodfecinicio { get; set; } 
        public DateTime? Acmodfin { get; set; } 
        public string Acmodestado { get; set; } 
        public string Acmodkey { get; set; } 
        public int? Acmodnrointentos { get; set; } 
        public int? Empcorcodi { get; set; } 
        public string Acmodusucreacion { get; set; } 
        public DateTime? Acmodfeccreacion { get; set; } 
        public string Acmodusumodificacion { get; set; } 
        public DateTime? Acmodfecmodificacion { get; set; } 
        public int? Acmodveces { get; set; } 
        public int? Modcodi { get; set; } 

        public string Emprnomb { get; set; }
        public string Modnomb { get; set; }
        public string Contactonomb { get; set; }
        public string Contactocorreo { get; set; }
    }
}
