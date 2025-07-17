using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_SUBSCRIPCIONITEM
    /// </summary>
    public class WbSubscripcionitemDTO : EntityBase
    {
        public int Subscripcodi { get; set; } 
        public int Publiccodi { get; set; }
        public string DesPublicacion { get; set; }
        public int Indicador { get; set; }
    }
}
