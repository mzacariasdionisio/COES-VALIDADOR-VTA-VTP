using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_BARRA_RELACION
    /// </summary>
    public class CmBarraRelacionDTO : EntityBase
    {
        public int Cmbarecodi { get; set; } 
        public int? Barrcodi { get; set; } 
        public int? Cnfbarcodi { get; set; } 
        public string Cmbaretipreg { get; set; } 
        public int? Barrcodi2 { get; set; } 
        public string Cmbaretiprel { get; set; } 
        public int? Configcodi { get; set; } 
        public DateTime? Cmbarevigencia { get; set; } 
        public DateTime? Cmbareexpira { get; set; } 
        public string Cmbareestado { get; set; } 
        public string Cmbareusucreacion { get; set; } 
        public DateTime? Cmbarefeccreacion { get; set; } 
        public string Cmbareusumodificacion { get; set; } 
        public DateTime? Cmbarefecmodificacion { get; set; } 

        public string Barrnombre { get; set; }
        public string TipoRelacion { get; set; }
        public string Cnfbarnombre { get; set; }
        public string Barrnombre2 { get; set; }
        public string Equinomb { get; set; }
        public string Vigencia { get; set; }
        public string Modificacion { get; set; }
        public string Expirtacion { get; set; }
        public string Barras { get; set; }
        public int? Cnfbarcodi2 { get; set; }

        #region Ticket_6245
        public string Cmbarereporte { get; set; }
        #endregion

    }
}
