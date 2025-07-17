using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_REGISTROIMEI
    /// </summary>
    public class WbRegistroimeiDTO : EntityBase
    {
        public int Regimecodi { get; set; } 
        public string Regimeusuario { get; set; } 
        public string Regimecodigoimei { get; set; } 
        public string Regimeestado { get; set; } 
        public string Regimeusucreacion { get; set; } 
        public DateTime? Regimefeccreacion { get; set; } 
        public string Regimeusumodificacion { get; set; } 
        public DateTime? Regimefecmodificacion { get; set; } 
    }
}
