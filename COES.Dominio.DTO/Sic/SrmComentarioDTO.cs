using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SRM_COMENTARIO
    /// </summary>
    public class SrmComentarioDTO : EntityBase
    {
        public int Srmcomcodi { get; set; } 
        public int? Srmreccodi { get; set; } 
        public int? Usercode { get; set; } 
        public int? Emprcodi { get; set; } 
        public DateTime? Srmcomfechacoment { get; set; } 
        public string Srmcomgruporespons { get; set; } 
        public string Srmcomcomentario { get; set; } 
        public string Srmcomactivo { get; set; } 
        public string Srmcomusucreacion { get; set; } 
        public DateTime? Srmcomfeccreacion { get; set; } 
        public string Srmcomusumodificacion { get; set; } 
        public DateTime? Srmcomfecmodificacion { get; set; } 
        public string Srmrecfecharecomend { get; set; }
        public string Username { get; set; }
        public string Emprnomb { get; set; }
        public string Fechacomentario { get; set; }
        public string Fechacreacion { get; set; }
        public string Fechamodificacion { get; set; }
    }
}
