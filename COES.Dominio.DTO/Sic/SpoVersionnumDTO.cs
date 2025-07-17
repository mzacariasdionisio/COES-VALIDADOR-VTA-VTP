using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SPO_VERSIONNUM
    /// </summary>
    public class SpoVersionnumDTO : EntityBase
    {
        public int Verncodi { get; set; } 
        public int? Numecodi { get; set; } 
        public DateTime? Vernfechaperiodo { get; set; } 
        public string Vernusucreacion { get; set; } 
        public int? Vernestado { get; set; }
        public int? Vernnro { get; set; } 
        public DateTime? Vernfeccreacion { get; set; } 
        public string Vernusumodificacion { get; set; } 
        public DateTime? Vernfecmodificacion { get; set; }

        public string Numhisabrev { get; set; }
        public Boolean Validar { get; set; }
    }
}
