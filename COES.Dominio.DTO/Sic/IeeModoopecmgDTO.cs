using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IEE_MODOOPECMG
    /// </summary>
    public class IeeModoopecmgDTO : EntityBase
    {
        public int Mocmcodigo { get; set; } 
        public int Grupocodi { get; set; } 
        public int Mocmtipocomb { get; set; } 
    }
}
