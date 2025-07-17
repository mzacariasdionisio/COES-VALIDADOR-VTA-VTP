using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_MENUREPORTE_TIPO
    /// </summary>
    public class SiMenureporteTipoDTO : EntityBase
    {
        public int Mreptipcodi { get; set; } 
        public string Mreptipdescripcion { get; set; } 
        public int Mprojcodi { get; set; } 
    }
}
