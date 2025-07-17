using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla MAP_TIPOCALCULO
    /// </summary>
    public class MapTipocalculoDTO : EntityBase
    {
        public int Tipoccodi { get; set; } 
        public string Tipocdesc { get; set; } 
        public string Tipocabrev { get; set; } 
    }
}
