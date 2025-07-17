using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_MENU_PROJECT
    /// </summary>
    public class SiMenuProjectDTO : EntityBase
    {
        public int Mprojcodi { get; set; } 
        public string Mprojdescripcion { get; set; } 
    }
}
