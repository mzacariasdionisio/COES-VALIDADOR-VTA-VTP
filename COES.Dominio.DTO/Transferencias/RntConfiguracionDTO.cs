using System;
using System.Collections.Generic;
using COES.Base.Core;
using System.Xml.Serialization;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RNT_CONFIGURACION
    /// </summary>
    [XmlRoot("RNT_CONFIGURACION")]
    public class RntConfiguracionDTO : EntityBase
    {
        public string ConfAtributo { get; set; } 
        public string ConfParametro { get; set; } 
        public string ConfValor { get; set; } 
        public int ConfigCodi { get; set; }
        public int ConfEstado { get; set; } 
        public string ConfUsuarioCreacion { get; set; } 
        public DateTime? ConfFechaCreacion { get; set; } 
        public string ConfUsuarioUpdate { get; set; } 
        public DateTime? ConfFechaUpdate { get; set; } 
    }
}
