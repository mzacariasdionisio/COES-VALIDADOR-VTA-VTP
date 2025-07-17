using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CP_NODO_DETALLE
    /// </summary>
    public class CpNodoDetalleDTO : EntityBase
    {
        public int Cpndetcodi { get; set; } 
        public int? Cpnconcodi { get; set; } 
        public int? Cpnodocodi { get; set; } 
        public string Cpndetvalor { get; set; } 
    }
}
