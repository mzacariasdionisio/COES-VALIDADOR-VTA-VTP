using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_SORTEO
    /// </summary>
    public class SiListarAreasDTO : EntityBase
    {
        public int equicodi { get; set; } 
        public string emprnomb { get; set; } 
        public string areanomb { get; set; } 
        public string equiabrev { get; set; } 
        public int? equipadre { get; set; } 
    }
}
