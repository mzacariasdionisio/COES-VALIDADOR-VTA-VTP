using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_BARRA_RELACION_DET
    /// </summary>
    public class CmBarraRelacionDetDTO : EntityBase
    {
        public int Cmbadecodi { get; set; } 
        public int Cmbarecodi { get; set; } 
        public int? Cnfbarcodi { get; set; } 
        public string Cmbadeestado { get; set; } 
        public string Cmbadeusucreacion { get; set; } 
        public DateTime? Cmbadefeccreacion { get; set; } 
        public string Cmbadeusumodificacion { get; set; } 
        public DateTime? Cmbadefecmodificacion { get; set; } 
        public string Barranombre { get; set; }
    }
}
