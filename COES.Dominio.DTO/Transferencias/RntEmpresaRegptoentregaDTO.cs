using System;
using System.Collections.Generic;
using COES.Base.Core;
using System.Xml.Serialization;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RNT_EMPRESA_REGPTOENTREGA
    /// </summary>
    [XmlRoot("RNT_EMPRESA_REGPTOENTREGA")]
    public class RntEmpresaRegptoentregaDTO : EntityBase
    {
        public int EmpGenCodi { get; set; }
        public string EmpRpeNombre { get; set; }
        public decimal? RegPorcentaje { get; set; } 
        public string PeeUsuarioCreacion { get; set; } 
        public DateTime? PeeFechaCreacion { get; set; } 
        public string PeeUsuarioUpdate { get; set; } 
        public DateTime? PeeFechaUpdate { get; set; } 
        public int RegPuntoEntCodi { get; set; } 
        public int EmprCodi { get; set; }
        public string RpeIncremento { get; set; }
    }
}
