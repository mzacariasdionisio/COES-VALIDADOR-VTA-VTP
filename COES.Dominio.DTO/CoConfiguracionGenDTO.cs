using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CO_CONFIGURACION_GEN
    /// </summary>
    public class CoConfiguracionGenDTO : EntityBase
    {
        public int Courgecodi { get; set; } 
        public int? Equicodi { get; set; } 
        public int? Courdecodi { get; set; } 
        public string Courgeusucreacion { get; set; } 
        public DateTime? Courgefeccreacion { get; set; } 
        public string Courgeusumodificacion { get; set; } 
        public DateTime? Courgefecmodificacion { get; set; } 
        public string Equinomb { get; set; }
        public int Seleccion { get; set; }
        public int? Grupocodi { get; set; }
        public string Emprnomb { get; set; }
        public string Centralnomb { get; set; }
        public string Codigo { get; set; }
        public string Estadoenvio { get; set; }
        public string Fechaenvio { get; set; }
        public string Usuarioenvio { get; set; }
       
    }
}
