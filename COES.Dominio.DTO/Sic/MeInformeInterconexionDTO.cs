using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_INFORME_INTERCONEXION
    /// </summary>
    public class MeInformeInterconexionDTO : EntityBase
    {
        public int Infintcodi { get; set; } 
        public int? Infintanio { get; set; } 
        public int? Infintnrosemana { get; set; } 
        public int? Infintversion { get; set; } 
        public string Infintestado { get; set; } 
        public string Infintusucreacion { get; set; } 
        public DateTime? Infintfeccreacion { get; set; } 
        public string Infintusumodificacion { get; set; } 
        public DateTime? Infintfecmodificacion { get; set; } 
        public string NombreSemana { get; set; }
    }
}
