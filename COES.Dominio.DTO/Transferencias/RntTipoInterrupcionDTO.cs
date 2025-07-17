using System;
using System.Collections.Generic;
using COES.Base.Core;
using System.Xml.Serialization;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RNT_TIPO_INTERRUPCION
    /// </summary>
    [XmlRoot("RNT_TIPO_INTERRUPCION")]
    public class RntTipoInterrupcionDTO : EntityBase
    {
        public int TipoIntCodi { get; set; } 
        public string TipoIntNombre { get; set; } 
        public string TipoIntUsuarioCreacion { get; set; } 
        public DateTime? TipoIntFechaCreacion { get; set; } 
        public string TipoIntUsuarioUpdate { get; set; } 
        public DateTime? TipoIntFechaUpdate { get; set; } 
    }
}
