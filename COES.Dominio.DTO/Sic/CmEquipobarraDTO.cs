using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_EQUIPOBARRA
    /// </summary>
    public class CmEquipobarraDTO : EntityBase
    {
        public int Cmeqbacodi { get; set; } 
        public int? Configcodi { get; set; } 
        public string Cmeqbaestado { get; set; } 
        public DateTime? Cmeqbavigencia { get; set; } 
        public DateTime? Cmeqbaexpira { get; set; } 
        public string Cmeqbausucreacion { get; set; } 
        public DateTime? Cmeqbafeccreacion { get; set; } 
        public string Cmeqbausumodificacion { get; set; } 
        public DateTime? Cmeqbafecmodificacion { get; set; } 
        public List<CmEquipobarraDetDTO> ListaDetalle { get; set; }
        public string Vigencia { get; set; }
        public string Modificacion { get; set; }
        public string Equinomb { get; set; }
        public string Expiracion { get; set; }
        public string Barras { get; set; }
    }
}
