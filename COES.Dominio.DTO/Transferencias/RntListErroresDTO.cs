using System;
using System.Collections.Generic;
using COES.Base.Core;
using System.Xml.Serialization;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RNT_PERIODO
    /// </summary>
    [XmlRoot("RNT_LISTERRORES")]
    public class RntListErroresDTO : EntityBase
    {
        public string tipontcse { get; set; } 
        public string celda { get; set; } 
        public string valor { get; set; } 
        public string tipo { get; set; } 
    }
}
