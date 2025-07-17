using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_CONFIGBARRA
    /// </summary>
    public class CmConfigbarraDTO : EntityBase
    {
        public int Cnfbarcodi { get; set; } 
        public string Cnfbarnodo { get; set; } 
        public string Cnfbarnombre { get; set; } 
        public string Cnfbarcoorx { get; set; } 
        public string Cnfbarcoory { get; set; } 
        public string Cnfbarestado { get; set; } 
        public string Cnfbarindpublicacion { get; set; } 
        public string Cnfbarusucreacion { get; set; } 
        public DateTime? Cnfbarfeccreacion { get; set; } 
        public string Cnfbarusumodificacion { get; set; } 
        public DateTime? Cnfbarfecmodificacion { get; set; }
        public string Cnfbardefecto { get; set; }
        public string Cnfbarnombncp { get; set; }

        //- Cambios EMS TNA
        public string Cnfbarnomtna { get; set; }
        //- Fin cambios EMS TNA

        #region Mejoras CMgN
        public int? Canalcodi { get; set; } 
        public int? Recurcodi { get; set; }
        public int? Topcodi { get; set; }
        public string Recurnombre { get; set; }
        #endregion
    }
}
